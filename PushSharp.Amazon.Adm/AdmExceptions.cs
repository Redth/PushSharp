using System;
using System.Collections.Generic;
using System.Text;

namespace PushSharp.Amazon.Adm
{
	public class AdmMessageTooLargeException : Exception
	{
		public AdmMessageTooLargeException(AdmNotification notification, string msg) : base(msg) 
		{
			Notification = notification;
		}

		public AdmNotification Notification { get; private set; }
	}

	public class AdmRateLimitExceededException : Exception
	{
		public AdmRateLimitExceededException(AdmNotification notification, string msg) : base(msg) 
		{
			Notification = notification;
		}

		public AdmNotification Notification { get; private set; }
	}

	public class AdmSendException : Exception
	{
		public AdmSendException(AdmNotification notification, string msg) : base(msg) 
		{
			Notification = notification;
		}

		public AdmNotification Notification { get; private set; }
	}

}
