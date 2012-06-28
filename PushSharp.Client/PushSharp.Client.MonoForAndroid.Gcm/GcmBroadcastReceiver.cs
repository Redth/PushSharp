
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

namespace PushSharp.Client.MonoForAndroid
{
	public class C2dmBroadcastReceiver<TService> : BroadcastReceiver where TService : Service
	{
		public C2dmBroadcastReceiver()
			: base()
		{ }

		public override void OnReceive(Context context, Intent intent)
		{
			var c2dmServiceIntent = new Intent(context, typeof(TService));
			c2dmServiceIntent.PutExtras(intent.Extras);
			c2dmServiceIntent.PutExtra("c2dm_action", intent.Action);

			//Start our service
			context.StartService(c2dmServiceIntent);
		}
	}

	//For future possible use...
	//public class C2dmReceiverAttribute : BroadcastReceiverAttribute
	//{
	//    public C2dmReceiverAttribute()
	//        : base()
	//    {
	//        Permission = C2dmClient.GOOGLE_PERMISSION_C2DM_SEND;
	//    }
	//}

	//public class C2dmReceiveIntentFilterAttribute : IntentFilterAttribute
	//{
	//    public C2dmReceiveIntentFilterAttribute(string packageName)
	//        : base(new string[] { C2dmClient.GOOGLE_ACTION_C2DM_INTENT_RECEIVE })
	//    {
	//        Categories = new string[] { packageName };
	//    }
	//}

	//public class C2dmRegistrationIntentFilterAttribute : IntentFilterAttribute
	//{
	//    public C2dmRegistrationIntentFilterAttribute(string packageName)
	//        : base(new string[] { C2dmClient.GOOGLE_ACTION_C2DM_INTENT_REGISTRATION })
	//    {
	//        Categories = new string[] { packageName };
	//    }
	//}
}

