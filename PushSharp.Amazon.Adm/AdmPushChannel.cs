using System;
using System.Net.Http.Headers;
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
		HttpClient http = new HttpClient();

		public AdmPushChannel (AdmPushChannelSettings admSettings)
		{
			this.admSettings = admSettings;

		    Expires = DateTime.UtcNow.AddYears(-1);

			//http.DefaultRequestHeaders.Add ("Content-Type", "application/json");
			http.DefaultRequestHeaders.Add ("X-Amzn-Type-Version", "com.amazon.device.messaging.ADMMessage@1.0");
			http.DefaultRequestHeaders.Add ("X-Amzn-Accept-Type", "com.amazon.device.messaging.ADMSendResult@1.0");
			http.DefaultRequestHeaders.Add ("Accept", "application/json");


            //http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);
            http.DefaultRequestHeaders.ConnectionClose = true;

            http.DefaultRequestHeaders.Remove("connection");
		}

		public void SendNotification (INotification notification, SendNotificationCallbackDelegate callback)
		{
			SendNotificationAsync (notification, callback);
		}

		void SendNotificationAsync(INotification notification, SendNotificationCallbackDelegate callback)
		{
            var n = notification as AdmNotification;

			try
			{
				Interlocked.Increment(ref waitCounter);

                if (string.IsNullOrEmpty(AccessToken) || Expires <= DateTime.UtcNow)
                {
                    UpdateAccessToken();
                    http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.AccessToken);
                }
                
			    var sc = new StringContent(n.ToJson());
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");

				var response = http.PostAsync (string.Format(admSettings.AdmSendUrl, n.RegistrationId), sc).Result;

				if (response.IsSuccessStatusCode)
				{
					callback(this, new SendNotificationResult(n));
					return;
				}

			    var data = response.Content.ReadAsStringAsync().Result;

				var json = JObject.Parse (data);

				var reason = json ["reason"].ToString ();


			    var regId = n.RegistrationId;
                
                if (json["registrationID"] != null)
                    regId = json["registrationID"].ToString();

				switch (response.StatusCode)
				{
					case HttpStatusCode.BadGateway: //400
                    case HttpStatusCode.BadRequest: //
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


		void UpdateAccessToken()
		{
			var http = new HttpClient ();

			var param = new Dictionary<string, string> ();
			param.Add ("grant_type", "client_credentials");
			param.Add ("scope", "messaging:push");
			param.Add ("client_id", admSettings.ClientId);
			param.Add ("client_secret", admSettings.ClientSecret);


			var result = http.PostAsync (admSettings.AdmAuthUrl, new FormUrlEncodedContent (param)).Result;
		    var data = result.Content.ReadAsStringAsync().Result;

			var json = JObject.Parse (data);

			this.AccessToken = json ["access_token"].ToString ();

		    JToken expiresJson = new JValue(3540);
		    if (json.TryGetValue("expires_in", out expiresJson))
		        Expires = DateTime.UtcNow.AddSeconds(expiresJson.ToObject<int>() - 60);
		    else
		        Expires = DateTime.UtcNow.AddSeconds(3540);

			if (result.Headers.Contains ("X-Amzn-RequestId"))
				this.LastAmazonRequestId = string.Join("; ", result.Headers.GetValues("X-Amzn-RequestId"));
            
			LastRequest = DateTime.UtcNow;
		}

        public DateTime Expires { get; set; }
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

