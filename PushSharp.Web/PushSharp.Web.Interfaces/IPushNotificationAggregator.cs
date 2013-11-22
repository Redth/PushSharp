using System;
using System.Collections.Generic;

namespace PushSharp.Web.Interfaces
{
	public interface IPushNotificationAggregator
	{
		IEnumerable<IPushNotification> GetJobsPushNotifications(DateTime startTime, int count);
		IEnumerable<IPushNotification> GetMessagesPushNotifications(DateTime startTime, int count);
	}
}