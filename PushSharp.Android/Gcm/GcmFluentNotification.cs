﻿using System;
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

		public static GcmNotification WithJson(this GcmNotification n, string json)
		{
			try { var nobj = Newtonsoft.Json.Linq.JObject.Parse(json); }
			catch { throw new InvalidCastException("Invalid JSON detected!"); }

			n.JsonData = json;
			return n;
		}

		public static GcmNotification WithTag(this GcmNotification n, object tag)
        {
            n.Tag = tag;
            return n;
        }
	}
}
