using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using PushSharp.Common;

namespace PushSharp.Android
{
	public class GcmNotification : Notification
	{
		public GcmNotification()
		{
			this.Platform = PlatformType.Android;

			this.RegistrationIds = new List<string>();
			this.CollapseKey = string.Empty;
			this.JsonData = string.Empty;
			this.DelayWhileIdle = null;
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
		/// If true, C2DM will only be delivered once the device's screen is on
		/// </summary>
		public bool? DelayWhileIdle
		{
			get;
			set;
		}

		internal string GetJson()
		{
			return string.Empty;
		}

		public override string ToString()
		{
			return GetJson();
		}

		internal int GetMessageSize()
		{
			////http://groups.google.com/group/android-c2dm/browse_thread/thread/c70575480be4f883?pli=1
			//// suggests that the max size of 1024 bytes only includes 
			//// only char counts of:  keys, values, and the collapse_data value
			//int size = HttpUtility.UrlEncode(this.CollapseKey).Length;

			//foreach (var key in this.Data.AllKeys)
			//    size += HttpUtility.UrlEncode(key).Length + HttpUtility.UrlEncode(this.Data[key]).Length;

			//return size;
			return 0;
		}
	}
}
