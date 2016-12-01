using System;

namespace PushSharp.Core
{
    public interface IServiceBroker<TNotification> where TNotification : INotification
    {
        event NotificationSuccessDelegate<TNotification> OnNotificationSucceeded;
        event NotificationFailureDelegate<TNotification> OnNotificationFailed;

        bool IsCompleted { get; }
        int ScaleSize { get; }
        IServiceConnectionFactory<TNotification> ServiceConnectionFactory { get; set; }

        void ChangeScale(int newScaleSize);
        void QueueNotification(TNotification notification);
        void RaiseNotificationSucceeded (TNotification notification);
        void RaiseNotificationFailed (TNotification notification, AggregateException ex);
        void Start();
        void Stop(bool immediately = false);
        System.Collections.Generic.IEnumerable<TNotification> TakeMany();
    }
}

