using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Apple;
using PushSharp.Core;

namespace PushSharp
{
	public static class PushBrokerExtensions
	{
		public static void RegisterAppleService(this PushNotificationBroker broker, ApplePushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			broker.RegisterService<AppleNotification, ApplePushService>(new ApplePushService(new ApplePushChannelFactory(), channelSettings,
			                                                               serviceSettings));

		}

		public static AppleNotification AppleNotification(this PushNotificationBroker broker)
		{
			return new AppleNotification();
		}
	}
}
