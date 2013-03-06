using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Common
{
	public interface IPushServiceRegistry
	{
		void Register(PushNotificationBroker notificationBroker);
	}

}
