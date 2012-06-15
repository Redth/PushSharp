using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Apple
{
	public class ConnectionFailureException : Exception
	{
		public ConnectionFailureException(string msg, Exception innerException) : base(msg, innerException) { }
	}


	public class NotificationFailureException : Exception
	{

		public NotificationFailureException(int errorStatusCode, AppleNotification notification) : base() 
		{
			this.ErrorStatusCode = errorStatusCode;
			this.Notification = notification;
		}

		public AppleNotification Notification { get; set; }

		public int ErrorStatusCode { get; set; }

		public string ErrorStatusDescription
		{
			get
			{
				var msg = string.Empty;

				switch (ErrorStatusCode)
				{
					case 0:
						msg = "No errors encountered";
						break;
					case 1:
						msg = "Processing error";
						break;
					case 2:
						msg = "Missing device token";
						break;
					case 3:
						msg = "Missing topic";
						break;
					case 4:
						msg = "Missing payload";
						break;
					case 5:
						msg = "Invalid token size";
						break;
					case 6:
						msg = "Invaid topic size";
						break;
					case 7:
						msg = "Invalid payload size";
						break;
					case 8:
						msg = "Invalid token";
						break;
					case 255:
						msg = "None (unknown)";
						break;
					default:
						msg = "Undocumented error status code";
						break;
				}
				return msg;
			}
		}
	}
	
}
