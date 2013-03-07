using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Android
{
	[Obsolete("Google has Deprecated C2DM, and you should now use GCM Instead.")]
	public class C2dmPushService : PushServiceBase
	{
		public C2dmPushService(IPushChannelFactory pushChannelFactory, C2dmPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
			: base(pushChannelFactory, channelSettings, serviceSettings)
		{
		}
	}

	public class C2dmPushChannelFactory : IPushChannelFactory
	{
		public PushChannelBase CreateChannel(PushServiceBase pushService)
		{
			return new C2dmPushChannel(pushService);
		}
	}
}
