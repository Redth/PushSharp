using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushSharp.Core;

namespace PushSharp
{
	public class PushNotificationBroker : IDisposable
	{
		private Dictionary<Type, List<PushServiceBase>> registeredServices;

		public ChannelEvents Events;
		
		public PushNotificationBroker(bool autoRegisterPushServices = true)
		{
			this.Events = new ChannelEvents();
			registeredServices = new Dictionary<Type, List<PushServiceBase>>();
		}

		public void RegisterService<TPushNotification, TPushService>(TPushService pushService) where TPushNotification : Notification where TPushService : PushServiceBase
		{
			var pushNotificationType = typeof (TPushNotification);

			if (registeredServices.ContainsKey(pushNotificationType))
				registeredServices[pushNotificationType].Add(pushService);
			else
				registeredServices.Add(pushNotificationType, new List<PushServiceBase>() { pushService });				
		}

		public void QueueNotification<TPushNotification>(TPushNotification notification) where TPushNotification : Notification
		{
			var pushNotificationType = typeof (TPushNotification);

			if (registeredServices.ContainsKey(pushNotificationType))
				registeredServices[pushNotificationType].ForEach(pushService => pushService.QueueNotification(notification));
		}

		public void StopAllServices(bool waitForQueuesToFinish = true)
		{
			//TODO: Make it happen, cap'n
		}

		void IDisposable.Dispose()
		{
			StopAllServices(false);
		}
	}	
}
