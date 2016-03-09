using System;

namespace PushSharp.Core
{
    public interface IServiceBroker<TNotification> where TNotification : INotification
    {
        event NotificationSuccessDelegate<TNotification> OnNotificationSucceeded;
        event NotificationFailureDelegate<TNotification> OnNotificationFailed;

        System.Collections.Generic.IEnumerable<TNotification> TakeMany ();
        bool IsCompleted { get; }

        void RaiseNotificationSucceeded (TNotification notification);
        void RaiseNotificationFailed (TNotification notification, AggregateException ex);
    }
}

