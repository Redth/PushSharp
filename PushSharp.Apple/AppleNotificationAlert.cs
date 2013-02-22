using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Newtonsoft.Json.Linq;

namespace PushSharp.Apple
{
	/// <summary>
	/// Alert Portion of the Notification Payload
	/// </summary>
	public class AppleNotificationAlert
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public AppleNotificationAlert()
		{
			Body = null;
			ActionLocalizedKey = null;
			LocalizedKey = null;
			LocalizedArgs = new List<object>();
            LaunchImage = null;
		}

		/// <summary>
		/// Body Text of the Notification's Alert
		/// </summary>
		public string Body
		{
			get;
			set;
		}

		/// <summary>
		/// Action Button's Localized Key
		/// </summary>
		public string ActionLocalizedKey
		{
			get;
			set;
		}

		/// <summary>
		/// Localized Key
		/// </summary>
		public string LocalizedKey
		{
			get;
			set;
		}

		/// <summary>
		/// Localized Argument List
		/// </summary>
		public List<object> LocalizedArgs
		{
			get;
			set;
		}

		public void AddLocalizedArgs(params object[] values)
		{
			this.LocalizedArgs.AddRange(values);
		}

        /// <summary>
        /// Filename of the Lauch Image (with ou without extension)
        /// >= iOS 4.0
        /// </summary>
        public string LaunchImage
        {
            get;
            set;
        }

		/// <summary>
		/// Determines if the Alert is empty and should be excluded from the Notification Payload
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				if (!string.IsNullOrEmpty(Body)
					|| !string.IsNullOrEmpty(ActionLocalizedKey)
					|| !string.IsNullOrEmpty(LocalizedKey)
					|| (LocalizedArgs != null && LocalizedArgs.Count > 0)
                    || !string.IsNullOrEmpty(LaunchImage)
                    )
					return false;
				else
					return true;
			}
		}
	}
}
