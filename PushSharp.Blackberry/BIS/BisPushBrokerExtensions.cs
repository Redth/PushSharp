using PushSharp.Blackberry;
using PushSharp.Core;

namespace PushSharp
{
	public static class BISPushBrokerExtensions
	{
		public static void RegisterBISService(this PushBroker broker, BISPushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
		{
			broker.RegisterService<BISNotification>(new BISPushService(channelSettings, serviceSettings));
		}

        public static BISNotification BlackberryNotification(this PushBroker broker)
		{
            return new BISNotification();
		}
	}
}
