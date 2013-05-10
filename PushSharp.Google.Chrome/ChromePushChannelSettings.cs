using System;
using System.Collections.Generic;
using System.Text;
using PushSharp.Core;

namespace PushSharp.Google.Chrome
{
	public class ChromePushChannelSettings : IPushChannelSettings
	{
		private const string CHROME_SEND_URL = "https://www.googleapis.com/gcm_for_chrome/v1/messages";

		public ChromePushChannelSettings(string clientId, string clientSecret)
		{
			this.ClientId = clientId;
			this.ClientSecret = clientSecret;

			this.Url = CHROME_SEND_URL;
		}

		public string ClientId { get; private set; }
		public string ClientSecret { get; private set; }

		internal string RefreshToken { get; set; }
		internal string GrantType { get; set; }

		public string Url { get; private set; }

		public void OverrideUrl(string url)
		{
			Url = url;
		}
	}
}
