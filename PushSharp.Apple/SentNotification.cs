using System;
using PushSharp.Core;

namespace PushSharp.Apple
{
	public class SentNotification
	{
		public SentNotification(AppleNotification notification)
		{
			this.Notification = notification;
			this.SentAt = DateTime.UtcNow;
			this.Identifier = notification.Identifier;
		}

		public AppleNotification Notification { get; set; }

		public DateTime SentAt { get; set; }

		public int Identifier { get; set; }

		public SendNotificationCallbackDelegate Callback { get; set; }
	}
}