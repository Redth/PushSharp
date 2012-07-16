using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
	public class WindowsNotificationFactory : Common.Notification
	{
		public WindowsToastNotification Toast()
		{
			return new WindowsToastNotification();
		}

		public WindowsTileNotification Tile()
		{
			return new WindowsTileNotification();
		}

		public WindowsRawNotification Raw()
		{
			return new WindowsRawNotification();
		}

		public WindowsBadgeNotification Badge()
		{
			return new WindowsBadgeNotification();
		}
	}
}
