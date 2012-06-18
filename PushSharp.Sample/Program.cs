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
			var appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Resources/PushSharp.Apns.Sandbox.p12"));
			push.StartApplePushService(new ApplePushChannelSettings(false, appleCert, "pushsharp"));

			//Configure and start Android C2DM
			//push.StartAndroidPushService(new Android.AndroidPushChannelSettings("<SENDERID>", "test", "<APPID>"));

			//Configure and start Windows Phone Notifications
			//push.StartWindowsPhonePushService(new WindowsPhone.WindowsPhonePushChannelSettings());

			//Fluent construction of an iOS notification
			push.QueueNotification(NotificationFactory.Apple()
				.ForDeviceToken("0071737321559691b28fffa1aa4c8259d970fe0fc496794ad0486552fc9ec3db")
				.WithAlert("Alert Text!")
				.WithSound("default")
				.WithBadge(7));

			//System.Threading.Thread.Sleep(2000);

			//Fluent construction of an iOS notification
			push.QueueNotification(NotificationFactory.Apple()
				.ForDeviceToken("0071737321559691b28fffa1aa4c8259d970fe0fc496794ad0486552fc9ec3db")
				.WithAlert("Alert Text 2!")
				.WithSound("default")
				.WithBadge(7));

			//System.Threading.Thread.Sleep(2000);

			//Fluent construction of an iOS notification
			push.QueueNotification(NotificationFactory.Apple()
				.ForDeviceToken("1071737321559691b28fffa1aa4c8259d970fe0fc496794ad0486552fc9ec3db")
				.WithAlert("Alert Text 3!")
				.WithSound("default")
				.WithBadge(7));

			//Fluent construction of an Android C2DM Notification
			//push.QueueNotification(NotificationFactory.Android()
			//	.ForDeviceRegistrationId("<C2DM-DEVICE-ID>")
			//	.WithData("alert", "Alert Text!")
			//	.WithData("badge", "7"));
						
			Console.WriteLine("PushSharp.Sample Started");
			Console.WriteLine("Type 'exit' and press return to stop");

			var input = Console.ReadLine();

			while (true)
			{
				if (input.Equals("exit", StringComparison.InvariantCultureIgnoreCase)
					|| input.Equals("quit", StringComparison.InvariantCultureIgnoreCase))
					break;
				

				input = Console.ReadLine();
			}
			
		}

		static void Events_OnNotificationSent(Common.Notification notification)
		{
			Console.WriteLine("Sent Notification: " + notification.ToString());
		}

		static void Events_OnNotificationSendFailure(Common.Notification notification, Exception notificationFailureException)
		{
			Console.WriteLine("Notification Failure: " + notification.ToString());
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
