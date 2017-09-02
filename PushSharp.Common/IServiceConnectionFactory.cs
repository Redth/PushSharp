namespace PushSharp.Common
{
    public interface IServiceConnectionFactory<TNotification> where TNotification : INotification
    {
        IServiceConnection<TNotification> Create ();
    }
}

