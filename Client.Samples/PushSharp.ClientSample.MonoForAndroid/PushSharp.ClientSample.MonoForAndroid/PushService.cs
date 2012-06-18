
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using PushSharp.Client.MonoForAndroid;

namespace PushSharp.ClientSample.MonoForAndroid
{
	//You must subclass this!
	[BroadcastReceiver(Permission = C2dmClient.GOOGLE_PERMISSION_C2DM_SEND)]
	[IntentFilter(new string[] { C2dmClient.GOOGLE_ACTION_C2DM_INTENT_RECEIVE },
		Categories = new string[] { "com.pushsharp.test" })]
	[IntentFilter(new string[] { C2dmClient.GOOGLE_ACTION_C2DM_INTENT_REGISTRATION },
		Categories = new string[] { "com.pushsharp.test" })]
	//[C2dmReceiver]
	//[C2dmReceiveIntentFilter("c2dmsharp.client.sample")]
	//[C2dmRegistrationIntentFilter("c2dmsharp.client.sample")]
	public class SampleBroadcastReceiver : C2dmBroadcastReceiver<PushService>
	{
	}

	[Service] //Must use the service tag
	public class PushService : C2dmService
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
			notification.SetLatestEventInfo(this,
				title,
				desc,
				PendingIntent.GetActivity(this, 0, uiIntent, 0));

			//Show the notification
			notificationManager.Notify(1, notification);
		}
	}
}

