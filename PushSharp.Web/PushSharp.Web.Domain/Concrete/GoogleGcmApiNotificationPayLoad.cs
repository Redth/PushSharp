using Newtonsoft.Json;
using PushSharp.Web.Interfaces.Settings;

namespace PushSharp.Web.Domain.Concrete
{
	public class GoogleGcmApiNotificationPayLoad : NotificationPayLoadBase
	{
		public GoogleGcmApiNotificationPayLoad(string key, string token, string message, string uuid) : base(key, token, message, uuid)
		{			
		}

		public override string GetApiEndPoint(IPushNotificationSettings settings)
		{
			return settings != null ? settings.GoogleGcmWebApiEndPoint : string.Empty;
		}

	    public string GetGoogleFormattedJson()
	    {
	        return JsonConvert.SerializeObject(new {alert = Message, badge = BadgeNumber, sound = SoundFileName });
	    }

	    public string SoundFileName { get; set; }

	    public string BadgeNumber { get; set; }
	}
}
