using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using PushSharp.Common;

namespace PushSharp.WindowsPhone
{
	public class WindowsPhonePushChannel : PushChannelBase
	{
		WindowsPhonePushChannelSettings windowsPhoneSettings;

		public WindowsPhonePushChannel(WindowsPhonePushChannelSettings settings)
			: base(settings)
		{
			windowsPhoneSettings = settings;
		}

		protected override void SendNotification(Notification notification)
		{
			var wpNotification = notification as WindowsPhoneNotification;


			var wr = HttpWebRequest.Create(wpNotification.EndPointUrl) as HttpWebRequest;
			wr.ContentType = "text/xml";

			if (!string.IsNullOrEmpty(wpNotification.MessageID))
				wr.Headers.Add("X-MessageID", wpNotification.MessageID);

			if (wpNotification.NotificationClass.HasValue)
			{
				var immediateValue = 3;
				var mediumValue = 13;
				var slowValue = 23;

				if (wpNotification is WindowsPhoneToastNotification)
				{
					immediateValue = 2;
					mediumValue = 12;
					slowValue = 22;
				}
				else if (wpNotification is WindowsPhoneTileNotification)
				{
					immediateValue = 1;
					mediumValue = 11;
					slowValue = 21;
				}

				var val = immediateValue;
				if (wpNotification.NotificationClass.Value == BatchingInterval.Medium)
					val = mediumValue;
				else if (wpNotification.NotificationClass.Value == BatchingInterval.Slow)
					val = slowValue;

				wr.Headers.Add("X-NotificationClass", val.ToString());
			}


			if (wpNotification is WindowsPhoneToastNotification)
				wr.Headers.Add("X-WindowsPhone-Target", "toast");
			else if (wpNotification is WindowsPhoneTileNotification)
				wr.Headers.Add("X-WindowsPhone-Target", "Tile");

			var data = Encoding.Default.GetBytes(wpNotification.PayloadToString());

			wr.ContentLength = data.Length;

			using (var rs = wr.GetRequestStream())
			{
				rs.Write(data, 0, data.Length);
			}

			wr.BeginGetResponse(new AsyncCallback(getResponseCallback), new object[] { wr, wpNotification });			
		}


		void getResponseCallback(IAsyncResult asyncResult)
		{
			var objs = (object[])asyncResult.AsyncState;

			var wr = (HttpWebRequest)objs[0];
			var wpNotification = (WindowsPhoneNotification)objs[1];
			//var callback = (Action)objs[2];

			var resp = wr.EndGetResponse(asyncResult);

			var wpStatus = resp.Headers["X-NotificationStatus"];
			var wpChannelStatus = resp.Headers["X-SubscriptionStatus"];
			var wpDeviceConnectionStatus = resp.Headers["X-DeviceConnectionStatus"];


			this.Events.RaiseNotificationSent(wpNotification);
		}
	}
}
