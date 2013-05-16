using System;
using System.Collections.Generic;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Google.Chrome
{
	public class ChromePushChannelSettings : IPushChannelSettings
	{
		private const string CHROME_SEND_URL = "https://www.googleapis.com/gcm_for_chrome/v1/messages";
		private const string CHROME_AUTH_URL = "https://accounts.google.com/o/auth2/auth";

		public ChromePushChannelSettings(string clientId, string clientSecret)
		{
			this.ClientId = clientId;
			this.ClientSecret = clientSecret;

			this.Url = CHROME_SEND_URL;
			this.AuthUrl = CHROME_AUTH_URL;
		}

		public string ClientId { get; private set; }
		public string ClientSecret { get; private set; }

		public string RefreshToken { get; set; }
		public string GrantType { get; set; }

		public string Url { get; private set; }
		public string AuthUrl { get; private set; }

		public void OverrideUrl(string url)
		{
			Url = url;
		}

		public void OverrideAuthUrl(string url)
		{
			AuthUrl = url;
		}
	}
}
