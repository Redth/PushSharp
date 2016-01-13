using System;
using PushSharp.Core;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Collections.Generic;

namespace PushSharp.Apple
{
    public class ApnsHttp2Notification : INotification
    {                
        /// <summary>
        /// Store whatever associated information you'd like here
        /// </summary>
        /// <value>The tag.</value>
        public object Tag { get; set; }

        /// <summary>
        /// Unique Identifier to match server responses with
        /// </summary>
        /// <value>The UUID.</value>
        public string Uuid { get; set; }

        /// <summary>
        /// Device Token to send notifications to
        /// </summary>
        /// <value>The device token.</value>
        public string DeviceToken { get; set; }

        /// <summary>
        /// JSON APNS Payload
        /// </summary>
        /// <value>The payload.</value>
        public JObject Payload { get; set; }

        /// <summary>
        /// The expiration date after which Apple will no longer store and forward this push notification.
        /// If no value is provided, an assumed value of one year from now is used.  If you do not wish
        /// for Apple to store and forward, set this value to Notification.DoNotStore.
        /// </summary>
        public DateTime? Expiration { get; set; }

        public ApnsPriority? Priority { get; set; }

        public string Topic { get;set; }

        public const int MAX_PAYLOAD_SIZE = 4096;
        public static readonly DateTime DoNotStore = DateTime.MinValue;

        public ApnsHttp2Notification () : this (string.Empty, new JObject ())
        {
        }

        public ApnsHttp2Notification (string deviceToken) : this (deviceToken, new JObject ())
        {
        }

        public ApnsHttp2Notification (string deviceToken, JObject payload)
        {
            DeviceToken = deviceToken;
            Payload = payload;

            Uuid = Guid.NewGuid ().ToString ("D");
        }

        public bool IsDeviceRegistrationIdValid ()
        {
            var r = new System.Text.RegularExpressions.Regex (@"^[0-9A-F]+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return r.Match (DeviceToken).Success;
        }

        public override string ToString ()
        {
            try { 
                if (Payload != null)
                    return Payload.ToString ();
            } catch {
            }

            return "{}";
        }
    }

    public enum ApnsPriority
    {
        Low = 5,
        High = 10
    }
}
