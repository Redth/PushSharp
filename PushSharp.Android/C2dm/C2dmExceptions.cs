using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Android
{
	public class MessageTransportException : Exception
	{
		public MessageTransportException(string message, C2dmMessageTransportResponse response)
			: base(message)
		{
			this.Response = response;
		}

		public C2dmMessageTransportResponse Response
		{
			get;
			private set;
		}
	}

	public class ServiceUnavailableTransportException : MessageTransportException
	{
		public ServiceUnavailableTransportException(TimeSpan retryAfter, C2dmMessageTransportResponse response)
			: base("Service Temporarily Unavailable.  Please wait the retryAfter amount and implement an Exponential Backoff", response)
		{
			this.RetryAfter = retryAfter;
		}

		public TimeSpan RetryAfter
		{
			get;
			private set;
		}
	}

	public class InvalidAuthenticationTokenTransportException : MessageTransportException
	{
		public InvalidAuthenticationTokenTransportException(C2dmMessageTransportResponse response)
			: base("Invalid ClientLogin GoogleLogin Authentication Token", response)
		{
		}
	}

	public class GoogleLoginAuthorizationException : Exception
	{
		public GoogleLoginAuthorizationException(string msg)
			: base(msg)
		{
		}
	}
}
