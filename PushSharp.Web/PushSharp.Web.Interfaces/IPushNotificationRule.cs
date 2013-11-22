namespace PushSharp.Web.Interfaces
{
	public interface IPushNotificationRule
	{
		bool Match(IPushNotification notification);
		INotificationPayLoad GetPayLoad(IPushNotification notification);
	}
}