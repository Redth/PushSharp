using System;

namespace PushSharp.Common
{
	public class MaxSendAttemptsReachedException : Exception
	{
		public MaxSendAttemptsReachedException() : base("The maximum number of Send attempts to send the notification was reached!") { }
	}
}
