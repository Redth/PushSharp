using PushSharp.Google.Chrome;
using PushSharp.Core;

namespace PushSharp
{
    public static class ChromePushBrokerExtensions
    {
		public static void RegisterChromeGcmService(this PushBroker broker, ChromePushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
		{
			RegisterChromeGcmService (broker, channelSettings, null, serviceSettings);
		}

        public static void RegisterChromeGcmService(this PushBroker broker, ChromePushChannelSettings channelSettings, string applicationId = null, IPushServiceSettings serviceSettings = null)
        {
            broker.RegisterService<ChromeNotification>(new ChromePushService(channelSettings, serviceSettings), applicationId);
        }

        public static ChromeNotification ChromeNotification(this PushBroker broker)
        {
            return new ChromeNotification();
        }
    }
}
