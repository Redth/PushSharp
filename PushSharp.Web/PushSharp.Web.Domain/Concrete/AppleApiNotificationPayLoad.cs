using PushSharp.Web.Interfaces.Settings;

namespace PushSharp.Web.Domain.Concrete
{
    public class AppleApiNotificationPayLoad : NotificationPayLoadBase
    {
        public AppleApiNotificationPayLoad(string token, string message, int badgeNo, string soundFile, string key, string uuid) : base(key, token, message, uuid)
        {
            Token = token;
            Message = message;
            BadgeNo = badgeNo;
            SoundFile = soundFile;
        }

    	public int BadgeNo { get; set; }
        public string SoundFile { get; set; }
    	
		public override string GetApiEndPoint(IPushNotificationSettings settings)
		{
			return settings != null ? settings.WebApiUrlBase + "/" + settings.AppleWebApiEndPoint : string.Empty;
		}
    }
}
