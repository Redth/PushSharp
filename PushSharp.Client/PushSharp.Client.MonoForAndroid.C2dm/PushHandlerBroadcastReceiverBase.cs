
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
	public class PushHandlerBroadcastReceiverBase<TService> : BroadcastReceiver where TService : PushHandlerServiceBase
	{
		public PushHandlerBroadcastReceiverBase()
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
}

