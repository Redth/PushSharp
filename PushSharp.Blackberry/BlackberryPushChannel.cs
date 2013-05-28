using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using PushSharp.Core;
using System.Net.Http;
using System.Threading.Tasks;

namespace PushSharp.Blackberry
{
    public class BlackberryPushChannel : IPushChannel
    {
        BlackberryPushChannelSettings bisChannelSettings;

        public BlackberryPushChannel(BlackberryPushChannelSettings channelSettings)
        {
            bisChannelSettings = channelSettings;
        }

		public class BlackberryHttpClient : HttpClient
		{
			public BlackberryHttpClient() : base()
			{
			}

			public Task<HttpResponseMessage> PostNotification(BlackberryPushChannelSettings channelSettings, BlackberryNotification n)
			{
				var authInfo = channelSettings.ApplicationId + ":" + channelSettings.Password;
				authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

				var c = new MultipartContent ("related", channelSettings.Boundary);
				c.Headers.TryAddWithoutValidation ("Authorization", "Basic " + authInfo);
				c.Headers.TryAddWithoutValidation ("Content-Type", "type: application/xml");
				var xml = n.ToPapXml ();

				c.Add (new StringContent (xml.ToString(), Encoding.UTF8, "application/xml"));

				c.Add (new ByteArrayContent(n.Message));
			
				return PostAsync (channelSettings.SendUrl, c);
			}
		}

		BlackberryHttpClient http = new BlackberryHttpClient ();

		public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			var n = notification as BlackberryNotification;

			var response = http.PostNotification (bisChannelSettings, n).Result;
			var description = string.Empty;

			var status = new BlackberryMessageStatus
			{
				Notification = n,
				HttpStatus = HttpStatusCode.ServiceUnavailable
			};

			var bbNotStatus = string.Empty;
			status.HttpStatus = response.StatusCode;

			var doc = XDocument.Load (response.Content.ReadAsStreamAsync ().Result);

			var result = doc.Descendants("response-result").SingleOrDefault();
			if (result != null)
			{
				bbNotStatus = result.Attribute("code").Value;
				description = result.Attribute("desc").Value;
			}
			else
			{
				result = doc.Descendants("badmessage-response").SingleOrDefault();
				if (result != null)
				{
					bbNotStatus = result.Attribute("code").Value;
					description = result.Attribute("desc").Value;
				}
			}

			BlackberryNotificationStatus notStatus;
			Enum.TryParse(bbNotStatus, true, out notStatus);
			status.NotificationStatus = notStatus;

			if (status.NotificationStatus == BlackberryNotificationStatus.NoAppReceivePush)
			{
				if (callback != null)
					callback(this, new SendNotificationResult(notification, false, new Exception("Device Subscription Expired")) { IsSubscriptionExpired = true });

				return;
			}

			if (status.HttpStatus == HttpStatusCode.OK
			    && status.NotificationStatus == BlackberryNotificationStatus.RequestAcceptedForProcessing)
			{
				if (callback != null)
					callback(this, new SendNotificationResult(notification));
				return;
			}

			if (callback != null)
				callback(this, new SendNotificationResult(status.Notification, false, new BisNotificationSendFailureException(status, description)));
		}

        public void Dispose()
        {
        }
    }
}
