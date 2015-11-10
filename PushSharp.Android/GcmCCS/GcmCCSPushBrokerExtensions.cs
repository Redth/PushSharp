using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Android;
using PushSharp.Core;

namespace PushSharp
{
	public static class GcmCCSPushBrokerExtensions
	{
		public static void RegisterGcmCCSService(this PushBroker broker, GcmCCSPushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
		{
			RegisterGcmCCSService (broker, channelSettings, null, serviceSettings);
		}

        public static void RegisterGcmCCSService(this PushBroker broker, GcmCCSPushChannelSettings channelSettings, string applicationId, IPushServiceSettings serviceSettings = null)
		{
            broker.RegisterService<GcmCCSNotification>(new GcmCCSPushService(new GcmCCSPushChannelFactory(), channelSettings, serviceSettings), applicationId);
		}

		public static GcmCCSNotification GcmNotification(this PushBroker broker)
		{
            return new GcmCCSNotification();
		}
	}
}
