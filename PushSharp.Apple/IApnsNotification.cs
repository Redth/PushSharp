using Newtonsoft.Json.Linq;
using PushSharp.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace PushSharp.Apple
{
    public interface IApnsNotification: INotification
    {
        public int Identifier { get; }

        public string DeviceToken { get; }

        public JObject Payload { get; }

        public DateTime? Expiration { get; }

        public bool LowPriority { get; }

        public byte[] ToBytes();
    }
}
