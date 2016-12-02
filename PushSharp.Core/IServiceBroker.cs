using System;

namespace PushSharp.Core
{
    public interface IServiceBroker
    {
        event NotificationSuccessDelegate<INotification> OnNotificationSucceeded;
        event NotificationFailureDelegate<INotification> OnNotificationFailed;

        bool IsCompleted { get; }
        int ScaleSize { get; }
        IServiceConnectionFactory ServiceConnectionFactory { get; set; }

        void ChangeScale(int newScaleSize);
        void QueueNotification(INotification notification);
        void RaiseNotificationSucceeded(INotification notification);
        void RaiseNotificationFailed(INotification notification, AggregateException ex);
        void Start();
        void Stop(bool immediately = false);
        System.Collections.Generic.IEnumerable<INotification> TakeMany();
    }
}

