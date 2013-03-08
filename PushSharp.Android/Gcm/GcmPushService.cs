using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Android
{
	public class GcmPushService : PushServiceBase
	{
		public GcmPushService(IGcmPushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public GcmPushService(IGcmPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public GcmPushService(IPushChannelFactory pushChannelFactory, IGcmPushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}
		
		public GcmPushService(IPushChannelFactory pushChannelFactory, IGcmPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new GcmPushChannelFactory(), channelSettings, serviceSettings)
		{
		}
	}

	public class GcmPushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			return new GcmPushChannel(channelSettings);
		}
	}
}
