using System;
using System.Net;

namespace PushSharp.Google
{
    public class GcmConfiguration
    {
        private const string GCM_SEND_URL = "https://fcm.googleapis.com/fcm/send";

        public GcmConfiguration (string senderAuthToken)
        {
            this.SenderAuthToken = senderAuthToken;
            this.GcmUrl = GCM_SEND_URL;

            this.ValidateServerCertificate = false;
        }

        public GcmConfiguration (string optionalSenderID, string senderAuthToken, string optionalApplicationIdPackageName)
        {
            this.SenderID = optionalSenderID;
            this.SenderAuthToken = senderAuthToken;
            this.ApplicationIdPackageName = optionalApplicationIdPackageName;
            this.GcmUrl = GCM_SEND_URL;

            this.ValidateServerCertificate = false;
        }

        public void SetProxy(string host, int port)
        {
            UseProxy = true;
            ProxyHost = host;
            ProxyPort = port;
            ProxyCredentials = CredentialCache.DefaultNetworkCredentials;
        }

        public void SetProxy(string host, int port, string username, string pass, string domain)
        {
            UseProxy = true;
            ProxyHost = host;
            ProxyPort = port;
            ProxyCredentials = new NetworkCredential(username, pass, domain);
        }

        public bool UseProxy { get; private set; }

        public string ProxyHost { get; private set; }

        public int ProxyPort { get; private set; }

        public NetworkCredential ProxyCredentials { get; private set; }

        public string SenderID { get; private set; }

        public string SenderAuthToken { get; private set; }

        public string ApplicationIdPackageName { get; private set; }

        public bool ValidateServerCertificate { get; set; }

        public string GcmUrl { get; set; }

        public void OverrideUrl (string url)
        {
            GcmUrl = url;
        }
    }
}

