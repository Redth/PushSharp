﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Core
{
	public delegate void SendNotificationCallbackDelegate(object sender, SendNotificationResult result);

	public delegate void PushChannelExceptionDelegate(object sender, Exception ex);

	public interface IPushChannelFactory
	{
		IPushChannel CreateChannel(IPushChannelSettings channelSettings);
	}

	public interface IPushService : IDisposable
	{
		event ChannelCreatedDelegate OnChannelCreated;
		event ChannelDestroyedDelegate OnChannelDestroyed;
		event NotificationSentDelegate OnNotificationSent;
		event NotificationFailedDelegate OnNotificationFailed;
		event NotificationRequeueDelegate OnNotificationRequeue;
		event ChannelExceptionDelegate OnChannelException;
		event ServiceExceptionDelegate OnServiceException;
		event DeviceSubscriptionExpiredDelegate OnDeviceSubscriptionExpired;
		event DeviceSubscriptionChangedDelegate OnDeviceSubscriptionChanged;

		IPushChannelFactory PushChannelFactory { get; }
		IPushServiceSettings ServiceSettings { get; }
		IPushChannelSettings ChannelSettings { get; }
		bool IsStopping { get; }
		void QueueNotification(INotification notification);
		void Stop(bool waitForQueueToFinish = true);
	}

	public interface IPushChannel : IDisposable
	{
		void SendNotification(INotification notification, SendNotificationCallbackDelegate callback);
	}

	public interface IPushChannelSettings
	{

	}

	public interface IPushServiceSettings
	{
		bool AutoScaleChannels { get; set; }
		int MaxAutoScaleChannels { get; set; }
		long MinAvgTimeToScaleChannels { get; set; }
		int Channels { get; set; }
		int MaxNotificationRequeues { get; set; }
		int NotificationSendTimeout { get; set; }
		TimeSpan IdleTimeout { get;set; }
	}

	public interface INotification
	{
		object Tag { get; set; }
		int QueuedCount { get; set; }
		bool IsValidDeviceRegistrationId();
		DateTime EnqueuedTimestamp { get; set; }
        bool IsTimedOut { get; set; }
	}

	public interface ILogger
	{
		void Debug (string format, params object[] objs);
		void Info (string format, params object[] objs);
		void Warning (string format, params object[] objs);
		void Error (string format, params object[] objs);
	}
}
