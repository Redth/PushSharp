using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Android
{
	public class GcmPushService : PushServiceBase
	{
		public GcmPushService(GcmPushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public GcmPushService(GcmPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public GcmPushService(IPushChannelFactory pushChannelFactory, GcmPushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}

		public GcmPushService(IPushChannelFactory pushChannelFactory, GcmPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new GcmPushChannelFactory(), channelSettings, serviceSettings)
		{
		}
	}

	public class GcmPushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			if (!(channelSettings is GcmPushChannelSettings))
				throw new ArgumentException("channelSettings must be of type GcmPushChannelSettings");

			return new GcmPushChannel(channelSettings as GcmPushChannelSettings);
		}
	}
}
