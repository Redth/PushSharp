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

		public ApplePushChannel(ApplePushChannelSettings channelSettings, PushServiceSettings serviceSettings = null) : base(channelSettings, serviceSettings)
		{
			this.appleSettings = channelSettings;

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

			//Start our cleanup task
			taskCleanup = new Task(Cleanup, TaskCreationOptions.LongRunning);
			taskCleanup.ContinueWith(t => 
			{
				var ex = t.Exception; 

				if (this.Events != null)
					this.Events.RaiseChannelException(ex, PlatformType.Apple, null);
			}, TaskContinuationOptions.OnlyOnFaulted);
			taskCleanup.Start();
		}

        public override PlatformType PlatformType
        {
            get { return Common.PlatformType.Apple; }
        }

		object sentLock = new object();
		object connectLock = new object();
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

		protected long trackedNotificationCount = 0;


		public override bool QueueNotification(Notification notification, bool countsAsRequeue = true, bool ignoreStoppingChannel = false)
		{
			if (base.QueueNotification(notification, countsAsRequeue, ignoreStoppingChannel))
			{
				if (!ignoreStoppingChannel)
					Interlocked.Increment(ref trackedNotificationCount);

				return true;
			}

			return false;
		}

		protected override void SendNotification(Common.Notification notification)
		{
			lock (sentLock)
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

					Interlocked.Decrement(ref trackedNotificationCount);
					
					this.Events.RaiseNotificationSendFailure(notification, nfex);
				}

				if (isOkToSend)
				{
					lock(connectLock)
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
					catch (Exception)
					{
						//If this failed, we probably had a networking error, so let's requeue the notification
						this.QueueNotification(notification, true, true);
					} 
				}
			}
		}

		public override void Stop(bool waitForQueueToDrain)
		{
			stopping = true;

			//See if we want to wait for the queue to drain before stopping
			if (waitForQueueToDrain)
			{
				var sentNotificationCount = 0;
				lock (sentLock)
					sentNotificationCount = sentNotifications.Count;

				while (QueuedNotificationCount > 0 || sentNotificationCount > 0 || Interlocked.Read(ref trackedNotificationCount) > 0)
				{
					Thread.Sleep(100);
	
					lock (sentLock)
						sentNotificationCount = sentNotifications.Count;
				}
			}

			if (!CancelTokenSource.IsCancellationRequested)
				CancelTokenSource.Cancel();

			//Wait on our tasks for a maximum of 5 seconds
			Task.WaitAll(new Task[] { base.taskSender, taskCleanup }, 5000);
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
			this.Events.RaiseNotificationSendFailure(failedNotification.Notification, new NotificationFailureException(status, failedNotification.Notification));
			sentNotifications.RemoveAt(failedIndex);

			//Don't GetRange if there's 0 items to get, or the call will fail
			if (sentNotifications.Count - (failedIndex + 1) > 0)
			{
				//All Notifications after the failed one have been shifted back one space now
				//Grab all the notifications from the list that are after the failed index
				var toRequeue = sentNotifications.GetRange(failedIndex, sentNotifications.Count - (failedIndex + 1)).ToList();
				//Remove that same range (those ones failed since they were sent after the one apple told us failed, so
				// apple will ignore them, and we need to requeue them to be tried again
				sentNotifications.RemoveRange(failedIndex, sentNotifications.Count - (failedIndex + 1));

				//Requeue all the messages that were sent afte the failed one, be sure it doesn't count as a 'requeue' to go towards the maximum # of retries
				//Also ignore that the channel is stopping
				foreach (var n in toRequeue)
					this.QueueNotification(n.Notification, false, true);
			}
		}

		void Cleanup()
		{
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
								
								Interlocked.Decrement(ref trackedNotificationCount);

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
					this.Events.RaiseChannelException(ex, PlatformType.Apple);
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
	}
}
