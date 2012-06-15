using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Common
{
	public class PushServiceSettings
	{
		public bool AutoScaleChannels { get; set; }

		public int MaxAutoScaleChannels { get; set; }

		public int Channels { get; set; }
	}
}
