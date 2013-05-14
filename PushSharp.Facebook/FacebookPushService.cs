using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Facebook
{
	public class FacebookPushService : PushServiceBase
	{
		public FacebookPushService(FacebookPushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public FacebookPushService(FacebookPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public FacebookPushService(IPushChannelFactory pushChannelFactory, FacebookPushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}

		public FacebookPushService(IPushChannelFactory pushChannelFactory, FacebookPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new FacebookPushChannelFactory(), channelSettings, serviceSettings)
		{
		}
	}

	public class FacebookPushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			if (!(channelSettings is FacebookPushChannelSettings))
				throw new ArgumentException("channelSettings must be of type FacebookPushChannelSettings");

			return new FacebookPushChannel(channelSettings as FacebookPushChannelSettings);
		}
	}
}
