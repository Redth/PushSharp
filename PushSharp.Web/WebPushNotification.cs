using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Core;

namespace PushSharp.Web
{
    public class WebPushNotification : INotification
    {
        public WebPushNotification(WebPushSubscription subscription)
        {
            Subscription = subscription;
        }

        public bool IsDeviceRegistrationIdValid()
        {
            return true;
        }

        public object Tag { get; set; }

        public WebPushSubscription Subscription { get; }

        public JObject Payload { get; set; }

        public string GetJson()
        {
            if (Payload == null)
            {
                return string.Empty;
            }

            // Ignore null values
            return JsonConvert.SerializeObject(Payload,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}