using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Core
{
	public delegate void SendNotificationCallbackDelegate(object sender, SendNotificationResult result);

	public delegate void PushChannelExceptionDelegate(object sender, Exception ex);

	public interface IPushChannelFactory
	{
		IPushChannel CreateChannel(IPushChannelSettings channelSettings);
	}

	public interface IPushService : IDisposable
	{
		
		IPushChannelFactory PushChannelFactory { get; }
		IPushServiceSettings ServiceSettings { get; }
		IPushChannelSettings ChannelSettings { get; }
		bool IsStopping { get; }
		void QueueNotification(INotification notification);
		void Stop(bool waitForQueueToFinish = true);
	}

	public interface IPushChannel : IDisposable
	{
		void SendNotification(INotification notification, SendNotificationCallbackDelegate callback);
	}

	public interface IPushChannelSettings
	{

	}

	public interface IPushServiceSettings
	{
		bool AutoScaleChannels { get; set; }
		int MaxAutoScaleChannels { get; set; }
		long MinAvgTimeToScaleChannels { get; set; }
		int Channels { get; set; }
		int MaxNotificationRequeues { get; set; }
	}

	public interface INotification
	{
		object Tag { get; set; }
		int QueuedCount { get; set; }
		bool IsValidDeviceRegistrationId();
		DateTime EnqueuedTimestamp { get; set; }
	}

	public class SendNotificationResult
	{
		public SendNotificationResult(INotification notification, bool shouldRequeue = false, Exception error = null)
		{
			this.Notification = notification;
			this.Error = error;
			this.ShouldRequeue = shouldRequeue;
			this.IsSubscriptionExpired = false;
		}

		public INotification Notification { get; set; }
		public Exception Error { get; set; }
		public bool ShouldRequeue { get; set; }

		public string NewSubscriptionId { get; set; }

		public bool IsSubscriptionExpired { get; set; }

		public bool IsSuccess
		{
			get { return Error == null; }
		}
	}
}
