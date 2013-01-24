
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

namespace PushSharp.Client
{
	public class PushClient
	{
		/// <summary>
		/// Incoming Intents have this action which indicates a Message was received
		/// </summary>
		public const string GOOGLE_ACTION_C2DM_INTENT_RECEIVE = "com.google.android.c2dm.intent.RECEIVE";

		/// <summary>
		/// Incoming Intents have this action which indicates Registration results
		/// </summary>
		public const string GOOGLE_ACTION_C2DM_INTENT_REGISTRATION = "com.google.android.c2dm.intent.REGISTRATION";

		/// <summary>
		/// Use this intent to start a register request
		/// </summary>
		public const string GOOGLE_ACTION_C2DM_INTENT_REGISTER = "com.google.android.c2dm.intent.REGISTER";

		/// <summary>
		/// Use this intent to start an unregister request
		/// </summary>
		public const string GOOGLE_ACTION_C2DM_INTENT_UNREGISTER = "com.google.android.c2dm.intent.UNREGISTER";

		/// <summary>
		/// Permission that the BroadcastReceiver needs
		/// </summary>
		public const string GOOGLE_PERMISSION_C2DM_SEND = "com.google.android.c2dm.permission.SEND";

		/// <summary>
		/// Register's the Device for C2DM Messages
		/// </summary>
		/// <param name="context">Context</param>
		/// <param name="senderIdEmail">Email address whitelisted as the Sender ID for your App</param>
		public static void Register(Context context, string senderIdEmail)
		{
			//Create our intent, with a pending intent to our app's broadcast
			Intent registrationIntent = new Intent(GOOGLE_ACTION_C2DM_INTENT_REGISTER);
			registrationIntent.PutExtra("app", PendingIntent.GetBroadcast(context, 0, new Intent(), 0));
			registrationIntent.PutExtra("sender", senderIdEmail);

			//Start intent
			context.StartService(registrationIntent);
		}

		/// <summary>
		/// Unregisters the Device from C2DM Messages
		/// </summary>
		/// <param name="context">Context</param>
		public static void UnRegister(Context context)
		{
			Intent unregIntent = new Intent(GOOGLE_ACTION_C2DM_INTENT_UNREGISTER);
			unregIntent.PutExtra("app", PendingIntent.GetBroadcast(context, 0, new Intent(), 0));
			context.StartService(unregIntent);
		}

		public static string GetRegistrationId(Context context)
		{
			var result = string.Empty;

			//Get the shared pref for c2dmsharp,  and read the registration id
			var prefs = context.GetSharedPreferences("c2dmsharp", FileCreationMode.Private);
			result = prefs.GetString("registration_id", string.Empty);

			return result;
		}
	}
}

