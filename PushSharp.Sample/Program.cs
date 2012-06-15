using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using PushSharp;
using PushSharp.Apple;
using PushSharp.Android;

namespace PushSharp.Sample
{
	class Program
	{
		static void Main(string[] args)
		{		
			//Create our service	
			PushService push = new PushService();

			//Wire up the events
			push.Events.OnDeviceSubscriptionExpired += new Common.ChannelEvents.DeviceSubscriptionExpired(Events_OnDeviceSubscriptionExpired);
			push.Events.OnChannelException += new Common.ChannelEvents.ChannelExceptionDelegate(Events_OnChannelException);
			push.Events.OnNotificationSendFailure += new Common.ChannelEvents.NotificationSendFailureDelegate(Events_OnNotificationSendFailure);
			push.Events.OnNotificationSent += new Common.ChannelEvents.NotificationSentDelegate(Events_OnNotificationSent);

			//Configure and start Apple APNS
			var appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppleSandbox.p12"));
			push.StartApplePushService(new ApplePushChannelSettings(false, appleCert, "test"));

			//Configure and start Android C2DM
			push.StartAndroidPushService(new Android.AndroidPushChannelSettings("<SENDERID>", "test", "<APPID>"));

			//Configure and start Windows Phone Notifications
			push.StartWindowsPhonePushService(new WindowsPhone.WindowsPhonePushChannelSettings());

			//Queue a notification easily!
			push.QueueNotification(new AppleNotification("<DEVICETOKEN>", new Apple.AppleNotificationPayload("ALERT Text!", 7)));

			//Or queue and android notification!
			var androidData = new NameValueCollection();
			androidData.Add("msg", "ALERT Text!");
			androidData.Add("badge", "7");
			push.QueueNotification(new AndroidNotification() { RegistrationId = "<C2DM-DEVICE-ID>", Data = androidData });

		}

		static void Events_OnNotificationSent(Common.Notification notification)
		{
			
		}

		static void Events_OnNotificationSendFailure(Common.Notification notification, Exception notificationFailureException)
		{
			
		}

		static void Events_OnChannelException(Exception exception)
		{
			
		}

		static void Events_OnDeviceSubscriptionExpired(Common.PlatformType platform, string deviceInfo)
		{
			
		}
	}
}
