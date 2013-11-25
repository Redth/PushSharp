using PushSharp.Web.Interfaces;
using PushSharp.Web.Interfaces.Settings;

namespace PushSharp.Web.Domain.Concrete
{
	public abstract class NotificationPayLoadBase : ApiPayLoadBase, INotificationPayLoad
	{
		public NotificationPayLoadBase()
		{			
		}

		protected NotificationPayLoadBase(string key, string token, string message, string uuid) : base(key)
		{
			Token = token;
			Message = message;
			DeviceUuid = uuid;
		}

		public abstract string GetApiEndPoint(IPushNotificationSettings settings);
		
		public string Token { get; set; }
		public string Message { get; set; }
		public string DeviceUuid { get; set; }
	}
}
