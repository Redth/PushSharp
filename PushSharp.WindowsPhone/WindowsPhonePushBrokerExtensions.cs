using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.WindowsPhone;
using PushSharp.Core;

namespace PushSharp
{
	public static class ApplePushBrokerExtensions
	{
		public static void RegisterWindowsPhoneService(this PushBroker broker, IWindowsPhonePushChannelSettings channelSettings, IPushServiceSettings serviceSettings)
		{
			var service = new WindowsPhonePushService(new WindowsPhonePushChannelFactory(), channelSettings, serviceSettings);

			broker.RegisterService<WindowsPhoneCycleTileNotification>(service);
			broker.RegisterService<WindowsPhoneFlipTileNotification>(service);
			broker.RegisterService<WindowsPhoneIconicTileNotification>(service);
			broker.RegisterService<WindowsPhoneTileNotification>(service);
			broker.RegisterService<WindowsPhoneToastNotification>(service);
			broker.RegisterService<WindowsPhoneRawNotification>(service);
		}

		public static WindowsPhoneCycleTileNotification WindowsPhoneCycleTileNotification(this PushBroker broker)
		{
			return new WindowsPhoneCycleTileNotification();
		}

		public static WindowsPhoneFlipTileNotification WindowsPhoneFlipTileNotification(this PushBroker broker)
		{
			return new WindowsPhoneFlipTileNotification();
		}

		public static WindowsPhoneIconicTileNotification WindowsPhoneIconicTileNotification(this PushBroker broker)
		{
			return new WindowsPhoneIconicTileNotification();
		}

		public static WindowsPhoneTileNotification WindowsPhoneTileNotification(this PushBroker broker)
		{
			return new WindowsPhoneTileNotification();
		}

		public static WindowsPhoneToastNotification WindowsPhoneToastNotification(this PushBroker broker)
		{
			return new WindowsPhoneToastNotification();
		}

		public static WindowsPhoneRawNotification WindowsPhoneRawNotification(this PushBroker broker)
		{
			return new WindowsPhoneRawNotification();
		}
	}
}
