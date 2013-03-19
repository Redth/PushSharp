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
		public C2dmPushService(C2dmPushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public C2dmPushService(C2dmPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public C2dmPushService(IPushChannelFactory pushChannelFactory, C2dmPushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}

		public C2dmPushService(IPushChannelFactory pushChannelFactory, C2dmPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new C2dmPushChannelFactory(), channelSettings, serviceSettings)
		{
		}
	}

	public class C2dmPushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			if (!(channelSettings is C2dmPushChannelSettings))
				throw new ArgumentException("channelSettings must be of type C2dmPushChannelSettings");

			return new C2dmPushChannel(channelSettings as C2dmPushChannelSettings);
		}
	}
}
