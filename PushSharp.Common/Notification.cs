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
	}

}
