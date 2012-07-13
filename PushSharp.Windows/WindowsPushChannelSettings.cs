using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
    public class WindowsPushChannelSettings : Common.PushChannelSettings
    {
		public string PackageSecurityIdentifier { get; set; }
		public string ClientSecret { get; set; }
    }
}
