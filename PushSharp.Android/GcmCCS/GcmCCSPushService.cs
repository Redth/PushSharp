using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Android
{
	public class GcmCCSPushService : PushServiceBase
	{
        public GcmCCSPushService(GcmCCSPushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

        public GcmCCSPushService(GcmCCSPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

        public GcmCCSPushService(IPushChannelFactory pushChannelFactory, GcmCCSPushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}

        public GcmCCSPushService(IPushChannelFactory pushChannelFactory, GcmCCSPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new GcmPushChannelFactory(), channelSettings, serviceSettings)
		{

		}

        
	}

	public class GcmCCSPushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			if (!(channelSettings is GcmCCSPushChannelSettings))
				throw new ArgumentException("channelSettings must be of type " + typeof(GcmPushChannelSettings).Name);

			return new GcmCCSPushChannel(channelSettings as GcmCCSPushChannelSettings);
		}
	}
}
