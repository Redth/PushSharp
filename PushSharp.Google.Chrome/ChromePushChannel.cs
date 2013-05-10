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

namespace PushSharp.Google.Chrome
{
	public class ChromePushChannel : IPushChannel
	{
		ChromePushChannelSettings chromeSettings = null;

		public ChromePushChannel(ChromePushChannelSettings channelSettings)
		{
			chromeSettings = channelSettings as ChromePushChannelSettings;	
		}

		static ChromePushChannel()
		{
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, policyErrs) => { return true; };
		}

		public async void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			bool success = false;
			string body = "Unknown Failure";

			var n = notification as ChromeNotification;

			try
			{
				var client = new HttpClient ();

				client.DefaultRequestHeaders.Add ("Content-Type", "application/json");
				client.DefaultRequestHeaders.Add ("Authorization", "Bearer " + chromeSettings.GrantType);
				client.DefaultRequestHeaders.Add ("channelId", n.ChannelId);
				client.DefaultRequestHeaders.Add ("subchannelId", ((int)n.SubChannelId).ToString());
				client.DefaultRequestHeaders.Add ("payload", n.Payload);

				var url = chromeSettings.Url;

				var result = await client.PostAsync (chromeSettings.Url, new StringContent(string.Empty));

				success = result.IsSuccessStatusCode;

				body = await result.Content.ReadAsStringAsync ();
			}
			catch (Exception ex)
			{
				body = ex.ToString ();
			}

			callback (this, new SendNotificationResult (n, false, success ? null : new NotificationFailureException(body, n)));
		}

		void IDisposable.Dispose()
		{

		}
	}
}
