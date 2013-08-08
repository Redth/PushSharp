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
		public static void RegisterAppleService(this PushBroker broker, ApplePushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
		{
			RegisterAppleService (broker, channelSettings, null, serviceSettings);
		}

		public static void RegisterAppleService(this PushBroker broker, ApplePushChannelSettings channelSettings, string applicationId, IPushServiceSettings serviceSettings = null)
		{
			broker.RegisterService<AppleNotification>(new ApplePushService(channelSettings, serviceSettings), applicationId);
		}

		public static AppleNotification AppleNotification(this PushBroker broker)
		{
			return new AppleNotification();
		}
	}
}
