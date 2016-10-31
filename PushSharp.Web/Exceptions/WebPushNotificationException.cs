using PushSharp.Core;

namespace PushSharp.Web.Exceptions
{
    public class WebPushNotificationException : NotificationException
    {
        public WebPushNotificationException(WebPushNotification notification, string msg, string description, bool isExpired)
            : base(msg, notification)
        {
            Notification = notification;
            Description = description;
            IsExpiredSubscription = isExpired;
        }

        public new WebPushNotification Notification { get; private set; }

        public string Description { get; private set; }

        public bool IsExpiredSubscription { get; set; }
    }
}