using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Net.Sockets;
using System.Net.Security;
using System.Threading;
using System.Net;
using PushSharp.Core;

namespace PushSharp.Apple
{
	public class ApplePushChannel : IPushChannel
	{
		private const int INITIAL_RECONNECT_DELAY = 3000;
		private const float RECONNECT_BACKOFF_MULTIPLIER = 1.5f;
		private const int CLEANUP_INTERVAL = 1000;

		public delegate void ConnectingDelegate(string host, int port);
		public event ConnectingDelegate OnConnecting;
		public delegate void ConnectedDelegate(string host, int port);
		public event ConnectedDelegate OnConnected;
		public delegate void ConnectionFailureDelegate(ConnectionFailureException exception);
		public event ConnectionFailureDelegate OnConnectionFailure;
		public delegate void WaitBeforeReconnectDelegate(int millisecondsToWait);
		public event WaitBeforeReconnectDelegate OnWaitBeforeReconnect;
		public event PushChannelExceptionDelegate OnException;

		private readonly CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
		private readonly CancellationToken _cancelToken;
		private readonly ApplePushChannelSettings _appleSettings;
		private readonly List<SentNotification> _sentNotifications = new List<SentNotification>();
		private X509Certificate _certificate;
		private X509CertificateCollection _certificates;
		private TcpClient _client;
		private SslStream _sslStream;
		private Stream _networkStream;
		private long _trackedNotificationCount;
		private bool _isConnected;
		private readonly object _connectionLock = new object();
		private readonly object _sendLock = new object();
		private int _connectionAttempt;
		private int _reconnectDelay = INITIAL_RECONNECT_DELAY;
		private IAsyncResult _readResult;
		private readonly byte[] _readBuffer = new byte[6];
		private Timer _cleanupTimer;
		private bool _cleaningUp;
		private readonly object _cleanupLock = new object();
		private int _cleanedUp;
		private int _reconnects;
		private readonly Guid _channelInstanceId = Guid.NewGuid();

		public ApplePushChannel(ApplePushChannelSettings channelSettings)
		{
			Log.Debug("Creating ApplePushChannel instance " + _channelInstanceId);

			_cancelToken = _cancelTokenSource.Token;

			_appleSettings = channelSettings;

			ConfigureCertificates();

			StartCleanupTimer();
		}

		private void ConfigureCertificates()
		{
			_certificate = _appleSettings.Certificate;

			_certificates = new X509CertificateCollection();

			if (_appleSettings.AddLocalAndMachineCertificateStores)
			{
				var store = new X509Store(StoreLocation.LocalMachine);
				_certificates.AddRange(store.Certificates);

				store = new X509Store(StoreLocation.CurrentUser);
				_certificates.AddRange(store.Certificates);
			}

			_certificates.Add(_certificate);

			if (_appleSettings.AdditionalCertificates != null)
			{
				foreach (var additionalCertificate in _appleSettings.AdditionalCertificates)
				{
					_certificates.Add(additionalCertificate);
				}
			}
		}

		public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			Interlocked.Increment(ref _trackedNotificationCount);

			var appleNotification = notification as AppleNotification;
			if (appleNotification == null)
			{
				throw new ArgumentException("Notification was not an AppleNotification", "notification");
			}

			Exception failure;
			byte[] notificationData;
			if (!TryGetNotificationData(appleNotification, out notificationData, out failure))
			{
				Interlocked.Decrement(ref _trackedNotificationCount);
				if (callback != null)
				{
					callback(this, new SendNotificationResult(notification, false, failure));
				}
				return;
			}

			try
			{
				EnsureConnected();

				lock (_sendLock)
				{
					PollConnection();

					Log.Debug("ApplePushChannel instance {0}: Sending notification {1}",
						_channelInstanceId,
						appleNotification.Identifier);

					_networkStream.Write(notificationData, 0, notificationData.Length);
					_networkStream.Flush();
					_sentNotifications.Add(new SentNotification(appleNotification)
					{
						Callback = callback
					});
				}
			}
			catch (Exception exception)
			{
				Disconnect();

				Log.Error("Exception during APNS Send with channel {2}: {0} -> {1}",
					appleNotification.Identifier,
					exception,
					_channelInstanceId);

				// If this failed, we probably had a networking error, so let's requeue the notification
				Interlocked.Decrement(ref _trackedNotificationCount);
				if (callback != null)
				{
					callback(this, new SendNotificationResult(notification, true, exception));
				}
			}
		}

		private void PollConnection()
		{
			var stillConnected = _isConnected
			                     && _client != null
			                     && _client.Connected
			                     && _client.Client.Poll(0, SelectMode.SelectWrite)
			                     && _networkStream != null
			                     && _networkStream.CanWrite;

			if (!stillConnected)
			{
				throw new InvalidOperationException("Connection to APNS has disconnected or is in an invalid state.");
			}
		}

		public void Dispose()
		{
			if (_cancelToken.IsCancellationRequested)
			{
				return;
			}

			Log.Info("ApplePushChannel {0} Dispose->Waiting for queues to drain...", _channelInstanceId);

			//See if we want to wait for the queue to drain before stopping
			int sentNotificationCount;
			lock (_sendLock)
			{
				sentNotificationCount = _sentNotifications.Count;
			}

			_cleanupTimer.Change(Timeout.Infinite, Timeout.Infinite);

			while (sentNotificationCount > 0 || Interlocked.Read(ref _trackedNotificationCount) > 0)
			{
				Cleanup();

				Thread.Sleep(100);

				lock (_sendLock)
				{
					sentNotificationCount = _sentNotifications.Count;
				}
			}

			Cleanup();

			_cleanupTimer.Dispose();
			_cancelTokenSource.Cancel();

			Log.Info("ApplePushChannel {2} Disposed : Cleaned up {0}, Reconnects: {1}",
				_cleanedUp,
				_reconnects,
				_channelInstanceId);
		}

		private void EnsureConnected()
		{
			if (!_isConnected)
			{
				lock (_connectionLock)
				{
					while (!_isConnected && !_cancelToken.IsCancellationRequested)
					{
						_connectionAttempt++;

						try
						{
							Connect();
							_isConnected = true;
							_connectionAttempt = 0;
						}
						catch (ConnectionFailureException exception)
						{
							_isConnected = false;
							ReportConnectionFailure(exception);

							if (_connectionAttempt >= _appleSettings.MaxConnectionAttempts)
							{
								throw new ConnectionFailureException(string.Format("Maximum number of attempts ({0}) to connect to {1}:{2} was reached!",
									_appleSettings.MaxConnectionAttempts, _appleSettings.Host, _appleSettings.Port), exception);
							}
						}

						if (!_isConnected)
						{
							WaitBeforeNextConnectionAttempt();
						}
						else
						{
							_reconnectDelay = INITIAL_RECONNECT_DELAY;

							var onConnected = OnConnected;
							if (onConnected != null)
							{
								onConnected(_appleSettings.Host, _appleSettings.Port);
							}
						}
					}
				}
			}
		}

		private void Connect()
		{
			if (_client != null)
			{
				Disconnect();
			}

			Log.Debug("Starting Connect with instance {0}", _channelInstanceId);
			_client = new TcpClient();

			var onConnecting = OnConnecting;
			if (onConnecting != null)
			{
				onConnecting(_appleSettings.Host, _appleSettings.Port);
			}

			try
			{
				var connectResult = _client.BeginConnect(_appleSettings.Host, _appleSettings.Port, null, null);
				if (!connectResult.AsyncWaitHandle.WaitOne(_appleSettings.ConnectionTimeout))
				{
					Disconnect();
					throw new TimeoutException("Timeout reached while attempting to connect.");
				}

				_client.EndConnect(connectResult);
				_client.SetSocketKeepAliveValues(
					(int)_appleSettings.KeepAlivePeriod.TotalMilliseconds,
					(int)_appleSettings.KeepAliveRetryPeriod.TotalMilliseconds);

				Interlocked.Increment(ref _reconnects);
			}
			catch (Exception exception)
			{
				throw new ConnectionFailureException("Connection to APNS host failed", exception);
			}

			SecureConnection();

			StartReader();
		}

		private void StartReader()
		{
			try
			{
				Log.Debug("Starting Reader for channel {0}", _channelInstanceId);
				// We don't want to block here, so just start a read and wait for the callback later.
				_readResult = _networkStream.BeginRead(_readBuffer, 0, _readBuffer.Length, ar =>
				{
					if (ar != _readResult)
					{
						return;
					}

					HandleResponse(ar);
				}, null);
			}
			catch (Exception exception)
			{
				Log.Error("Error starting read from APNS channel from instance {1}: {0}",
					exception.ToString(),
					_channelInstanceId);
				Disconnect();
			}
		}

		private void HandleResponse(IAsyncResult readResult)
		{
			Log.Debug("Handling response from APNS on instance {0}", _channelInstanceId);
			// Prevent any sends while we process this
			lock (_sendLock)
			{
				try
				{
					var bytesRead = _networkStream.EndRead(readResult);

					if (bytesRead > 0)
					{
						// Get the enhanced format response
						// byte 0 is always '1', byte 1 is the status, bytes 2,3,4,5 are the identifier of the notification
						var status = _readBuffer[1];
						var identifier = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(_readBuffer, 2));

						HandleFailedNotification(identifier, status);
					}
				}
				catch (Exception exception)
				{
					Log.Error("Error reading APNS response on instance{1}: {0}",
						exception.ToString(),
						_channelInstanceId);
				}
				finally
				{
					// Apple only respond if there was a failure.  If there was a failure, then all 
					// notifications sent after the failed one must be re-sent
					// Apple will close the connection so we must re-connect
					Disconnect();
				}
			}
		}

		private void HandleFailedNotification(int identifier, byte status)
		{
			// Get the index of our failed notification (by identifier)
			var failedIndex = _sentNotifications.FindIndex(n => n.Identifier == identifier);

			if (failedIndex < 0)
			{
				return;
			}

			Log.Info("Failed Notification on channel {1}: {0}",
				identifier,
				_channelInstanceId);

			//Get all the notifications before the failed one and mark them as sent!
			if (failedIndex > 0)
			{
				var successful = _sentNotifications.GetRange(0, failedIndex);

				successful.ForEach(n =>
				{
					Interlocked.Decrement(ref _trackedNotificationCount);

					if (n.Callback != null)
					{
						n.Callback(this, new SendNotificationResult(n.Notification));
					}
				});

				_sentNotifications.RemoveRange(0, failedIndex);
			}

			//Get the failed notification itself
			var failedNotification = _sentNotifications[0];

			//Fail and remove the failed index from the list
			Interlocked.Decrement(ref _trackedNotificationCount);

			if (failedNotification.Callback != null)
			{
				failedNotification.Callback(this, new SendNotificationResult(failedNotification.Notification,
					false,
					new NotificationFailureException(status, failedNotification.Notification)));
			}

			_sentNotifications.RemoveAt(0);

			// Notifications sent after the failure must be re-sent
			_sentNotifications.Reverse();
			_sentNotifications.ForEach(n =>
			{
				Interlocked.Decrement(ref _trackedNotificationCount);

				if (failedNotification.Callback != null)
				{
					failedNotification.Callback(this,
						new SendNotificationResult(n.Notification,
							true,
							new Exception("Sent after previously failed Notification."))
						{
							CountsAsRequeue = false
						});
				}
			});

			_sentNotifications.Clear();
		}

		private void SecureConnection()
		{
			Log.Debug("Starting SecureConnection from instance {0}", _channelInstanceId);
			if (_appleSettings.SkipSsl)
			{
				_networkStream = _client.GetStream();
				return;
			}

			var certificateValidator = _appleSettings != null && _appleSettings.ValidateServerCertificate
				? (RemoteCertificateValidationCallback)ValidateRemoteCertificate
				: (sender, cert, chain, policyErrors) => true;

			_sslStream = new SslStream(_client.GetStream(),
				false,
				certificateValidator,
				(sender, host, certificates, certificate, issuers) => _certificate);

			try
			{
				_sslStream.AuthenticateAsClient(_appleSettings.Host, _certificates, SslProtocols.Tls, false);
			}
			catch (AuthenticationException exception)
			{
				throw new ConnectionFailureException("SSL Stream Failed to Authenticate as Client", exception);
			}

			if (!_sslStream.IsMutuallyAuthenticated)
			{
				throw new ConnectionFailureException("SSL Stream Failed to Authenticate", null);
			}

			if (!_sslStream.CanWrite)
			{
				throw new ConnectionFailureException("SSL Stream is not Writable", null);
			}

			_networkStream = _sslStream;
		}

		private void Disconnect()
		{
			Log.Debug("Disconnecting channel {0}", _channelInstanceId);
			lock (_connectionLock)
			{
				if (_networkStream != null)
				{
					_networkStream.Dispose();
				}

				if (_client != null)
				{
					try
					{
						// Attempt to shutdown the socket gracefully if it's still active
						_client.Client.Shutdown(SocketShutdown.Both);
						_client.Client.Close();
					}
					catch (Exception exception)
					{
						Log.Debug("Exception while disconnecting TcpClient for instance {1}: {0}",
							exception.ToString(),
							_channelInstanceId);
					}
					_client.Client.Dispose();
				}

				_networkStream = null;
				_sslStream = null;
				_client = null;

				_isConnected = false;
			}
		}

		private void WaitBeforeNextConnectionAttempt()
		{
			var onWaitBeforeReconnect = OnWaitBeforeReconnect;
			if (onWaitBeforeReconnect != null)
			{
				onWaitBeforeReconnect(_reconnectDelay);
			}

			var slept = 0;
			while (slept <= _reconnectDelay && !_cancelToken.IsCancellationRequested)
			{
				Thread.Sleep(250);
				slept += 250;
			}

			_reconnectDelay = (int)(RECONNECT_BACKOFF_MULTIPLIER * _reconnectDelay);
		}

		private void ReportConnectionFailure(ConnectionFailureException exception)
		{
			var onConnectionFailure = OnConnectionFailure;
			if (onConnectionFailure != null)
			{
				onConnectionFailure(exception);
			}

			var onException = OnException;
			if (onException != null)
			{
				onException(this, exception);
			}
		}

		private bool TryGetNotificationData(AppleNotification appleNotification, out byte[] notificationData, out Exception failure)
		{
			try
			{
				notificationData = appleNotification.ToBytes();
				failure = null;
				return true;
			}
			catch (NotificationFailureException exception)
			{
				notificationData = null;
				failure = exception;
				return false;
			}
		}

		private void StartCleanupTimer()
		{
			_cleanupTimer = new Timer(x => CleanupTimerAction(),
				null,
				TimeSpan.FromMilliseconds(CLEANUP_INTERVAL),
				TimeSpan.FromMilliseconds(CLEANUP_INTERVAL));
		}

		private void CleanupTimerAction()
		{
			try
			{
				Cleanup();
			}
			catch (Exception exception)
			{
				Log.Error("Error in timed cleanup for channel {1}, continuing.  {0}",
					exception.ToString(),
					_channelInstanceId);
			}
		}

		private void Cleanup()
		{
			if (!_cleaningUp)
			{
				lock (_cleanupLock)
				{
					if (!_cleaningUp)
					{
						CleanupInternal();
					}
				}
			}
		}

		/// <summary>
		/// Main processing for Cleanup
		/// </summary>
		/// <remarks>
		/// The cleanup process serves 2 purposes:
		///  1 - Attempting to re-open the connection if is dropped for any reason
		///  2 - Successful notifications do not get acknowedged by Apple.  Once we can safely assume success,
		///      this process will remove notificaitons from tracking lists and call back to indicate a successful send
		/// </remarks>
		private void CleanupInternal()
		{
			_cleaningUp = true;
			var continueCleanup = true;
			while (continueCleanup)
			{
				AttemptConnection();

				if (_sentNotifications.Count > 0)
				{
					lock (_sendLock)
					{
						if (_sentNotifications.Count > 0)
						{
							// Don't expire any notifications while we are in a connecting state, o rat least ensure all notifications have been sent
							// in case we may have no connection because no notifications were causing a connection to be initiated
							if (_isConnected)
							{
								continueCleanup = ProcessOldestNotification();
							}
							else
							{
								// We weren't connected, bump up the sentat timestamp
								// so that we wait a while after reconnecting to expire this message
								_sentNotifications[0].SentAt = DateTime.UtcNow;
								continueCleanup = false;
							}
						}
					}
				}
				else
				{
					continueCleanup = false;
				}
			}

			_cleaningUp = false;
		}

		private void AttemptConnection()
		{
			// Attempt to keep the connection alive regardless of other activity
			try
			{
				EnsureConnected();
			}
			catch (Exception exception)
			{
				var onException = OnException;
				if (onException != null)
				{
					onException(this, exception);
				}
			}
		}

		private bool ProcessOldestNotification()
		{
			bool continueCleanup;
			var oldestNotification = _sentNotifications[0];

			var millisecondsSinceSend = (DateTime.UtcNow - oldestNotification.SentAt).TotalMilliseconds;
			if (millisecondsSinceSend > _appleSettings.MillisecondsToWaitBeforeMessageDeclaredSuccess)
			{
				Interlocked.Decrement(ref _trackedNotificationCount);
				Log.Debug("ApplePushChannel instance {0} - notification {1} successfully sent",
					_channelInstanceId,
					oldestNotification.Identifier);
				if (oldestNotification.Callback != null)
				{
					oldestNotification.Callback(this, new SendNotificationResult(oldestNotification.Notification));
				}

				_sentNotifications.RemoveAt(0);

				Interlocked.Increment(ref _cleanedUp);

				continueCleanup = true;
			}
			else
			{
				continueCleanup = false;
			}

			return continueCleanup;
		}


		private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
		{
			return policyErrors == SslPolicyErrors.None;
		}
	}
}

