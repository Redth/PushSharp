using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Util;
using PushSharp.Client;

namespace PushSharp.ClientSample.MonoForAndroid
{
	[Activity(Label = "C2DM-Sharp Sample", MainLauncher = true, 
		LaunchMode= Android.Content.PM.LaunchMode.SingleTask)]
	public class DefaultActivity : Activity
	{
		//NOTE: You need to put your own email here!
		// Whichever one you registered as the 'Role' email with google
		public const string senderIdEmail = "pushsharp@altusapps.com";


		TextView textRegistrationStatus = null;
		TextView textRegistrationId = null;
		TextView textLastMsg = null;
		Button buttonRegister = null;
		bool registered = false;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			textRegistrationStatus = FindViewById<TextView>(Resource.Id.textRegistrationStatus);
			textRegistrationId = FindViewById<TextView>(Resource.Id.textRegistrationId);
			textLastMsg = FindViewById<TextView>(Resource.Id.textLastMessage);
			buttonRegister = FindViewById<Button>(Resource.Id.buttonRegister);

			Log.Info("C2DM-Sharp-UI", "Hello World");

			this.buttonRegister.Click += delegate
			{
				if (!registered)
				{
					Log.Info("C2DM-Sharp", "Registering...");
					PushClient.Register(this, senderIdEmail);
				}
				else
				{
					Log.Info("C2DM-Sharp", "Unregistering...");
					PushClient.UnRegister(this);
				}

				RunOnUiThread(() =>
				{
					//Disable the button so that we can't click it again
					//until we get back to the activity from a notification
					this.buttonRegister.Enabled = false;
				});
			};			
		}

		protected override void OnResume()
		{
			base.OnResume();

			updateView();
		}

		void updateView()
		{
			//Get the stored latest registration id
			var registrationId = PushClient.GetRegistrationId(this);

			//If it's empty, we need to register
			if (string.IsNullOrEmpty(registrationId))
			{
				registered = false;
				this.textRegistrationStatus.Text = "Registered: No";
				this.textRegistrationId.Text = "Id: N/A";
				this.buttonRegister.Text = "Register...";

				Log.Info("C2DM-Sharp", "Not registered...");
			}
			else
			{
				registered = true;
				this.textRegistrationStatus.Text = "Registered: Yes";
				this.textRegistrationId.Text = "Id: " + registrationId;
				this.buttonRegister.Text = "Unregister...";

				Log.Info("C2DM-Sharp", "Already Registered: " + registrationId);
			}

			var prefs = GetSharedPreferences("c2dm.client.sample", FileCreationMode.Private);
			this.textLastMsg.Text = "Last Msg: " + prefs.GetString("last_msg", "N/A");

			//Enable the button as it was normally disabled
			this.buttonRegister.Enabled = true;
		}
	}
}


