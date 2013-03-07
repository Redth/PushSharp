using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Apple;
using PushSharp.Core;

namespace PushSharp
{
	public static class ApplePushBrokerExtensions
	{
		public static void RegisterAppleService(this PushBroker broker, ApplePushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			broker.RegisterService<AppleNotification>(new ApplePushService(new ApplePushChannelFactory(), channelSettings, serviceSettings));
		}

		public static AppleNotification AppleNotification(this PushBroker broker)
		{
			return new AppleNotification();
		}
	}
}
