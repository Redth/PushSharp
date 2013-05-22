using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Blackberry
{

    public class BISPushService : PushServiceBase
    {
        public BISPushService()
            : this(default(IPushChannelFactory), null, default(IPushServiceSettings))
        {
        }

        public BISPushService(BISPushChannelSettings channelSettings)
            : this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
        {
        }

        public BISPushService(BISPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
            : this(default(IPushChannelFactory), channelSettings, serviceSettings)
        {
        }

        public BISPushService(IPushChannelFactory pushChannelFactory, BISPushChannelSettings channelSettings)
            : this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
        {
        }

        public BISPushService(IPushChannelFactory pushChannelFactory, BISPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
            : base(pushChannelFactory ?? new BISPushChannelFactory(), channelSettings ?? new BISPushChannelSettings(), serviceSettings)
        {
        }
    }
    public class BISPushChannelFactory : IPushChannelFactory
    {
        public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
        {
            if (!(channelSettings is BISPushChannelSettings))
                throw new ArgumentException("channelSettings must be of type BISPushChannelSettings");

            return new BISPushChannel(channelSettings as BISPushChannelSettings);
        }
    }
}
