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
			// IMPORTANT: Make sure you use the right Push certificate.  Apple allows you to generate one for connecting to Sandbox,
			//   and one for connecting to Production.  You must use the right one, to match the provisioning profile you build your
			//   app with!
			var appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Resources/PushSharp.Apns.Sandbox.p12"));
			
			//IMPORTANT: If you are using a Development provisioning Profile, you must use the Sandbox push notification server 
			//  (so you would leave the first arg in the ctor of ApplePushChannelSettings as 'false')
			//  If you are using an AdHoc or AppStore provisioning profile, you must use the Production push notification server
			//  (so you would change the first arg in the ctor of ApplePushChannelSettings to 'true')
			push.StartApplePushService(new ApplePushChannelSettings(false, appleCert, "pushsharp"));
			
			//Configure and start Android C2DM
			//push.StartAndroidPushService(new Android.AndroidPushChannelSettings("pushsharp@altusapps.com", "pushitg00d", "com.pushsharp.test"));
			push.StartGoogleCloudMessagingPushService(new GcmPushChannelSettings("pushsharp@altusapps.com", "AIzaSyB3_87o2GES59rAR7H5KytGTWbH7oD9LWE", "com.pushsharp.test"));

			//Configure and start Windows Phone Notifications
			//push.StartWindowsPhonePushService(new WindowsPhone.WindowsPhonePushChannelSettings());
			
			//Fluent construction of a Windows Phone Toast notification
			//push.QueueNotification(NotificationFactory.WindowsPhone().Toast()
			//    .ForEndpointUri(new Uri("http://sn1.notify.live.net/throttledthirdparty/01.00/AAFCoNoCXidwRpn5NOxvwSxPAgAAAAADAgAAAAQUZm52OkJCMjg1QTg1QkZDMkUxREQ"))
			//    .ForOSVersion(WindowsPhone.WindowsPhoneDeviceOSVersion.MangoSevenPointFive)
			//    .WithBatchingInterval(WindowsPhone.BatchingInterval.Immediate)
			//    .WithNavigatePath("/MainPage.xaml")
			//    .WithText1("PushSharp")
			//    .WithText2("This is a Toast"));

			//Fluent construction of an iOS notification
			//IMPORTANT: For iOS you MUST MUST MUST use your own DeviceToken here that gets generated within your iOS app itself when the Application Delegate
			//  for registered for remote notifications is called, and the device token is passed back to you
			//push.QueueNotification(NotificationFactory.Apple()
			//    .ForDeviceToken("1071737321559691b28fffa1aa4c8259d970fe0fc496794ad0486552fc9ec3db")
			//    .WithAlert("1 Alert Text!")
			//    .WithSound("default")
			//    .WithBadge(7));

			//Fluent construction of an Android C2DM Notification
			//push.QueueNotification(NotificationFactory.Android()
			//    .ForDeviceRegistrationId("APA91bFwgGgA2uXHvgT0ij8vJbY7Kgf-qQfzsTj-QnLXNGHghysJx-BUj4OGu8xe1w0T2-H2rgMi_0NzIBo5gVs2jfiY1h_L3ohux2cSwZjJDHthNRZ38C2Ej1TcuUfZWa4ZHoVbNigR_ezXjuzJ8kD4dH_dCx2X3w")
			//    .WithCollapseKey("NONE")
			//    .WithData("alert", "Alert Text!")
			//    .WithData("badge", "7"));

			push.QueueNotification(NotificationFactory.AndroidGcm()
				.ForDeviceRegistrationId("APA91bFwgGgA2uXHvgT0ij8vJbY7Kgf-qQfzsTj-QnLXNGHghysJx-BUj4OGu8xe1w0T2-H2rgMi_0NzIBo5gVs2jfiY1h_L3ohux2cSwZjJDHthNRZ38C2Ej1TcuUfZWa4ZHoVbNigR_ezXjuzJ8kD4dH_dCx2X3w")
				.WithCollapseKey("NONE"));
				//.WithData("alert", "Alert Text!")
				//.WithData("badge", "7"));

			Console.WriteLine("Waiting for Queue to Finish...");

			//Stop and wait for the queues to drains
			push.StopAllServices(true);

			Console.WriteLine("Queue Finished, press return to exit...");
			Console.ReadLine();			
		}

		static void Events_OnNotificationSent(Common.Notification notification)
		{
			Console.WriteLine("Sent: " + notification.Platform.ToString() + " -> " + notification.ToString());
		}

		static void Events_OnNotificationSendFailure(Common.Notification notification, Exception notificationFailureException)
		{
			Console.WriteLine("Failure: " + notification.Platform.ToString() + " -> " + notification.ToString());
		}

		static void Events_OnChannelException(Exception exception)
		{
			Console.WriteLine("Channel Exception: " + exception.ToString());
		}

		static void Events_OnDeviceSubscriptionExpired(Common.PlatformType platform, string deviceInfo)
		{
			Console.WriteLine("Device Subscription Expired: " + platform.ToString() + " -> " + deviceInfo);
		}
	}
}
