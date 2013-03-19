using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushSharp.Core;

namespace PushSharp.Windows
{
	public class WindowsPushService : PushServiceBase
	{
		public WindowsPushService(WindowsPushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public WindowsPushService(WindowsPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public WindowsPushService(IPushChannelFactory pushChannelFactory, WindowsPushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}

		public WindowsPushService(IPushChannelFactory pushChannelFactory, WindowsPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new WindowsPushChannelFactory(), channelSettings, serviceSettings)
		{ }
	}

	public class WindowsPushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			if (!(channelSettings is WindowsPushChannelSettings))
				throw new ArgumentException("channelSettings must be of type WindowsPushChannelSettings");

			return new WindowsPushChannel(channelSettings as WindowsPushChannelSettings);
		}
	}
}
