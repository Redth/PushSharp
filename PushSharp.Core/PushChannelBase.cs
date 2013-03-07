using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PushSharp.Core
{
	public abstract class PushChannelBase
	{
		public ChannelEvents Events = new ChannelEvents();
		
		public PushServiceBase PushService { get; set; }
		

		public abstract void SendNotification(Notification notification);

		protected PushChannelBase(PushServiceBase pushService)
		{
			this.PushService = pushService;			
		}

		public virtual void Stop()
		{

		}

	}
}
