using System;
using PushSharp.Common;

namespace PushSharp.Firefox
{
    public class FirefoxNotificationException : NotificationException
    {
        public FirefoxNotificationException (FirefoxNotification notification, string msg)
            : base (msg, notification)
        {
            Notification = notification;
        }

        public new FirefoxNotification Notification { get; private set; }
    }
}
