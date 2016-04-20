using System;
using PushSharp.Core;

namespace PushSharp.Amazon
{
    public class AdmRateLimitExceededException : NotificationException
    {
        public AdmRateLimitExceededException (string reason, AdmNotification notification) 
            : base ("Rate Limit Exceeded (" + reason + ")", notification)
        {
            Notification = notification;
            Reason = reason;
        }

        public new AdmNotification Notification { get; set; }
        public string Reason { get; private set; }
    }

    public class AdmMessageTooLargeException : NotificationException 
    {
        public AdmMessageTooLargeException (AdmNotification notification) 
            : base ("ADM Message too Large, must be <= 6kb", notification)
        {
            Notification = notification;
        }

        public new AdmNotification Notification { get; set; }
    }
}
