using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Facebook;
using PushSharp.Core;

namespace PushSharp
{
	public static class FacebookPushBrokerExtensions
	{
		public static void RegisterFacebookService(this PushBroker broker, FacebookPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			broker.RegisterService<FacebookNotification>(new FacebookPushService(new FacebookPushChannelFactory(), channelSettings, serviceSettings));
		}

		public static FacebookNotification FacebookNotification(this PushBroker broker)
		{
			return new FacebookNotification();
		}
	}
}
