using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Core;

namespace PushSharp.Firebase
{
	public class FcmNotification : INotification
	{
		public FcmNotification()
		{
			RegistrationIds = new List<string>();
			CollapseKey = null;
			Data = null;
		}

		public bool IsDeviceRegistrationIdValid()
		{
			return RegistrationIds != null && RegistrationIds.Any();
		}

		[JsonIgnore]
		public object Tag { get; set; }

		[JsonIgnore]
		public string MessageId { get; internal set; }

		/// <summary>
		/// Registration ID or Group/Topic to send notification to.  Overrides RegsitrationIds.
		/// </summary>
		[JsonProperty("to")]
		public string To { get; set; }

		/// <summary>
		/// Registration ID of the Device(s).  Maximum of 1000 registration Id's per notification.
		/// </summary>
		[JsonProperty("registration_ids")]
		public List<string> RegistrationIds { get; set; }

		/// <summary>
		/// This parameter specifies a logical expression of conditions that determine the message target.
		/// more info 
		/// </summary>
		[JsonProperty("condition")]
		public string Condition { get; set; }

		/// <summary>
		/// Only the latest message with the same collapse key will be delivered
		/// </summary>
		[JsonProperty("collapse_key")]
		public string CollapseKey { get; set; }

		/// <summary>
		/// Corresponds to iOS APNS priorities (Normal is 5 and high is 10).  Default is Normal.
		/// </summary>
		/// <value>The priority.</value>
		[JsonProperty("priority"), JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
		public FcmNotificationPriority? Priority { get; set; }

		/// <summary>
		/// On iOS, use this field to represent content-available in the APNS payload. When a notification or message is sent and this is set to true, an inactive client app is awoken. On Android, data messages wake the app by default. On Chrome, currently not supported.
		/// </summary>
		/// <value>The content available.</value>
		[JsonProperty("content_available")]
		public bool? ContentAvailable { get; set; }

		/// <summary>
		/// Time in seconds that a message should be kept on the server if the device is offline.  Default (and maximum) is 4 weeks.
		/// </summary>
		[JsonProperty("time_to_live")]
		public int? TimeToLive { get; set; }

		/// <summary>
		/// A string containing the package name of your application. When set, messages will only be sent to registration IDs that match the package name
		/// </summary>
		[JsonProperty("restricted_package_name")]
		public string RestrictedPackageName { get; set; }

		/// <summary>
		/// If true, dry_run attribute will be sent in payload causing the notification not to be actually sent, but the result returned simulating the message
		/// </summary>
		[JsonProperty("dry_run")]
		public bool? DryRun { get; set; }

		/// <summary>
		/// JSON Payload to be sent in the message
		/// </summary>
		[JsonProperty("data")]
		public JObject Data { get; set; }

		/// <summary>
		/// Notification JSON payload, More info https://firebase.google.com/docs/cloud-messaging/http-server-ref#notification-payload-support
		/// </summary>
		/// <value>The notification payload.</value>
		[JsonProperty("notification")]
		public JObject Notification { get; set; }


		internal string GetJson()
		{
			// If 'To' was used instead of RegistrationIds, let's make RegistrationId's null
			// so we don't serialize an empty array for this property
			// otherwise, google will complain that we specified both instead
			if (RegistrationIds != null && RegistrationIds.Count <= 0 && !string.IsNullOrEmpty(To))
				RegistrationIds = null;

			// Ignore null values
			return JsonConvert.SerializeObject(this,
					new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
		}

		public override string ToString()
		{
			return GetJson();
		}

		public FcmNotification Clone(string registrationID)
		{
			return new FcmNotification
			{
				Tag = Tag,
				MessageId = MessageId,
				Condition = Condition,
				Notification = Notification,
				RestrictedPackageName = RestrictedPackageName,
				TimeToLive = TimeToLive,
				RegistrationIds = new List<string> { registrationID },
				CollapseKey = CollapseKey,
				Data = Data,
				ContentAvailable = ContentAvailable,
				DryRun = DryRun,
				Priority = Priority,
				To = To
			};
		}
	}

	public enum FcmNotificationPriority
	{
		[EnumMember(Value = "normal")]
		Normal = 5,
		[EnumMember(Value = "high")]
		High = 10
	}
}