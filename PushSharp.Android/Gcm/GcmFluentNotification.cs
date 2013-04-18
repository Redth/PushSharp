using System;
using System.Collections.Generic;
using PushSharp.Android;

namespace PushSharp
{
    public static class GcmFluentNotification
    {
        public static GcmNotification ForDeviceRegistrationId(this GcmNotification n, string deviceRegistrationId)
        {
            n.RegistrationIds.Add(deviceRegistrationId);
            return n;
        }

        public static GcmNotification ForDeviceRegistrationId(this GcmNotification n, IEnumerable<string> deviceRegistrationIds)
        {
            n.RegistrationIds.AddRange(deviceRegistrationIds);
            return n;
        }

        public static GcmNotification WithCollapseKey(this GcmNotification n, string collapseKey)
        {
            n.CollapseKey = collapseKey;
            return n;
        }

        public static GcmNotification WithDelayWhileIdle(this GcmNotification n, bool delayWhileIdle = false)
        {
            n.DelayWhileIdle = delayWhileIdle;
            return n;
        }

        public static GcmNotification WithTimeToLive(this GcmNotification n, int ttlSeconds)
        {
            n.TimeToLive = ttlSeconds;
            return n;
        }

        public static GcmNotification WithJson(this GcmNotification n, string json)
        {
            try { Newtonsoft.Json.Linq.JObject.Parse(json); }
            catch { throw new InvalidCastException("Invalid JSON detected!"); }

            n.JsonData = json;
            return n;
        }

        public static GcmNotification WithData(this GcmNotification n, IDictionary<String, String> data)
        {
            if (data == null)
                return n;

            Newtonsoft.Json.Linq.JObject lDataObj;
            if (!String.IsNullOrEmpty(n.JsonData))
            {
                lDataObj = Newtonsoft.Json.Linq.JObject.Parse(n.JsonData);
            }
            else
                lDataObj = new Newtonsoft.Json.Linq.JObject();

            foreach (var pair in data)
            {
                lDataObj.Add(pair.Key, new Newtonsoft.Json.Linq.JValue(pair.Value));
            }

            n.JsonData = lDataObj.ToString(Newtonsoft.Json.Formatting.None);
            return n;
        }

        public static GcmNotification WithData(this GcmNotification n, String key, String value)
        {
            return WithData(n, new Dictionary<String, String>() { { key, value } });
        }

        public static GcmNotification WithTag(this GcmNotification n, object tag)
        {
            n.Tag = tag;
            return n;
        }

        public static GcmNotification WithDryRun(this GcmNotification n)
        {
            n.DryRun = true;
            return n;
        }
    }
}
