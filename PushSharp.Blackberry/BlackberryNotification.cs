using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using PushSharp.Common;

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
