using System;
using System.Text;
using System.Net;

using PushSharp.Core;

namespace PushSharp.FirefoxOS
{
    /// <summary>
    /// Used for sending notification to FirefoxOS devices. 
    /// </summary>
    public class FirefoxOSPushChannel : IPushChannel
    {
        // Assembly version.
        private Version version;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirefoxOSPushChannel"/> class.
        /// </summary>
        public FirefoxOSPushChannel()
        {
            version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        }

        public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
        {
            var message = notification as FirefoxOSNotification;
            var data = Encoding.UTF8.GetBytes(message.ToString());

            var request = (HttpWebRequest)WebRequest.Create(message.EndPointUrl);

            request.Method = "PUT";
            request.ContentLength = data.Length;
            request.UserAgent = string.Format("PushSharp (version: {0})", version);

            using (var rs = request.GetRequestStream())
            {
                rs.Write(data, 0, data.Length);
            }

            try
            {
                request.BeginGetResponse(ResponseCallback, new object[] { request, message, callback });
            }
            catch (WebException ex)
            {
                callback(this, new SendNotificationResult(message, false, ex));
            }
        }

        private void ResponseCallback(IAsyncResult ar)
        {
            var state = (object[])ar.AsyncState;

            var request = (HttpWebRequest)state[0];
            var message = (FirefoxOSNotification)state[1];
            var callback = (SendNotificationCallbackDelegate)state[2];

            HttpWebResponse response = null;

            try
            {
                response = request.EndGetResponse(ar) as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent)
                {
                    callback(this, new SendNotificationResult(message));
                }
            }
            catch (WebException ex)
            {
                callback(this, new SendNotificationResult(message, false, ex));
            }
        }

        public void Dispose() { }
    }
}