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
			this.Events.OnChannelException += new ChannelEvents.ChannelExceptionDelegate(events_OnChannelException);
			this.Events.OnNotificationSendFailure += new ChannelEvents.NotificationSendFailureDelegate(events_OnNotificationSendFailure);
			this.Events.OnNotificationSent += new ChannelEvents.NotificationSentDelegate(events_OnNotificationSent);
			this.Events.OnDeviceSubscriptionExpired += new ChannelEvents.DeviceSubscriptionExpired(events_OnDeviceSubscriptionExpired);
		}

		void events_OnDeviceSubscriptionExpired(PlatformType platform, string deviceInfo)
		{
			Console.WriteLine("Device Expired: " + platform.ToString() + " - " + deviceInfo);
		}

		void events_OnNotificationSent(Notification notification)
		{
			Console.WriteLine("Notification Sent: " + notification.ToString());
		}

		void events_OnNotificationSendFailure(Notification notification, Exception notificationFailureException)
		{
			Console.WriteLine("Notification Failure: " + notificationFailureException.Message);
		}

		void events_OnChannelException(Exception exception)
		{
			Console.WriteLine("Channel Exception: " + exception.ToString());
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
