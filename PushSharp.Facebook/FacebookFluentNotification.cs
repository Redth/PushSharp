using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Facebook;

namespace PushSharp
{
	public static class FacebookFluentNotification
	{
		public static FacebookNotification ForDeviceRegistrationId(this FacebookNotification n, string deviceRegistrationId)
		{
			n.RegistrationIds.Add(deviceRegistrationId);
			return n;
		}

		public static FacebookNotification ForDeviceRegistrationId(this FacebookNotification n, IEnumerable<string> deviceRegistrationIds)
		{
			n.RegistrationIds.AddRange(deviceRegistrationIds);
			return n;
		}


		public static FacebookNotification ForWallMessage(this FacebookNotification n, string message)
		{
			n.NotificationType = FacebookNotificationType.Wall;
			n.Message = message;
			return n;
		}

		public static FacebookNotification ForApplicationRequest(this FacebookNotification n, string message, string title = null)
		{
			n.NotificationType = FacebookNotificationType.ApplicationRequest;
			n.Message = message;
			n.Title = title;
			return n;
		}

		public static FacebookNotification ForNotification(this FacebookNotification n, string message, string callBackUrl, string category = null)
		{
			n.NotificationType = FacebookNotificationType.Notification;
			n.Message = message;
			n.Category = category;
			n.CallbackUrl = callBackUrl;
			return n;
		}

		public static FacebookNotification WithCallbackUrl(this FacebookNotification n, string callbackUrl)
		{
			n.CallbackUrl = callbackUrl;
			return n;
		}

		public static FacebookNotification WithMessage(this FacebookNotification n, string message)
		{
			n.Message = message;
			return n;
		}

		public static FacebookNotification WithCategory(this FacebookNotification n, string category)
		{
			n.Category = category;
			return n;
		}

		public static FacebookNotification WithNotificationType(this FacebookNotification n, FacebookNotificationType notificationType)
		{
			n.NotificationType = notificationType;
			return n;
		}

	}
}
