using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Amazon.Adm;
using PushSharp.Core;

namespace PushSharp
{
    public static class AdmPushBrokerExtensions
    {
        public static void RegisterAdmService(this PushBroker broker, AdmPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
        {
            broker.RegisterService<AdmNotification>(new AdmPushService(new AdmPushChannelFactory(), channelSettings, serviceSettings));
        }

        public static AdmNotification AdmNotification(this PushBroker broker)
        {
            return new AdmNotification();
        }
    }
}
