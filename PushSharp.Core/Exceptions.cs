using System;

namespace PushSharp.Core
{
    public class DeviceSubscriptonExpiredException : Exception
    {
        public DeviceSubscriptonExpiredException () : base ("Device Subscription has Expired")
        {
            ExpiredAt = DateTime.UtcNow;
        }

        public string OldSubscriptionId { get; set; }
        public string NewSubscriptionId { get; set; }
        public DateTime ExpiredAt { get; set; }
    }

    public class NotificationException : Exception
    {
        public NotificationException (string message, INotification notification) : base (message)
        {
            Notification = notification;
        }

        public NotificationException (string message, INotification notification, Exception innerException)
            : base (message, innerException)
        {
            Notification = notification;
        }

        public INotification Notification { get; set; }
    }

    public class RetryAfterException : Exception
    {
        public RetryAfterException (string message, DateTime retryAfterUtc) : base (message)
        {
            RetryAfterUtc = retryAfterUtc;
        }

        public DateTime RetryAfterUtc { get; set; }
    }
}
