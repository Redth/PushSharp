using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PushSharp.Core;
using PushSharp.Web.Exceptions;
using PushSharp.Web.Helpers;

namespace PushSharp.Web
{
    public class WebPushConnection : IServiceConnection<WebPushNotification>
    {
        readonly HttpClient http;
        private static string DefaultTTL = "2419200";

        public WebPushConfiguration Configuration { get; }

        public WebPushConnection(WebPushConfiguration configuration)
        {
            Configuration = configuration;
            http = new HttpClient();

            http.DefaultRequestHeaders.UserAgent.Clear();
            http.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("PushSharp", "3.0"));
        }

        public async Task Send(WebPushNotification notification)
        {
            var request = BuildRequest(notification);
            var response = await http.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                await processResponseError(response, notification).ConfigureAwait(false);
            }
        }

        private HttpRequestMessage BuildRequest(WebPushNotification notification)
        {
            var subscription = notification.Subscription;
            var json = notification.GetJson();

            /*
                see for details about message encryption 
                https://tools.ietf.org/html/draft-ietf-webpush-encryption-04
                https://developers.google.com/web/updates/2016/03/web-push-encryption
            */
            var encryptedPayload = EncryptionHelper.Encrypt(subscription, json);

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, subscription.EndPoint);
            if (encryptedPayload != null)
            {
                request.Content = new ByteArrayContent(encryptedPayload.Payload);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                request.Content.Headers.ContentLength = encryptedPayload.Payload.Length;
                request.Content.Headers.ContentEncoding.Add("aesgcm");
                request.Headers.Add("Crypto-Key", "keyid=p256dh;dh=" + WebEncoder.Base64UrlEncode(encryptedPayload.PublicKey));
                request.Headers.Add("Encryption", "keyid=p256dh;salt=" + WebEncoder.Base64UrlEncode(encryptedPayload.Salt));
            }

            request.Headers.TryAddWithoutValidation("TTL", DefaultTTL);

            var isGsm = subscription.EndPoint.StartsWith(Configuration.GcmEndPoint, StringComparison.Ordinal);
            if (isGsm)
            {
                if (string.IsNullOrEmpty(Configuration.GcmAPIKey))
                {
                    throw new Exception("GcmAPIKey is required.");
                }
                request.Headers.TryAddWithoutValidation("Authorization", "key=" + Configuration.GcmAPIKey);
            }

            return request;
        }

        async Task processResponseError(HttpResponseMessage httpResponse, WebPushNotification notification)
        {
            string responseBody = null;

            try
            {
                responseBody = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch { }

            var msg = $"HTTP Error: Status: {httpResponse.StatusCode}, ReasonPhrase: {httpResponse.ReasonPhrase}";

            var isExpiredSubscr = IsExpiredSubscription(httpResponse);
            throw new WebPushNotificationException(notification, msg, responseBody, isExpiredSubscr);
        }

        private bool IsExpiredSubscription(HttpResponseMessage response)
        {
            //both gcm and mozilla push service send gone response for outdated subscriptions
            if (response.StatusCode == HttpStatusCode.Gone)
            {
                return true;
            }

            //gcm returns such response for invalid subscription
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                if (response.ReasonPhrase == "UnauthorizedRegistration")
                {
                    return true;
                }
            }

            //firefox push service sends notfound for invalid subscription
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return true;
            }

            return false;
        }
    }
}