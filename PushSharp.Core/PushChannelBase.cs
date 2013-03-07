using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PushSharp.Core
{
	public abstract class PushChannelBase : IPushChannel
	{
		protected PushChannelBase(IPushService pushService)
		{
			this.PushService = pushService;
			this.Events = new ChannelEvents();
		}

		public ChannelEvents Events { get; set; }
		public IPushService PushService { get; set; }
		
		public abstract void SendNotification(Notification notification);
		
		public virtual void Stop()
		{
		}
	}
}
