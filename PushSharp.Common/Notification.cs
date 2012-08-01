using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Common
{
	public abstract class Notification
	{
		public PlatformType Platform { get; set; }


        /// <summary>
        /// Gets or sets the tag. Can use to store miscellaneous information 
        /// you might need in a event you subscribe to. 
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public object Tag { get; set; }

		internal DateTime EnqueuedTimestamp { get; set; }

		/// <summary>
		/// How many times a message was queued for sending.  This counter will increase every time a message is queued or requeued.
		/// </summary>
		public int QueuedCount { get; set; }

		/// <summary>
		/// Does the message have a valid registration Id from what we can check client side?
		/// </summary>
		public virtual bool IsValidDeviceRegistrationId()
		{
			return true;
		}
	}

}
