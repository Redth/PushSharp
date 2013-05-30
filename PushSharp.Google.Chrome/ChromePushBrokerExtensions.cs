using PushSharp.Google.Chrome;
using PushSharp.Core;

namespace PushSharp
{
    public static class ChromePushBrokerExtensions
    {
        public static void RegisterChromeGcmService(this PushBroker broker, ChromePushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
        {
            broker.RegisterService<ChromeNotification>(new ChromePushService(channelSettings, serviceSettings));
        }

        public static ChromeNotification BlackberryNotification(this PushBroker broker)
        {
            return new ChromeNotification();
        }
    }
}
