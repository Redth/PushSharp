using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Common
{
	public class ChannelEvents
	{
		public delegate void ChannelCreatedDelegate(object sender, int newChannelCount);
		public event ChannelCreatedDelegate OnChannelCreated;

		public void RaiseChannelCreated(object sender, int newChannelCount)
		{
			var evt = this.OnChannelCreated;
			if (evt != null)
				evt(sender, newChannelCount);
		}

		public delegate void ChannelDestroyedDelegate(object sender, int newChannelCount);
		public event ChannelDestroyedDelegate OnChannelDestroyed;

		public void RaiseChannelDestroyed(object sender, int newChannelCount)
		{
			var evt = this.OnChannelDestroyed;
			if (evt != null)
				evt(sender, newChannelCount);
		}

		public delegate void NotificationSendFailureDelegate(object sender, Notification notification, Exception notificationFailureException);
		public event NotificationSendFailureDelegate OnNotificationSendFailure;

		public void RaiseNotificationSendFailure(object sender, Notification notification, Exception notificationFailureException)
		{
			var evt = this.OnNotificationSendFailure;
			if (evt != null)
				evt(sender, notification, notificationFailureException);
		}

		public delegate void NotificationSentDelegate(object sender, Notification notification);
		public event NotificationSentDelegate OnNotificationSent;

		public void RaiseNotificationSent(object sender, Notification notification)
		{
			var evt = this.OnNotificationSent;
			if (evt != null)
				evt(sender, notification);
		}

		public delegate void ChannelExceptionDelegate(object sender, Exception exception, Notification notification = null);
		public event ChannelExceptionDelegate OnChannelException;

		public void RaiseChannelException(object sender, Exception exception, Notification notification = null)
		{
			var evt = this.OnChannelException;
			if (evt != null)
				evt(sender, exception, notification);
		}


		public delegate void DeviceSubscriptionExpired(object sender, string deviceInfo, Notification notification = null);
		public event DeviceSubscriptionExpired OnDeviceSubscriptionExpired;

		public void RaiseDeviceSubscriptionExpired(object sender, string deviceInfo, Notification notification = null)
		{
			var evt = this.OnDeviceSubscriptionExpired;
			if (evt != null)
				evt(sender, deviceInfo, notification);
		}

		public delegate void DeviceSubscriptionIdChanged(object sender, string oldDeviceInfo, string newDeviceInfo, Notification notification = null);
		public event DeviceSubscriptionIdChanged OnDeviceSubscriptionIdChanged;

		public void RaiseDeviceSubscriptionIdChanged(object sender, string oldDeviceInfo, string newDeviceInfo, Notification notification = null)
		{
			var evt = this.OnDeviceSubscriptionIdChanged;
			if (evt != null)
				evt(sender, oldDeviceInfo, newDeviceInfo, notification);
		}


		public void RegisterProxyHandler(ChannelEvents proxy)
		{
			this.OnChannelCreated += proxy.RaiseChannelCreated;
			this.OnChannelDestroyed += proxy.RaiseChannelDestroyed;
			this.OnChannelException += proxy.RaiseChannelException;
			this.OnNotificationSendFailure += proxy.RaiseNotificationSendFailure;
			this.OnNotificationSent += proxy.RaiseNotificationSent;
			this.OnDeviceSubscriptionExpired += proxy.RaiseDeviceSubscriptionExpired;
			this.OnDeviceSubscriptionIdChanged += proxy.RaiseDeviceSubscriptionIdChanged;
		}

		public void UnRegisterProxyHandler(ChannelEvents proxy)
		{
			this.OnChannelCreated -= proxy.RaiseChannelCreated;
			this.OnChannelDestroyed -= proxy.RaiseChannelDestroyed;
			this.OnChannelException -= proxy.RaiseChannelException;
			this.OnNotificationSendFailure -= proxy.RaiseNotificationSendFailure;
			this.OnNotificationSent -= proxy.RaiseNotificationSent;
			this.OnDeviceSubscriptionExpired -= proxy.RaiseDeviceSubscriptionExpired;
			this.OnDeviceSubscriptionIdChanged -= proxy.RaiseDeviceSubscriptionIdChanged;
		}
	}
}
