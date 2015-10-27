using System;

namespace PushSharp.Core
{
	public class ServiceBrokerConfiguration // : IPushServiceSettings
	{
		public ServiceBrokerConfiguration()
		{
			this.AutoScaleChannels = true;
			this.MaxAutoScaleChannels = 20;
			//this.MinAvgTimeToScaleChannels = 100;
			this.Channels = 1;
			//this.MaxNotificationRequeues = 5;
			//this.NotificationSendTimeout = 15000;
			//this.IdleTimeout = TimeSpan.FromMinutes (5);
		}

		public bool AutoScaleChannels { get; set; }
		public int MaxAutoScaleChannels { get; set; }
		//public long MinAvgTimeToScaleChannels { get; set; }
		public int Channels { get; set; }
		//public int MaxNotificationRequeues { get; set; }
		//public int NotificationSendTimeout { get; set; }
		//public TimeSpan IdleTimeout { get;set; }
	}
}

