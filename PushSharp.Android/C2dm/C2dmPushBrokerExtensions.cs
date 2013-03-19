using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Android;
using PushSharp.Core;

namespace PushSharp
{
	public static class C2dmPushBrokerExtensions
	{
		public static void RegisterC2dmService(this PushBroker broker, C2dmPushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
		{
			broker.RegisterService<C2dmNotification>(new C2dmPushService(channelSettings, serviceSettings));
		}

		public static C2dmNotification C2dmNotification(this PushBroker broker)
		{
			return new C2dmNotification();
		}
	}
}
