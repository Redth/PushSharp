using System;

namespace PushSharp.Amazon
{
    public class AdmConfiguration
    {
        const string ADM_SEND_URL = "https://api.amazon.com/messaging/registrations/{0}/messages";
        const string ADM_AUTH_URL = "https://api.amazon.com/auth/O2/token";

        public AdmConfiguration (string clientId, string clientSecret)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            AdmSendUrl = ADM_SEND_URL;
            AdmAuthUrl = ADM_AUTH_URL;
        }

        public string ClientId { get; private set; }
        public string ClientSecret { get; private set; }

        public string AdmSendUrl { get; private set; }
        public string AdmAuthUrl { get; private set; }

        public void OverrideSendUrl(string url)
        {
            AdmSendUrl = url;
        }

        public void OverrideAuthUrl(string url)
        {
            AdmAuthUrl = url;
        }
    }
}

