using PushSharp.Web.Business.Settings;

namespace PushSharp.Web.Tests
{
    public class TestConfigurationManager : IConfigurationManager
    {
        public string GetAppSetting(string name)
        {
            return System.Configuration.ConfigurationManager.AppSettings[name];
        }
    }
}
