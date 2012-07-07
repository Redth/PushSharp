﻿using PushSharp.Common;

namespace PushSharp.Blackberry
{
	public class BlackberryNotification : Notification
	{
		public BlackberryNotification()
			: base()
		{
			this.Platform = PlatformType.Blackberry;
		}

		public string WidgetNotificationUrl { get; set; }

		public string PushPin { get; set; }

		public string Message { get; set; }
	}
}
