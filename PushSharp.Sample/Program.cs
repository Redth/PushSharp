using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using PushSharp;
using PushSharp.Android;
using PushSharp.Apple;
using PushSharp.Core;
using PushSharp.Windows;
using PushSharp.WindowsPhone;

//using PushSharp.Android;
//using PushSharp.WindowsPhone;
//using PushSharp.Windows;

namespace PushSharp.Sample
{
	class Program
	{
		static void Main(string[] args)
		{		
			//Create our push services broker
			var push = new PushBroker();

			//Wire up the events for all the services that the broker registers
			push.OnNotificationSent += NotificationSent;
			push.OnChannelException += ChannelException;
			push.OnServiceException += ServiceException;
			push.OnNotificationFailed += NotificationFailed;
			push.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
			push.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
			push.OnChannelCreated += ChannelCreated;
			push.OnChannelDestroyed += ChannelDestroyed;
			

			//------------------------------------------------
			//IMPORTANT NOTE about Push Service Registrations
			//------------------------------------------------
			//Some of the methods in this sample such as 'RegisterAppleServices' depend on you referencing the correct
			//assemblies, and having the correct 'using PushSharp;' in your file since they are extension methods!!!

			// If you don't want to use the extension method helpers you can register a service like this:
			//push.RegisterService<WindowsPhoneToastNotification>(new WindowsPhonePushService());
			
			//If you register your services like this, you must register the service for each type of notification
			//you want it to handle.  In the case of WindowsPhone, there are several notification types!



			//-------------------------
			// APPLE NOTIFICATIONS
			//-------------------------
			//Configure and start Apple APNS
			// IMPORTANT: Make sure you use the right Push certificate.  Apple allows you to generate one for connecting to Sandbox,
			//   and one for connecting to Production.  You must use the right one, to match the provisioning profile you build your
			//   app with!
			var appleCert = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Resources/PushSharp.Apns.Sandbox.p12"));
            //IMPORTANT: If you are using a Development provisioning Profile, you must use the Sandbox push notification server 
            //  (so you would leave the first arg in the ctor of ApplePushChannelSettings as 'false')
            //  If you are using an AdHoc or AppStore provisioning profile, you must use the Production push notification server
            //  (so you would change the first arg in the ctor of ApplePushChannelSettings to 'true')
            push.RegisterAppleService(new ApplePushChannelSettings(appleCert, "CERTIFICATE PASSWORD HERE")); //Extension method
			//Fluent construction of an iOS notification
			//IMPORTANT: For iOS you MUST MUST MUST use your own DeviceToken here that gets generated within your iOS app itself when the Application Delegate
			//  for registered for remote notifications is called, and the device token is passed back to you
			push.QueueNotification(new AppleNotification()
			                           .ForDeviceToken("DEVICE TOKEN HERE")
			                           .WithAlert("Hello World!")
			                           .WithBadge(7)
			                           .WithSound("sound.caf"));

			
			//---------------------------
			// ANDROID GCM NOTIFICATIONS
			//---------------------------
			//Configure and start Android GCM
			//IMPORTANT: The API KEY comes from your Google APIs Console App, under the API Access section, 
			//  by choosing 'Create new Server key...'
			//  You must ensure the 'Google Cloud Messaging for Android' service is enabled in your APIs Console
			push.RegisterGcmService(new GcmPushChannelSettings("YOUR Google API's Console API Access  API KEY for Server Apps HERE"));
			//Fluent construction of an Android GCM Notification
			//IMPORTANT: For Android you MUST use your own RegistrationId here that gets generated within your Android app itself!
			push.QueueNotification(new GcmNotification().ForDeviceRegistrationId("DEVICE REGISTRATION ID HERE")
			                      .WithJson("{\"alert\":\"Hello World!\",\"badge\":7,\"sound\":\"sound.caf\"}"));
			

			//-----------------------------
			// WINDOWS PHONE NOTIFICATIONS
			//-----------------------------
            //Configure and start Windows Phone Notifications
			push.RegisterWindowsPhoneService();
			//Fluent construction of a Windows Phone Toast notification
			//IMPORTANT: For Windows Phone you MUST use your own Endpoint Uri here that gets generated within your Windows Phone app itself!
			push.QueueNotification(new WindowsPhoneToastNotification()
				.ForEndpointUri(new Uri("DEVICE REGISTRATION CHANNEL URI HERE"))
				.ForOSVersion(WindowsPhoneDeviceOSVersion.MangoSevenPointFive)
				.WithBatchingInterval(BatchingInterval.Immediate)
				.WithNavigatePath("/MainPage.xaml")
				.WithText1("PushSharp")
				.WithText2("This is a Toast"));
			

			//-------------------------
			// WINDOWS NOTIFICATIONS
			//-------------------------
			//Configure and start Windows Notifications
			push.RegisterWindowsService(new WindowsPushChannelSettings("WINDOWS APP PACKAGE NAME HERE",
				"WINDOWS APP PACKAGE SECURITY IDENTIFIER HERE", "CLIENT SECRET HERE"));
			//Fluent construction of a Windows Toast Notification
            push.QueueNotification(new WindowsToastNotification()
				.AsToastText01("This is a test")
				.ForChannelUri("DEVICE CHANNEL URI HERE"));

			

			Console.WriteLine("Waiting for Queue to Finish...");

			//Stop and wait for the queues to drains
			push.StopAllServices();

			Console.WriteLine("Queue Finished, press return to exit...");
			Console.ReadLine();			
		}

		static void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
		{
			//Currently this event will only ever happen for Android GCM
			Console.WriteLine("Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
		}

		static void NotificationSent(object sender, INotification notification)
		{
			Console.WriteLine("Sent: " + sender + " -> " + notification);
		}

		static void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
		{
			Console.WriteLine("Failure: " + sender + " -> " + notificationFailureException.Message + " -> " + notification);
		}

		static void ChannelException(object sender, IPushChannel channel, Exception exception)
		{
			Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
		}

		static void ServiceException(object sender, Exception exception)
		{
			Console.WriteLine("Channel Exception: " + sender + " -> " + exception);
		}

		static void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
		{
			Console.WriteLine("Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId);
		}

		static void ChannelDestroyed(object sender)
		{
			Console.WriteLine("Channel Destroyed for: " + sender);
		}

		static void ChannelCreated(object sender, IPushChannel pushChannel)
		{
			Console.WriteLine("Channel Created for: " + sender);
		}
	}
}
