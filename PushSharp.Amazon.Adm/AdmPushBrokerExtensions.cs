using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Amazon.Adm;
using PushSharp.Core;

namespace PushSharp
{
    public static class GcmPushBrokerExtensions
    {
        public static void RegisterGcmService(this PushBroker broker, AdmPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
        {
            broker.RegisterService<AdmNotification>(new AdmPushService(new AdmPushChannelFactory(), channelSettings, serviceSettings));
        }

        public static AdmNotification GcmNotification(this PushBroker broker)
        {
            return new AdmNotification();
        }
    }
}
