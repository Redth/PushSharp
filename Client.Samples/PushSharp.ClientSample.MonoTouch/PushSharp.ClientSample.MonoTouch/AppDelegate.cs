using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.ObjCRuntime;

namespace PushSharp.ClientSample.AppleMonoTouch
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the 
	// User Interface of the application, as well as listening (and optionally responding) to 
	// application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		UINavigationController navController;

		//
		// This method is invoked when the application has loaded and is ready to run. In this 
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			navController = new UINavigationController();

			// If you have defined a view, add it here:
			window.AddSubview (navController.View);


			// make the window visible
			window.MakeKeyAndVisible ();

			//Register for remote notifications
			UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(UIRemoteNotificationType.Alert
			                                                                   | UIRemoteNotificationType.Badge
			                                                                   | UIRemoteNotificationType.Sound);
			return true;
		}

		public override void RegisteredForRemoteNotifications (UIApplication application, NSData deviceToken)
		{
			var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");
			var newDeviceToken = deviceToken.ToString().Replace("<", "").Replace(">", "").Replace(" ", "");
			
			if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(newDeviceToken))
			{
				//TODO: Put your own logic here to notify your server that the device token has changed/been created!
			}

			//Save device token now
			NSUserDefaults.StandardUserDefaults.SetString(newDeviceToken, "PushDeviceToken");

			Console.WriteLine("Device Token: " + newDeviceToken);
		}

		public override void FailedToRegisterForRemoteNotifications (UIApplication application, NSError error)
		{
			Console.WriteLine("Failed to register for notifications");
		}

		public override void ReceivedRemoteNotification (UIApplication application, NSDictionary userInfo)
		{
			Console.WriteLine("Received Remote Notification!");
		}
	}
}

