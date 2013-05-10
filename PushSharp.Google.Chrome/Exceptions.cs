using System;
using System.Collections.Generic;
using System.Text;

namespace PushSharp.Google.Chrome
{
	public class NotificationFailureException : Exception
	{

		public NotificationFailureException(string errorMessage, ChromeNotification notification) : base() 
		{
			this.ErrorMessage = errorMessage;
			this.Notification = notification;
		}

		public ChromeNotification Notification { get; set; }

		public string ErrorMessage { get; set; }

		public override string ToString()
		{
			var nstr = string.Empty;

			if (Notification != null)
				nstr = Notification.ToString();

			return string.Format("Chrome NotificationFailureException -> {0} : {1}", ErrorMessage, nstr);
		}
	}

}
