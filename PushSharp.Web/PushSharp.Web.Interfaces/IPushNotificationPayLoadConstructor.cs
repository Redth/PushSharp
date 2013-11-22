using System.Collections.Generic;

namespace PushSharp.Web.Interfaces
{
	public interface IPushNotificationPayLoadConstructor
	{
		IEnumerable<INotificationPayLoad> ConstructPayLoad(IEnumerable<IPushNotification> notifications);
	}
}