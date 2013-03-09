using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using PushSharp.Core;

namespace PushSharp.WindowsPhone
{
	public class WindowsPhonePushChannelSettings : IPushChannelSettings
	{
		public WindowsPhonePushChannelSettings() : this(null) { }

		public WindowsPhonePushChannelSettings(string certificateFile, string certificateFilePwd)
			: this(System.IO.File.ReadAllBytes(certificateFile), certificateFilePwd) { }

		public WindowsPhonePushChannelSettings(byte[] certificateData, string certificateFilePwd)
			: this(new X509Certificate2(certificateData, certificateFilePwd, 
				X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable)) { }

		
		public WindowsPhonePushChannelSettings(X509Certificate2 certificate = null)
		{
			this.WebServiceCertificate = certificate;
		}

		public X509Certificate2 WebServiceCertificate { get; private set; }
	}
}
