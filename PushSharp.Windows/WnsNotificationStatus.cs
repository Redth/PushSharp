using System;

namespace PushSharp.Windows
{
    public class WnsNotificationStatus
    {
        public string MessageId { get; set; }
        public string DebugTrace { get; set; }
        public string ErrorDescription { get; set; }

        public WnsNotificationSendStatus NotificationStatus { get; set; }
        public WnsDeviceConnectionStatus DeviceConnectionStatus { get; set; }

        public WnsNotification Notification { get; set; }

        public System.Net.HttpStatusCode HttpStatus { get; set; }
    }

    public enum WnsNotificationSendStatus
    {
        Received,
        Dropped,
        ChannelThrottled
    }

    public enum WnsDeviceConnectionStatus
    {
        Connected,
        Disconnected,
        TempDisconnected
    }

    public enum WnsNotificationCachePolicyType
    {
        Cache,
        NoCache
    }

    public enum WnsNotificationType
    {
        Badge,
        Tile,
        Toast,
        Raw
    }
}

