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
		ConcurrentDictionary<int, SentNotification> sentNotifications = new ConcurrentDictionary<int, SentNotification>();

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
		
		int reconnectDelay = 3000;
		float reconnectBackoffMultiplier = 1.5f;
		
		byte[] readBuffer = new byte[6];
		bool connected = false;
		
		X509Certificate certificate;
		X509CertificateCollection certificates;
		TcpClient client;
		SslStream stream;		
			
		Task taskCleanup;

		protected override void SendNotification(Common.Notification notification)
		{
			var appleNotification = notification as AppleNotification;

			Connect();

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
				try { stream.Write(notificationData); }
				catch { this.QueueNotification(notification); } //If this failed, we probably had a networking error, so let's requeue the notification
			}
		}

		public override void Stop(bool waitForQueueToDrain)
		{
			//Call the base stop first
			base.Stop(waitForQueueToDrain);

			//Wait on our cleanup task
			taskCleanup.Wait(30000);
		}
		
		void Reader()
		{
			//try
			//{
				var result = stream.BeginRead(readBuffer, 0, 6, new AsyncCallback((asyncResult) =>
				{
					//try
					//{
						var bytesRead = stream.EndRead(asyncResult);

						if (bytesRead > 0)
						{
							//Get the enhanced format response
							// byte 0 is always '1', byte 1 is the status, bytes 2,3,4,5 are the identifier of the notification
							var status = readBuffer[1];
							var identifier = IPAddress.NetworkToHostOrder(BitConverter.ToInt32(readBuffer, 2));

							SentNotification sentNotification = null;

							if (sentNotifications.TryGetValue(identifier, out sentNotification))
							{ 
								var nfex = new NotificationFailureException(status, sentNotification.Notification);

								//Raise alert that notification failed
								this.Events.RaiseNotificationSendFailure(sentNotification.Notification, nfex);
							}

							//Start reading again
							Reader();
						}
						else
							connected = false;
					//}
					//catch { }

					//Otherwise, our connection was closed

				}), null);
			//}
			//catch { }
		}


		void Cleanup()
		{
			while (true)
			{
				var toRemove = (from n in sentNotifications.Values
								where n.SentAt < DateTime.UtcNow.AddSeconds(2)
								select n.Identifier).ToArray();

				if (toRemove != null && toRemove.Length > 0)
				{
					SentNotification removedNotification = null;

					foreach (var id in toRemove)
					{
						if (sentNotifications.TryRemove(id, out removedNotification))
						{
							//Alert that we sent this one successfully since there was no error response
							this.Events.RaiseNotificationSent(removedNotification.Notification);
						}
					}
				}
				else if (CancelToken.IsCancellationRequested) //If there were no items left and the program is ending, break out
					break;
				else //Sleep since there are no items to remove and no cancellation token requested
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
			
			stream = new SslStream(client.GetStream(), false,
				new RemoteCertificateValidationCallback((sender, cert, chain, sslPolicyErrors) => { return true; }),
				new LocalCertificateSelectionCallback((sender, targetHost, localCerts, remoteCert, acceptableIssuers) =>
				{
					return certificate;
				}));

			try
			{
				stream.AuthenticateAsClient(this.appleSettings.Host, this.certificates, System.Security.Authentication.SslProtocols.Ssl3, false);
			}
			catch (System.Security.Authentication.AuthenticationException ex)
			{
				throw new ConnectionFailureException("SSL Stream Failed to Authenticate as Client", ex);
			}

			if (!stream.IsMutuallyAuthenticated)
				throw new ConnectionFailureException("SSL Stream Failed to Authenticate", null);

			if (!stream.CanWrite)
				throw new ConnectionFailureException("SSL Stream is not Writable", null);

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
