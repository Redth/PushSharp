using PushSharp.Core;

namespace PushSharp.Web
{
    public class WebPushServiceBroker : ServiceBroker<WebPushNotification>
    {
        public WebPushServiceBroker(WebPushConfiguration configuration) : base(new WebPushServiceConnectionFactory(configuration))
        {
        }
    }
}