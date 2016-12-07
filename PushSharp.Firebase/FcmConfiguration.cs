using System;

namespace PushSharp.Firebase
{
	public class FcmConfiguration
	{
		private const string FCM_SEND_URL = "https://fcm.googleapis.com/fcm/send";

		public FcmConfiguration(string senderAuthToken)
		{
			this.SenderAuthToken = senderAuthToken;
			this.FcmUrl = FCM_SEND_URL;

			this.ValidateServerCertificate = false;
		}

		public FcmConfiguration(string optionalSenderID, string senderAuthToken, string optionalApplicationIdPackageName)
		{
			this.SenderID = optionalSenderID;
			this.SenderAuthToken = senderAuthToken;
			this.ApplicationIdPackageName = optionalApplicationIdPackageName;
			this.FcmUrl = FCM_SEND_URL;

			this.ValidateServerCertificate = false;
		}

		public string SenderID { get; private set; }

		public string SenderAuthToken { get; private set; }

		public string ApplicationIdPackageName { get; private set; }

		public bool ValidateServerCertificate { get; set; }

		public string FcmUrl { get; set; }

		public void OverrideUrl(string url)
		{
			FcmUrl = url;
		}
	}
}