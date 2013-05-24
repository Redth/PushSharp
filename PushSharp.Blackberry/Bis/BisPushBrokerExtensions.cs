using PushSharp.Blackberry;
using PushSharp.Core;

namespace PushSharp
{
	public static class BISPushBrokerExtensions
	{
		public static void RegisterBISService(this PushBroker broker, BisPushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
		{
			broker.RegisterService<BisNotification>(new BisPushService(channelSettings, serviceSettings));
		}

        public static BisNotification BlackberryNotification(this PushBroker broker)
		{
            return new BisNotification();
		}
	}
}
