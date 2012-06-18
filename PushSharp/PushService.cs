using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Common;

namespace PushSharp
{
	public class PushService
	{
		//public PushSettings Settings { get; private set; }

		public ChannelEvents Events;

		Apple.ApplePushService appleService = null;
		Android.AndroidPushService androidService = null;
		WindowsPhone.WindowsPhonePushService wpService = null;
		Blackberry.BlackberryPushService bbService = null;

		public PushService()
		{
			//this.Settings = settings;
			
			this.Events = new ChannelEvents();
		}

		public void StartApplePushService(Apple.ApplePushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			appleService = new Apple.ApplePushService(channelSettings, serviceSettings);
			appleService.Events.RegisterProxyHandler(this.Events);
		}

		public void StartAndroidPushService(Android.AndroidPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			androidService = new Android.AndroidPushService(channelSettings, serviceSettings);
			androidService.Events.RegisterProxyHandler(this.Events);
		}

		public void StartWindowsPhonePushService(WindowsPhone.WindowsPhonePushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			wpService = new WindowsPhone.WindowsPhonePushService(channelSettings, serviceSettings);
			wpService.Events.RegisterProxyHandler(this.Events);
		}

		public void StartBlackberryPushService(Blackberry.BlackberryPushChannelSettings channelSettings, PushServiceSettings serviceSettings = null)
		{
			bbService = new Blackberry.BlackberryPushService(channelSettings, serviceSettings);
			bbService.Events.RegisterProxyHandler(this.Events);
		}

		public void QueueNotification(Notification notification)
		{
			switch (notification.Platform)
			{
				case PlatformType.Apple:
					appleService.QueueNotification(notification);
					break;
				case PlatformType.Android:
					androidService.QueueNotification(notification);
					break;
				case PlatformType.WindowsPhone:
					wpService.QueueNotification(notification);
					break;
				case PlatformType.Blackberry:
					bbService.QueueNotification(notification);
					break;
			}
		}

	}

	
}
