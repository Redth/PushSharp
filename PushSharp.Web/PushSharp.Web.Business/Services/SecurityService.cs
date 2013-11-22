using System;
using System.Security.Cryptography;
using PushSharp.Web.Business.Settings;

namespace PushSharp.Web.Business.Services
{
    public interface ISecurityService
    {
        string GenerateHash(string input);
        bool ValidateRequestKey(string key);
    }

    public class SecurityService : ISecurityService
    {
        private readonly IConfigurationManager _ConfigurationManager;

        public SecurityService(IConfigurationManager configurationManager)
        {
            _ConfigurationManager = configurationManager;
        }

        public string GenerateHash(string input)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            return BitConverter.ToString(hash);
        }

        public bool ValidateRequestKey(string authenticationKey)
        {
            var webApiKey = _ConfigurationManager.GetAppSetting(Constants.ApiAuthenticationKey);
            var result = false;
            if (!string.IsNullOrEmpty(webApiKey) && !string.IsNullOrEmpty(authenticationKey))
            {
                var myKeyHash = GenerateHash(webApiKey);
                var userKeyHash = GenerateHash(authenticationKey);
                result = myKeyHash == userKeyHash;
            }
            return result;
        }
    }
}
