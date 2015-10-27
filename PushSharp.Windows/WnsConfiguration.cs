using System;

namespace PushSharp.Windows
{
    public class WnsConfiguration
    {
        public WnsConfiguration (string packageName, string packageSecurityIdentifier, string clientSecret)
        {
            PackageName = packageName;
            PackageSecurityIdentifier = packageSecurityIdentifier;
            ClientSecret = clientSecret;
        }

        public string PackageName { get; private set; }
        public string PackageSecurityIdentifier { get; private set; }
        public string ClientSecret { get; private set; }
    }
}

