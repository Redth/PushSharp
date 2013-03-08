using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushSharp.Core;

namespace PushSharp.Windows
{
	public interface IWindowsPushChannelSettings : IPushChannelSettings
	{
		string PackageName { get; }
		string PackageSecurityIdentifier { get; }
		string ClientSecret { get; }
	}


	public class WindowsPushChannelSettings : IWindowsPushChannelSettings
    {
		public WindowsPushChannelSettings(string packageName, string packageSecurityIdentifier, string clientSecret)
		{
			this.PackageName = packageName;
			this.PackageSecurityIdentifier = packageSecurityIdentifier;
			this.ClientSecret = clientSecret;
		}

		public string PackageName { get; private set; }
		public string PackageSecurityIdentifier { get; private set; }
		public string ClientSecret { get; private set; }
    }
}
