using System;
using System.Threading.Tasks;

namespace PushSharp.Core
{
	public delegate void NotificationSuccessDelegate<TNotification, TResponse> (TNotification notification, TResponse response) where TNotification : INotification where TResponse : IResponse;
	public delegate void NotificationFailureDelegate<TNotification, TResponse> (TNotification notification, TResponse response, AggregateException exception) where TNotification : INotification where TResponse : IResponse;

	public delegate void NotificationSuccessDelegate<TNotification>(TNotification notification) where TNotification : INotification;
	public delegate void NotificationFailureDelegate<TNotification>(TNotification notification, AggregateException exception) where TNotification : INotification;

	public interface IServiceConnection<TNotification, TResponse>
		where TNotification : INotification
		where TResponse : IResponse
	{
		Task<TResponse> Send(TNotification notification);
	}

	public interface IServiceConnection<TNotification>
		where TNotification : INotification
	{
		Task Send(TNotification notification);
	}
}

