using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using PushSharp.Common;

namespace PushSharp.Android
{
	public class AndroidNotification : Notification
	{
		public AndroidNotification()
		{
			this.Platform = PlatformType.Android;

			this.RegistrationId = string.Empty;
			this.CollapseKey = string.Empty;
			this.Data = new NameValueCollection();
			this.DelayWhileIdle = null;
		}

		/// <summary>
		/// Registration ID of the Device
		/// </summary>
		public string RegistrationId
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
		/// Key/Value pairs to be sent to the Device (as extras in the Intent)
		/// </summary>
		public NameValueCollection Data
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

		internal string GetPostData()
		{
			var sb = new StringBuilder();

			sb.AppendFormat("registration_id={0}&collapse_key={1}&", //&auth={2}&",
				HttpUtility.UrlEncode(this.RegistrationId),
				HttpUtility.UrlEncode(this.CollapseKey)
				//HttpUtility.UrlEncode(this.GoogleLoginAuthorizationToken)
				);

			if (this.DelayWhileIdle.HasValue)
				sb.AppendFormat("delay_while_idle={0}&", this.DelayWhileIdle.Value ? "true" : "false");

			foreach (var key in this.Data.AllKeys)
			{
				sb.AppendFormat("data.{0}={1}&",
					HttpUtility.UrlEncode(key),
					HttpUtility.UrlEncode(this.Data[key]));
			}

			//Remove trailing & if necessary
			if (sb.Length > 0 && sb[sb.Length - 1] == '&')
				sb.Remove(sb.Length - 1, 1);

			return sb.ToString();
		}

		internal int GetMessageSize()
		{
			//http://groups.google.com/group/android-c2dm/browse_thread/thread/c70575480be4f883?pli=1
			// suggests that the max size of 1024 bytes only includes 
			// only char counts of:  keys, values, and the collapse_data value
			int size = HttpUtility.UrlEncode(this.CollapseKey).Length;

			foreach (var key in this.Data.AllKeys)
				size += HttpUtility.UrlEncode(key).Length + HttpUtility.UrlEncode(this.Data[key]).Length;

			return size;
		}
	}
}
