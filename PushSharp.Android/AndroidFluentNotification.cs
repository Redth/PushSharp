using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Android;

namespace PushSharp
{
	public static class AndroidFluentNotification
	{
		public static AndroidNotification ForDeviceRegistrationId(this AndroidNotification n, string deviceRegistrationId)
		{
			n.RegistrationId = deviceRegistrationId;
			return n;
		}

		public static AndroidNotification WithCollapseKey(this AndroidNotification n, string collapseKey)
		{
			n.CollapseKey = collapseKey;
			return n;
		}

		public static AndroidNotification WithDelayWhileIdle(this AndroidNotification n, bool delayWhileIdle = false)
		{
			n.DelayWhileIdle = delayWhileIdle;
			return n;
		}

		public static AndroidNotification WithData(this AndroidNotification n, string key, string value)
		{
			if (n.Data == null)
				n.Data = new System.Collections.Specialized.NameValueCollection();

			n.Data.Add(key, value);
			return n;
		}
	}
}
