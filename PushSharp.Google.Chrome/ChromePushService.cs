using System;
using System.Collections.Generic;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Google.Chrome
{
	public class ChromePushService : PushServiceBase
	{
		public ChromePushService(ChromePushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public ChromePushService(ChromePushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public ChromePushService(IPushChannelFactory pushChannelFactory, ChromePushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}

		public ChromePushService(IPushChannelFactory pushChannelFactory, ChromePushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new GcmPushChannelFactory(), channelSettings, serviceSettings)
		{
		}
	}

	public class GcmPushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			if (!(channelSettings is ChromePushChannelSettings))
				throw new ArgumentException("channelSettings must be of type " + typeof(ChromePushChannelSettings).ToString());

			return new ChromePushChannel(channelSettings as ChromePushChannelSettings);
		}
	}
}
