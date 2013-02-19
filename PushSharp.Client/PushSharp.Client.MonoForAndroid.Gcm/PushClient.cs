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

namespace PushSharp.Client
{
	public class PushClient
	{
		const string TAG = "GCMRegistrar";
		const string BACKOFF_MS = "backoff_ms";
		const string GSF_PACKAGE = "com.google.android.gsf";
		const string PREFERENCES = "com.google.android.gcm";
		const int DEFAULT_BACKOFF_MS = 3000;
		const string PROPERTY_REG_ID = "regId";
		const string PROPERTY_APP_VERSION = "appVersion";
		const string PROPERTY_ON_SERVER = "onServer";

		//static GCMBroadcastReceiver sRetryReceiver;

		public static void CheckDevice(Context context)
		{
			var version = (int)Build.VERSION.SdkInt;
			if (version < 8)
				throw new InvalidOperationException("Device must be at least API Level 8 (instead of " + version + ")");

			var packageManager = context.PackageManager;

			try 
			{
				packageManager.GetPackageInfo(GSF_PACKAGE, 0); 
			}
			catch
			{
				throw new InvalidOperationException("Device does not have package " + GSF_PACKAGE);
			}
		}

		public static void CheckManifest(Context context)
		{
			var packageManager = context.PackageManager;
			var packageName = context.PackageName;
			var permissionName = packageName + ".permission.C2D_MESSAGE";

			try
			{
				packageManager.GetPermissionInfo(permissionName, Android.Content.PM.PackageInfoFlags.Permissions);
			}
			catch
			{
				throw new AccessViolationException("Application does not define permission: " + permissionName);
			}

			Android.Content.PM.PackageInfo receiversInfo;

			try
			{
				receiversInfo = packageManager.GetPackageInfo(packageName, Android.Content.PM.PackageInfoFlags.Receivers);
			}
			catch
			{
				throw new InvalidOperationException("Could not get receivers for package " + packageName);
			}

			var receivers = receiversInfo.Receivers;

			if (receivers == null || receivers.Count <= 0)
				throw new InvalidOperationException("No Receiver for package " + packageName);

			if (Log.IsLoggable(TAG, LogPriority.Verbose))
				Log.Verbose(TAG, "number of receivers for " + packageName + ": " + receivers.Count);

			var allowedReceivers = new HashSet<string>();

			foreach (var receiver in receivers)
			{
				if (GCMConstants.PERMISSION_GCM_INTENTS.Equals(receiver.Permission))
					allowedReceivers.Add(receiver.Name);
			}

			if (allowedReceivers.Count <= 0)
				throw new InvalidOperationException("No receiver allowed to receive " + GCMConstants.PERMISSION_GCM_INTENTS);

			CheckReceiver(context, allowedReceivers, GCMConstants.INTENT_FROM_GCM_REGISTRATION_CALLBACK);
			CheckReceiver(context, allowedReceivers, GCMConstants.INTENT_FROM_GCM_MESSAGE);
		}

		private static void CheckReceiver(Context context, HashSet<string> allowedReceivers, string action)
		{
			var pm = context.PackageManager;
			var packageName = context.PackageName;
			
			var intent = new Intent(action);
			intent.SetPackage(packageName);

			var receivers = pm.QueryBroadcastReceivers(intent, Android.Content.PM.PackageInfoFlags.IntentFilters);

			if (receivers == null || receivers.Count <= 0)
				throw new InvalidOperationException("No receivers for action " + action);

			if (Log.IsLoggable(TAG, LogPriority.Verbose))
				Log.Verbose(TAG, "Found " + receivers.Count + " receivers for action " + action);
			
			foreach (var receiver in receivers)
			{
				var name = receiver.ActivityInfo.Name;
				if (!allowedReceivers.Contains(name))
					throw new InvalidOperationException("Receiver " + name + " is not set with permission " + GCMConstants.PERMISSION_GCM_INTENTS);
			}
		}

		public static void Register(Context context, params string[] senderIds)
		{
			SetRetryBroadcastReceiver(context);
			ResetBackoff(context);

			internalRegister(context, senderIds);
		}

		internal static void internalRegister(Context context, params string[] senderIds)
		{
			if (senderIds == null || senderIds.Length <= 0)
				throw new ArgumentException("No senderIds");

			var senders = string.Join(",", senderIds);

			Log.Verbose(TAG, "Registering app " + context.PackageName + " of senders " + senders);

			var intent = new Intent(GCMConstants.INTENT_TO_GCM_REGISTRATION);
			intent.SetPackage(GSF_PACKAGE);
			intent.PutExtra(GCMConstants.EXTRA_APPLICATION_PENDING_INTENT,
				PendingIntent.GetBroadcast(context, 0, new Intent(), 0));
			intent.PutExtra(GCMConstants.EXTRA_SENDER, senders);
			
			context.StartService(intent);
		}

		public static void UnRegister(Context context)
		{
			SetRetryBroadcastReceiver(context);
			ResetBackoff(context);
			internalUnRegister(context);
		}

		internal static void internalUnRegister(Context context)
		{
			Log.Verbose(TAG, "Unregistering app " + context.PackageName);
			
			var intent = new Intent(GCMConstants.INTENT_TO_GCM_UNREGISTRATION);
			intent.SetPackage(GSF_PACKAGE);
			intent.PutExtra(GCMConstants.EXTRA_APPLICATION_PENDING_INTENT,
				PendingIntent.GetBroadcast(context, 0, new Intent(), 0));
			
			context.StartService(intent);
		}

		static void SetRetryBroadcastReceiver(Context context)
		{
			return;

			/*if (sRetryReceiver == null)
			{
				sRetryReceiver = new GCMBroadcastReceiver();
				var category = context.PackageName;

				var filter = new IntentFilter(GCMConstants.INTENT_FROM_GCM_LIBRARY_RETRY);
				filter.AddCategory(category);

				var permission = category + ".permission.C2D_MESSAGE";

				Log.Verbose(TAG, "Registering receiver");

				context.RegisterReceiver(sRetryReceiver, filter, permission, null);
			}*/
		}

		public static string GetRegistrationId(Context context)
		{
			var prefs = GetGCMPreferences(context);

			var registrationId = prefs.GetString(PROPERTY_REG_ID, "");

			int oldVersion = prefs.GetInt(PROPERTY_APP_VERSION, int.MinValue);
			int newVersion = GetAppVersion(context);

			if (oldVersion != int.MinValue && oldVersion != newVersion)
			{
				Log.Verbose(TAG, "App version changed from " + oldVersion + " to " + newVersion + "; resetting registration id");
				
				ClearRegistrationId(context);
				registrationId = string.Empty;
			}

			return registrationId;
		}

		public static bool IsRegistered(Context context)
		{
			var registrationId = GetRegistrationId(context);

			return !string.IsNullOrEmpty(registrationId);
		}

		internal static string ClearRegistrationId(Context context)
		{
			return SetRegistrationId(context, "");
		}

		internal static string SetRegistrationId(Context context, string registrationId)
		{
			var prefs = GetGCMPreferences(context);

			var oldRegistrationId = prefs.GetString(PROPERTY_REG_ID, "");
			int appVersion = GetAppVersion(context);
			Log.Verbose(TAG, "Saving registrationId on app version " + appVersion);
			var editor = prefs.Edit();
			editor.PutString(PROPERTY_REG_ID, registrationId);
			editor.PutInt(PROPERTY_APP_VERSION, appVersion);
			editor.Commit();
			return oldRegistrationId;
		}


		public static void SetRegisteredOnServer(Context context, bool flag)
		{
			var prefs = GetGCMPreferences(context);
			Log.Verbose(TAG, "Setting registered on server status as: " + flag);
			var editor = prefs.Edit();
			editor.PutBoolean(PROPERTY_ON_SERVER, flag);
			editor.Commit();
		}

		public static bool IsRegisteredOnServer(Context context)
		{
			var prefs = GetGCMPreferences(context);
			bool isRegistered = prefs.GetBoolean(PROPERTY_ON_SERVER, false);
			Log.Verbose(TAG, "Is registered on server: " + isRegistered);
			return isRegistered;
		}

		static int GetAppVersion(Context context)
		{
			try
			{
				var packageInfo = context.PackageManager.GetPackageInfo(context.PackageName, 0);
				return packageInfo.VersionCode;
			}
			catch
			{
				throw new InvalidOperationException("Could not get package name");
			}
		}

		internal static void ResetBackoff(Context context)
		{
			Log.Debug(TAG, "resetting backoff for " + context.PackageName);
			SetBackoff(context, DEFAULT_BACKOFF_MS);
		}

		internal static int GetBackoff(Context context)
		{
			var prefs = GetGCMPreferences(context);
			return prefs.GetInt(BACKOFF_MS, DEFAULT_BACKOFF_MS);
		}

		internal static void SetBackoff(Context context, int backoff)
		{
			var prefs = GetGCMPreferences(context);
			var editor = prefs.Edit();
			editor.PutInt(BACKOFF_MS, backoff);
			editor.Commit();
		}

		static ISharedPreferences GetGCMPreferences(Context context)
		{
			return context.GetSharedPreferences(PREFERENCES, FileCreationMode.Private);
		}
	}
}
