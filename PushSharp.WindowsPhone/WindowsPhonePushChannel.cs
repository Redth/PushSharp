﻿using System;
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

        public WindowsPhonePushChannel(WindowsPhonePushChannelSettings settings, PushServiceSettings serviceSettings = null)
			: base(settings,serviceSettings)
		{
			windowsPhoneSettings = settings;
		}

		protected override void SendNotification(Notification notification)
		{
			var wpNotification = notification as WindowsPhoneNotification;


			var wr = HttpWebRequest.Create(wpNotification.EndPointUrl) as HttpWebRequest;
			wr.ContentType = "text/xml";
			wr.Method = "POST";

			if (wpNotification.MessageID != null)
				wr.Headers.Add("X-MessageID", wpNotification.MessageID.ToString());

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

			var payload = wpNotification.PayloadToString();

			var data = Encoding.Default.GetBytes(payload);

			wr.ContentLength = data.Length;

			using (var rs = wr.GetRequestStream())
			{
				rs.Write(data, 0, data.Length);
			}

			try
			{
				wr.BeginGetResponse(new AsyncCallback(getResponseCallback), new object[] { wr, wpNotification });
			}
			catch (WebException wex)
			{
				//Handle different httpstatuses
				var status = ParseStatus(wex.Response as HttpWebResponse, wpNotification);

				HandleStatus(status);
			}
		}


		void getResponseCallback(IAsyncResult asyncResult)
		{
			//Good list of statuses:
			//http://msdn.microsoft.com/en-us/library/ff941100(v=vs.92).aspx

			var objs = (object[])asyncResult.AsyncState;

			var wr = (HttpWebRequest)objs[0];
			var wpNotification = (WindowsPhoneNotification)objs[1];
			
			var resp = wr.EndGetResponse(asyncResult) as HttpWebResponse;

			var status = ParseStatus(resp, wpNotification);

			HandleStatus(status);
		}

		WindowsPhoneMessageStatus ParseStatus(HttpWebResponse resp, WindowsPhoneNotification notification)
		{
			var result = new WindowsPhoneMessageStatus();

			result.Notification = notification;
			result.HttpStatus = resp.StatusCode;

			var wpStatus = resp.Headers["X-NotificationStatus"];
			var wpChannelStatus = resp.Headers["X-SubscriptionStatus"];
			var wpDeviceConnectionStatus = resp.Headers["X-DeviceConnectionStatus"];
			var messageID = resp.Headers["X-MessageID"];

			Guid msgGuid = Guid.NewGuid();
			if (Guid.TryParse(messageID, out msgGuid))
				result.MessageID = msgGuid;

			WPDeviceConnectionStatus devConStatus = WPDeviceConnectionStatus.InActive;
			Enum.TryParse<WPDeviceConnectionStatus>(wpDeviceConnectionStatus, out devConStatus);
			result.DeviceConnectionStatus = devConStatus;

			WPNotificationStatus notStatus = WPNotificationStatus.Dropped;
			Enum.TryParse<WPNotificationStatus>(wpStatus, out notStatus);
			result.NotificationStatus = notStatus;

			WPSubscriptionStatus subStatus = WPSubscriptionStatus.Expired;
			Enum.TryParse<WPSubscriptionStatus>(wpChannelStatus, out subStatus);
			result.SubscriptionStatus = subStatus;

			return result;
		}

		void HandleStatus(WindowsPhoneMessageStatus status)
		{
			if (status.HttpStatus == HttpStatusCode.OK
				&& status.NotificationStatus == WPNotificationStatus.Received)
			{
				Events.RaiseNotificationSent(status.Notification);
				return;
			}
			
			Events.RaiseNotificationSendFailure(status.Notification, new WindowsPhoneNotificationSendFailureException(status));
		}
	}
}
