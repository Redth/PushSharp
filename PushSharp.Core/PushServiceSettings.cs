using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Core
{
	public class PushServiceSettings
	{
		public PushServiceSettings()
		{
			this.AutoScaleChannels = true;
			this.MaxAutoScaleChannels = 100;
			this.MinAvgTimeToScaleChannels = 100;
			this.Channels = 1;
			this.MaxNotificationRequeues = 5;
		}

		public bool AutoScaleChannels { get; set; }
		public int MaxAutoScaleChannels { get; set; }
		public long MinAvgTimeToScaleChannels { get; set; }
		public int Channels { get; set; }
		public int MaxNotificationRequeues { get; set; }
	}
}
