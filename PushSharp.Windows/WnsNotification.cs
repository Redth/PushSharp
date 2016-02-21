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

        public abstract WnsNotificationType Type { get; }

        public bool IsDeviceRegistrationIdValid ()
        {
            return true;
        }

        public object Tag { get; set; }
    }

    public class WnsTileNotification : WnsNotification
    {
        public override WnsNotificationType Type
        {
            get { return WnsNotificationType.Tile; }
        }

        public WnsNotificationCachePolicyType? CachePolicy { get; set; }

        public string NotificationTag { get; set; }
    }

    public class WnsToastNotification : WnsNotification
    {
        public override WnsNotificationType Type
        {
            get { return WnsNotificationType.Toast; }
        }
    }

    public class WnsBadgeNotification : WnsNotification
    {
        public override WnsNotificationType Type
        {
            get { return WnsNotificationType.Badge; }
        }

        public WnsNotificationCachePolicyType? CachePolicy { get; set; }
    }

}

