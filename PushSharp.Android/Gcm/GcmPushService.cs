using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Android
{
	public class GcmPushService : PushServiceBase
	{
		public GcmPushService(IPushChannelFactory pushChannelFactory, GcmPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
			: base(pushChannelFactory, channelSettings, serviceSettings)
		{
		}
	}

	public class GcmPushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushService pushService)
		{
			return new GcmPushChannel(pushService);
		}
	}
}
