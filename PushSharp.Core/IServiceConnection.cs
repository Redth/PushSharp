using System;
using System.Threading.Tasks;

namespace PushSharp.Core
{
    public delegate void NotificationSuccessDelegate<TNotification>(TNotification notification);
    public delegate void NotificationFailureDelegate<TNotification>(TNotification notification, AggregateException exception);

    public interface IServiceConnection<T> where T : INotification
    {
        Task Send(T notification);
    }
}

