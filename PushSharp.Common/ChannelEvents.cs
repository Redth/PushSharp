using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Common
{
	public class ChannelEvents
	{
		public delegate void NotificationSendFailureDelegate(Notification notification, Exception notificationFailureException);
		public event NotificationSendFailureDelegate OnNotificationSendFailure;

		public void RaiseNotificationSendFailure(Notification notification, Exception notificationFailureException)
		{
			var evt = this.OnNotificationSendFailure;
			if (evt != null)
				evt(notification, notificationFailureException);
		}

		public delegate void NotificationSentDelegate(Notification notification);
		public event NotificationSentDelegate OnNotificationSent;

		public void RaiseNotificationSent(Notification notification)
		{
			var evt = this.OnNotificationSent;
			if (evt != null)
				evt(notification);
		}

		public delegate void ChannelExceptionDelegate(Exception exception);
		public event ChannelExceptionDelegate OnChannelException;

		public void RaiseChannelException(Exception exception)
		{
			var evt = this.OnChannelException;
			if (evt != null)
				evt(exception);
		}


		public delegate void DeviceSubscriptionExpired(PlatformType platform, string deviceInfo);
		public event DeviceSubscriptionExpired OnDeviceSubscriptionExpired;

		public void RaiseDeviceSubscriptionExpired(PlatformType platform, string deviceInfo)
		{
			var evt = this.OnDeviceSubscriptionExpired;
			if (evt != null)
				evt(platform, deviceInfo);
		}


		public void RegisterProxyHandler(ChannelEvents proxy)
		{
			this.OnChannelException += new ChannelExceptionDelegate((exception) => proxy.RaiseChannelException(exception));

			this.OnNotificationSendFailure += new NotificationSendFailureDelegate((notification, exception) => proxy.RaiseNotificationSendFailure(notification, exception));

			this.OnNotificationSent += new NotificationSentDelegate((notification) => proxy.RaiseNotificationSent(notification));

			this.OnDeviceSubscriptionExpired += new DeviceSubscriptionExpired((platform, deviceInfo) => proxy.RaiseDeviceSubscriptionExpired(platform, deviceInfo));
		}

		public void UnRegisterProxyHandler(ChannelEvents proxy)
		{
			this.OnChannelException -= new ChannelExceptionDelegate((exception) => proxy.RaiseChannelException(exception));

			this.OnNotificationSendFailure -= new NotificationSendFailureDelegate((notification, exception) => proxy.RaiseNotificationSendFailure(notification, exception));

			this.OnNotificationSent -= new NotificationSentDelegate((notification) => proxy.RaiseNotificationSent(notification));

			this.OnDeviceSubscriptionExpired -= new DeviceSubscriptionExpired((platform, deviceInfo) => proxy.RaiseDeviceSubscriptionExpired(platform, deviceInfo));
		}
	}
}
