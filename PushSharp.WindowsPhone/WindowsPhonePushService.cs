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
			: this(default(IPushChannelFactory), null, default(IPushServiceSettings))
		{
		}

		public WindowsPhonePushService(WindowsPhonePushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public WindowsPhonePushService(WindowsPhonePushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public WindowsPhonePushService(IPushChannelFactory pushChannelFactory, WindowsPhonePushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}

		public WindowsPhonePushService(IPushChannelFactory pushChannelFactory, WindowsPhonePushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new WindowsPhonePushChannelFactory(), channelSettings ?? new WindowsPhonePushChannelSettings(), serviceSettings)
		{
		}
	}
	public class WindowsPhonePushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			if (!(channelSettings is WindowsPhonePushChannelSettings))
				throw new ArgumentException("channelSettings must be of type WindowsPhonePushChannelSettings");

			return new WindowsPhonePushChannel(channelSettings as WindowsPhonePushChannelSettings);
		}
	}
}
