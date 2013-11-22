using System;
using PushSharp.Web.Interfaces.Events;
using PushSharp.Web.Interfaces.Settings;

namespace PushSharp.Web.Interfaces
{
	public interface IWebApiProxy : IDisposable
	{
		event WebApiRequestCompleted WebApiRequestCompleted;
		IPushNotificationSettings Settings { get; set; }
		bool Process(INotificationPayLoad payLoad);
	}
}