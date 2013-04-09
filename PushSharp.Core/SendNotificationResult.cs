using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Core
{
	public class SendNotificationResult
	{
		public SendNotificationResult(INotification notification, bool shouldRequeue = false, Exception error = null)
		{
			this.Notification = notification;
			this.Error = error;
			this.ShouldRequeue = shouldRequeue;
			this.IsSubscriptionExpired = false;
			this.CountsAsRequeue = true;
		}

		public INotification Notification { get; set; }
		public Exception Error { get; set; }
		public bool ShouldRequeue { get; set; }
		public bool CountsAsRequeue { get; set; }
		public string OldSubscriptionId { get; set; }
		public string NewSubscriptionId { get; set; }
		public DateTime SubscriptionExpiryUtc { get;set; }
		public bool IsSubscriptionExpired { get; set; }
		public bool IsSuccess { get { return Error == null; } }
	}
}
