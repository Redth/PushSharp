using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.WindowsPhone
{
	public class WindowsPhonePushService : PushServiceBase
	{
		public WindowsPhonePushService(IPushChannelFactory pushChannelFactory, WindowsPhonePushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
			: base(pushChannelFactory, channelSettings ?? new WindowsPhonePushChannelSettings(), serviceSettings)
		{
		}
	}
	public class WindowsPhonePushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushService pushService)
		{
			return new WindowsPhonePushChannel(pushService);
		}
	}
}
