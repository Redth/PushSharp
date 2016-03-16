using System;
using System.Collections.Generic;

namespace PushSharp.Core
{
    public class DeviceSubscriptonExpiredException : NotificationException
    {
        public DeviceSubscriptonExpiredException (INotification notification) : base ("Device Subscription has Expired", notification)
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

    public class RetryAfterException : NotificationException
    {
        public RetryAfterException (INotification notification, string message, DateTime retryAfterUtc) : base (message, notification)
        {
            RetryAfterUtc = retryAfterUtc;
        }

        public DateTime RetryAfterUtc { get; set; }
    }
}
