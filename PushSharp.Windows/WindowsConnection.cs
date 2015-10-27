using System;
using PushSharp.Core;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Linq;
using System.Net;

namespace PushSharp.Windows
{
    public class WnsServiceConnectionFactory : IServiceConnectionFactory<WnsNotification>
    {
        public WnsServiceConnectionFactory (WnsConfiguration configuration)
        {
            Configuration = configuration;
        }

        public WnsConfiguration Configuration { get; private set; }

        public IServiceConnection<WnsNotification> Create()
        {
            return new WnsServiceConnection (Configuration);
        }
    }

    public class WnsServiceBroker : ServiceBroker<WnsNotification>
    {
        public WnsServiceBroker (WnsConfiguration configuration) : base (new WnsServiceConnectionFactory (configuration))
        {
        }
    }

    public class WnsServiceConnection : IServiceConnection<WnsNotification>
    {           
        HttpClient http;

        public WnsServiceConnection (WnsConfiguration configuration)
        {
            Configuration = configuration;

            //TODO: Microsoft recommends we disable expect-100 to improve latency
            // Not sure how to do this in httpclient
            http = new HttpClient ();
        }

        public WnsConfiguration Configuration { get; private set; }
        public string AccessToken { get; private set; }
        public string TokenType { get; private set; }

        public async Task Send (WnsNotification notification)
        {           
            //See if we need an access token
            if (string.IsNullOrEmpty(AccessToken))
                RenewAccessToken();
            
            //https://cloud.notify.windows.com/?token=.....
            //Authorization: Bearer {AccessToken}
            //

            http.DefaultRequestHeaders.Add ("X-WNS-Type", string.Format ("wns/{0}", notification.Type.ToString ().ToLower ()));
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", AccessToken);

            if (notification.RequestForStatus.HasValue)
                http.DefaultRequestHeaders.Add("X-WNS-RequestForStatus", notification.RequestForStatus.Value.ToString().ToLower());

            if (notification.TimeToLive.HasValue)
                http.DefaultRequestHeaders.Add("X-WNS-TTL", notification.TimeToLive.Value.ToString()); //Time to live in seconds

            if (notification.Type == WindowsNotificationType.Tile)
            {
                var winTileNot = notification as WnsTileNotification;

                if (winTileNot != null && winTileNot.CachePolicy.HasValue)
                    http.DefaultRequestHeaders.Add("X-WNS-Cache-Policy", winTileNot.CachePolicy == WnsNotificationCachePolicyType.Cache ? "cache" : "no-cache");

                if (winTileNot != null && !string.IsNullOrEmpty(winTileNot.NotificationTag))
                    http.DefaultRequestHeaders.Add("X-WNS-Tag", winTileNot.NotificationTag); // TILE only
            }
            else if (notification.Type == WindowsNotificationType.Badge)
            {
                var winTileBadge = notification as WindowsBadgeNotification;

                if (winTileBadge != null && winTileBadge.CachePolicy.HasValue)
                    http.DefaultRequestHeaders.Add("X-WNS-Cache-Policy", winTileBadge.CachePolicy == WnsNotificationCachePolicyType.Cache ? "cache" : "no-cache");
            }

            var content = new StringContent (
                notification.Payload.ToString (), // Get XML payload 
                System.Text.Encoding.UTF8, 
                notification.Type == WindowsNotificationType.Raw ? "application/octet-stream" : "application/xml");
            
            var result = await http.PostAsync (notification.ChannelUri, content);

            var status = ParseStatus (result, notification);

            //RESPONSE HEADERS
            // X-WNS-Debug-Trace   string
            // X-WNS-DeviceConnectionStatus  connected | disconnected | tempdisconnected   (if RequestForStatus was set to true)
            // X-WNS-Error-Description  string
            // X-WNS-Msg-ID   string  (max 16 char)
            // X-WNS-NotificationStatus   received | dropped | channelthrottled
            //      

            // 200 OK
            // 400  One or more headers were specified incorrectly or conflict with another header.
            // 401  The cloud service did not present a valid authentication ticket. The OAuth ticket may be invalid.
            // 403  The cloud service is not authorized to send a notification to this URI even though they are authenticated.
            // 404  The channel URI is not valid or is not recognized by WNS. - Raise Expiry
            // 405  Invalid Method - never will get
            // 406  The cloud service exceeded its throttle limit.
            // 410  The channel expired. - Raise Expiry
            // 413  The notification payload exceeds the 5000 byte size limit.
            // 500  An internal failure caused notification delivery to fail.
            // 503  The server is currently unavailable.

            // OK, everything worked!
            if (status.HttpStatus == HttpStatusCode.OK
                && status.NotificationStatus == WnsNotificationSendStatus.Received) {
                return;
            }

            //404 or 410
            if (status.HttpStatus == HttpStatusCode.NotFound || status.HttpStatus == HttpStatusCode.Gone) { 
                throw new DeviceSubscriptonExpiredException {
                    OldSubscriptionId = notification.ChannelUri,
                    ExpiredAt = DateTime.UtcNow
                };
            }

            // Any other error
            throw new NotificationException (status.ToString (), notification);
        }

        async Task RenewAccessToken()
        {
            var p = new Dictionary<string, string> {
                { "grant_type", "client_credentials" },
                { "client_id", Configuration.PackageSecurityIdentifier },
                { "client_scope", Configuration.ClientSecret },
                { "scope", "notify.windows.com" }
            };

            var result = await http.PostAsync ("https://login.live.com/accesstoken.srf", new FormUrlEncodedContent (p));

            var data = await result.Content.ReadAsStringAsync ();

            var json = new JObject();

            try { json = JObject.Parse (data); }
            catch { }

            var accessToken = json.Value<string>("access_token");
            var tokenType = json.Value<string>("token_type");

            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(tokenType)) {
                AccessToken = accessToken;
                TokenType = tokenType;
            } else {
                throw new UnauthorizedAccessException("Could not retrieve access token for the supplied Package Security Identifier (SID) and client secret");
            }
        }


        WnsNotificationStatus ParseStatus(HttpResponseMessage resp, WnsNotification notification)
        {
            var result = new WnsNotificationStatus();

            result.Notification = notification;
            result.HttpStatus = resp.StatusCode;

            var wnsDebugTrace = resp.Headers.GetValues ("X-WNS-Debug-Trace").FirstOrDefault ();
            var wnsDeviceConnectionStatus = resp.Headers.GetValues ("X-WNS-DeviceConnectionStatus").FirstOrDefault () ?? "connected";
            var wnsErrorDescription = resp.Headers.GetValues ("X-WNS-Error-Description").FirstOrDefault ();
            var wnsMsgId = resp.Headers.GetValues ("X-WNS-Msg-ID").FirstOrDefault ();
            var wnsNotificationStatus = resp.Headers.GetValues ("X-WNS-NotificationStatus").FirstOrDefault () ?? "";

            result.DebugTrace = wnsDebugTrace;
            result.ErrorDescription = wnsErrorDescription;
            result.MessageId = wnsMsgId;

            if (wnsNotificationStatus.Equals("received", StringComparison.InvariantCultureIgnoreCase))
                result.NotificationStatus = WnsNotificationSendStatus.Received;
            else if (wnsNotificationStatus.Equals("dropped", StringComparison.InvariantCultureIgnoreCase))
                result.NotificationStatus = WnsNotificationSendStatus.Dropped;
            else
                result.NotificationStatus = WnsNotificationSendStatus.ChannelThrottled;

            if (wnsDeviceConnectionStatus.Equals("connected", StringComparison.InvariantCultureIgnoreCase))
                result.DeviceConnectionStatus = WnsDeviceConnectionStatus.Connected;
            else if (wnsDeviceConnectionStatus.Equals("tempdisconnected", StringComparison.InvariantCultureIgnoreCase))
                result.DeviceConnectionStatus = WnsDeviceConnectionStatus.TempDisconnected;
            else
                result.DeviceConnectionStatus = WnsDeviceConnectionStatus.Disconnected;

            return result;
        }
    }
}

