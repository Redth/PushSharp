using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.Net.Security;

using PushSharp.Core;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace PushSharp.Google.Chrome
{
	public class ChromePushChannel : IPushChannel
	{
		ChromePushChannelSettings chromeSettings = null;

		public string AccessToken { get; private set; }
		public DateTime Expires { get; private set; }

		HttpClient http = new HttpClient();

		public ChromePushChannel(ChromePushChannelSettings channelSettings)
		{
			chromeSettings = channelSettings as ChromePushChannelSettings;	
		}

		static ChromePushChannel()
		{
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, policyErrs) => { return true; };
		}

		public void RefreshAccessToken()
		{
			var p = new Dictionary<string, string> ();

			p.Add ("client_id", chromeSettings.ClientId);
			p.Add ("client_secret", chromeSettings.ClientSecret);
			p.Add ("refresh_token", chromeSettings.RefreshToken);
			p.Add ("grant_type", chromeSettings.GrantType);

			var response = http.PostAsync (chromeSettings.AuthUrl, new FormUrlEncodedContent (p)).Result;

			var result = response.Content.ReadAsStringAsync ().Result;

			Console.WriteLine ("RESPONSE: " + result);
		}

		public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			bool success = false;
			string body = "Unknown Failure";

			var n = notification as ChromeNotification;

			try
			{
				var client = new HttpClient ();

				var url = chromeSettings.Url;

				var sc = new StringContent(string.Empty);
				sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				sc.Headers.TryAddWithoutValidation("Authorization", "Bearer " + chromeSettings.GrantType);
				sc.Headers.TryAddWithoutValidation ("channelId", n.ChannelId);
				sc.Headers.TryAddWithoutValidation ("subchannelId", ((int)n.SubChannelId).ToString());
				sc.Headers.TryAddWithoutValidation ("payload", n.Payload);


				var result = client.PostAsync (chromeSettings.Url, sc).Result;

				success = result.IsSuccessStatusCode;

				body = result.Content.ReadAsStringAsync ().Result;
			}
			catch (Exception ex)
			{
				body = ex.ToString ();
			}

			Console.WriteLine (body);

			callback (this, new SendNotificationResult (n, false, success ? null : new NotificationFailureException(body, n)));
		}

		void IDisposable.Dispose()
		{

		}
	}
}
