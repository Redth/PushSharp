using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Net.Sockets;
using System.Net.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using PushSharp.Core;

namespace PushSharp.Apple
{
	public class ApplePushChannel : IPushChannel
	{
		#region Constants
		private const string hostSandbox = "gateway.sandbox.push.apple.com";
		private const string hostProduction = "gateway.push.apple.com";
		private const int initialReconnectDelay = 3000;
		#endregion
				
		public delegate void ConnectingDelegate(string host, int port);
		public event ConnectingDelegate OnConnecting;

		public delegate void ConnectedDelegate(string host, int port);
		public event ConnectedDelegate OnConnected;

		public delegate void ConnectionFailureDelegate(ConnectionFailureException exception);
		public event ConnectionFailureDelegate OnConnectionFailure;

		public delegate void WaitBeforeReconnectDelegate(int millisecondsToWait);
		public event WaitBeforeReconnectDelegate OnWaitBeforeReconnect;

		public event PushChannelExceptionDelegate OnException;

		private CancellationTokenSource cancelTokenSrc = new CancellationTokenSource();
		private CancellationToken cancelToken;
		ApplePushChannelSettings appleSettings = null;
		List<SentNotification> sentNotifications = new List<SentNotification>();

		private Timer timerCleanup;
		
		public ApplePushChannel(ApplePushChannelSettings channelSettings)
		{
			cancelToken = cancelTokenSrc.Token;

			appleSettings = channelSettings;

			certificate = this.appleSettings.Certificate;

            certificates = new X509CertificateCollection();

            if (appleSettings.AddLocalAndMachineCertificateStores)
            {
                var store = new X509Store(StoreLocation.LocalMachine);
                certificates.AddRange(store.Certificates);

                store = new X509Store(StoreLocation.CurrentUser);
                certificates.AddRange(store.Certificates);
            }

			certificates.Add(certificate);

            if (this.appleSettings.AdditionalCertificates != null)
                foreach (var addlCert in this.appleSettings.AdditionalCertificates)
                    certificates.Add(addlCert);

			timerCleanup = new Timer(state => Cleanup(), null, TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(1000));

		}

		
		int connectionAttemptCounter = 0;
		object sentLock = new object();
		object connectLock = new object();
		object streamWriteLock = new object();
		object cleanupLock = new object();
		int reconnectDelay = 3000;
		float reconnectBackoffMultiplier = 1.5f;
		
		byte[] readBuffer = new byte[6];
		volatile bool connected = false;
		volatile bool isInCleanup = false;

		X509Certificate certificate;
		X509CertificateCollection certificates;
		TcpClient client;
		SslStream stream;
		System.IO.Stream networkStream;
		
		long trackedNotificationCount = 0;
		

		public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			lock (sentLock)
			{
				Interlocked.Increment(ref trackedNotificationCount);

				var appleNotification = notification as AppleNotification;
				
				bool isOkToSend = true;
				byte[] notificationData = new byte[] {};

				try
				{
					notificationData = appleNotification.ToBytes();
				}
				catch (NotificationFailureException nfex)
				{
					//Bad notification format already
					isOkToSend = false;

					Interlocked.Decrement(ref trackedNotificationCount);
					
					if (callback != null)
						callback(this, new SendNotificationResult(notification, false, nfex));
				}
				

				if (isOkToSend)
				{
					try
					{
						lock (connectLock)
							Connect();

						lock (streamWriteLock)
						{
							bool stillConnected = client.Connected
											&& client.Client.Poll(0, SelectMode.SelectWrite)
											&& networkStream.CanWrite;

							if (!stillConnected)
								throw new ObjectDisposedException("Connection to APNS is not Writable");
								
							lock (sentLock)
							{
								if (notificationData.Length > 45)
								{
									networkStream.Write(notificationData, 0, 45);
									networkStream.Write(notificationData, 45, notificationData.Length - 45);
								}
								else
									networkStream.Write(notificationData, 0, notificationData.Length);
								
								networkStream.Flush();

								sentNotifications.Add(new SentNotification(appleNotification) {Callback = callback});
							}
						}
					}
					catch (ConnectionFailureException cex)
					{
						connected = false;

						//If this failed, we probably had a networking error, so let's requeue the notification
						Interlocked.Decrement(ref trackedNotificationCount);

						if (callback != null)
							callback(this, new SendNotificationResult(notification, false, cex));
					}
					catch (Exception ex)
					{
						connected = false;

						//If this failed, we probably had a networking error, so let's requeue the notification
						Interlocked.Decrement(ref trackedNotificationCount);

						if (callback != null)
							callback(this, new SendNotificationResult(notification, true, ex));
					}
				}
			}

		}

		public void Dispose()
		{
			if (cancelToken.IsCancellationRequested)
				return;

			Log.Info("ApplePushChannel->Waiting...");

			timerCleanup.Change(Timeout.Infinite, Timeout.Infinite);

			try
			{
				Cleanup();
			}
			catch
			{
			}

			//See if we want to wait for the queue to drain before stopping
			var sentNotificationCount = 0;
			lock (sentLock)
				sentNotificationCount = sentNotifications.Count;

			while (sentNotificationCount > 0 || Interlocked.Read(ref trackedNotificationCount) > 0)
			{
				Thread.Sleep(100);
	
				lock (sentLock)
					sentNotificationCount = sentNotifications.Count;
			}

			cancelTokenSrc.Cancel();

			Log.Info("ApplePushChannel->DISPOSE.");
		}
		
		void Reader()
		{
			try
			{
				var result = networkStream.BeginRead(readBuffer, 0, 6, new AsyncCallback((asyncResult) =>
				{
					lock (sentLock)
					{
						try
						{
							var bytesRead = networkStream.EndRead(asyncResult);

							if (bytesRead > 0)
							{
								//We now expect apple to close the connection on us anyway, so let's try and close things
								// up here as well to get a head start
								//Hopefully this way we have less messages written to the stream that we have to requeue
								try { stream.Close(); stream.Dispose(); }
								catch { }

								try { client.Close(); stream.Dispose(); }
								catch { }

								//Get the enhanced format response
								// byte 0 is always '1', byte 1 is the status, bytes 2,3,4,5 are the identifier of the notification
								var status = readBuffer[1];
								var identifier = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(readBuffer, 2));

								HandleFailedNotification(identifier, status);
								
								//Start reading again
								Reader();
							}
							else
							{
								connected = false;
							}
						}
						catch
						{
							connected = false;
						}

					} // End Lock
					
				}), null);
			}
			catch
			{
				connected = false;
			}
		}

		void HandleFailedNotification(int identifier, byte status)
		{
			//Get the index of our failed notification (by identifier)
			var failedIndex = sentNotifications.FindIndex(n => n.Identifier == identifier);
			
			if (failedIndex < 0)
				return;

			//Get the failed notification itself
			var failedNotification = sentNotifications[failedIndex];
			
			//Fail and remove the failed index from the list
			Interlocked.Decrement(ref trackedNotificationCount);

			if (failedNotification.Callback != null)
				failedNotification.Callback(this, new SendNotificationResult(failedNotification.Notification, false, new NotificationFailureException(status, failedNotification.Notification) ));

			sentNotifications.RemoveAt(failedIndex);

			//Don't GetRange if there's 0 items to get, or the call will fail
			if (sentNotifications.Count - (failedIndex) > 0)
			{
				//All Notifications after the failed one have been shifted back one space now
				//Grab all the notifications from the list that are after the failed index
				var toRequeue = sentNotifications.GetRange(failedIndex, sentNotifications.Count - (failedIndex)).ToList();
				//Remove that same range (those ones failed since they were sent after the one apple told us failed, so
				// apple will ignore them, and we need to requeue them to be tried again
				sentNotifications.RemoveRange(failedIndex, sentNotifications.Count - (failedIndex));

				//Requeue all the messages that were sent afte the failed one, be sure it doesn't count as a 'requeue' to go towards the maximum # of retries
				//Also ignore that the channel is stopping
				foreach (var n in toRequeue)
				{
					Interlocked.Decrement(ref trackedNotificationCount);

					if (failedNotification.Callback != null)
						failedNotification.Callback(this, new SendNotificationResult(n.Notification, true, new Exception("Sent after previously failed Notification.")) { CountsAsRequeue = false });
				}
			}
		}
		
		void Cleanup()
		{
			if (isInCleanup)
				return;

			isInCleanup = true;
			
			while (true)
			{
				lock(connectLock)
					Connect();

				bool wasRemoved = false;

				lock (sentLock)
				{
					//See if anything is here to process
					if (sentNotifications.Count > 0)
					{
						//Don't expire any notifications while we are in a connecting state, o rat least ensure all notifications have been sent
						// in case we may have no connection because no notifications were causing a connection to be initiated
						if (connected)
						{
							//Get the oldest sent message
							var n = sentNotifications[0];

							//If it was sent more than 3 seconds ago,
							// we have to assume it was sent successfully!
							if (n.SentAt < DateTime.UtcNow.AddMilliseconds(-1 * appleSettings.MillisecondsToWaitBeforeMessageDeclaredSuccess))
							{
								wasRemoved = true;
								
								Interlocked.Decrement(ref trackedNotificationCount);

								if (n.Callback != null)
									n.Callback(this, new SendNotificationResult(n.Notification));

								sentNotifications.RemoveAt(0);
							}
							else
								wasRemoved = false;
						}
						else
						{
							//In fact, if we weren't connected, bump up the sentat timestamp
							// so that we wait awhile after reconnecting to expire this message
							try { sentNotifications[0].SentAt = DateTime.UtcNow; }
							catch { }
						}
					}
				}

				//if (this.cancelToken.IsCancellationRequested)
				//	break;
				//else
				if (!wasRemoved)
					break; // Thread.Sleep(250);
			}

			isInCleanup = false;
		}
	
		void Connect()
		{
			//Keep trying to connect
			while (!connected && !cancelToken.IsCancellationRequested)
			{
				connectionAttemptCounter++;

				try
				{
					connect();
					connected = true;

					//Reset connection attempt counter
					connectionAttemptCounter = 0;
				}
				catch (ConnectionFailureException ex)
				{
					connected = false;

					//Report the error
					var cf = this.OnConnectionFailure;

					if (cf != null)
						cf(ex);

					var evt = this.OnException;
					if (evt != null)
						evt(this, ex);
				}

				if (!connected && connectionAttemptCounter >= appleSettings.MaxConnectionAttempts)
				{
					throw new ConnectionFailureException(string.Format("Maximum number of attempts ({0}) to connect to {1}:{2} was reached!", 
						appleSettings.MaxConnectionAttempts, appleSettings.Host, appleSettings.Port), new TimeoutException());
				}

				if (!connected)
				{
					//Notify we are waiting before reconnecting
					var eowbrd = this.OnWaitBeforeReconnect;
					if (eowbrd != null)
						eowbrd(reconnectDelay);

					//Sleep for a delay
					int slept = 0;
					while (slept <= reconnectDelay && !this.cancelToken.IsCancellationRequested)
					{
						Thread.Sleep(250);
						slept += 250;
					}

					//Increase the delay by the exponential backoff multiplier
					reconnectDelay = (int)(reconnectBackoffMultiplier * reconnectDelay);
				}
				else
				{
					//Reset connect delay
					reconnectDelay = initialReconnectDelay;

					//Notify we are connected
					var eoc = this.OnConnected;
					if (eoc != null)
						eoc(this.appleSettings.Host, this.appleSettings.Port);
				}
			}
		}

		void connect()
		{
			client = new TcpClient();

			//Notify we are connecting
			var eoc = this.OnConnecting;
			if (eoc != null)
				eoc(this.appleSettings.Host, this.appleSettings.Port);
			
			try
			{
				var connectDone = new AutoResetEvent(false);
			
				//Connect async so we can utilize a connection timeout
				client.BeginConnect(
					appleSettings.Host, appleSettings.Port,
					new AsyncCallback(
						delegate(IAsyncResult ar)
						{
							try
							{
								client.EndConnect(ar);

								//Set keep alive on the socket may help maintain our APNS connection
								client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

								//Trigger the reset event so we can continue execution below
								connectDone.Set();
							}
							catch (Exception ex)
							{
								Log.Error("APNS Connect Callback Failed: " + ex);
							}
						}
					), client
				);

				if (!connectDone.WaitOne(appleSettings.ConnectionTimeout))
				{
					throw new TimeoutException("Connection to Host Timed Out!");
				}
			}
			catch (Exception ex)
			{
				throw new ConnectionFailureException("Connection to Host Failed", ex);
			}
			
			if (appleSettings.SkipSsl)
			{
				networkStream = client.GetStream();
			}
			else
			{
				stream = new SslStream(client.GetStream(), false,
					(sender, cert, chain, sslPolicyErrors) => true, //Don't validate remote cert
					(sender, targetHost, localCerts, remoteCert, acceptableIssuers) => certificate); //

				try
				{
					stream.AuthenticateAsClient(this.appleSettings.Host, this.certificates, System.Security.Authentication.SslProtocols.Ssl3, false);
					//stream.AuthenticateAsClient(this.appleSettings.Host);
				}
				catch (System.Security.Authentication.AuthenticationException ex)
				{
					throw new ConnectionFailureException("SSL Stream Failed to Authenticate as Client", ex);
				}

				if (!stream.IsMutuallyAuthenticated)
					throw new ConnectionFailureException("SSL Stream Failed to Authenticate", null);

				if (!stream.CanWrite)
					throw new ConnectionFailureException("SSL Stream is not Writable", null);

				networkStream = stream;
			}
			
			//Start reading from the stream asynchronously
			Reader();
		}
		
	}

	public class SentNotification
	{
		public SentNotification(AppleNotification notification)
		{
			this.Notification = notification;
			this.SentAt = DateTime.UtcNow;
			this.Identifier = notification.Identifier;
		}

		public AppleNotification Notification { get; set; }

		public DateTime SentAt { get; set; }

		public int Identifier { get; set; }

		public SendNotificationCallbackDelegate Callback { get; set; }
	}
}
