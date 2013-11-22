using PushSharp.Web.Interfaces.Settings;

namespace PushSharp.Web.Business.Settings
{
    public class GoogleServiceSettings : ServiceSettingsBase, IGoogleServiceSettings
    {
        public string GoogleApiAccessKey { get; set; }
    }
}