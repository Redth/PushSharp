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
		public static void RegisterWindowsService(this PushBroker broker, WindowsPushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
		{
			RegisterWindowsService (broker, channelSettings, null, serviceSettings);
		}

		public static void RegisterWindowsService(this PushBroker broker, WindowsPushChannelSettings channelSettings, string applicationId, IPushServiceSettings serviceSettings = null)
		{
			var service = new WindowsPushService(new WindowsPushChannelFactory(), channelSettings, serviceSettings);

			broker.RegisterService<WindowsRawNotification>(service, applicationId);
			broker.RegisterService<WindowsTileNotification>(service, applicationId);
			broker.RegisterService<WindowsToastNotification>(service, applicationId);
			broker.RegisterService<WindowsBadgeNumericNotification>(service, applicationId);
			broker.RegisterService<WindowsBadgeGlyphNotification>(service, applicationId);
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
