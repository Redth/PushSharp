using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Common;

namespace PushSharp.WindowsPhone
{
	public class WindowsPhonePushService : PushServiceBase
	{
		public WindowsPhonePushService(PushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
			: base(channelSettings, serviceSettings)
		{
		}

		protected override PushChannelBase CreateChannel(PushChannelSettings channelSettings)
		{
			return new WindowsPhonePushChannel(channelSettings as WindowsPhonePushChannelSettings);
		}
	}
}
