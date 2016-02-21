using System;
using PushSharp.Core;

namespace PushSharp.Amazon
{
    public class AdmRateLimitExceededException : NotificationException
    {
        public AdmRateLimitExceededException (string reason, INotification notification) 
            : base ("Rate Limit Exceeded (" + reason + ")", notification)
        {
            Reason = reason;
        }

        public string Reason { get; private set; }
    }

    public class AdmMessageTooLargeException : NotificationException 
    {
        public AdmMessageTooLargeException (INotification notification) 
            : base ("ADM Message too Large, must be <= 6kb", notification)
        {
        }
    }
}

