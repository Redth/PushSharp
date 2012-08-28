using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using PushSharp;
using PushSharp.Apple;
using PushSharp.Android;
using PushSharp.WindowsPhone;
using PushSharp.Windows;

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
			push.Events.OnDeviceSubscriptionIdChanged += new Common.ChannelEvents.DeviceSubscriptionIdChanged(Events_OnDeviceSubscriptionIdChanged);
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
			
			//Configure and start Android GCM
			//IMPORTANT: The SENDER_ID is your Google API Console App Project ID.
			//  Be sure to get the right Project ID from your Google APIs Console.  It's not the named project ID that appears in the Overview,
			//  but instead the numeric project id in the url: eg: https://code.google.com/apis/console/?pli=1#project:785671162406:overview
			//  where 785671162406 is the project id, which is the SENDER_ID to use!
			push.StartGoogleCloudMessagingPushService(new GcmPushChannelSettings("785671162406", "AIzaSyC2PZNXQDVaUpZGmtsF_Vp8tHtIABVjazI", "com.pushsharp.test"));

			//Configure and start Windows Phone Notifications
			push.StartWindowsPhonePushService(new WindowsPhonePushChannelSettings());

			//Configure and start Windows Notifications
			push.StartWindowsPushService(new WindowsPushChannelSettings("BUILD.64beb1a1-5444-4660-8b27-bcc740f9c7ca", 
				"ms-app://s-1-15-2-259456210-2622405444-520366611-1750679940-1314087242-2560077863-3994015833", "7-GIUO1ubmrqOwQUBzXpnqiSw30LS2xr"));
			
			//Fluent construction of a Windows Toast Notification
			push.QueueNotification(NotificationFactory.Windows().Toast().AsToastText01("This is a test").ForChannelUri("YOUR_CHANNEL_URI_HERE"));

			//Fluent construction of a Windows Phone Toast notification
			//IMPORTANT: For Windows Phone you MUST use your own Endpoint Uri here that gets generated within your Windows Phone app itself!
			push.QueueNotification(NotificationFactory.WindowsPhone().Toast()
				.ForEndpointUri(new Uri("http://sn1.notify.live.net/throttledthirdparty/01.00/AAFCoNoCXidwRpn5NOxvwSxPAgAAAAADAgAAAAQUZm52OkJCMjg1QTg1QkZDMkUxREQ"))
				.ForOSVersion(WindowsPhone.WindowsPhoneDeviceOSVersion.MangoSevenPointFive)
				.WithBatchingInterval(WindowsPhone.BatchingInterval.Immediate)
				.WithNavigatePath("/MainPage.xaml")
				.WithText1("PushSharp")
				.WithText2("This is a Toast"));

			//Fluent construction of an iOS notification
			//IMPORTANT: For iOS you MUST MUST MUST use your own DeviceToken here that gets generated within your iOS app itself when the Application Delegate
			//  for registered for remote notifications is called, and the device token is passed back to you
			push.QueueNotification(NotificationFactory.Apple()
				.ForDeviceToken("1071737321559691b28fffa1aa4c8259d970fe0fc496794ad0486552fc9ec3db")
				.WithAlert("1 Alert Text!")
				.WithSound("default")
				.WithBadge(7));

			//Fluent construction of an Android GCM Notification
			//IMPORTANT: For Android you MUST use your own RegistrationId here that gets generated within your Android app itself!
			push.QueueNotification(NotificationFactory.AndroidGcm()
				.ForDeviceRegistrationId("APA91bG7J-cZjkURrqi58cEd5ain6hzi4i06T0zg9eM2kQAprV-fslFiq60hnBUVlnJPlPV-4K7X39aHIe55of8fJugEuYMyAZSUbmDyima5ZTC7hn4euQ0Yflj2wMeTxnyMOZPuwTLuYNiJ6EREeI9qJuJZH9Zu9g")
				.WithCollapseKey("NONE")
				.WithJson("{\"alert\":\"Alert Text!\",\"badge\":\"7\"}"));
										
			Console.WriteLine("Waiting for Queue to Finish...");

			//Stop and wait for the queues to drains
			push.StopAllServices(true);

			Console.WriteLine("Queue Finished, press return to exit...");
			Console.ReadLine();			
		}

		static void Events_OnDeviceSubscriptionIdChanged(Common.PlatformType platform, string oldDeviceInfo, string newDeviceInfo, Common.Notification notification)
		{
			//Currently this event will only ever happen for Android GCM
			Console.WriteLine("Device Registration Changed:  Old-> " + oldDeviceInfo + "  New-> " + newDeviceInfo);
		}

		static void Events_OnNotificationSent(Common.Notification notification)
		{
			Console.WriteLine("Sent: " + notification.Platform.ToString() + " -> " + notification.ToString());
		}

		static void Events_OnNotificationSendFailure(Common.Notification notification, Exception notificationFailureException)
		{
			Console.WriteLine("Failure: " + notification.Platform.ToString() + " -> " + notificationFailureException.Message + " -> " + notification.ToString());
		}

		static void Events_OnChannelException(Exception exception, Common.Notification notification)
		{
			Console.WriteLine("Channel Exception: " + exception.ToString());
		}

		static void Events_OnDeviceSubscriptionExpired(Common.PlatformType platform, string deviceInfo, Common.Notification notification)
		{
			Console.WriteLine("Device Subscription Expired: " + platform.ToString() + " -> " + deviceInfo);
		}
	}
}
