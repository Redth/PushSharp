using System;

namespace PushSharp.Web.Interfaces.Events
{
	public delegate void WebApiRequestCompleted(object sender, WebApiCallEventArgs e);

	public class WebApiCallEventArgs : EventArgs
	{
		public WebApiCallEventArgs(IApiCallResult result)
		{
			Result = result;
		}

		public IApiCallResult Result { get; set; }
	}
}