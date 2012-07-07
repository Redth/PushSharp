﻿using System;

namespace PushSharp.WindowsPhone
{
	public class WindowsPhoneMessageStatus
	{
		public Guid MessageID { get; set; }
		public WPNotificationStatus NotificationStatus { get; set; }
		public WPDeviceConnectionStatus DeviceConnectionStatus { get; set; }
		public WPSubscriptionStatus SubscriptionStatus { get; set; }

		public WindowsPhoneNotification Notification { get; set; }

		public System.Net.HttpStatusCode HttpStatus { get; set; }
	}

	public enum WPNotificationStatus
	{
		Received,
		Dropped,
		Suppressed,
		QueueFull
	}

	public enum WPDeviceConnectionStatus
	{
		Connected,
		InActive,
		Disconnected,
		TempDisconnected
	}

	public enum WPSubscriptionStatus
	{
		Active,
		Expired
	}



}
