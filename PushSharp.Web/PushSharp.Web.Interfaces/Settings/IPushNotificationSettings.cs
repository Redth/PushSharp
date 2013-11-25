namespace PushSharp.Web.Interfaces.Settings
{
	public interface IPushNotificationSettings
	{
		string WebApiKey { get; set; }
		string WebApiUrlBase { get; set; }
		string AppleWebApiEndPoint { get; set; }
		string GoogleGcmWebApiEndPoint { get; set; }
		string WindowsPhoneWebApiEndPoint { get; set; }
		int? MaxBatchSize { get; set; }
	}
}