using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Apple
{
	public static class FluentNotification
	{
		public static AppleNotification WithPayload(this AppleNotification n, string alert)
		{
			n.Payload = new AppleNotificationPayload() { Alert = alert };
			return n;
		}

		public static AppleNotification WithPayload(this AppleNotification n, int badge)
		{
			n.Payload = new AppleNotificationPayload() { Badge = badge };
			return n;
		}

		public static AppleNotification WithPayload(this AppleNotification n, string sound)
		{
			n.Payload = new AppleNotificationPayload() { Sound = sound };
			return n;
		}

	}
}
