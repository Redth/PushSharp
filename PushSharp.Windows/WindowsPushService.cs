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
		public WindowsPushService(IPushChannelFactory pushChannelFactory, WindowsPushChannelSettings channelSettings, PushServiceSettings serviceSettings)
			: base(pushChannelFactory, channelSettings, serviceSettings)
		{ }
	}

	public class WindowsPushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushService pushService)
		{
			return new WindowsPushChannel(pushService);
		}
	}
}
