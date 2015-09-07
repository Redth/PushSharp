using Newtonsoft.Json.Linq;
using PushSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Android
{
    public class GcmCCSNotification : Notification
    {
        //public static GcmCCSNotification ForSingleResult(GcmCCSMessageTransportResponse response, int resultIndex)
        //{
        //    var result = new GcmCCSNotification();
        //    result.Tag = response.Message.Tag;
        //    result.RegistrationIds.Add(response.Message.RegistrationIds[resultIndex]);
        //    result.CollapseKey = response.Message.CollapseKey;
        //    result.JsonData = response.Message.JsonData;
        //    result.DelayWhileIdle = response.Message.DelayWhileIdle;
        //    return result;
        //}

        public static GcmCCSNotification ForSingleRegistrationId(GcmCCSNotification msg, string registrationId)
        {
            var result = new GcmCCSNotification(msg.MessageID);
            result.Tag = msg.Tag;
            result.To = registrationId;
            result.CollapseKey = msg.CollapseKey;
            result.JsonData = msg.JsonData;
            result.DelayWhileIdle = msg.DelayWhileIdle;
            return result;
        }

        public GcmCCSNotification(string messageID)
        {
            this.MessageID = messageID;
            this.CollapseKey = string.Empty;
            this.JsonData = string.Empty;
            this.DelayWhileIdle = null;
        }
        public GcmCCSNotification()
        {
            this.MessageID = Guid.NewGuid().ToString();
            this.CollapseKey = string.Empty;
            this.JsonData = string.Empty;
            this.DelayWhileIdle = null;
        }
        public string To { get; set; }

        public string MessageID { get; set; }
        /// <summary>
        /// Only the latest message with the same collapse key will be delivered
        /// </summary>
        public string CollapseKey
        {
            get;
            set;
        }

        /// <summary>
        /// JSON Payload to be sent in the message
        /// </summary>
        public string JsonData
        {
            get;
            set;
        }

        /// <summary>
        /// If true, GCM will only be delivered once the device's screen is on
        /// </summary>
        public bool? DelayWhileIdle
        {
            get;
            set;
        }

        /// <summary>
        /// Time in seconds that a message should be kept on the server if the device is offline.  Default is 4 weeks.
        /// </summary>
        public int? TimeToLive
        {
            get;
            set;
        }

        /// <summary>
        /// If true, dry_run attribute will be sent in payload causing the notification not to be actually sent, but the result returned simulating the message
        /// </summary>
        public bool? DryRun
        {
            get;
            set;
        }

        /// <summary>
        /// A string that maps a single user to multiple registration IDs associated with that user. This allows a 3rd-party server to send a single message to multiple app instances (typically on multiple devices) owned by a single user.
        /// </summary>
        public string NotificationKey { get; set; }

        /// <summary>
        /// A string containing the package name of your application. When set, messages will only be sent to registration IDs that match the package name
        /// </summary>
        public string TargetPackageName { get; set; }


        internal string GetJson()
        {
            var json = new JObject();

            if (!string.IsNullOrEmpty(this.CollapseKey))
                json["collapse_key"] = this.CollapseKey;

            if (this.TimeToLive.HasValue)
                json["time_to_live"] = this.TimeToLive.Value;

            //json["registration_ids"] = new JArray(this.RegistrationIds.ToArray());
            json["to"] = this.To;

            json["message_id"] = this.MessageID;

            if (this.DelayWhileIdle.HasValue)
                json["delay_while_idle"] = this.DelayWhileIdle.Value;

            if (DryRun.HasValue && DryRun.Value)
                json["dry_run"] = true;

            if (!string.IsNullOrEmpty(this.JsonData))
            {
                var jsonData = JObject.Parse(this.JsonData);

                if (jsonData != null)
                    json["data"] = jsonData;
            }

            if (!string.IsNullOrWhiteSpace(this.NotificationKey))
                json["notification_key"] = NotificationKey;

            if (!string.IsNullOrWhiteSpace(this.TargetPackageName))
                json["restricted_package_name"] = TargetPackageName;


            return json.ToString();
        }

        public override string ToString()
        {
            return GetJson();
        }


    }
}
