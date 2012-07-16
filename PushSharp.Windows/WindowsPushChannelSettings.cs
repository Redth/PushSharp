using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
    public class WindowsPushChannelSettings : Common.PushChannelSettings
    {
		public WindowsPushChannelSettings(string packageName, string packageSecurityIdentifier, string clientSecret)
		{
			this.PackageName = packageName;
			this.PackageSecurityIdentifier = packageSecurityIdentifier;
			this.ClientSecret = clientSecret;
		}

		public string PackageName { get; set; }
		public string PackageSecurityIdentifier { get; set; }
		public string ClientSecret { get; set; }
    }
}
