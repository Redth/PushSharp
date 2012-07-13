using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
	public abstract class WindowsNotification : Common.Notification
	{
		protected WindowsNotification()
		{
			this.Platform = Common.PlatformType.Windows;
			
		}

		public string ChannelUri { get; set; }

		public bool? RequestForStatus { get; set; }
		public int? TimeToLive { get; set; }
		

		public abstract string PayloadToString();

		public abstract WindowsNotificationType Type { get; }
	}

	public class WindowsTileNotification : WindowsNotification
	{
		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Tile; }
		}

		public WindowsNotificationCachePolicyType? CachePolicy { get; set; }
		public string Tag { get; set; }

		public override string PayloadToString()
		{
			//TODO: Implement
			return string.Empty;
		}
	}

	public class WindowsToastNotification : WindowsNotification
	{
		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Toast; }
		}

		public override string PayloadToString()
		{
			//TODO: Implement
			return string.Empty;
		}
	}

	public class WindowsBadgeNotification : WindowsNotification
	{
		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Badge; }
		}

		public WindowsNotificationCachePolicyType? CachePolicy { get; set; }

		public override string PayloadToString()
		{
			//TODO: Implement
			return string.Empty;
		}
	}

	public class WindowsRawNotification : WindowsNotification
	{
		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Raw; }
		}

		public override string PayloadToString()
		{
			//TODO: Implement
			return string.Empty;
		}
	}

	public enum WindowsNotificationCachePolicyType
	{
		Cache,
		NoCache
	}

	public enum WindowsNotificationType
	{
		Badge,
		Tile,
		Toast,
		Raw
	}
	
}
