using System;
using System.Net.Http;
using PushSharp.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PushSharp.Amazon.Adm
{
	public class AdmPushService : PushServiceBase
	{
		public AdmPushService(AdmPushChannelSettings channelSettings)
			: this(default(IPushChannelFactory), channelSettings, default(IPushServiceSettings))
		{
		}

		public AdmPushService(AdmPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: this(default(IPushChannelFactory), channelSettings, serviceSettings)
		{
		}

		public AdmPushService(IPushChannelFactory pushChannelFactory, AdmPushChannelSettings channelSettings)
			: this(pushChannelFactory, channelSettings, default(IPushServiceSettings))
		{
		}

		public AdmPushService(IPushChannelFactory pushChannelFactory, AdmPushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
			: base(pushChannelFactory ?? new AdmPushChannelFactory(), channelSettings, serviceSettings)
		{
		}
	}

	public class AdmPushChannelFactory : IPushChannelFactory
	{
		public IPushChannel CreateChannel(IPushChannelSettings channelSettings)
		{
			if (!(channelSettings is AdmPushChannelSettings))
				throw new ArgumentException("channelSettings must be of type " + typeof(AdmPushChannelSettings).Name);

			return new AdmPushChannel(channelSettings as AdmPushChannelSettings);
		}
	}
}

