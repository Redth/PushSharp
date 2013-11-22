using PushSharp.Web.Interfaces.Settings;

namespace PushSharp.Web.Interfaces
{
	public interface INotificationPayLoad
	{
		string Token { get; set; }
		string Message { get; set; }
		string AuthenticationKey { get; set; }
		string DeviceUuid { get; set; }
		string GetApiEndPoint(IPushNotificationSettings settings);
	}
}