using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using PushSharp.Common;

namespace PushSharp.Android
{
	public class GcmPushChannel : PushChannelBase
	{
		GcmPushChannelSettings gcmSettings = null;
		GcmMessageTransportAsync transport;
		long waitCounter = 0;

		public GcmPushChannel(GcmPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null) : base(channelSettings, serviceSettings) 
		{
			gcmSettings = channelSettings;
			
			transport = new GcmMessageTransportAsync();
			
			transport.MessageResponseReceived += new Action<GcmMessageTransportResponse>(transport_MessageResponseReceived);

			transport.UnhandledException += new Action<GcmNotification, Exception>(transport_UnhandledException);
		}

        public override PlatformType PlatformType
        {
            get { return PlatformType.AndroidGcm; }
        }

		void transport_UnhandledException(GcmNotification notification, Exception exception)
		{
			//Raise individual failures for each registration id for the notification
			foreach (var r in notification.RegistrationIds)
			{
				this.Events.RaiseNotificationSendFailure(GcmNotification.ForSingleRegistrationId(notification, r), exception);
			}

			this.Events.RaiseChannelException(exception, PlatformType.AndroidGcm, notification);

			Interlocked.Decrement(ref waitCounter);
		}

		void transport_MessageResponseReceived(GcmMessageTransportResponse response)
		{
			int index = 0;

			//Loop through every result in the response
			// We will raise events for each individual result so that the consumer of the library
			// can deal with individual registrationid's for the notification
			foreach (var r in response.Results)
			{
				var singleResultNotification = GcmNotification.ForSingleResult(response, index);

				if (r.ResponseStatus == GcmMessageTransportResponseStatus.Ok)
				{
					//It worked! Raise success
					this.Events.RaiseNotificationSent(singleResultNotification); 
				}
				else if (r.ResponseStatus == GcmMessageTransportResponseStatus.CanonicalRegistrationId)
				{
					//Swap Registrations Id's
					var newRegistrationId = r.CanonicalRegistrationId;

					this.Events.RaiseDeviceSubscriptionIdChanged(PlatformType.AndroidGcm, singleResultNotification.RegistrationIds[0], newRegistrationId, singleResultNotification);

				}
				else if (r.ResponseStatus == GcmMessageTransportResponseStatus.Unavailable)
				{
					this.QueueNotification(singleResultNotification);
				}
				else if (r.ResponseStatus == GcmMessageTransportResponseStatus.NotRegistered)
				{
					//Raise failure and device expired
					this.Events.RaiseDeviceSubscriptionExpired(PlatformType.AndroidGcm, singleResultNotification.RegistrationIds[0], singleResultNotification);
				}
				else
				{
					//Raise failure, for unknown reason
					this.Events.RaiseNotificationSendFailure(singleResultNotification, new GcmMessageTransportException(r.ResponseStatus.ToString(), response));
				}

				index++;
			}

			Interlocked.Decrement(ref waitCounter);
		}

		protected override void SendNotification(Notification notification)
		{
			Interlocked.Increment(ref waitCounter);

			transport.Send(notification as GcmNotification, gcmSettings.SenderAuthToken, gcmSettings.SenderID, gcmSettings.ApplicationIdPackageName);
		}

		public override void Stop(bool waitForQueueToDrain)
		{
			base.Stop(waitForQueueToDrain);

			var slept = 0;
			while (Interlocked.Read(ref waitCounter) > 0 && slept <= 5000)
			{
				slept += 100;
				Thread.Sleep(100);
			}
		}
	}
}
