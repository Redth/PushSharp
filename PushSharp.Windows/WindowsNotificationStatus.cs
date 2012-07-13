using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
	public class WindowsNotificationStatus
	{
		public string MessageID { get; set; }
		public string DebugTrace { get; set; }
		public string ErrorDescription { get; set; }

		public WindowsNotificationSendStatus NotificationStatus { get; set; }
		public WindowsDeviceConnectionStatus DeviceConnectionStatus { get; set; }
		
		public WindowsNotification Notification { get; set; }

		public System.Net.HttpStatusCode HttpStatus { get; set; }
	}

	public enum WindowsNotificationSendStatus
	{
		Received,
		Dropped,
		ChannelThrottled
	}

	public enum WindowsDeviceConnectionStatus
	{
		Connected,
		Disconnected,
		TempDisconnected
	}
}
