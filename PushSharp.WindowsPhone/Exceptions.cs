using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.WindowsPhone
{
	public class WindowsPhoneNotificationSendFailureException : Exception
	{
		public WindowsPhoneNotificationSendFailureException(WindowsPhoneMessageStatus msgStatus)
			: base()
		{
			this.MessageStatus = msgStatus;
		}

		public WindowsPhoneMessageStatus MessageStatus
		{
			get;
			set;
		}
	}
}
