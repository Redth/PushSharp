using System;
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
	}

	public interface ILogger
	{
		void Debug (string format, params object[] objs);
		void Info (string format, params object[] objs);
		void Warning (string format, params object[] objs);
		void Error (string format, params object[] objs);
	}

    public interface IPushBroker : IDisposable
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

        /// <summary>
        /// Registers the service to be eligible to handle queued notifications of the specified type
        /// </summary>
        /// <param name="pushService">Push service to be registered</param>
        /// <param name="applicationId">Arbitrary Application identifier to register this service with.  When queueing notifications you can specify the same Application identifier to ensure they get queued to the same service instance </param>
        /// <param name="raiseErrorOnDuplicateRegistrations">If set to <c>true</c> raises an error if there is an existing registration for the given notification type.</param>
        /// <typeparam name="TPushNotification">Type of notifications to register the service for</typeparam>
        void RegisterService<TPushNotification>(IPushService pushService, string applicationId, bool raiseErrorOnDuplicateRegistrations = true) where TPushNotification : Notification;

        /// <summary>
        /// Registers the service to be eligible to handle queued notifications of the specified type
        /// </summary>
        /// <param name="pushService">Push service to be registered</param>
        /// <param name="raiseErrorOnDuplicateRegistrations">If set to <c>true</c> raises an error if there is an existing registration for the given notification type.</param>
        /// <typeparam name="TPushNotification">Type of notifications to register the service for</typeparam>
        void RegisterService<TPushNotification>(IPushService pushService, bool raiseErrorOnDuplicateRegistrations = true) where TPushNotification : Notification;

        /// <summary>
        /// Queues a notification to ALL SERVICES registered for this type of notification
        /// </summary>
        /// <param name="notification">Notification.</param>
        /// <typeparam name="TPushNotification">The 1st type parameter.</typeparam>
        void QueueNotification<TPushNotification>(TPushNotification notification) where TPushNotification : Notification;

        /// <summary>
        /// Queues the notification to all services registered for this type of notification with a matching applicationId
        /// </summary>
        /// <param name="notification">Notification</param>
        /// <param name="applicationId">Application identifier</param>
        /// <typeparam name="TPushNotification">Type of Notification</typeparam>
        void QueueNotification<TPushNotification>(TPushNotification notification, string applicationId) where TPushNotification : Notification;

        /// <summary>
        /// Gets all the registered services
        /// </summary>
        /// <returns>The registered services</returns>
        /// <typeparam name="TNotification">Type of notification</typeparam>
        IEnumerable<IPushService> GetAllRegistrations();

        /// <summary>
        /// Gets all the registered services for the given notification type
        /// </summary>
        /// <returns>The registered services</returns>
        /// <typeparam name="TNotification">Type of notification</typeparam>
        IEnumerable<IPushService> GetRegistrations<TNotification>();

        IEnumerable<IPushService> GetRegistrations(string applicationId);

        /// <summary>
        /// Gets all the registered services for the given notification type and application identifier
        /// </summary>
        /// <returns>The registered services</returns>
        /// <param name="applicationId">Application identifier </param>
        /// <typeparam name="TNotification">Type of notification</typeparam>
        IEnumerable<IPushService> GetRegistrations<TNotification>(string applicationId);

        /// <summary>
        /// Stops all services that have been registered with the broker
        /// </summary>
        /// <param name="waitForQueuesToFinish">If set to <c>true</c> wait for queues to finish.</param>
        void StopAllServices(bool waitForQueuesToFinish = true);

        /// <summary>
        /// Stops and removes all registered services for the given notification type
        /// </summary>
        /// <param name="waitForQueuesToFinish">If set to <c>true</c> waits for the queues to be drained before returning.</param>
        /// <typeparam name="TNotification">Notification Type</typeparam>
        void StopAllServices<TNotification>(bool waitForQueuesToFinish = true);

        /// <summary>
        /// Stops and removes all registered services for the given application identifier and notification type
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="waitForQueuesToFinish">If set to <c>true</c> waits for queues to be drained before returning.</param>
        /// <typeparam name="TNotification">The 1st type parameter.</typeparam>
        void StopAllServices<TNotification>(string applicationId, bool waitForQueuesToFinish = true);

        /// <summary>
        /// Stops and removes all registered services for the given application identifier
        /// </summary>
        /// <param name="applicationId">Application identifier.</param>
        /// <param name="waitForQueuesToFinish">If set to <c>true</c> waits for queues to be drained before returning.</param>
        void StopAllServices(string applicationId, bool waitForQueuesToFinish = true);
    }
}
