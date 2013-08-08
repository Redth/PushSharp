using System;

namespace PushSharp.Core
{
	public class ServiceRegistration
	{
		public static ServiceRegistration Create<TNotification>(IPushService service, string applicationId = null)
		{
			return new ServiceRegistration () {
				ApplicationId =  applicationId,
				Service = service,
				NotificationType = typeof(TNotification)
			};
		}

		public IPushService Service { get;set; }
		public string ApplicationId { get;set; }
		public Type NotificationType { get;set; }
	}
}

