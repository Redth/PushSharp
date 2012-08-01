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

		private const string APNS_SANDBOX_FEEDBACK_HOST = "feedback.sandbox.push.apple.com";
		private const string APNS_PRODUCTION_FEEDBACK_HOST = "feedback.push.apple.com";

		private const int APNS_SANDBOX_PORT = 2195;
		private const int APNS_PRODUCTION_PORT = 2195;

		private const int APNS_SANDBOX_FEEDBACK_PORT = 2196;
		private const int APNS_PRODUCTION_FEEDBACK_PORT = 2196;
		#endregion

		public ApplePushChannelSettings(bool production, string certificateFile, string certificateFilePwd) 
			: this(production, System.IO.File.ReadAllBytes(certificateFile), certificateFilePwd) { }

		public ApplePushChannelSettings(bool production, byte[] certificateData, string certificateFilePwd)
		{
			this.Host = production ? APNS_PRODUCTION_HOST : APNS_SANDBOX_HOST;
			this.FeedbackHost = production ? APNS_PRODUCTION_FEEDBACK_HOST : APNS_SANDBOX_FEEDBACK_HOST;
			this.Port = production ? APNS_PRODUCTION_PORT : APNS_SANDBOX_PORT;
			this.FeedbackPort = production ? APNS_PRODUCTION_FEEDBACK_PORT : APNS_SANDBOX_FEEDBACK_PORT;

			this.CertificateData = certificateData;
			this.CertificateFilePassword = certificateFilePwd;

			this.MillisecondsToWaitBeforeMessageDeclaredSuccess = 3000;

			this.FeedbackIntervalMinutes = 10;
			this.FeedbackTimeIsUTC = false;
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

		public string FeedbackHost
		{
			get;
			private set;
		}

		public int FeedbackPort
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

		public bool SkipSsl
		{
			get;
			set;
		}

		public int MillisecondsToWaitBeforeMessageDeclaredSuccess
		{
			get;
			set;
		}

		public int FeedbackIntervalMinutes
		{
			get;
			set;
		}

		public bool FeedbackTimeIsUTC
		{
			get;
			set;
		}
	}
}
