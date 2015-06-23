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
	public class PushHandlerBroadcastReceiverBase<TIntentService> : BroadcastReceiver where TIntentService : PushHandlerServiceBase
	{
		const string TAG = "PushHandlerBroadcastReceiver";

		public override void OnReceive(Context context, Intent intent)
		{
			Log.Verbose(TAG, "OnReceive: " + intent.Action);
			var className = context.PackageName + GCMConstants.DEFAULT_INTENT_SERVICE_CLASS_NAME;

			Log.Verbose(TAG, "GCM IntentService Class: " + className);

			PushHandlerServiceBase.RunIntentInService(context, intent, typeof(TIntentService));
		}
	}
}