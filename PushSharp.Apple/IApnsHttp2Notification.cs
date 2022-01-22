using Newtonsoft.Json.Linq;
using PushSharp.Core;
using System;

namespace PushSharp.Apple
{
    public interface IApnsHttp2Notification: INotification
    {
        public string Uuid { get; set; }

        public string DeviceToken { get; set; }

        public JObject Payload { get; set; }

        public DateTime? Expiration { get; set; }

        public ApnsPriority? Priority { get; set; }

        public string Topic { get; set; }

    }
}
