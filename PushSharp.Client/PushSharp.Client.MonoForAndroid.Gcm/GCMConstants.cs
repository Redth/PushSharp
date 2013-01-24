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
	public class GCMConstants
	{
		public const string INTENT_TO_GCM_REGISTRATION = "com.google.android.c2dm.intent.REGISTER";

		/**
		* Intent sent to GCM to unregister the application.
		*/
		public const string INTENT_TO_GCM_UNREGISTRATION = "com.google.android.c2dm.intent.UNREGISTER";

		/**
		* Intent sent by GCM indicating with the result of a registration request.
		*/
		public const string INTENT_FROM_GCM_REGISTRATION_CALLBACK = "com.google.android.c2dm.intent.REGISTRATION";

		/**
		* Intent used by the GCM library to indicate that the registration call
		* should be retried.
		*/
		public const string INTENT_FROM_GCM_LIBRARY_RETRY = "com.google.android.gcm.intent.RETRY";

		/**
		* Intent sent by GCM containing a message.
		*/
		public const string INTENT_FROM_GCM_MESSAGE = "com.google.android.c2dm.intent.RECEIVE";

		/**
		* Extra used on {@link #INTENT_TO_GCM_REGISTRATION} to indicate the sender
		* account (a Google email) that owns the application.
		*/
		public const string EXTRA_SENDER = "sender";

		/**
		* Extra used on {@link #INTENT_TO_GCM_REGISTRATION} to get the application
		* id.
		*/
		public const string EXTRA_APPLICATION_PENDING_INTENT = "app";

		/**
		* Extra used on {@link #INTENT_FROM_GCM_REGISTRATION_CALLBACK} to indicate
		* that the application has been unregistered.
		*/
		public const string EXTRA_UNREGISTERED = "unregistered";

		/**
		* Extra used on {@link #INTENT_FROM_GCM_REGISTRATION_CALLBACK} to indicate
		* an error when the registration fails. See constants starting with ERROR_
		* for possible values.
		*/
		public const string EXTRA_ERROR = "error";

		/**
		* Extra used on {@link #INTENT_FROM_GCM_REGISTRATION_CALLBACK} to indicate
		* the registration id when the registration succeeds.
		*/
		public const string EXTRA_REGISTRATION_ID = "registration_id";

		/**
		* Type of message present in the {@link #INTENT_FROM_GCM_MESSAGE} intent.
		* This extra is only set for special messages sent from GCM, not for
		* messages originated from the application.
		*/
		public const string EXTRA_SPECIAL_MESSAGE = "message_type";

		/**
		* Special message indicating the server deleted the pending messages.
		*/
		public const string VALUE_DELETED_MESSAGES = "deleted_messages";

		/**
		* Number of messages deleted by the server because the device was idle.
		* Present only on messages of special type
		* {@link #VALUE_DELETED_MESSAGES}
		*/
		public const string EXTRA_TOTAL_DELETED = "total_deleted";

		/**
		* Permission necessary to receive GCM intents.
		*/
		public const string PERMISSION_GCM_INTENTS = "com.google.android.c2dm.permission.SEND";

		/**
		* @see GCMBroadcastReceiver
		*/
		public const string DEFAULT_INTENT_SERVICE_CLASS_NAME = ".GCMIntentService";

		/**
		* The device can't read the response, or there was a 500/503 from the
		* server that can be retried later. The application should use exponential
		* back off and retry.
		*/
		public const string ERROR_SERVICE_NOT_AVAILABLE = "SERVICE_NOT_AVAILABLE";

		/**
		* There is no Google account on the phone. The application should ask the
		* user to open the account manager and add a Google account.
		*/
		public const string ERROR_ACCOUNT_MISSING = "ACCOUNT_MISSING";

		/**
		* Bad password. The application should ask the user to enter his/her
		* password, and let user retry manually later. Fix on the device side.
		*/
		public const string ERROR_AUTHENTICATION_FAILED = "AUTHENTICATION_FAILED";

		/**
		* The request sent by the phone does not contain the expected parameters.
		* This phone doesn't currently support GCM.
		*/
		public const string ERROR_INVALID_PARAMETERS = "INVALID_PARAMETERS";
		/**
		* The sender account is not recognized. Fix on the device side.
		*/
		public const string ERROR_INVALID_SENDER = "INVALID_SENDER";

		/**
		* Incorrect phone registration with Google. This phone doesn't currently
		* support GCM.
		*/
		public const string ERROR_PHONE_REGISTRATION_ERROR = "PHONE_REGISTRATION_ERROR";

	}
}