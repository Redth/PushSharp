using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using PushSharp.Core;

namespace PushSharp.Facebook
{
	public class FacebookNotification : Notification
	{
		public static FacebookNotification ForSingleResult(FacebookMessageTransportResponse response, int resultIndex)
		{
			var result = new FacebookNotification();
			result.Tag = response.Message.Tag;
			result.RegistrationIds.Add(response.Message.RegistrationIds[resultIndex]);
			result.CallbackUrl = response.Message.CallbackUrl;
			result.Message = response.Message.Message;
			result.Category = response.Message.Category;
			result.NotificationType = response.Message.NotificationType;
			result.Title = response.Message.Title;
			return result;
		}

		public static FacebookNotification ForSingleRegistrationId(FacebookNotification msg, string registrationId)
		{
			var result = new FacebookNotification();
			result.Tag = msg.Tag;
			result.RegistrationIds.Add(registrationId);
			result.CallbackUrl = msg.CallbackUrl;
			result.Message = msg.Message;
			result.Category = msg.Category;
			result.Title = msg.Title;
			result.NotificationType = msg.NotificationType;
			return result;
		}

		public FacebookNotification()
		{
			this.RegistrationIds = new List<string>();
			this.CallbackUrl = null;
			this.Message = null;
			this.Category = null;
			this.Title = null;
			this.NotificationType = FacebookNotificationType.Notification;
		}

		public string CommandNotification
		{
			get
			{
				string v_Ret = null;

				switch (this.NotificationType)
				{
					case FacebookNotificationType.ApplicationRequest:
						v_Ret = "apprequests";
						break;
					case FacebookNotificationType.Wall:
						v_Ret = "feed";
						break;
					default:
						v_Ret = "notifications";
						break;
				}
				return v_Ret;
			}
		}

		/// <summary>
		/// Registration ID of the Device
		/// </summary>
		public List<string> RegistrationIds
		{
			get;
			set;
		}

		/// <summary>
		/// Callback url from notification (="href" parameter of Facebook Notification API)
		/// This url is called when user click on notification
		/// </summary>
		public string CallbackUrl { get; set; }

		/// <summary>
		/// Message of the notification (="template" parameter of Facebook Notification API)
		/// You can use @[UserID] to show user name in the message
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Message of the notification (="title" parameter of Facebook Notification API)
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Category of the message (="ref" parameter of Facebook Notification API)
		/// </summary>
		public string Category { get; set; }

		/// <summary>
		/// Notification type
		/// </summary>
		public FacebookNotificationType NotificationType { get; set; }

		internal string GetJson()
		{
			var json = new JObject();

			if (!string.IsNullOrEmpty(this.CallbackUrl))
				json["CallbackUrl"] = this.CallbackUrl;

			if (!string.IsNullOrEmpty(this.Message))
				json["Message"] = this.Message;

			if (!string.IsNullOrEmpty(this.Category))
				json["Category"] = this.Category;

			if (!string.IsNullOrEmpty(this.Title))
				json["Title"] = this.Title;

			json["NotificationType"] = this.NotificationType.ToString();

			return json.ToString();
		}

		public override string ToString()
		{
			return GetJson();
		}
	}

	public enum FacebookNotificationType
	{
		Notification,
		ApplicationRequest,
		Wall
	}
}
