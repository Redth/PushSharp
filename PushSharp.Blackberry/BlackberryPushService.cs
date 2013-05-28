using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Blackberry
{

    public class BlackberryPushService : PushServiceBase
    {
        public BlackberryPushService()
            : this(default(IPushChannelFactory), null, default(IPushServiceSettings))
        {
        }

        public BlackberryPushService(BlackberryPushChannelSettings channelSettings)
            : this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
        {
        }

        public BlackberryPushService(BlackberryPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
            : this(default(IPushChannelFactory), channelSettings, serviceSettings)
        {
        }

        public BlackberryPushService(IPushChannelFactory pushChannelFactory, BlackberryPushChannelSettings channelSettings)
            : this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
        {
        }

        public BlackberryPushService(IPushChannelFactory pushChannelFactory, BlackberryPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
            : base(pushChannelFactory ?? new BisPushChannelFactory(), channelSettings ?? new BlackberryPushChannelSettings(), serviceSettings)
        {
        }
    }
    public class BisPushChannelFactory : IPushChannelFactory
    {
        public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
        {
            if (!(channelSettings is BlackberryPushChannelSettings))
                throw new ArgumentException("channelSettings must be of type: " + typeof(BlackberryPushChannelSettings).Name);

            return new BlackberryPushChannel(channelSettings as BlackberryPushChannelSettings);
        }
    }
}
