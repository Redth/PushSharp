﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json.Linq;
using PushSharp.Core;

namespace PushSharp.Android
{
	public class GcmNotification : Notification
	{
		public static GcmNotification ForSingleResult(GcmMessageTransportResponse response, int resultIndex)
		{
			var result = new GcmNotification();
			result.Tag = response.Message.Tag;
			result.RegistrationIds.Add(response.Message.RegistrationIds[resultIndex]);
			result.CollapseKey = response.Message.CollapseKey;
			result.JsonData = response.Message.JsonData;
			result.DelayWhileIdle = response.Message.DelayWhileIdle;
			return result;
		}

		public static GcmNotification ForSingleRegistrationId(GcmNotification msg, string registrationId)
		{
			var result = new GcmNotification();
			result.Tag = msg.Tag;
			result.RegistrationIds.Add(registrationId);
			result.CollapseKey = msg.CollapseKey;
			result.JsonData = msg.JsonData;
			result.DelayWhileIdle = msg.DelayWhileIdle;
			return result;
		}

		public GcmNotification()
		{
			this.RegistrationIds = new List<string>();
			this.CollapseKey = string.Empty;
			this.JsonData = string.Empty;
			this.DelayWhileIdle = null;
		}

		/// <summary>
		/// Registration ID of the Device(s).  Maximum of 1000 registration Id's per notification.
		/// </summary>
		public List<string> RegistrationIds
		{
			get;
			set;
		}

		/// <summary>
		/// Only the latest message with the same collapse key will be delivered
		/// </summary>
		public string CollapseKey
		{
			get;
			set;
		}

		/// <summary>
		/// JSON Payload to be sent in the message
		/// </summary>
		public string JsonData
		{
			get;
			set;
		}

		/// <summary>
		/// If true, GCM will only be delivered once the device's screen is on
		/// </summary>
		public bool? DelayWhileIdle
		{
			get;
			set;
		}

		/// <summary>
		/// Time in seconds that a message should be kept on the server if the device is offline.  Default is 4 weeks.
		/// </summary>
		public int? TimeToLive
		{
			get;
			set;
		}

		/// <summary>
		/// If true, dry_run attribute will be sent in payload causing the notification not to be actually sent, but the result returned simulating the message
		/// </summary>
		public bool? DryRun 
		{ 
			get; 
			set; 
		}

		internal string GetJson()
		{
			var json = new JObject();

			if (!string.IsNullOrEmpty(this.CollapseKey))
				json["collapse_key"] = this.CollapseKey;
			
			if (this.TimeToLive.HasValue)
				json["time_to_live"] = this.TimeToLive.Value;

			json["registration_ids"] = new JArray(this.RegistrationIds.ToArray());
				
			if (this.DelayWhileIdle.HasValue)
				json["delay_while_idle"] = this.DelayWhileIdle.Value;

			if (DryRun.HasValue && DryRun.Value)
				json["dry_run"] = true;

			if (!string.IsNullOrEmpty(this.JsonData))
			{
				var jsonData = JObject.Parse(this.JsonData);

				if (jsonData != null)
					json["data"] = jsonData;
			}

			return json.ToString();
		}

		public override string ToString()
		{
			return GetJson();
		}
	}
}
