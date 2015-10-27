using System;
using PushSharp.Core;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PushSharp.Google
{
    public class GcmXmppNotification : INotification
    {
        public GcmXmppNotification ()
        {
            MessageId = "m-" + Guid.NewGuid ().ToString ("N");
            CollapseKey = string.Empty;
            Data = null;
            DelayWhileIdle = null;
        }

        public bool IsDeviceRegistrationIdValid ()
        {
            return !string.IsNullOrEmpty (To);
        }

        [JsonIgnore]
        public object Tag { get;set; }

        [JsonProperty  ("message_id")]
        public string MessageId { get;set; }

        /// <summary>
        /// Registration ID or Group/Topic to send notification to.  Overrides RegsitrationIds.
        /// </summary>
        /// <value>To.</value>
        [JsonProperty ("to")]
        public string To { get;set; }

        /// <summary>
        /// Only the latest message with the same collapse key will be delivered
        /// </summary>
        [JsonProperty ("collapse_key")]
        public string CollapseKey { get; set; }

        /// <summary>
        /// JSON Payload to be sent in the message
        /// </summary>
        [JsonProperty ("data")]
        public JObject Data { get; set; }

        /// <summary>
        /// Notification JSON payload
        /// </summary>
        /// <value>The notification payload.</value>
        [JsonProperty ("notification")]
        public JObject Notification { get; set; }

        [JsonProperty ("delivery_receipt_requested")]
        public bool? DeliveryReceiptRequested { get; set; }

        /// <summary>
        /// If true, GCM will only be delivered once the device's screen is on
        /// </summary>
        [JsonProperty ("delay_while_idle")]
        public bool? DelayWhileIdle { get; set; }

        /// <summary>
        /// Time in seconds that a message should be kept on the server if the device is offline.  Default (and maximum) is 4 weeks.
        /// </summary>
        [JsonProperty ("time_to_live")]
        public int? TimeToLive { get; set; }

        /// <summary>
        /// If true, dry_run attribute will be sent in payload causing the notification not to be actually sent, but the result returned simulating the message
        /// </summary>
        [JsonProperty ("dry_run")]
        public bool? DryRun { get; set; }

        /// <summary>
        /// A string that maps a single user to multiple registration IDs associated with that user. This allows a 3rd-party server to send a single message to multiple app instances (typically on multiple devices) owned by a single user.
        /// </summary>
        [Obsolete ("Deprecated on GCM Server API.  Use Device Group Messaging to send to multiple devices.")]
        public string NotificationKey { get; set; }

        /// <summary>
        /// A string containing the package name of your application. When set, messages will only be sent to registration IDs that match the package name
        /// </summary>
        [JsonProperty ("restricted_package_name")]
        public string RestrictedPackageName { get; set; }

        /// <summary>
        /// On iOS, use this field to represent content-available in the APNS payload. When a notification or message is sent and this is set to true, an inactive client app is awoken. On Android, data messages wake the app by default. On Chrome, currently not supported.
        /// </summary>
        /// <value>The content available.</value>
        [JsonProperty ("content_available")]
        public bool? ContentAvailable { get; set; }

        /// <summary>
        /// Corresponds to iOS APNS priorities (Normal is 5 and high is 10).  Default is Normal.
        /// </summary>
        /// <value>The priority.</value>
        [JsonProperty ("priority")]
        public GcmNotificationPriority? Priority { get; set; }

        internal string GetJson ()
        {
            return JsonConvert.SerializeObject (this);
        }

        public override string ToString ()
        {
            return GetJson ();
        }


        public string ToJson ()
        {
            return JsonConvert.SerializeObject (this);
        }
    }
}

