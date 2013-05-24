using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Blackberry
{

    public class BisPushService : PushServiceBase
    {
        public BisPushService()
            : this(default(IPushChannelFactory), null, default(IPushServiceSettings))
        {
        }

        public BisPushService(BisPushChannelSettings channelSettings)
            : this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
        {
        }

        public BisPushService(BisPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
            : this(default(IPushChannelFactory), channelSettings, serviceSettings)
        {
        }

        public BisPushService(IPushChannelFactory pushChannelFactory, BisPushChannelSettings channelSettings)
            : this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
        {
        }

        public BisPushService(IPushChannelFactory pushChannelFactory, BisPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
            : base(pushChannelFactory ?? new BisPushChannelFactory(), channelSettings ?? new BisPushChannelSettings(), serviceSettings)
        {
        }
    }
    public class BisPushChannelFactory : IPushChannelFactory
    {
        public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
        {
            if (!(channelSettings is BisPushChannelSettings))
                throw new ArgumentException("channelSettings must be of type: " + typeof(BisPushChannelSettings).Name);

            return new BisPushChannel(channelSettings as BisPushChannelSettings);
        }
    }
}
