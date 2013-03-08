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
		public WindowsPushService(IWindowsPushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public WindowsPushService(IWindowsPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public WindowsPushService(IPushChannelFactory pushChannelFactory, IWindowsPushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}
		
		public WindowsPushService(IPushChannelFactory pushChannelFactory, IWindowsPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new WindowsPushChannelFactory(), channelSettings, serviceSettings)
		{ }
	}

	public class WindowsPushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			return new WindowsPushChannel(channelSettings);
		}
	}
}
