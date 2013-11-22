namespace PushSharp.Web.Domain.Concrete
{
	public class PushNotificationApiCallResult : ApiCallResult
	{
		public PushNotificationApiCallResult(bool isSuccessful, string message, string uuid, string notificaitonMessage) : base(isSuccessful, message)
		{
			DeviceUuid = uuid;
			NotificationMessage = notificaitonMessage;
		}

		public string DeviceUuid { get; set; }
		public string NotificationMessage { get; set; }
	}
}
