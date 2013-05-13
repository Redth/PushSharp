using System;
using PushSharp.Core;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading;

namespace PushSharp.Amazon.Adm
{
	public class AdmPushChannel : IPushChannel
	{
		AdmPushChannelSettings admSettings;
		long waitCounter = 0;

		public AdmPushChannel (AdmPushChannelSettings admSettings)
		{
			this.admSettings = admSettings;
		}

		public void SendNotification (INotification notification, SendNotificationCallbackDelegate callback)
		{
			SendNotificationAsync (notification, callback);
		}

		async Task SendNotificationAsync(INotification notification, SendNotificationCallbackDelegate callback)
		{
			if (string.IsNullOrEmpty (AccessToken))
				UpdateAccessToken ().Wait ();

			try
			{
				Interlocked.Increment(ref waitCounter);

				var n = notification as AdmNotification;

				var http = new HttpClient ();
				http.DefaultRequestHeaders.Add ("Authorization", "Bearer " + this.AccessToken);
				http.DefaultRequestHeaders.Add ("Content-Type", "application/json");
				http.DefaultRequestHeaders.Add ("X-Amzn-Type-Version", "com.amazon.device.messaging.ADMMessage@1.0");
				http.DefaultRequestHeaders.Add ("X-Amzn-Accept-Type", "com.amazon.device.messaging.ADMSendResult@1.0");
				http.DefaultRequestHeaders.Add ("Accept", "application/json");

				var response = await http.PostAsync (string.Format(admSettings.AdmSendUrl, n.RegistrationId), new StringContent (n.ToJson()));

				if (response.IsSuccessStatusCode)
				{
					callback(this, new SendNotificationResult(n));
					return;
				}
			
				var json = JObject.Parse (await response.Content.ReadAsStringAsync());

				var reason = json ["reason"].ToString ();
				var regId = json ["registrationID"].ToString ();

				switch (response.StatusCode)
				{
					case HttpStatusCode.BadGateway: //400
						if ("InvalidRegistrationId".Equals (reason, StringComparison.InvariantCultureIgnoreCase))
						{
							callback (this, new SendNotificationResult (n)
							{
								IsSubscriptionExpired = true,
								OldSubscriptionId = regId
							});
						}
						callback(this, new SendNotificationResult(n, false, new AdmSendException(n, "Send Failed: " + reason)));
						break;
					case HttpStatusCode.Unauthorized: //401
						//Access token expired
						this.AccessToken = null;
						callback(this, new SendNotificationResult(n, true));
						break;
					case HttpStatusCode.Forbidden: //403
						callback(this, new SendNotificationResult(n, true, new AdmRateLimitExceededException(n, "Rate Limit Exceeded (" + reason + ")"))); 
						break;
					case HttpStatusCode.RequestEntityTooLarge: //413
						callback(this, new SendNotificationResult(n, false, new AdmMessageTooLargeException(n, "ADM Message too Large, must be <= 6kb")));
						break;
					default:
						callback(this, new SendNotificationResult(n, false, new AdmSendException(n, "Unknown Failure")));
						break;
				}
			}
			catch (Exception ex)
			{
				callback(this, new SendNotificationResult(n, false, new AdmSendException(n, "Unknown Failure")));
			}
			finally
			{
				Interlocked.Decrement (ref waitCounter);
			}
		}


		async Task UpdateAccessToken()
		{
			var http = new HttpClient ();

			var param = new Dictionary<string, string> ();
			param.Add ("grant_type", "client_credentials");
			param.Add ("scope", "messaging:push");
			param.Add ("client_id", admSettings.ClientId);
			param.Add ("client_secret", admSettings.ClientSecret);


			var result = await http.PostAsync (admSettings.AdmAuthUrl, new FormUrlEncodedContent (param));

			var json = JObject.Parse (await result.Content.ReadAsStringAsync());

			this.AccessToken = json ["access_token"].ToString ();

			if (result.Headers.Contains ("X-Amzn-RequestId"))
				this.LastAmazonRequestId = string.Join("; ", result.Headers.GetValues("X-Amzn-RequestId"));

			LastRequest = DateTime.UtcNow;
		}

		public DateTime LastRequest { get; private set; }
		public string LastAmazonRequestId { get; private set; }
		public string AccessToken { get; private set; }


		public void Dispose ()
		{
			var slept = 0;
			while (Interlocked.Read(ref waitCounter) > 0 && slept <= 5000)
			{
				slept += 100;
				Thread.Sleep(100);
			}
		}

	}
}

