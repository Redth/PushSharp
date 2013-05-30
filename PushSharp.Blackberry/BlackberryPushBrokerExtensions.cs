using PushSharp.Blackberry;
using PushSharp.Core;

namespace PushSharp
{
	public static class BISPushBrokerExtensions
	{
		public static void RegisterBlackberryService(this PushBroker broker, BlackberryPushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
		{
			broker.RegisterService<BlackberryNotification>(new BlackberryPushService(channelSettings, serviceSettings));
		}

        public static BlackberryNotification BlackberryNotification(this PushBroker broker)
		{
            return new BlackberryNotification();
		}
	}
}
