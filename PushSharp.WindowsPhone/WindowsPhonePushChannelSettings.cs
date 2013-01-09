using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using PushSharp.Common;

namespace PushSharp.WindowsPhone
{
	public class WindowsPhonePushChannelSettings : PushChannelSettings
	{

		public WindowsPhonePushChannelSettings()
		{
		}

		public WindowsPhonePushChannelSettings(X509Certificate2 certificate)
		{
			this.Certificate = certificate;
		}

		public X509Certificate2 Certificate { get; private set; }
	}
}
