using System;
using PushSharp.Core;

namespace PushSharp.Web.Business.Settings
{
    public class ServiceSettingsBase
    {
        protected IConfigurationManager ConfigurationManager;
        private static LogLevel _logLevel;

        public LogLevel LogLevel
        {
            get 
            {
                return LoadLogLevel();
            }
        }

        private LogLevel LoadLogLevel()
        {
            if (!Enum.IsDefined(typeof(LogLevel), _logLevel))
            {
                var setting = ConfigurationManager.GetAppSetting(Constants.LogLevel);
                if (!string.IsNullOrEmpty(setting))
                {
                    int settingLevel;
                    if (!int.TryParse(setting, out settingLevel))
                    {
                        settingLevel = 4;
                    }
                    _logLevel = (LogLevel)settingLevel;
                }
                else
                {
                    _logLevel = LogLevel.Info;
                }
            }
            return _logLevel;
        }
    }
}