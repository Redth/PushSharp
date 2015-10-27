using PushSharp.Core;
using System.Xml.Linq;

namespace PushSharp.Windows
{
    public abstract class WnsNotification : INotification
    {
        public string ChannelUri { get; set; }

        public bool? RequestForStatus { get; set; }
        public int? TimeToLive { get; set; }

        public XElement Payload { get; set; }

        public abstract WindowsNotificationType Type { get; }

        public bool IsDeviceRegistrationIdValid ()
        {
            return true;
        }

        public object Tag { get; set; }
    }

    public class WnsTileNotification : WnsNotification
    {
        public override WindowsNotificationType Type
        {
            get { return WindowsNotificationType.Tile; }
        }

        public WnsNotificationCachePolicyType? CachePolicy { get; set; }

        public string NotificationTag { get; set; }
    }

    public class WindowsToastNotification : WnsNotification
    {
        public override WindowsNotificationType Type
        {
            get { return WindowsNotificationType.Toast; }
        }
    }

    public class WindowsBadgeNotification : WnsNotification
    {
        public override WindowsNotificationType Type
        {
            get { return WindowsNotificationType.Badge; }
        }

        public WnsNotificationCachePolicyType? CachePolicy { get; set; }
    }

}

