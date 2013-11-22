using System.Web.Configuration;

namespace PushSharp.Web.Business.Settings
{
    public class WebApiConfigurationManager : IConfigurationManager
    {
        public string GetAppSetting(string name)
        {
            return WebConfigurationManager.AppSettings[name];
        }
    }
}
