using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PushSharp.Core
{
	public abstract class PushChannelBase
	{
		public ChannelEvents Events = new ChannelEvents();
		
		public PushChannelSettings ChannelSettings { get; private set; }
		public PushServiceSettings ServiceSettings { get; private set; }
		public PushServiceBase PushService { get; set; }
		

		public abstract void SendNotification(Notification notification);

		protected PushChannelBase(PushServiceBase pushService, PushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			this.PushService = pushService;
			this.ChannelSettings = channelSettings;
			this.ServiceSettings = serviceSettings ?? new PushServiceSettings();
		}

		public virtual void Stop()
		{

		}

	}
}
