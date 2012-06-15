using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Apple
{
	public class ApplePushChannelSettings : Common.PushChannelSettings
	{
		#region Constants
		private const string APNS_SANDBOX_HOST = "gateway.sandbox.push.apple.com";
		private const string APNS_PRODUCTION_HOST = "gateway.push.apple.com";

		private const int APNS_SANDBOX_PORT = 2195;
		private const int APNS_PRODUCTION_PORT = 2195;
		#endregion

		public ApplePushChannelSettings(bool production, string certificateFile, string certificateFilePwd) 
			: this(production, System.IO.File.ReadAllBytes(certificateFile), certificateFilePwd) { }

		public ApplePushChannelSettings(bool production, byte[] certificateData, string certificateFilePwd)
		{
			this.Host = production ? APNS_PRODUCTION_HOST : APNS_SANDBOX_HOST;
			this.Port = production ? APNS_PRODUCTION_PORT : APNS_SANDBOX_PORT;

			this.CertificateData = certificateData;
			this.CertificateFilePassword = certificateFilePwd;
		}

		public string Host
		{
			get;
			private set;
		}

		public int Port
		{
			get;
			private set;
		}

		public byte[] CertificateData
		{
			get;
			private set;
		}

		public string CertificateFilePassword
		{
			get;
			private set;
		}

	}
}
