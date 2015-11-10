using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Android;

namespace PushSharp
{
    public static class GcmCCSFluentNotification
	{
        public static GcmCCSNotification ForDeviceRegistrationId(this GcmCCSNotification n, string deviceRegistrationId)
		{
            n.To = deviceRegistrationId;
			return n;
		}

        public static GcmCCSNotification WithCollapseKey(this GcmCCSNotification n, string collapseKey)
		{
			n.CollapseKey = collapseKey;
			return n;
		}

        public static GcmCCSNotification WithDelayWhileIdle(this GcmCCSNotification n, bool delayWhileIdle = false)
		{
			n.DelayWhileIdle = delayWhileIdle;
			return n;
		}

        public static GcmCCSNotification WithTimeToLive(this GcmCCSNotification n, int ttlSeconds)
        {
            n.TimeToLive = ttlSeconds;
            return n;
        }

        public static GcmCCSNotification WithDeliveryReceiptRequest(this GcmCCSNotification n,bool requestDeliverRecipt)
        {
            n.RequestDeliveryReceipt = requestDeliverRecipt;
            return n;
        }

        public static GcmCCSNotification WithJson(this GcmCCSNotification n, string json)
		{
			try { Newtonsoft.Json.Linq.JObject.Parse(json); }
			catch { throw new InvalidCastException("Invalid JSON detected!"); }

			n.JsonData = json;
			return n;
		}

        public static GcmCCSNotification WithData(this GcmCCSNotification n, IDictionary<string, string> data)
		{
			if (data == null)
				return n;

			var json = new Newtonsoft.Json.Linq.JObject();

			try
			{
				if (!String.IsNullOrEmpty (n.JsonData))
					json = Newtonsoft.Json.Linq.JObject.Parse (n.JsonData);
			}
			catch { } 

			foreach (var pair in data)
				json.Add (pair.Key, new Newtonsoft.Json.Linq.JValue (pair.Value));

			n.JsonData = json.ToString (Newtonsoft.Json.Formatting.None);

			return n;
		}

        public static GcmCCSNotification WithTag(this GcmCCSNotification n, object tag)
        {
            n.Tag = tag;
            return n;
        }

        public static GcmCCSNotification WithDryRun(this GcmCCSNotification n)
		{
			n.DryRun = true;
			return n;
		}
	}
}
