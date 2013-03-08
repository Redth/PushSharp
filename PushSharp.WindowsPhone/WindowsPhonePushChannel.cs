using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using PushSharp.Core;

namespace PushSharp.WindowsPhone
{
	public class WindowsPhonePushChannel : IPushChannel
	{
		WindowsPhonePushChannelSettings windowsPhoneSettings;

		public WindowsPhonePushChannel(IPushChannelSettings channelSettings)
		{
			windowsPhoneSettings = channelSettings as WindowsPhonePushChannelSettings;
		}

		public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			var wpNotification = notification as WindowsPhoneNotification;
			
			var wr = HttpWebRequest.Create(wpNotification.EndPointUrl) as HttpWebRequest;
			wr.ContentType = "text/xml";
			wr.Method = "POST";
			
			var immediateValue = 3;
			var mediumValue = 13;
			var slowValue = 23;

			if (wpNotification is WindowsPhoneToastNotification)
			{
				immediateValue = 2;
				mediumValue = 12;
				slowValue = 22;
			}
			else if (wpNotification is WindowsPhoneTileNotification ||
                wpNotification is WindowsPhoneCycleTileNotification ||
                wpNotification is WindowsPhoneFlipTileNotification ||
                wpNotification is WindowsPhoneIconicTileNotification)
			{
				immediateValue = 1;
				mediumValue = 11;
				slowValue = 21;
			}

			var val = immediateValue;

			if (wpNotification.NotificationClass.HasValue)
			{
				if (wpNotification.NotificationClass.Value == BatchingInterval.Medium)
					val = mediumValue;
				else if (wpNotification.NotificationClass.Value == BatchingInterval.Slow)
					val = slowValue;
			}
			
			wr.Headers.Add("X-NotificationClass", val.ToString());
			
			if (wpNotification is WindowsPhoneToastNotification)
				wr.Headers.Add("X-WindowsPhone-Target", "toast");
            else if (wpNotification is WindowsPhoneTileNotification ||
                wpNotification is WindowsPhoneCycleTileNotification ||
                wpNotification is WindowsPhoneFlipTileNotification ||
                wpNotification is WindowsPhoneIconicTileNotification)
				wr.Headers.Add("X-WindowsPhone-Target", "token");

			if (wpNotification.MessageID != null)
				wr.Headers.Add("X-MessageID", wpNotification.MessageID.ToString());

			var payload = wpNotification.PayloadToString();

			var data = Encoding.UTF8.GetBytes(payload);

			wr.ContentLength = data.Length;

			if (this.windowsPhoneSettings.WebServiceCertificate != null)
				wr.ClientCertificates.Add(this.windowsPhoneSettings.WebServiceCertificate);

			using (var rs = wr.GetRequestStream())
			{
				rs.Write(data, 0, data.Length);
			}

			try
			{
				wr.BeginGetResponse(getResponseCallback, new object[] { wr, wpNotification, callback });
			}
			catch (WebException wex)
			{
				//Handle different httpstatuses
				var status = ParseStatus(wex.Response as HttpWebResponse, wpNotification);

				HandleStatus(callback, status, wpNotification);
			}
		}


		void getResponseCallback(IAsyncResult asyncResult)
		{
			//Good list of statuses:
			//http://msdn.microsoft.com/en-us/library/ff941100(v=vs.92).aspx

			var objs = (object[])asyncResult.AsyncState;

			var wr = (HttpWebRequest)objs[0];
			var wpNotification = (WindowsPhoneNotification)objs[1];
			var callback = (SendNotificationCallbackDelegate) objs[2];

			var resp = wr.EndGetResponse(asyncResult) as HttpWebResponse;

			var status = ParseStatus(resp, wpNotification);

			HandleStatus(callback, status, wpNotification);
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

			WPDeviceConnectionStatus devConStatus = WPDeviceConnectionStatus.NotAvailable;
            Enum.TryParse<WPDeviceConnectionStatus>(wpDeviceConnectionStatus, true, out devConStatus);
			result.DeviceConnectionStatus = devConStatus;

			WPNotificationStatus notStatus = WPNotificationStatus.NotAvailable;
            Enum.TryParse<WPNotificationStatus>(wpStatus, true, out notStatus);
			result.NotificationStatus = notStatus;

			WPSubscriptionStatus subStatus = WPSubscriptionStatus.NotAvailable;
            Enum.TryParse<WPSubscriptionStatus>(wpChannelStatus, true, out subStatus);
			result.SubscriptionStatus = subStatus;

			return result;
		}
		
		void HandleStatus(SendNotificationCallbackDelegate callback, WindowsPhoneMessageStatus status, WindowsPhoneNotification notification = null)
		{	
			if (status.SubscriptionStatus == WPSubscriptionStatus.Expired)
			{
				if (callback != null)
					callback(this, new SendNotificationResult(notification, false, new Exception("Device Subscription Expired")) { IsSubscriptionExpired = true });

				return;
			}

			if (status.HttpStatus == HttpStatusCode.OK
				&& status.NotificationStatus == WPNotificationStatus.Received)
			{
				if (callback != null)
					callback(this, new SendNotificationResult(notification));
				return;
			}

			if (callback != null)
				callback(this, new SendNotificationResult(status.Notification, false, new WindowsPhoneNotificationSendFailureException(status)));
		}

		public void Dispose()
		{
		}
	}
}
