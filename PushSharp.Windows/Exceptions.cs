using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
	public class WindowsNotificationSendFailureException : Exception
	{
		public WindowsNotificationSendFailureException(WindowsNotificationStatus status)
			: base()
		{
			this.NotificationStatus = status;
		}

		public WindowsNotificationStatus NotificationStatus
		{
			get;
			set;
		}
	}
}
