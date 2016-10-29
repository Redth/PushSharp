using System;
using System.Collections.Generic;
using PushSharp.Core;

namespace PushSharp.Firebase
{
	public class FcmNotificationException : NotificationException
	{
		public FcmNotificationException(FcmNotification notification, string msg) : base(msg, notification)
		{
			Notification = notification;
		}

		public FcmNotificationException(FcmNotification notification, string msg, string description) : base(msg, notification)
		{
			Notification = notification;
			Description = description;
		}

		public new FcmNotification Notification { get; private set; }
		public string Description { get; private set; }
	}

	public class FcmMulticastResultException : Exception
	{
		public FcmMulticastResultException() : base("One or more Registration Id's failed in the multicast notification")
		{
			Succeeded = new List<FcmNotification>();
			Failed = new Dictionary<FcmNotification, Exception>();
		}

		public List<FcmNotification> Succeeded { get; set; }

		public Dictionary<FcmNotification, Exception> Failed { get; set; }
	}
}

