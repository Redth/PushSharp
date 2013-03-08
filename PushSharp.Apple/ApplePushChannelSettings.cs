using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using PushSharp.Core;

namespace PushSharp.Apple
{
	public interface IApplePushChannelSettings : IPushChannelSettings
	{
		bool DetectProduction(X509Certificate2 certificate);

		string Host { get; }
		int Port { get; }
		string FeedbackHost { get; }
		int FeedbackPort { get; }
		X509Certificate2 Certificate { get; }
		List<X509Certificate2> AdditionalCertificates { get; }
		bool AddLocalAndMachineCertificateStores { get; set; }
		bool SkipSsl { get; set; }
		int MillisecondsToWaitBeforeMessageDeclaredSuccess { get; set; }
		int FeedbackIntervalMinutes { get; set; }
		bool FeedbackTimeIsUTC { get; set; }
	}

	public class ApplePushChannelSettings : IApplePushChannelSettings
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

		public ApplePushChannelSettings(bool production, string certificateFile, string certificateFilePwd, bool disableCertificateCheck = false) 
			: this(production, System.IO.File.ReadAllBytes(certificateFile), certificateFilePwd, disableCertificateCheck) { }

		public ApplePushChannelSettings(string certificateFile, string certificateFilePwd, bool disableCertificateCheck = false)
			: this(System.IO.File.ReadAllBytes(certificateFile), certificateFilePwd, disableCertificateCheck) { }

		//Need to load the private key seperately from apple
		// Fixed by danielgindi@gmail.com :
		//      The default is UserKeySet, which has caused internal encryption errors,
		//      Because of lack of permissions on most hosting services.
		//      So MachineKeySet should be used instead.
		public ApplePushChannelSettings(bool production, byte[] certificateData, string certificateFilePwd, bool disableCertificateCheck = false)
			: this(production, new X509Certificate2(certificateData, certificateFilePwd,
				X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable), disableCertificateCheck) { }

		public ApplePushChannelSettings(byte[] certificateData, string certificateFilePwd, bool disableCertificateCheck = false)
			: this(new X509Certificate2(certificateData, certificateFilePwd,
				X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable), disableCertificateCheck) { }

		public ApplePushChannelSettings(X509Certificate2 certificate, bool disableCertificateCheck = false)
		{
			Initialize(DetectProduction(certificate), certificate, disableCertificateCheck);
		}

		public ApplePushChannelSettings(bool production, X509Certificate2 certificate, bool disableCertificateCheck = false)
		{
			Initialize(production, certificate, disableCertificateCheck);
		}

		void Initialize(bool production, X509Certificate2 certificate, bool disableCertificateCheck)
		{
			this.Host = production ? APNS_PRODUCTION_HOST : APNS_SANDBOX_HOST;
			this.FeedbackHost = production ? APNS_PRODUCTION_FEEDBACK_HOST : APNS_SANDBOX_FEEDBACK_HOST;
			this.Port = production ? APNS_PRODUCTION_PORT : APNS_SANDBOX_PORT;
			this.FeedbackPort = production ? APNS_PRODUCTION_FEEDBACK_PORT : APNS_SANDBOX_FEEDBACK_PORT;

			this.Certificate = certificate;

			this.MillisecondsToWaitBeforeMessageDeclaredSuccess = 3000;

			this.FeedbackIntervalMinutes = 10;
			this.FeedbackTimeIsUTC = false;

            this.AdditionalCertificates = new List<X509Certificate2>();
            this.AddLocalAndMachineCertificateStores = false;

			if (!disableCertificateCheck)	
				CheckProductionCertificateMatching(production);
		}

		public bool DetectProduction(X509Certificate2 certificate)
		{
			bool production = false;

			if (certificate != null)
			{
				var subjectName = certificate.SubjectName.Name;

				if (subjectName.Contains("Apple Production IOS Push Services"))
					production = true;
			}
			
			return production;
		}

		void CheckProductionCertificateMatching(bool production)
		{
			if (this.Certificate != null)
			{
				var issuerName = this.Certificate.IssuerName.Name;
				var subjectName = this.Certificate.SubjectName.Name;

				if (!issuerName.Contains("Apple"))
					throw new ArgumentException("Your Certificate does not appear to be issued by Apple!  Please check to ensure you have the correct certificate!");

				if (production && !subjectName.Contains("Apple Production IOS Push Services"))
					throw new ArgumentException("You have selected the Production server, yet your Certificate does not appear to be the Production certificate!  Please check to ensure you have the correct certificate!");


				if (!production && !subjectName.Contains("Apple Development IOS Push Services"))
						throw new ArgumentException("You have selected the Development/Sandbox (Not production) server, yet your Certificate does not appear to be the Development/Sandbox certificate!  Please check to ensure you have the correct certificate!");				
			}
			else
				throw new ArgumentNullException("You must provide a Certificate to connect to APNS with!");
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

		public X509Certificate2 Certificate
		{
			get;
			private set;
		}

        public List<X509Certificate2> AdditionalCertificates
        {
            get;
            private set;
        }

        public bool AddLocalAndMachineCertificateStores
        {
            get;
            set;
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
