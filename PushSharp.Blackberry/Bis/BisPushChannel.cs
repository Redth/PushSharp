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
    public class BisPushChannel : IPushChannel
    {
        BisPushChannelSettings bisChannelSettings;

        public BisPushChannel(BisPushChannelSettings channelSettings)
        {
            bisChannelSettings = channelSettings;
        }

		public class BisHttpClient : HttpClient
		{
			public BisHttpClient() : base()
			{
				this.DefaultRequestHeaders.Add("Accept", "text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2");
				this.DefaultRequestHeaders.Connection.Add("Keep-Alive");

				//TODO: PreAuthenticate
			}

			public Task<HttpResponseMessage> PostNotification(BisPushChannelSettings channelSettings, BisNotification n)
			{
				var authInfo = channelSettings.ApplicationId + ":" + channelSettings.Password;
				authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

				var c = new MultipartContent ("application/xml", channelSettings.Boundary);
				c.Headers.Add ("Authorization", "Basic " + authInfo);

				var pushId = DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture);
				var deliverBefore = DateTime.UtcNow.AddMinutes(5).ToString("s", CultureInfo.InvariantCulture) + "Z";

				var xml = new StringBuilder();

				xml.AppendLine("<?xml version=\"1.0\"?>");
				xml.AppendLine("<!DOCTYPE pap PUBLIC \"-//WAPFORUM//DTD PAP 2.1//EN\" \"http://www.openmobilealliance.org/tech/DTD/pap_2.1.dtd\">");
				xml.AppendLine("<pap>");
				xml.AppendLine("<push-message push-id=\"" + pushId + "\" deliver-before-timestamp=\"" + deliverBefore + "\" source-reference=\"" + channelSettings.ApplicationId + "\">");
				xml.AppendLine("<address address-value=\"" + n.DeviceToken + "\"/>");
				xml.AppendLine("<quality-of-service delivery-method=\"unconfirmed\"/>");
				xml.AppendLine("</push-message>");
				xml.AppendLine("</pap>");


				c.Add (new StringContent (xml.ToString(), Encoding.UTF8, "application/xml"));


				var data = new StringBuilder ();
				data.AppendLine(n.PayloadToString());

				//if (n.BISNoType != n.JpegImage)
				//{
				data.AppendLine(n.Message);
				//}
				//else
				//{
				//    dataToSend.AppendLine(n.GetJpegImageBytes());
				//}

				c.Add (new StringContent (data.ToString(), Encoding.UTF8, n.ContentType));
			
				return PostAsync (channelSettings.SendUrl, c);
			}
		}

		BisHttpClient http = new BisHttpClient ();

		public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			var n = notification as BisNotification;

			var response = http.PostNotification (bisChannelSettings, n).Result;
			var description = string.Empty;

			var status = new BisMessageStatus
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

			BisNotificationStatus notStatus;
			Enum.TryParse(bbNotStatus, true, out notStatus);
			status.NotificationStatus = notStatus;

			if (status.NotificationStatus == BisNotificationStatus.NoAppReceivePush)
			{
				if (callback != null)
					callback(this, new SendNotificationResult(notification, false, new Exception("Device Subscription Expired")) { IsSubscriptionExpired = true });

				return;
			}

			if (status.HttpStatus == HttpStatusCode.OK
			    && status.NotificationStatus == BisNotificationStatus.RequestAcceptedForProcessing)
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
