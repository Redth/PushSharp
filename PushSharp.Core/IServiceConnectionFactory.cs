using System;

namespace PushSharp.Core
{
    public interface IServiceConnectionFactory<TNotification, TResponse> where TNotification : INotification where TResponse : IResponse
    {
        IServiceConnection<TNotification, TResponse> Create ();
    }

	public interface IServiceConnectionFactory<TNotification> where TNotification : INotification
	{
		IServiceConnection<TNotification> Create();
	}

}

