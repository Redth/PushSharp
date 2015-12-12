using System;

namespace PushSharp.Windows
{
    public class WnsNotificationException : PushSharp.Core.NotificationException
    {
        public WnsNotificationException (WnsNotificationStatus status) : base (status.ErrorDescription, status.Notification) 
        {
            Notification = status.Notification;
            Status = status;
        }

        public new WnsNotification Notification { get; set; }
        public WnsNotificationStatus Status { get; private set; }
    }
}

