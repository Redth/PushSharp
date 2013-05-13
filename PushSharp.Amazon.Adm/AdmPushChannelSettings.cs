using System;
using System.Collections.Generic;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Amazon.Adm
{
	public class AdmPushChannelSettings : IPushChannelSettings
	{
		private const string ADM_SEND_URL = "https://api.amazon.com/messaging/registrations/{0}/messages";
		private const string ADM_AUTH_URL = "https://api.amazon.com/messaging/";

		public AdmPushChannelSettings(string clientId, string clientSecret)
		{
			this.ClientId = clientId;
			this.ClientSecret = clientSecret;
			this.AdmSendUrl = ADM_SEND_URL;
			this.AdmAuthUrl = ADM_AUTH_URL;
		}

		public string ClientId { get; private set; }
		public string ClientSecret { get; private set; }

		public string AdmSendUrl { get; private set; }
		public string AdmAuthUrl { get; private set; }

		public void OverrideSendUrl(string url)
		{
			AdmSendUrl = url;
		}

		public void OverrideAuthUrl(string url)
		{
			AdmAuthUrl = url;
		}
	}
}
