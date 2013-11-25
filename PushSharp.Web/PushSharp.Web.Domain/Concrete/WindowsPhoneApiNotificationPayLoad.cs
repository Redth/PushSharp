using PushSharp.Web.Interfaces.Settings;

namespace PushSharp.Web.Domain.Concrete
{
    public class WindowsPhoneApiNotificationPayLoad : NotificationPayLoadBase
    {
        public override string GetApiEndPoint(IPushNotificationSettings settings)
        {
            return settings != null ? settings.WindowsPhoneWebApiEndPoint : string.Empty;
        }

        public string NavigationPath { get; set; }
        public string TextMessage2 { get; set; }
    }
}