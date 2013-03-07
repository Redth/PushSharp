using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Core
{
	public interface IPushChannelFactory
	{
		IPushChannel CreateChannel(IPushService pushService);
	}

	public interface IPushService : IDisposable
	{
		ChannelEvents Events { get; set; }
		IPushChannelFactory PushChannelFactory { get; }
		PushServiceSettings ServiceSettings { get; }
		PushChannelSettings ChannelSettings { get; }
		bool IsStopping { get; }
		void QueueNotification(Notification notification, bool countsAsRequeue = true, bool ignoreStoppingChannel = false);
		void Stop(bool waitForQueueToFinish = true);
	}

	public interface IPushChannel
	{
		ChannelEvents Events { get; set; }
		IPushService PushService { get; set; }
		void SendNotification(Notification notification);
		void Stop();
	}
}
