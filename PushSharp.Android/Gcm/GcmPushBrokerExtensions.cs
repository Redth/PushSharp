using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Android;
using PushSharp.Core;

namespace PushSharp
{
	public static class GcmPushBrokerExtensions
	{
		public static void RegisterGcmService(this PushBroker broker, GcmPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			broker.RegisterService<GcmNotification>(new GcmPushService(new GcmPushChannelFactory(), channelSettings, serviceSettings));
		}

		public static GcmNotification GcmNotification(this PushBroker broker)
		{
			return new GcmNotification();
		}
	}
}
