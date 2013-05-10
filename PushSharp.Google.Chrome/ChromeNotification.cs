using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using PushSharp.Core;

namespace PushSharp.Google.Chrome
{
	public class ChromeNotification : Notification
	{
		public ChromeNotification()
		{
			this.ChannelId = string.Empty;
			this.SubChannelId = ChromeNotificationSubChannel.SubChannel0;
			this.Payload = string.Empty;
		}

		/// <summary>
		/// Channel ID of the Device
		/// </summary>
		public string ChannelId	{ get; set;	}
		public ChromeNotificationSubChannel SubChannelId { get; set; }
		public string Payload { get; set; }

		public override string ToString()
		{
			return string.Format ("{0}:{1} -> {2}", ChannelId, (int)(SubChannelId), Payload);
		}
	}

	public enum ChromeNotificationSubChannel
	{
		SubChannel0 = 0,
		SubChannel1 = 1,
		SubChannel2 = 2,
		SubChannel3 = 3
	}
}
