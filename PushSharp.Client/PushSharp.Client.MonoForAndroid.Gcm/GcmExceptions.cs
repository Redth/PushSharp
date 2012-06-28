
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
	public class NoGoogleAccountsOnDeviceRegistrationException : Exception
	{
		public NoGoogleAccountsOnDeviceRegistrationException()
			: base("No Google Accounts are setup on this device!")
		{
		}
	}

	public class C2dmRegistrationError : Exception
	{
		public static string GetErrorDescription(string errorCode)
		{
			string description = string.Empty;

			switch (errorCode.ToUpper().Trim())
			{
				case "SERVICE_NOT_AVAILABLE":
					description = "Registration Server Temporarily Unavailable.  Try again later.";
					break;
				case "ACCOUNT_MISSING":
					description = "No Google Accounts exist on the Device.";
					break;
				case "AUTHENTICATION_FAILED":
					description = "Invalid Password, Authentication Failed";
					break;
				case "TOO_MANY_REGISTRATIONS":
					description = "Too many applications registered with C2DM";
					break;
				case "INVALID_SENDER":
					description = "Google Account sender is not recognized";
					break;
				case "PHONE_REGISTRATION_ERROR":
					description = "Phone does not support C2DM";
					break;
			}

			return description;
		}


		public C2dmRegistrationError(string errorCode, string message)
			: base(message)
		{
			this.ErrorCode = errorCode;
		}

		public string ErrorCode
		{
			get;
			set;
		}
	}
}

