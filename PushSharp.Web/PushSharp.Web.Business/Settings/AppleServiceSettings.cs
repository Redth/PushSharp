using System;
using System.IO;
using PushSharp.Web.Interfaces.Settings;

namespace PushSharp.Web.Business.Settings
{
    public class AppleServiceSettings : ServiceSettingsBase, IAppleServiceSettings
    {
        public AppleServiceSettings(IConfigurationManager configurationManager)
        {
            ConfigurationManager = configurationManager;
        }

        public byte[] CertificateContents
        {
            get
            {
                return LoadCertificateContents();
            }
        }

        public string CertificatePassword
        {
            get 
            {
                return LoadCertificatePassword();
            }
        }

        #region Implementation

        private byte[] LoadCertificateContents()
        {
            if (_certificateContents == null)
            {
                var certificatePath = ConfigurationManager.GetAppSetting(Constants.IosPushNotificationCertificatePath);
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, certificatePath);
                if (File.Exists(fullPath))
                {
                    _certificateContents = File.ReadAllBytes(fullPath);
                }
            }
            return _certificateContents;
        }

        private  string LoadCertificatePassword()
        {
            if (string.IsNullOrEmpty(_certificatePassword))
            {
                _certificatePassword = ConfigurationManager.GetAppSetting(Constants.IosPushNotificationCertificatePassword);
            }
            return _certificatePassword;
        }

        private static byte[] _certificateContents;
        private static string _certificatePassword;

        #endregion
        
    }
}