using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
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

            http = new BlackberryHttpClient(bisChannelSettings);
        }

		public class BlackberryHttpClient : HttpClient
		{
		    private BlackberryPushChannelSettings channelSettings;

			public BlackberryHttpClient(BlackberryPushChannelSettings channelSettings) : base()
			{
			    this.channelSettings = channelSettings;

                var authInfo = channelSettings.ApplicationId + ":" + channelSettings.Password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                this.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authInfo);
			    this.DefaultRequestHeaders.ConnectionClose = true;

			    this.DefaultRequestHeaders.Remove("connection");
			}

			public Task<HttpResponseMessage> PostNotification(BlackberryPushChannelSettings channelSettings, BlackberryNotification n)
			{
				var c = new MultipartContent ("related", channelSettings.Boundary);
                c.Headers.Remove("Content-Type");
                c.Headers.TryAddWithoutValidation("Content-Type", "multipart/related; boundary=" + channelSettings.Boundary + "; type=application/xml");
				
				var xml = n.ToPapXml ();


				c.Add (new StringContent (xml, Encoding.UTF8, "application/xml"));

			    var bc = new ByteArrayContent(n.Content.Content);
                bc.Headers.Add("Content-Type", n.Content.ContentType);
                
                foreach (var header in n.Content.Headers)
                    bc.Headers.Add(header.Key, header.Value);

                c.Add(bc);
			
				return PostAsync (channelSettings.SendUrl, c);
			}
		}

        private BlackberryHttpClient http;

		public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			var n = notification as BlackberryNotification;

		    try
		    {
		        var response = http.PostNotification(bisChannelSettings, n).Result;
		        var description = string.Empty;

		        var status = new BlackberryMessageStatus
		            {
		                Notification = n,
		                HttpStatus = HttpStatusCode.ServiceUnavailable
		            };

		        var bbNotStatus = string.Empty;
		        status.HttpStatus = response.StatusCode;

		        var doc = XDocument.Load(response.Content.ReadAsStreamAsync().Result);

		        XElement result = doc.Descendants().FirstOrDefault(desc =>
                                                                   desc.Name == "response-result" ||
                                                                   desc.Name == "badmessage-response");
		        if (result != null)
		        {
		            bbNotStatus = result.Attribute("code").Value;
		            description = result.Attribute("desc").Value;
		        }
		        

		        BlackberryNotificationStatus notStatus;
		        Enum.TryParse(bbNotStatus, true, out notStatus);
		        status.NotificationStatus = notStatus;

		        if (status.NotificationStatus == BlackberryNotificationStatus.NoAppReceivePush)
		        {
		            if (callback != null)
		                callback(this,
		                         new SendNotificationResult(notification, false, new Exception("Device Subscription Expired"))
		                             {
		                                 IsSubscriptionExpired = true
		                             });

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
		            callback(this,
		                     new SendNotificationResult(status.Notification, false,
		                                                new BisNotificationSendFailureException(status, description)));
		    }
		    catch (Exception ex)
		    {
		        if (callback != null)
                    callback(this, new SendNotificationResult(notification, false, ex));
		    }
		}

        public void Dispose()
        {
        }
    }
}
