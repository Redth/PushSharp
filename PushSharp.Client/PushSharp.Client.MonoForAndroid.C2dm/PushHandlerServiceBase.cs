
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
	public class PushHandlerServiceBase : IntentService
	{
		public PushHandlerServiceBase()
			: base()
		{
		}

		protected override void OnHandleIntent(Intent intent)
		{
			try
			{
				//See what the action is
				var c2dmAction = intent.GetStringExtra("c2dm_action");

				//We need an action otherwise don't know what to do
				if (string.IsNullOrEmpty(c2dmAction))
					return;

				//Handle the c2dm intent, decide which it is
				if (c2dmAction == PushClient.GOOGLE_ACTION_C2DM_INTENT_REGISTRATION)
				{
					var registrationId = intent.GetStringExtra("registration_id");

					if (!string.IsNullOrEmpty(registrationId))
					{
						//Get the shared preferences, editor, and save the id
						var prefs = GetSharedPreferences("c2dmsharp", FileCreationMode.Private);
						var editor = prefs.Edit();
						editor.PutString("registration_id", registrationId);
						editor.Commit();

						//Call our base method
						this.OnRegistered(registrationId);
						return;
					}

					var unregistered = intent.GetStringExtra("unregistered");

					if (!string.IsNullOrEmpty(unregistered))
					{
						//Get the shared preferences, last id, editor, and clear the id
						var prefs = GetSharedPreferences("c2dmsharp", FileCreationMode.Private);
						var lastRegistrationId = prefs.GetString("registration_id", string.Empty);
						var editor = prefs.Edit();
						editor.PutString("registration_id", string.Empty);
						editor.Commit();

						this.OnUnregistered(lastRegistrationId);
						return;
					}

					var error = intent.GetStringExtra("error");
					if (!string.IsNullOrEmpty(error))
					{
						this.OnRegistrationError(new C2dmRegistrationError(error, C2dmRegistrationError.GetErrorDescription(error)));
						return;
					}
				}
				else if (c2dmAction == PushClient.GOOGLE_ACTION_C2DM_INTENT_RECEIVE)
				{
					this.OnMessageReceived(intent.Extras);
				}

			}
			catch (Exception ex)
			{
				Android.Util.Log.Error("C2DM-Sharp-BaseService", "Error Processing Intent: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		//These methods are meant to be overridden by the referencing project
		public virtual void OnRegistered(string registrationId)
		{
		}

		public virtual void OnUnregistered(string lastRegistrationId)
		{
		}

		public virtual void OnMessageReceived(Bundle extras)
		{
		}

		public virtual void OnRegistrationError(Exception ex)
		{
		}
	}
}

