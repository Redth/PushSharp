using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushSharp.Core;

namespace PushSharp
{
	public class PushBroker : IPushBroker
	{
		public event ChannelCreatedDelegate OnChannelCreated;
		public event ChannelDestroyedDelegate OnChannelDestroyed;
		public event NotificationSentDelegate OnNotificationSent;
		public event NotificationFailedDelegate OnNotificationFailed;
		public event NotificationRequeueDelegate OnNotificationRequeue;
		public event ChannelExceptionDelegate OnChannelException;
		public event ServiceExceptionDelegate OnServiceException;
		public event DeviceSubscriptionExpiredDelegate OnDeviceSubscriptionExpired;
		public event DeviceSubscriptionChangedDelegate OnDeviceSubscriptionChanged;

		readonly object serviceRegistrationsLock = new object();

		List<ServiceRegistration> serviceRegistrations;
		
		public PushBroker()
		{
			serviceRegistrations = new List<ServiceRegistration> ();
		}

		/// <summary>
		/// Registers the service to be eligible to handle queued notifications of the specified type
		/// </summary>
		/// <param name="pushService">Push service to be registered</param>
		/// <param name="applicationId">Arbitrary Application identifier to register this service with.  When queueing notifications you can specify the same Application identifier to ensure they get queued to the same service instance </param>
		/// <param name="raiseErrorOnDuplicateRegistrations">If set to <c>true</c> raises an error if there is an existing registration for the given notification type.</param>
		/// <typeparam name="TPushNotification">Type of notifications to register the service for</typeparam>
		public void RegisterService<TPushNotification>(IPushService pushService, string applicationId, bool raiseErrorOnDuplicateRegistrations = true) where TPushNotification : Notification
		{
			if (raiseErrorOnDuplicateRegistrations && GetRegistrations<TPushNotification> (applicationId).Any ())
				throw new InvalidOperationException ("There's already a service registered to handle " + typeof(TPushNotification).Name + " notification types for the Application Id: " + (applicationId ?? "[ANY].  If you want to register the service anyway, pass in the raiseErrorOnDuplicateRegistrations=true parameter to this method."));

			var registration = ServiceRegistration.Create<TPushNotification> (pushService, applicationId);

			lock (serviceRegistrationsLock)
				serviceRegistrations.Add (registration);

			pushService.OnChannelCreated += OnChannelCreated;
			pushService.OnChannelDestroyed += OnChannelDestroyed;
			pushService.OnChannelException += OnChannelException;
			pushService.OnDeviceSubscriptionExpired += OnDeviceSubscriptionExpired;
			pushService.OnNotificationFailed += OnNotificationFailed;
			pushService.OnNotificationSent += OnNotificationSent;
			pushService.OnNotificationRequeue += OnNotificationRequeue;
			pushService.OnServiceException += OnServiceException;
			pushService.OnDeviceSubscriptionChanged += OnDeviceSubscriptionChanged;
		}

		/// <summary>
		/// Registers the service to be eligible to handle queued notifications of the specified type
		/// </summary>
		/// <param name="pushService">Push service to be registered</param>
		/// <param name="raiseErrorOnDuplicateRegistrations">If set to <c>true</c> raises an error if there is an existing registration for the given notification type.</param>
		/// <typeparam name="TPushNotification">Type of notifications to register the service for</typeparam>
		public void RegisterService<TPushNotification>(IPushService pushService, bool raiseErrorOnDuplicateRegistrations = true) where TPushNotification : Notification
		{
			RegisterService<TPushNotification> (pushService, null, raiseErrorOnDuplicateRegistrations);
		}

		/// <summary>
		/// Queues a notification to ALL SERVICES registered for this type of notification
		/// </summary>
		/// <param name="notification">Notification.</param>
		/// <typeparam name="TPushNotification">The 1st type parameter.</typeparam>
		public void QueueNotification<TPushNotification>(TPushNotification notification) where TPushNotification : Notification
		{
			var services = GetRegistrations<TPushNotification> ();

			foreach (var s in services)
				s.QueueNotification (notification);
		}

		/// <summary>
		/// Queues the notification to all services registered for this type of notification with a matching applicationId
		/// </summary>
		/// <param name="notification">Notification</param>
		/// <param name="applicationId">Application identifier</param>
		/// <typeparam name="TPushNotification">Type of Notification</typeparam>
		public void QueueNotification<TPushNotification>(TPushNotification notification, string applicationId) where TPushNotification : Notification
		{
			var services = GetRegistrations<TPushNotification> (applicationId);

			if (services == null || !services.Any())
				throw new IndexOutOfRangeException("There are no Registered Services that handle this type of Notification");

			foreach (var s in services)
				s.QueueNotification (notification);
		}

		/// <summary>
		/// Gets all the registered services
		/// </summary>
		/// <returns>The registered services</returns>
		/// <typeparam name="TNotification">Type of notification</typeparam>
		public IEnumerable<IPushService> GetAllRegistrations()
		{
			lock (serviceRegistrationsLock)
				return from s in serviceRegistrations select s.Service;
		}

		/// <summary>
		/// Gets all the registered services for the given notification type
		/// </summary>
		/// <returns>The registered services</returns>
		/// <typeparam name="TNotification">Type of notification</typeparam>
		public IEnumerable<IPushService> GetRegistrations<TNotification>()
		{
			return GetRegistrations<TNotification> (null);
		}

		public IEnumerable<IPushService> GetRegistrations(string applicationId)
		{
			lock (serviceRegistrationsLock)
				return from s in serviceRegistrations where s.ApplicationId.Equals(applicationId) select s.Service;
		}

		/// <summary>
		/// Gets all the registered services for the given notification type and application identifier
		/// </summary>
		/// <returns>The registered services</returns>
		/// <param name="applicationId">Application identifier </param>
		/// <typeparam name="TNotification">Type of notification</typeparam>
		public IEnumerable<IPushService> GetRegistrations<TNotification>(string applicationId)
		{
			var type = typeof(TNotification);

			if (string.IsNullOrEmpty (applicationId))
			{
				lock (serviceRegistrationsLock)
				{
					return from sr in serviceRegistrations
					   where sr.NotificationType == type
					   select sr.Service;
				}
			}
			else
			{
				lock (serviceRegistrationsLock)
				{
                    return from sr in serviceRegistrations
                           where (!string.IsNullOrEmpty (sr.ApplicationId) && sr.ApplicationId.Equals (applicationId))
                               && sr.NotificationType == type
                           select sr.Service;
				}
			}
		}

		/// <summary>
		/// Stops all services that have been registered with the broker
		/// </summary>
		/// <param name="waitForQueuesToFinish">If set to <c>true</c> wait for queues to finish.</param>
		public void StopAllServices(bool waitForQueuesToFinish = true)
		{
			var stopping = new List<ServiceRegistration> ();

			lock (serviceRegistrationsLock)
			{
				stopping.AddRange (serviceRegistrations);

				serviceRegistrations.Clear ();
			}

			stopping.AsParallel ().ForAll (sr => StopService (sr, waitForQueuesToFinish));
		}

		/// <summary>
		/// Stops and removes all registered services for the given notification type
		/// </summary>
		/// <param name="waitForQueuesToFinish">If set to <c>true</c> waits for the queues to be drained before returning.</param>
		/// <typeparam name="TNotification">Notification Type</typeparam>
		public void StopAllServices<TNotification>(bool waitForQueuesToFinish = true)
		{
			StopAllServices<TNotification> (null, waitForQueuesToFinish);
		}

		/// <summary>
		/// Stops and removes all registered services for the given application identifier and notification type
		/// </summary>
		/// <param name="applicationId">Application identifier.</param>
		/// <param name="waitForQueuesToFinish">If set to <c>true</c> waits for queues to be drained before returning.</param>
		/// <typeparam name="TNotification">The 1st type parameter.</typeparam>
		public void StopAllServices<TNotification>(string applicationId, bool waitForQueuesToFinish = true)
		{
			var type = typeof(TNotification);

			var stopping = new List<ServiceRegistration> ();

			lock (serviceRegistrationsLock)
			{
				if (string.IsNullOrEmpty (applicationId))
				{
					var services = from s in serviceRegistrations where s.NotificationType == type select s;

					if (services != null && services.Any ())
						stopping.AddRange (services);

					serviceRegistrations.RemoveAll (s => s.NotificationType == type);
				}
				else
				{
					var services = from s in serviceRegistrations where s.NotificationType == type 
						&& s.ApplicationId.Equals (applicationId) select s;

					if (services != null && services.Any ())
						stopping.AddRange (services);

					serviceRegistrations.RemoveAll (s => s.NotificationType == type && s.ApplicationId.Equals(applicationId));
				}
			}

			if (stopping != null && stopping.Any())
				stopping.AsParallel().ForAll(sr => StopService(sr, waitForQueuesToFinish));
		}

		/// <summary>
		/// Stops and removes all registered services for the given application identifier
		/// </summary>
		/// <param name="applicationId">Application identifier.</param>
		/// <param name="waitForQueuesToFinish">If set to <c>true</c> waits for queues to be drained before returning.</param>
		public void StopAllServices(string applicationId, bool waitForQueuesToFinish = true)
		{
			var stopping = new List<ServiceRegistration> ();

			lock (serviceRegistrationsLock)
			{
				var services = from s in serviceRegistrations where s.ApplicationId.Equals(applicationId) select s;

				if (services != null && services.Any ())
					stopping.AddRange (services);

				serviceRegistrations.RemoveAll (s => s.ApplicationId.Equals(applicationId));
			}

			if (stopping != null && stopping.Any())
				stopping.AsParallel().ForAll(sr => StopService(sr, waitForQueuesToFinish));
		}

		void StopService(ServiceRegistration sr, bool waitForQueuesToFinish)
		{
			sr.Service.Stop (waitForQueuesToFinish);

			sr.Service.OnChannelCreated -= OnChannelCreated;
			sr.Service.OnChannelDestroyed -= OnChannelDestroyed;
			sr.Service.OnChannelException -= OnChannelException;
			sr.Service.OnDeviceSubscriptionExpired -= OnDeviceSubscriptionExpired;
			sr.Service.OnNotificationFailed -= OnNotificationFailed;
			sr.Service.OnNotificationSent -= OnNotificationSent;
			sr.Service.OnServiceException -= OnServiceException;
			sr.Service.OnDeviceSubscriptionChanged -= OnDeviceSubscriptionChanged;
		}

		void IDisposable.Dispose()
		{
			StopAllServices(false);
		}
	}	
}
