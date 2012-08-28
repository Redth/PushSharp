using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Android;

namespace PushSharp
{
	public static class C2dmFluentNotification
	{
		public static C2dmNotification ForDeviceRegistrationId(this C2dmNotification n, string deviceRegistrationId)
		{
			n.RegistrationId = deviceRegistrationId;
			return n;
		}

		public static C2dmNotification WithCollapseKey(this C2dmNotification n, string collapseKey)
		{
			n.CollapseKey = collapseKey;
			return n;
		}

		public static C2dmNotification WithDelayWhileIdle(this C2dmNotification n, bool delayWhileIdle = false)
		{
			n.DelayWhileIdle = delayWhileIdle;
			return n;
		}

		public static C2dmNotification WithData(this C2dmNotification n, string key, string value)
		{
			if (n.Data == null)
				n.Data = new System.Collections.Specialized.NameValueCollection();

			n.Data.Add(key, value);
			return n;
		}

        public static C2dmNotification WithTag(this C2dmNotification n, object tag)
        {
            n.Tag = tag;

            return n;
        }
	}
}
