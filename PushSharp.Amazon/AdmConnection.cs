using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Net;
using PushSharp.Core;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace PushSharp.Amazon
{
    public class AdmServiceConnectionFactory : IServiceConnectionFactory<AdmNotification>
    {
        public AdmServiceConnectionFactory (AdmConfiguration configuration)
        {
            Configuration = configuration;
        }

        public AdmConfiguration Configuration { get; private set; }

        public IServiceConnection<AdmNotification> Create()
        {
            return new AdmServiceConnection (Configuration);
        }
    }

    public class AdmServiceBroker : ServiceBroker<AdmNotification>
    {
        public AdmServiceBroker (AdmConfiguration configuration) : base (new AdmServiceConnectionFactory (configuration))
        {
        }
    }

    public class AdmServiceConnection : IServiceConnection<AdmNotification>
    {
        public AdmServiceConnection (AdmConfiguration configuration)
        {
            Configuration = configuration;

            Expires = DateTime.UtcNow.AddYears(-1);

            http.DefaultRequestHeaders.Add ("X-Amzn-Type-Version", "com.amazon.device.messaging.ADMMessage@1.0");
            http.DefaultRequestHeaders.Add ("X-Amzn-Accept-Type", "com.amazon.device.messaging.ADMSendResult@1.0");
            http.DefaultRequestHeaders.Add ("Accept", "application/json");
            http.DefaultRequestHeaders.ConnectionClose = true;

            http.DefaultRequestHeaders.Remove("connection");
        }

        public AdmConfiguration Configuration { get; private set; }

        public DateTime Expires { get; set; }
        public DateTime LastRequest { get; private set; }
        public string LastAmazonRequestId { get; private set; }
        public string AccessToken { get; private set; }

        readonly HttpClient http = new HttpClient ();

        public async Task Send (AdmNotification notification)
        {
            try
            {
                if (string.IsNullOrEmpty(AccessToken) || Expires <= DateTime.UtcNow) {
                    await UpdateAccessToken ();
                    http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + AccessToken);
                    //http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
                }

                var sc = new StringContent(notification.ToJson ());
                sc.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await http.PostAsync (string.Format(Configuration.AdmSendUrl, notification.RegistrationId), sc);

                // We're done here if it was a success
                if (response.IsSuccessStatusCode) {                    
                    return;
                }

                var data = await response.Content.ReadAsStringAsync();

                var json = JObject.Parse (data);

                var reason = json ["reason"].ToString ();

                var regId = notification.RegistrationId;

                if (json["registrationID"] != null)
                    regId = json["registrationID"].ToString();

                switch (response.StatusCode)
                {
                case HttpStatusCode.BadGateway: //400
                case HttpStatusCode.BadRequest: //
                    if ("InvalidRegistrationId".Equals (reason, StringComparison.InvariantCultureIgnoreCase)) {
                        throw new DeviceSubscriptionExpiredException (notification) {
                            OldSubscriptionId = regId,
                            ExpiredAt = DateTime.UtcNow
                        };
                    }
                    throw new NotificationException ("Notification Failed: " + reason, notification);
                case HttpStatusCode.Unauthorized: //401
                    //Access token expired
                    AccessToken = null;
                    throw new UnauthorizedAccessException ("Access token failed authorization");
                case HttpStatusCode.Forbidden: //403
                    throw new AdmRateLimitExceededException (reason, notification);
                case HttpStatusCode.RequestEntityTooLarge: //413
                    throw new AdmMessageTooLargeException (notification);
                default:
                    throw new NotificationException ("Unknown ADM Failure", notification);
                }
            }
            catch (Exception ex)
            {
                throw new NotificationException ("Unknown ADM Failure", notification, ex);
            }
        }


        async Task UpdateAccessToken()
        {
            var http = new HttpClient ();

            var param = new Dictionary<string, string> ();
            param.Add ("grant_type", "client_credentials");
            param.Add ("scope", "messaging:push");
            param.Add ("client_id", Configuration.ClientId);
            param.Add ("client_secret", Configuration.ClientSecret);


            var result = await http.PostAsync (Configuration.AdmAuthUrl, new FormUrlEncodedContent (param));
            var data = await result.Content.ReadAsStringAsync();

            var json = JObject.Parse (data);

            AccessToken = json ["access_token"].ToString ();

            JToken expiresJson = new JValue(3540);
            if (json.TryGetValue("expires_in", out expiresJson))
                Expires = DateTime.UtcNow.AddSeconds(expiresJson.ToObject<int>() - 60);
            else
                Expires = DateTime.UtcNow.AddSeconds(3540);

            if (result.Headers.Contains ("X-Amzn-RequestId"))
                this.LastAmazonRequestId = string.Join("; ", result.Headers.GetValues("X-Amzn-RequestId"));

            LastRequest = DateTime.UtcNow;
        }

    }
}
