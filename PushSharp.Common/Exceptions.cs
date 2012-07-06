using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Common
{
	public class MaxSendAttemptsReachedException : Exception
	{
		public MaxSendAttemptsReachedException() : base("The maximum number of Send attempts to send the notification was reached!") { }
	}
}
