using System;
using System.Collections.Generic;
using System.Text;
using PushSharp.Google.Chrome;

namespace PushSharp
{
	public static class GcmFluentNotification
	{
		public static ChromeNotification ForChannelId(this ChromeNotification n, string channelId)
		{
			n.ChannelId = channelId;
			return n;
		}

		public static ChromeNotification ForSubChannelId(this ChromeNotification n, ChromeNotificationSubChannel subChannel)
		{
			n.SubChannelId = subChannel;
			return n;
		}

		public static ChromeNotification WithPayload(this ChromeNotification n, string payload)
		{
			if (payload.Length > 256)
				throw new ArgumentOutOfRangeException ("Payload length must be <= 256");

			n.Payload = payload;
			return n;
		}
	}
}
