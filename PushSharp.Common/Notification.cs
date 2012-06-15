using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Common
{
	public abstract class Notification
	{
		public PlatformType Platform { get; set; }

		internal DateTime EnqueuedTimestamp { get; set; }
	}

}
