using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushSharp.Core;

namespace PushSharp
{
	public class PushBroker : IDisposable
	{
		private Dictionary<Type, List<IPushService>> registeredServices;
		
		public PushBroker()
		{
			registeredServices = new Dictionary<Type, List<IPushService>>();
		}

		public void RegisterService<TPushNotification>(IPushService pushService) where TPushNotification : Notification
		{
			var pushNotificationType = typeof (TPushNotification);

			if (registeredServices.ContainsKey(pushNotificationType))
				registeredServices[pushNotificationType].Add(pushService);
			else
				registeredServices.Add(pushNotificationType, new List<IPushService>() { pushService });				
		}

		public void QueueNotification<TPushNotification>(TPushNotification notification) where TPushNotification : Notification
		{
			var pushNotificationType = typeof (TPushNotification);

			if (registeredServices.ContainsKey(pushNotificationType))
				registeredServices[pushNotificationType].ForEach(pushService => pushService.QueueNotification(notification));
			else
				throw new IndexOutOfRangeException("There are no Registered Services that handle this type of Notification");
		}

		public IEnumerable<IPushService> GetRegistrations<TNotification>()
		{
			var type = typeof(TNotification);

			if (registeredServices != null && registeredServices.ContainsKey(type))
				return registeredServices[type];

			return null;
		}

		public void StopAllServices(bool waitForQueuesToFinish = true)
		{
			registeredServices.Values.AsParallel().ForAll(svc => svc.ForEach(svcOn => svcOn.Stop(waitForQueuesToFinish)));
		}

		void IDisposable.Dispose()
		{
			StopAllServices(false);
		}
	}	
}
