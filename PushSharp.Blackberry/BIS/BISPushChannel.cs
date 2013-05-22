using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using PushSharp.Core;

namespace PushSharp.Blackberry
{
    public class BISPushChannel : IPushChannel
    {
        BISPushChannelSettings BisSettings;

        public BISPushChannel(BISPushChannelSettings channelSettings)
        {
            BisSettings = channelSettings;
        }

        public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
        {
            var bbNotification = notification as BISNotification;
            var bbr = WebRequest.Create(BisSettings.BbUrl) as HttpWebRequest;

            if (bbNotification == null || bbr == null)
            {
                return;
            }

            bbr.Method = "POST";
            bbr.Accept = "text/html, image/gif, image/jpeg, *; q=.2, */*; q=.2";
            bbr.KeepAlive = true;
            bbr.PreAuthenticate = true;
            bbr.ContentType = "multipart/related; boundary=" + BisSettings.Boundary + "; type=application/xml";

            var authInfo = BisSettings.BbApplicationId + ":" + BisSettings.PushPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            bbr.Headers.Add("Authorization", "Basic " + authInfo);

            var dataToSend = new StringBuilder();

            dataToSend.AppendLine("--" + BisSettings.Boundary);
            dataToSend.AppendLine("Content-Type: application/xml; charset=UTF-8");

            dataToSend.AppendLine("");
            dataToSend.AppendLine("<?xml version=\"1.0\"?>");
            dataToSend.AppendLine("<!DOCTYPE pap PUBLIC \"-//WAPFORUM//DTD PAP 2.1//EN\" \"http://www.openmobilealliance.org/tech/DTD/pap_2.1.dtd\">");
            dataToSend.AppendLine("<pap>");
            var pushId = DateTime.Now.ToFileTime().ToString(CultureInfo.InvariantCulture);
            var deliverBefore = DateTime.UtcNow.AddMinutes(5).ToString("s", CultureInfo.InvariantCulture) + "Z";
            dataToSend.AppendLine("<push-message push-id=\"" + pushId + "\" deliver-before-timestamp=\"" + deliverBefore + "\" source-reference=\"" + BisSettings.BbApplicationId + "\">");
            dataToSend.AppendLine("<address address-value=\"" + bbNotification.DeviceToken + "\"/>");
            dataToSend.AppendLine("<quality-of-service delivery-method=\"unconfirmed\"/>");
            dataToSend.AppendLine("</push-message>");
            dataToSend.AppendLine("</pap>");
            dataToSend.AppendLine("--" + BisSettings.Boundary);

            

            dataToSend.AppendLine("Content-Type: " + bbNotification.BISContentType);

            dataToSend.AppendLine("Push-Message-ID: " + pushId);

            //Custom payload goes here,I can add as many payload items as I want
            //but I have to put each item in a new line i.e I used profileId
            dataToSend.AppendLine(bbNotification.PayLoadToString());

           
            //if (bbNotification.BISNoType != BISNotificationType.JpegImage)
            //{
                dataToSend.AppendLine(bbNotification.Message);
            //}
            //else
            //{
            //    dataToSend.AppendLine(bbNotification.GetJpegImageBytes());
            //}

            dataToSend.AppendLine("--" + BisSettings.Boundary + "--");
            dataToSend.AppendLine("");

            var data = Encoding.UTF8.GetBytes(dataToSend.ToString());
            bbr.ContentLength = data.Length;

            using (var rs = bbr.GetRequestStream())
            {
                rs.Write(data, 0, data.Length);
            }

            try
            {
                bbr.BeginGetResponse(GetResponseCallback, new object[] { bbr, bbNotification, callback });
            }
            catch (WebException wex)
            {
                //    Handle different statuses
                string desc = string.Empty;
                var status = new BISMessageStatus();
                ParseStatus(wex.Response as HttpWebResponse, bbNotification, ref status, ref desc);
                HandleStatus(callback, status, desc, bbNotification);
            }
        }


        void GetResponseCallback(IAsyncResult asyncResult)
        {
            //Check these references:
            //http://docs.blackberry.com/en/developers/deliverables/51382/pap_messages_implemented_1254561_11.jsp
            //http://docs.blackberry.com/en/developers/deliverables/51382/pap_ref_response-result_603623_11.jsp
            //Expected response-result codes are found in this xml
            //http://docs.blackberry.com/en/developers/deliverables/51382/pap_response_status_codes_607260_11.xml

            var objs = (object[])asyncResult.AsyncState;

            var wr = (HttpWebRequest)objs[0];
            var bbNotification = (BISNotification)objs[1];
            var callback = (SendNotificationCallbackDelegate)objs[2];

            HttpWebResponse resp = null;

            try
            {
                resp = wr.EndGetResponse(asyncResult) as HttpWebResponse;
            }
            catch (WebException webEx)
            {
                resp = webEx.Response as HttpWebResponse;
            }
            catch { }

            var desc = string.Empty;
            var status = new BISMessageStatus();
            ParseStatus(resp, bbNotification, ref status, ref desc);
            HandleStatus(callback, status, desc, bbNotification);

        }

        void ParseStatus(HttpWebResponse resp, BISNotification notification, ref BISMessageStatus status, ref string desc)
        {
            status = new BISMessageStatus
                {
                    Notification = notification,
                    HttpStatus = HttpStatusCode.ServiceUnavailable
                };

            var bbNotStatus = string.Empty;
            if (resp != null)
            {
                status.HttpStatus = resp.StatusCode;

                using (var responseStream = resp.GetResponseStream())
                {
                    var doc = XDocument.Load(responseStream);
                    var result = doc.Descendants("response-result").SingleOrDefault();
                    if (result != null)
                    {
                        bbNotStatus = result.Attribute("code").Value;
                        desc = result.Attribute("desc").Value;
                    }
                    else
                    {
                        result = doc.Descendants("badmessage-response").SingleOrDefault();
                        if (result != null)
                        {
                            bbNotStatus = result.Attribute("code").Value;
                            desc = result.Attribute("desc").Value;
                        }
                    }

                }
            }

            BISNotificationStatus notStatus;
            Enum.TryParse(bbNotStatus, true, out notStatus);
            status.NotificationStatus = notStatus;

        }

        void HandleStatus(SendNotificationCallbackDelegate callback, BISMessageStatus status, string desc, BISNotification notification = null)
        {
            if (status.NotificationStatus == BISNotificationStatus.NoAppReceivePush)
            {
                if (callback != null)
                    callback(this, new SendNotificationResult(notification, false, new Exception("Device Subscription Expired")) { IsSubscriptionExpired = true });

                return;
            }

            if (status.HttpStatus == HttpStatusCode.OK
                && status.NotificationStatus == BISNotificationStatus.RequestAcceptedForProcessing)
            {
                if (callback != null)
                    callback(this, new SendNotificationResult(notification));
                return;
            }

            if (callback != null)
                callback(this, new SendNotificationResult(status.Notification, false, new BISNotificationSendFailureException(status, desc)));
        }

        public void Dispose()
        {
        }
    }
}
