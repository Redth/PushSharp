﻿using PushSharp.Common;

namespace PushSharp.Android
{
	public class GcmPushService : PushServiceBase
	{
		public GcmPushService(PushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
			: base(channelSettings, serviceSettings)
		{
		}

		protected override PushChannelBase CreateChannel(PushChannelSettings channelSettings)
		{
			return new GcmPushChannel(channelSettings as GcmPushChannelSettings);
		}
	}
}
