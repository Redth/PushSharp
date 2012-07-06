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
using PushSharp.Common;

namespace PushSharp.Apple
{
	public class ApplePushChannel : PushChannelBase
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


		ApplePushChannelSettings appleSettings = null;
		List<SentNotification> sentNotifications = new List<SentNotification>();

		public ApplePushChannel(ApplePushChannelSettings settings) : base(settings)
		{
			this.appleSettings = settings as ApplePushChannelSettings;

			//Need to load the private key seperately from apple
			// Fixed by danielgindi@gmail.com :
			//      The default is UserKeySet, which has caused internal encryption errors,
			//      Because of lack of permissions on most hosting services.
			//      So MachineKeySet should be used instead.
			certificate = new X509Certificate2(settings.CertificateData, settings.CertificateFilePassword, 
				X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

			certificates = new X509CertificateCollection();
			certificates.Add(certificate);

			//Start our cleanup task
			taskCleanup = new Task(() => Cleanup(), TaskCreationOptions.LongRunning);
			taskCleanup.ContinueWith((t) => { var ex = t.Exception; }, TaskContinuationOptions.OnlyOnFaulted);
			taskCleanup.Start();
		}

		object sentLock = new object();
		object streamWriteLock = new object();
		int reconnectDelay = 3000;
		float reconnectBackoffMultiplier = 1.5f;
		
		byte[] readBuffer = new byte[6];
		bool connected = false;
		
		X509Certificate certificate;
		X509CertificateCollection certificates;
		TcpClient client;
		SslStream stream;
		System.IO.Stream networkStream;
		Task taskCleanup;
		
		protected override void SendNotification(Common.Notification notification)
		{
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

				this.Events.RaiseNotificationSendFailure(notification, nfex);
			}

			if (isOkToSend)
			{
				Connect();
				
				try 
				{
					lock (streamWriteLock)
					{
						lock (sentLock)
						{
							networkStream.Write(notificationData, 0, notificationData.Length);

							sentNotifications.Add(new SentNotification(appleNotification));
						}
					}
				}
				catch (Exception ex)
				{ 
					this.QueueNotification(notification); 
				} //If this failed, we probably had a networking error, so let's requeue the notification
			}
		}

		public override void Stop(bool waitForQueueToDrain)
		{
			stopping = true;

			//See if we want to wait for the queue to drain before stopping
			if (waitForQueueToDrain)
			{
				while (QueuedNotificationCount > 0 || sentNotifications.Count > 0)
					Thread.Sleep(50);
			}

			//Sleep a bit to prevent any race conditions
			//especially since our cleanup method may need 3 seconds
			Thread.Sleep(5000);

			if (!CancelTokenSource.IsCancellationRequested)
				CancelTokenSource.Cancel();

			//Wait on our tasks for a maximum of 30 seconds
			Task.WaitAll(new Task[] { base.taskSender, taskCleanup }, 30000);
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

								int failedNotificationIndex = -1;
								SentNotification failedNotification = null;

								//Try and find the failed notification in our sent list
								for (int i = 0; i < sentNotifications.Count; i++)
								{
									var n = sentNotifications[i];

									if (n.Identifier.Equals(identifier))
									{
										failedNotificationIndex = i;
										failedNotification = n;
										break;
									}
								}

								//Don't bother doing anything unless we know what failed
								if (failedNotification != null && failedNotificationIndex > -1)
								{
									//Anything before the failed message must have sent OK
									// so let's expedite the success status Success for all those before the failed one
									if (failedNotificationIndex > 0)
									{
										for (int i = 0; i < failedNotificationIndex; i++)
											this.Events.RaiseNotificationSent(sentNotifications[i].Notification);
									}

									//The notification that failed needs to have a failure event raised
									// we don't requeue it because apple told us it failed for real
									this.Events.RaiseNotificationSendFailure(failedNotification.Notification,
										new NotificationFailureException(status, failedNotification.Notification));

									// finally, raise failure for anything after the index of this failed one
									// in the sent list, since we may have sent them but apple will have disregarded
									// anything after the failed one and not told us about it
									if (failedNotificationIndex < sentNotifications.Count - 1)
									{
										//Requeue the failed notification since we're not sure it's a bad
										// notification, just that it was sent after a bad one was
										for (int i = failedNotificationIndex + 1; i <= sentNotifications.Count - 1; i++)
											this.QueueNotification(sentNotifications[i].Notification, false);
									}

									//Now clear out the sent list since we processed them all manually above
									sentNotifications.Clear();
								} 

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

		void Cleanup()
		{
			bool wasRemoved = false;

			while (true)
			{
				lock (sentLock)
				{
					//See if anything is here to process
					if (sentNotifications.Count > 0)
					{
						//Don't expire any notifications while we are in a connecting state
						if (connected || CancelToken.IsCancellationRequested)
						{
							//Get the oldest sent message
							var n = sentNotifications[0];

							//If it was sent more than 3 seconds ago,
							// we have to assume it was sent successfully!
							if (n.SentAt < DateTime.UtcNow.AddMilliseconds(-1 * appleSettings.MillisecondsToWaitBeforeMessageDeclaredSuccess))
							{
								wasRemoved = true;
								this.Events.RaiseNotificationSent(n.Notification);
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

				if (this.CancelToken.IsCancellationRequested)
					break;
				else if (!wasRemoved)
					Thread.Sleep(250);
			}
		}

		void Connect()
		{
			//Keep trying to connect
			while (!connected && !CancelToken.IsCancellationRequested)
			{
				try
				{
					connect();
					connected = true;
				}
				catch (ConnectionFailureException ex)
				{
					connected = false;

					//Report the error
					var cf = this.OnConnectionFailure;

					if (cf != null)
						cf(ex);

					//Raise a channel exception
					this.Events.RaiseChannelException(ex);
				}

				if (!connected)
				{
					//Notify we are waiting before reconnecting
					var eowbrd = this.OnWaitBeforeReconnect;
					if (eowbrd != null)
						eowbrd(reconnectDelay);

					//Sleep for a delay
					int slept = 0;
					while (slept <= reconnectDelay && !this.CancelToken.IsCancellationRequested)
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
				client.Connect(this.appleSettings.Host, this.appleSettings.Port);
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
					new RemoteCertificateValidationCallback((sender, cert, chain, sslPolicyErrors) => { return true; }),
					new LocalCertificateSelectionCallback((sender, targetHost, localCerts, remoteCert, acceptableIssuers) =>
					{
						return certificate;
					}));

				try
				{
					//stream.AuthenticateAsClient(this.appleSettings.Host, this.certificates, System.Security.Authentication.SslProtocols.Ssl3, false);
					stream.AuthenticateAsClient(this.appleSettings.Host);
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
	}
}
