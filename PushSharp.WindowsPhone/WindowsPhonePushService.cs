using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.WindowsPhone
{
	public class WindowsPhonePushService : PushServiceBase
	{
		public WindowsPhonePushService()
			: this(default(IPushChannelFactory), default(IWindowsPhonePushChannelSettings), default(IPushServiceSettings))
		{
		}

		public WindowsPhonePushService(IWindowsPhonePushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public WindowsPhonePushService(IWindowsPhonePushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public WindowsPhonePushService(IPushChannelFactory pushChannelFactory, IWindowsPhonePushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}

		public WindowsPhonePushService(IPushChannelFactory pushChannelFactory, IWindowsPhonePushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new WindowsPhonePushChannelFactory(), channelSettings ?? new WindowsPhonePushChannelSettings(), serviceSettings)
		{
		}
	}
	public class WindowsPhonePushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			return new WindowsPhonePushChannel(channelSettings);
		}
	}
}
