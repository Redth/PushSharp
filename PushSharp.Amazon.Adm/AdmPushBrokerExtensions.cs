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
		public static void RegisterAdmService(this IPushBroker broker, AdmPushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
		{
			RegisterAdmService (broker, channelSettings, null, serviceSettings);
		}

        public static void RegisterAdmService(this IPushBroker broker, AdmPushChannelSettings channelSettings, string applicationId, IPushServiceSettings serviceSettings = null)
        {
            broker.RegisterService<AdmNotification>(new AdmPushService(new AdmPushChannelFactory(), channelSettings, serviceSettings));
        }

        public static AdmNotification AdmNotification(this IPushBroker broker)
        {
            return new AdmNotification();
        }
    }
}
