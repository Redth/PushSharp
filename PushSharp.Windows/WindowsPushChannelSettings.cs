using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushSharp.Core;

namespace PushSharp.Windows
{
    public class WindowsPushChannelSettings : PushChannelSettings
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
