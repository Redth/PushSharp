using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Windows;
using PushSharp.Core;

namespace PushSharp
{
	public static class WindowsPushBrokerExtensions
	{
		public static void RegisterAppleService(this PushBroker broker, WindowsPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			var service = new WindowsPushService(new WindowsPushChannelFactory(), channelSettings, serviceSettings);

			broker.RegisterService<WindowsRawNotification>(service);
			broker.RegisterService<WindowsTileNotification>(service);
			broker.RegisterService<WindowsToastNotification>(service);
			broker.RegisterService<WindowsBadgeNumericNotification>(service);
			broker.RegisterService<WindowsBadgeGlyphNotification>(service);
		}

		public static WindowsRawNotification WindowsRawNotification(this PushBroker broker)
		{
			return new WindowsRawNotification();
		}

		public static WindowsTileNotification WindowsTileNotification(this PushBroker broker)
		{
			return new WindowsTileNotification();
		}

		public static WindowsToastNotification WindowsToastNotification(this PushBroker broker)
		{
			return new WindowsToastNotification();
		}

		public static WindowsBadgeNumericNotification WindowsBadgeNumericNotification(this PushBroker broker)
		{
			return new WindowsBadgeNumericNotification();
		}

		public static WindowsBadgeGlyphNotification WindowsBadgeGlyphNotification(this PushBroker broker)
		{
			return new WindowsBadgeGlyphNotification();
		}
	}
}
