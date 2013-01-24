
using System;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using PushSharp.Client;

//VERY VERY VERY IMPORTANT NOTE!!!!
// Your package name MUST NOT start with an uppercase letter.
// Android does not allow permissions to start with an upper case letter
// If it does you will get a very cryptic error in logcat and it will not be obvious why you are crying!
// So please, for the love of all that is kind on this earth, use a LOWERCASE first letter in your Package Name!!!!
[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")] //, ProtectionLevel = Android.Content.PM.Protection.Signature)]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]

[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]

namespace PushSharp.ClientSample.MonoForAndroid
{
	//You must subclass this!
	[BroadcastReceiver(Permission = PushClient.GOOGLE_PERMISSION_C2DM_SEND)]
	[IntentFilter(new string[] { PushClient.GOOGLE_ACTION_C2DM_INTENT_RECEIVE }, Categories = new string[] { "@PACKAGE_NAME@" })]
	[IntentFilter(new string[] { PushClient.GOOGLE_ACTION_C2DM_INTENT_REGISTRATION }, Categories = new string[] { "com.pushsharp.test" })]
	public class PushHandlerBroadcastReceiver : PushHandlerBroadcastReceiverBase<PushService>
	{
	}

	[Service] //Must use the service tag
	public class PushService : PushHandlerServiceBase
	{
		public override void OnRegistrationError(Exception ex)
		{
			Log.Error("C2DM-Sharp-Service", "Registration Failed: " + ex.Message);

			//Create notification or do something useful
		}

		public override void OnRegistered(string registrationId)
		{
			Log.Info("C2DM-Sharp-Service", "Registered: " + registrationId);
			//Send back to the server
			//	var wc = new WebClient();
			//	var result = wc.UploadString("http://your.server.com/api/register/", "POST", 
			//		"{ 'registrationId' : '" + registrationId + "' }");

			createNotification("C2DM# Registered...", "The device has been Registered, Tap to View!");
		}

		public override void OnUnregistered(string lastRegistrationId)
		{
			Log.Info("C2DM-Sharp-Service", "Unregistered: " + lastRegistrationId);
			//Remove from the web service
			//	var wc = new WebClient();
			//	var result = wc.UploadString("http://your.server.com/api/unregister/", "POST",
			//		"{ 'registrationId' : '" + lastRegistrationId + "' }");

			createNotification("C2DM# Unregistered...", "The device has been unregistered, Tap to View!");
		}

		public override void OnMessageReceived(Bundle extras)
		{
			Log.Info("C2DM-Sharp-Service", "Message Received!");

			var msg = new StringBuilder();
			foreach (var key in extras.KeySet())
				msg.AppendLine(key + "=" + extras.Get(key).ToString());

			//Store the message
			var prefs = GetSharedPreferences("c2dm.client.sample", FileCreationMode.Private);
			var edit = prefs.Edit();
			edit.PutString("last_msg", msg.ToString());
			edit.Commit();

			createNotification("C2DM# Msg Rec'd", "Message Received for C2DM-Sharp... Tap to View!");
		}

		void createNotification(string title, string desc)
		{
			//Create notification
			var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

			//Create an intent to show ui
			var uiIntent = new Intent(this, typeof(DefaultActivity));

			//Create the notification
			var notification = new Notification(Android.Resource.Drawable.SymActionEmail, title);

			//Auto cancel will remove the notification once the user touches it
			notification.Flags = NotificationFlags.AutoCancel;

			//Set the notification info
			//we use the pending intent, passing our ui intent over which will get called
			//when the notification is tapped.
			notification.SetLatestEventInfo(this, title, desc, PendingIntent.GetActivity(this, 0, uiIntent, 0));

			//Show the notification
			notificationManager.Notify(1, notification);
		}
	}
}

