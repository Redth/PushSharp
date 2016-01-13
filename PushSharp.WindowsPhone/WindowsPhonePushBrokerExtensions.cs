using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.WindowsPhone;
using PushSharp.Core;

namespace PushSharp
{
	public static class WindowsPhonePushBrokerExtensions
	{
		public static void RegisterWindowsPhoneService(this IPushBroker broker, string applicationId, IPushServiceSettings serviceSettings = null)
		{
			RegisterWindowsPhoneService (broker, null, applicationId, serviceSettings);
		}

		public static void RegisterWindowsPhoneService(this IPushBroker broker, IPushServiceSettings serviceSettings = null)
		{
			RegisterWindowsPhoneService (broker, null, null, serviceSettings);
		}

		public static void RegisterWindowsPhoneService(this IPushBroker broker, WindowsPhonePushChannelSettings channelSettings, IPushServiceSettings serviceSettings = null)
		{
			RegisterWindowsPhoneService (broker, channelSettings, null, serviceSettings);
		}

		public static void RegisterWindowsPhoneService(this IPushBroker broker, WindowsPhonePushChannelSettings channelSettings, string applicationId, IPushServiceSettings serviceSettings = null)
		{
			var service = new WindowsPhonePushService(new WindowsPhonePushChannelFactory(), channelSettings, serviceSettings);

			broker.RegisterService<WindowsPhoneCycleTileNotification>(service, applicationId);
			broker.RegisterService<WindowsPhoneFlipTileNotification>(service, applicationId);
			broker.RegisterService<WindowsPhoneIconicTileNotification>(service, applicationId);
			broker.RegisterService<WindowsPhoneTileNotification>(service, applicationId);
			broker.RegisterService<WindowsPhoneToastNotification>(service, applicationId);
			broker.RegisterService<WindowsPhoneRawNotification>(service, applicationId);
		}

		public static WindowsPhoneCycleTileNotification WindowsPhoneCycleTileNotification(this IPushBroker broker)
		{
			return new WindowsPhoneCycleTileNotification();
		}

		public static WindowsPhoneFlipTileNotification WindowsPhoneFlipTileNotification(this IPushBroker broker)
		{
			return new WindowsPhoneFlipTileNotification();
		}

		public static WindowsPhoneIconicTileNotification WindowsPhoneIconicTileNotification(this IPushBroker broker)
		{
			return new WindowsPhoneIconicTileNotification();
		}

		public static WindowsPhoneTileNotification WindowsPhoneTileNotification(this IPushBroker broker)
		{
			return new WindowsPhoneTileNotification();
		}

		public static WindowsPhoneToastNotification WindowsPhoneToastNotification(this IPushBroker broker)
		{
			return new WindowsPhoneToastNotification();
		}

		public static WindowsPhoneRawNotification WindowsPhoneRawNotification(this IPushBroker broker)
		{
			return new WindowsPhoneRawNotification();
		}
	}
}
