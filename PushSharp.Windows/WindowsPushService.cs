using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
	public class WindowsPushService : Common.PushServiceBase<WindowsPushChannelSettings>
	{
		public WindowsPushService(WindowsPushChannelSettings channelSettings, Common.PushServiceSettings serviceSettings) : base(channelSettings, serviceSettings)
		{ }

		protected override Common.PushChannelBase CreateChannel(Common.PushChannelSettings channelSettings)
		{
			return new WindowsPushChannel(channelSettings as WindowsPushChannelSettings);
		}

		public override Common.PlatformType Platform
		{
			get { return Common.PlatformType.Windows; }
		}
	}
}
