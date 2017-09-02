using System;
using PushSharp.Common;

namespace PushSharp.Windows
{
    public class WnsNotificationException : NotificationException
    {
        public WnsNotificationException (WnsNotificationStatus status) : base (status.ErrorDescription, status.Notification) 
        {
            Notification = status.Notification;
            Status = status;
        }

        public new WnsNotification Notification { get; set; }
        public WnsNotificationStatus Status { get; private set; }

        public override string ToString ()
        {
            return base.ToString() + " Status = " + Status.HttpStatus;
        }
    }
}

