using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Common;

namespace PushSharp.WindowsPhone
{
	public class WindowsPhonePushService : PushServiceBase<WindowsPhonePushChannelSettings>
	{
		public WindowsPhonePushService(WindowsPhonePushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
			: base(channelSettings ?? new WindowsPhonePushChannelSettings(), serviceSettings)
		{
		}

		protected override PushChannelBase CreateChannel(PushChannelSettings channelSettings)
		{
			return new WindowsPhonePushChannel(channelSettings as WindowsPhonePushChannelSettings);
		}

		public override PlatformType Platform
		{
			get { return PlatformType.WindowsPhone; }
		}
	}
}
