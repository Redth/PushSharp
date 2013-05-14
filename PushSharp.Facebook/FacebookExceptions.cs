using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Facebook
{
	public class FacebookMessageTransportException : Exception
	{
		public FacebookMessageTransportException(string message, FacebookMessageTransportResponse response)
			: base(message)
		{
			this.Response = response;
		}

		public FacebookMessageTransportResponse Response
		{
			get;
			private set;
		}
	}

	public class FacebookBadRequestTransportException : FacebookMessageTransportException
	{
		public FacebookBadRequestTransportException(FacebookMessageTransportResponse response)
			: base("Bad Request or Malformed JSON", response)
		{
		}
	}

	public class FacebookAuthenticationErrorTransportException : FacebookMessageTransportException
	{
		public FacebookAuthenticationErrorTransportException(FacebookMessageTransportResponse response)
			: base("Authentication Failed", response)
		{
		}
	}

	public class FacebookServiceUnavailableTransportException : FacebookMessageTransportException
	{
		public FacebookServiceUnavailableTransportException(TimeSpan retryAfter, FacebookMessageTransportResponse response)
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
}
