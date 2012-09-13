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

		public delegate void ChannelExceptionDelegate(Exception exception, PlatformType platformType, Notification notification = null);
		public event ChannelExceptionDelegate OnChannelException;

		public void RaiseChannelException(Exception exception, PlatformType platformType, Notification notification = null)
		{
			var evt = this.OnChannelException;
			if (evt != null)
				evt(exception, platformType, notification);
		}


		public delegate void DeviceSubscriptionExpired(PlatformType platform, string deviceInfo, Notification notification = null);
		public event DeviceSubscriptionExpired OnDeviceSubscriptionExpired;

		public void RaiseDeviceSubscriptionExpired(PlatformType platform, string deviceInfo, Notification notification = null)
		{
			var evt = this.OnDeviceSubscriptionExpired;
			if (evt != null)
				evt(platform, deviceInfo, notification);
		}

		public delegate void DeviceSubscriptionIdChanged(PlatformType platform, string oldDeviceInfo, string newDeviceInfo, Notification notification = null);
		public event DeviceSubscriptionIdChanged OnDeviceSubscriptionIdChanged;

		public void RaiseDeviceSubscriptionIdChanged(PlatformType platform, string oldDeviceInfo, string newDeviceInfo, Notification notification = null)
		{
			var evt = this.OnDeviceSubscriptionIdChanged;
			if (evt != null)
				evt(platform, oldDeviceInfo, newDeviceInfo, notification);
		}


		public void RegisterProxyHandler(ChannelEvents proxy)
		{
			this.OnChannelException += new ChannelExceptionDelegate((exception, platformType, notification) => proxy.RaiseChannelException(exception, platformType, notification));

			this.OnNotificationSendFailure += new NotificationSendFailureDelegate((notification, exception) => proxy.RaiseNotificationSendFailure(notification, exception));

			this.OnNotificationSent += new NotificationSentDelegate((notification) => proxy.RaiseNotificationSent(notification));

			this.OnDeviceSubscriptionExpired += new DeviceSubscriptionExpired((platform, deviceInfo, notification) => proxy.RaiseDeviceSubscriptionExpired(platform, deviceInfo, notification));

			this.OnDeviceSubscriptionIdChanged += new DeviceSubscriptionIdChanged((platform, oldDeviceInfo, newDeviceInfo, notification) => proxy.RaiseDeviceSubscriptionIdChanged(platform, oldDeviceInfo, newDeviceInfo, notification));
		}

		public void UnRegisterProxyHandler(ChannelEvents proxy)
		{
			this.OnChannelException -= new ChannelExceptionDelegate((exception, platformType, notification) => proxy.RaiseChannelException(exception, platformType, notification));

			this.OnNotificationSendFailure -= new NotificationSendFailureDelegate((notification, exception) => proxy.RaiseNotificationSendFailure(notification, exception));

			this.OnNotificationSent -= new NotificationSentDelegate((notification) => proxy.RaiseNotificationSent(notification));

			this.OnDeviceSubscriptionExpired -= new DeviceSubscriptionExpired((platform, deviceInfo, notification) => proxy.RaiseDeviceSubscriptionExpired(platform, deviceInfo, notification));

			this.OnDeviceSubscriptionIdChanged -= new DeviceSubscriptionIdChanged((platform, oldDeviceInfo, newDeviceInfo, notification) => proxy.RaiseDeviceSubscriptionIdChanged(platform, oldDeviceInfo, newDeviceInfo, notification));
		}
	}
}
