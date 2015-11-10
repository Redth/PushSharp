using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Android
{
	public class GcmCCSPushChannelSettings : IPushChannelSettings
	{
		private const string GCM_SEND_URL = "https://android.googleapis.com/gcm/send";

		public GcmCCSPushChannelSettings(string senderAuthToken)
		{
			this.SenderAuthToken = senderAuthToken;
			this.GcmUrl = GCM_SEND_URL;
        }

        public GcmCCSPushChannelSettings(string optionalSenderID, string senderAuthToken, string optionalApplicationIdPackageName)
		{
			this.SenderID = optionalSenderID;
			this.SenderAuthToken = senderAuthToken;
			this.ApplicationIdPackageName = optionalApplicationIdPackageName;
			this.GcmUrl = GCM_SEND_URL;
        }

		public string SenderID { get; private set; }
		public string SenderAuthToken { get; private set; }
		public string ApplicationIdPackageName { get; private set; }

		public string GcmUrl { get; set; }

		public void OverrideUrl(string url)
		{
			GcmUrl = url;
		}
	}
}
