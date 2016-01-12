using PushSharp.Blackberry;
using PushSharp.Core;

namespace PushSharp
{
	public static class BISPushBrokerExtensions
	{
		public static void RegisterBlackberryService(this IPushBroker broker, BlackberryPushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
		{
			RegisterBlackberryService (broker, channelSettings, null, serviceSettings);
		}

		public static void RegisterBlackberryService(this IPushBroker broker, BlackberryPushChannelSettings channelSettings, string applicationId = null, IPushServiceSettings serviceSettings = null)
		{
			broker.RegisterService<BlackberryNotification>(new BlackberryPushService(channelSettings, serviceSettings), applicationId);
		}

        public static BlackberryNotification BlackberryNotification(this IPushBroker broker)
		{
            return new BlackberryNotification();
		}
	}
}
