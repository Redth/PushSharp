using System;

namespace PushSharp.Core
{
    public interface IServiceConnectionFactory
    {
        IServiceConnection<INotification> Create();
    }
}

