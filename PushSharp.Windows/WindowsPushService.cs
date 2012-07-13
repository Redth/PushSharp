using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
	public class WindowsPushService : Common.PushServiceBase
	{
		protected override Common.PushChannelBase CreateChannel(Common.PushChannelSettings channelSettings)
		{
			return new WindowsPushChannel(channelSettings as WindowsPushChannelSettings);
		}
	}
}
