using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using PushSharp;
using PushSharp.Apple;
using PushSharp.Core;

//using PushSharp.Android;
//using PushSharp.WindowsPhone;
//using PushSharp.Windows;

namespace PushSharp.Sample
{
	class Program
	{
		static void Main(string[] args)
		{		
			//Create our service	
			var push = new PushBroker();

			//Wire up the events
			push.OnNotificationSent += NotificationSent;
			push.OnChannelException += ChannelException;
			push.OnServiceException += ServiceException;
			push.OnNotificationFailed += NotificationFailed;
			push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
			push.OnChannelCreated += ChannelCreated;
			push.OnChannelDestroyed += ChannelDestroyed;

			//Configure and start Apple APNS
			// IMPORTANT: Make sure you use the right Push certificate.  Apple allows you to generate one for connecting to Sandbox,
			//   and one for connecting to Production.  You must use the right one, to match the provisioning profile you build your
			//   app with!
			var appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Resources/PushSharp.Apns.Sandbox.p12"));

            //IMPORTANT: If you are using a Development provisioning Profile, you must use the Sandbox push notification server 
            //  (so you would leave the first arg in the ctor of ApplePushChannelSettings as 'false')
            //  If you are using an AdHoc or AppStore provisioning profile, you must use the Production push notification server
            //  (so you would change the first arg in the ctor of ApplePushChannelSettings to 'true')
            //push.StartApplePushService(new ApplePushChannelSettings(appleCert, "pushsharp"));

			var appleSettings = new ApplePushChannelSettings(appleCert, "pushsharp");
			
			
			push.RegisterAppleService(appleSettings);

			push.QueueNotification(push.AppleNotification().ForDeviceToken("cff441e214b2b2283df799f0b8b16c17a59b7ac077e2867ea54ebf6086e55866")
				.WithAlert("TEST"));

			//push.RegisterGcmService(new PushSharp.Android.GcmPushChannelSettings("AIzaSyC2PZNXQDVaUpZGmtsF_Vp8tHtIABVjazI"));


			//push.QueueNotification(push.GcmNotification().ForDeviceRegistrationId("!APA91bGVsy23QWxcDGB_uQwM3aIJyQ4VtwVj4WWcXEwNcp01azdFFiNu7EwLSUXFEtF_cz8qK82E880VfOYQOacXMBUg-7ReeDiq6rsb1w8y1jc3rmLHj07uYERlp0WWPtT1N29w-EDH5GZeMsiQ_oE355ExJdrfQw")
			 //                      .WithJson("{\"key\":\"value1\"}"));

			



            //Configure and start Android GCM
            //IMPORTANT: The SENDER_ID is your Google API Console App Project ID.
            //  Be sure to get the right Project ID from your Google APIs Console.  It's not the named project ID that appears in the Overview,
            //  but instead the numeric project id in the url: eg: https://code.google.com/apis/console/?pli=1#project:785671162406:overview
            //  where 785671162406 is the project id, which is the SENDER_ID to use!
           // push.StartGoogleCloudMessagingPushService(new GcmPushChannelSettings("785671162406", "AIzaSyC2PZNXQDVaUpZGmtsF_Vp8tHtIABVjazI", "com.pushsharp.test"));

            //Configure and start Windows Phone Notifications
           // push.StartWindowsPhonePushService(new WindowsPhonePushChannelSettings());

			//Configure and start Windows Notifications
			//push.StartWindowsPushService(new WindowsPushChannelSettings("677AltusApps.PushSharpTest",
			//	"ms-app://s-1-15-2-397915024-884168245-3562497613-3307968140-4074292843-797285123-433377759", "ei5Lott1HEbbZBv2wGDTUsrCjU++Pj8Z"));

            //Fluent construction of a Windows Toast Notification
            //push.QueueNotification(NotificationFactory.Windows().Toast().AsToastText01("This is a test").ForChannelUri("YOUR_CHANNEL_URI_HERE"));

            //Fluent construction of a Windows Phone Toast notification
            //IMPORTANT: For Windows Phone you MUST use your own Endpoint Uri here that gets generated within your Windows Phone app itself!
           // push.QueueNotification(NotificationFactory.WindowsPhone().Toast()
             //   .ForEndpointUri(new Uri("http://sn1.notify.live.net/throttledthirdparty/01.00/AAFCoNoCXidwRpn5NOxvwSxPAgAAAAADAgAAAAQUZm52OkJCMjg1QTg1QkZDMkUxREQ"))
               // .ForOSVersion(WindowsPhone.WindowsPhoneDeviceOSVersion.MangoSevenPointFive)
                //.WithBatchingInterval(WindowsPhone.BatchingInterval.Immediate)
               // .WithNavigatePath("/MainPage.xaml")
               // .WithText1("PushSharp")
               // .WithText2("This is a Toast"));

            //Fluent construction of an iOS notification
            //IMPORTANT: For iOS you MUST MUST MUST use your own DeviceToken here that gets generated within your iOS app itself when the Application Delegate
            //  for registered for remote notifications is called, and the device token is passed back to you
          ///  push.QueueNotification(new AppleNotification()
            ///    .ForDeviceToken("1071737321559691b28fffa1aa4c8259d970fe0fc496794ad0486552fc9ec3db")
            //    .WithAlert("1 Alert Text!")
            //    .WithSound("default")
            //    .WithBadge(7));

            //Fluent construction of an Android GCM Notification
            //IMPORTANT: For Android you MUST use your own RegistrationId here that gets generated within your Android app itself!
           //push.QueueNotification(NotificationFactory.AndroidGcm()
           //     .ForDeviceRegistrationId("APA91bG7J-cZjkURrqi58cEd5ain6hzi4i06T0zg9eM2kQAprV-fslFiq60hnBUVlnJPlPV-4K7X39aHIe55of8fJugEuYMyAZSUbmDyima5ZTC7hn4euQ0Yflj2wMeTxnyMOZPuwTLuYNiJ6EREeI9qJuJZH9Zu9g")
           //     .WithCollapseKey("NONE")
           //     .WithJson("{\"alert\":\"Alert Text!\",\"badge\":\"7\"}"));

			//push.QueueNotification(NotificationFactory.Windows()
			//	.Toast()
			//	.ForChannelUri("https://bn1.notify.windows.com/?token=AgUAAACC2u7flXAmaevcggrLenaSdExjVfIHvr6KSZrg0KeuGrcz877rPJprPL9bEuQH%2bacmmm%2beUyXNXEM8oRNit%2bzPoigksDOq6bIFyV3XGmhUmXadysLokl5rlmTscvHGAbs%3d")
			//	.WithRequestForStatus(true)
			//	.AsToastText01("This is a test!"));

			Console.WriteLine("Waiting for Queue to Finish...");

			//Stop and wait for the queues to drains
			push.StopAllServices(true);

			Console.WriteLine("Queue Finished, press return to exit...");
			Console.ReadLine();			
		}

		static void DeviceSubscriptionIdChanged(object sender, string oldDeviceInfo, string newDeviceInfo, Core.Notification notification)
		{
			//Currently this event will only ever happen for Android GCM
			Console.WriteLine("Device Registration Changed:  Old-> " + oldDeviceInfo + "  New-> " + newDeviceInfo);
		}

		static void NotificationSent(object sender, INotification notification)
		{
			Console.WriteLine("Sent: " + sender.ToString() + " -> " + notification.ToString());
		}

		static void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
		{
			Console.WriteLine("Failure: " + sender.ToString() + " -> " + notificationFailureException.Message + " -> " + notification.ToString());
		}

		static void ChannelException(object sender, IPushChannel channel, Exception exception)
		{
			Console.WriteLine("Channel Exception: " + sender.ToString() + " -> " + exception.ToString());
		}

		static void ServiceException(object sender, Exception exception)
		{
			Console.WriteLine("Channel Exception: " + sender.ToString() + " -> " + exception.ToString());
		}

		static void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
		{
			Console.WriteLine("Device Subscription Expired: " + sender.ToString() + " -> " + expiredDeviceSubscriptionId);
		}

		static void ChannelDestroyed(object sender)
		{
			Console.WriteLine("Channel Destroyed for: " + sender.ToString());
		}

		static void ChannelCreated(object sender, IPushChannel pushChannel)
		{
			Console.WriteLine("Channel Created for: " + sender.ToString());
		}
	}
}
