using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace PushSharp.ClientSample.MonoMac
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		MainWindowController mainWindowController;
		
		public AppDelegate ()
		{
		}

		public override void FinishedLaunching (NSObject notification)
		{
			mainWindowController = new MainWindowController ();
			mainWindowController.Window.MakeKeyAndOrderFront (this);

			NSApplication.SharedApplication.RegisterForRemoteNotificationTypes(NSRemoteNotificationType.Badge);
		}

		public override void RegisteredForRemoteNotifications (NSApplication application, NSData deviceToken)
		{
			var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");
			
			//There's probably a better way to do this
			var strFormat = new NSString("%@");
			var dt = new NSString(MonoTouch.ObjCRuntime.Messaging.IntPtr_objc_msgSend_IntPtr_IntPtr(new MonoTouch.ObjCRuntime.Class("NSString").Handle, new MonoTouch.ObjCRuntime.Selector("stringWithFormat:").Handle, strFormat.Handle, deviceToken.Handle));
			var newDeviceToken = dt.ToString().Replace("<", "").Replace(">", "").Replace(" ", "");
			
			if (string.IsNullOrEmpty(oldDeviceToken) || !deviceToken.Equals(newDeviceToken))
			{
				//TODO: Put your own logic here to notify your server that the device token has changed/been created!
			}
			
			//Save device token now
			NSUserDefaults.StandardUserDefaults.SetString(newDeviceToken, "PushDeviceToken");
			
			Console.WriteLine("Device Token: " + newDeviceToken);
		}

		public override void FailedToRegisterForRemoteNotifications (NSApplication application, NSError error)
		{
			Console.WriteLine("Failed to register for notifications");
		}

		public override void ReceivedRemoteNotification (NSApplication application, NSDictionary userInfo)
		{
			Console.WriteLine("Received Remote Notification!");
		}
	}
}

