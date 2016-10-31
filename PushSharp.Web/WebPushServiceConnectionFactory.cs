using PushSharp.Core;

namespace PushSharp.Web
{
    public class WebPushServiceConnectionFactory : IServiceConnectionFactory<WebPushNotification>
    {
        public WebPushConfiguration Configuration { get; }

        public WebPushServiceConnectionFactory(WebPushConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceConnection<WebPushNotification> Create()
        {
            return new WebPushConnection(Configuration);
        }
    }
}