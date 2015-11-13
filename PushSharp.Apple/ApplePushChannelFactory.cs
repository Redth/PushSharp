using System;
using PushSharp.Core;

namespace PushSharp.Apple
{
	public class ApplePushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			if (!(channelSettings is ApplePushChannelSettings))
				throw new ArgumentException("Channel Settings must be of type ApplePushChannelSettings");

			return new ApplePushChannel(channelSettings as ApplePushChannelSettings);
		}
	}
}