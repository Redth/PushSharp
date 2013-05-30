using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.Net.Security;
using System.Web;
using PushSharp.Core;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;

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
		    Expires = DateTime.UtcNow.AddYears(-1);
		    AccessToken = string.Empty;
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
		    p.Add("grant_type", "refresh_token");

			var response = http.PostAsync (chromeSettings.AuthUrl, new FormUrlEncodedContent (p)).Result;

			var result = response.Content.ReadAsStringAsync ().Result;

		    var json = JObject.Parse(result);

		    this.AccessToken = json["access_token"].ToString();

            JToken expiresJson = new JValue(3400);
		    json.TryGetValue("expires_in", out expiresJson);

		    this.Expires = DateTime.UtcNow.AddSeconds(expiresJson.Value<int>());

            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);
		}

		public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			bool success = false;
			string body = "Unknown Failure";

			var n = notification as ChromeNotification;

			try
			{
                if (string.IsNullOrEmpty(this.AccessToken) || DateTime.UtcNow >= this.Expires)
                    RefreshAccessToken();
                
			    var json = new JObject();
			    json["channelId"] = n.ChannelId;
			    json["subchannelId"] = ((int)n.SubChannelId).ToString();
			    json["payload"] = n.Payload;
                
				var sc = new StringContent(json.ToString());
				sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                
				sc.Headers.TryAddWithoutValidation("Authorization", "Bearer " + this.AccessToken);
				
				var result = http.PostAsync (chromeSettings.Url, sc).Result;

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
