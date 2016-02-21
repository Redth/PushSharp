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
using System.Text;

namespace PushSharp.Windows
{
    public class WnsServiceConnectionFactory : IServiceConnectionFactory<WnsNotification>
    {
        private WnsAccessTokenManager wnsAccessTokenManager;

        public WnsServiceConnectionFactory (WnsConfiguration configuration)
        {
            Configuration = configuration;
            wnsAccessTokenManager = new WnsAccessTokenManager(configuration);
        }

        public WnsConfiguration Configuration { get; private set; }

        public IServiceConnection<WnsNotification> Create()
        {
            return new WnsServiceConnection (Configuration, wnsAccessTokenManager);
        }
    }

    public class WnsServiceBroker : ServiceBroker<WnsNotification>
    {
        public WnsServiceBroker (WnsConfiguration configuration) : base (new WnsServiceConnectionFactory (configuration))
        {
        }
    }

    public class WnsAccessTokenManager
    {
        private WnsConfiguration configuration;
        private Task renewAccessTokenTask = null;
        private string accessToken = null;
        private HttpClient httpClient;

        public WnsAccessTokenManager(WnsConfiguration configuration)
        {
            this.httpClient = new HttpClient();
            this.configuration = configuration;
        }

        public async Task<string> GetAccessToken() {
            if (accessToken == null)
            {
                if (renewAccessTokenTask == null)
                {
                    Console.WriteLine("RenewAccessToken()");
                    renewAccessTokenTask = RenewAccessToken();
                    await renewAccessTokenTask;
                }
                else
                {
                    Console.WriteLine("Await RenewAccessToken()");
                    await renewAccessTokenTask;
                }
            }
            
            return accessToken;
        }

        public void InvalidateAccessToken(string currentAccessToken)
        {
            if (accessToken == currentAccessToken)
            {
                accessToken = null;
            }
        }

        private async Task RenewAccessToken()
        {
            var p = new Dictionary<string, string> {
                    { "grant_type", "client_credentials" },
                    { "client_id", configuration.PackageSecurityIdentifier },
                    { "client_secret", configuration.ClientSecret },
                    { "scope", "notify.windows.com" }
                };

            var result = await httpClient.PostAsync("https://login.live.com/accesstoken.srf", new FormUrlEncodedContent(p));

            var data = await result.Content.ReadAsStringAsync();

            var json = new JObject();

            try { json = JObject.Parse(data); }
            catch { }

            var accessToken = json.Value<string>("access_token");
            var tokenType = json.Value<string>("token_type");

            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(tokenType))
            {
                this.accessToken = accessToken;
            }
            else
            {
                this.accessToken = null;
                throw new UnauthorizedAccessException("Could not retrieve access token for the supplied Package Security Identifier (SID) and client secret");
            }
        }
    }


    public class WnsServiceConnection : IServiceConnection<WnsNotification>
    {
        WnsAccessTokenManager wnsAccessTokenManager;

        public WnsServiceConnection (WnsConfiguration configuration, WnsAccessTokenManager wnsAccessTokenManager)
        {
            Configuration = configuration;
            this.wnsAccessTokenManager = wnsAccessTokenManager;

            //TODO: Microsoft recommends we disable expect-100 to improve latency
            // Not sure how to do this in httpclient
        }

        public WnsConfiguration Configuration { get; private set; }

        public async Task Send (WnsNotification notification)
        {
            var accessToken = await this.wnsAccessTokenManager.GetAccessToken();
            
            //https://cloud.notify.windows.com/?token=.....
            //Authorization: Bearer {AccessToken}
            //
            var http = new HttpClient();

            http.DefaultRequestHeaders.TryAddWithoutValidation ("X-WNS-Type", string.Format ("wns/{0}", notification.Type.ToString ().ToLower ()));
            //http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Bearer", AccessToken);
            http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);

            if (notification.RequestForStatus.HasValue)
                http.DefaultRequestHeaders.TryAddWithoutValidation ("X-WNS-RequestForStatus", notification.RequestForStatus.Value.ToString().ToLower());

            if (notification.TimeToLive.HasValue)
                http.DefaultRequestHeaders.TryAddWithoutValidation ("X-WNS-TTL", notification.TimeToLive.Value.ToString()); //Time to live in seconds

            if (notification.Type == WnsNotificationType.Tile)
            {
                var winTileNot = notification as WnsTileNotification;

                if (winTileNot != null && winTileNot.CachePolicy.HasValue)
                    http.DefaultRequestHeaders.Add("X-WNS-Cache-Policy", winTileNot.CachePolicy == WnsNotificationCachePolicyType.Cache ? "cache" : "no-cache");

                if (winTileNot != null && !string.IsNullOrEmpty(winTileNot.NotificationTag))
                    http.DefaultRequestHeaders.Add("X-WNS-Tag", winTileNot.NotificationTag); // TILE only
            }
            else if (notification.Type == WnsNotificationType.Badge)
            {
                var winTileBadge = notification as WnsBadgeNotification;

                if (winTileBadge != null && winTileBadge.CachePolicy.HasValue)
                    http.DefaultRequestHeaders.Add("X-WNS-Cache-Policy", winTileBadge.CachePolicy == WnsNotificationCachePolicyType.Cache ? "cache" : "no-cache");
            }

            var content = new StringContent (
                notification.Payload.ToString (), // Get XML payload 
                System.Text.Encoding.UTF8, 
                notification.Type == WnsNotificationType.Raw ? "application/octet-stream" : "text/xml");
            
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

            //401
            if (status.HttpStatus == HttpStatusCode.Unauthorized)
            {
                wnsAccessTokenManager.InvalidateAccessToken(accessToken);
                throw new RetryAfterException("Access token expired", DateTime.UtcNow.AddSeconds(5));
            }

            // Any other error
            throw new WnsNotificationException (status);
        }

        WnsNotificationStatus ParseStatus(HttpResponseMessage resp, WnsNotification notification)
        {
            var result = new WnsNotificationStatus();

            result.Notification = notification;
            result.HttpStatus = resp.StatusCode;

            var wnsDebugTrace = TryGetHeaderValue (resp.Headers, "X-WNS-DEBUG-TRACE") ?? "";
            var wnsDeviceConnectionStatus = TryGetHeaderValue (resp.Headers, "X-WNS-DEVICECONNECTIONSTATUS") ?? "connected";
            var wnsErrorDescription = TryGetHeaderValue (resp.Headers, "X-WNS-ERROR-DESCRIPTION") ?? "";
            var wnsMsgId = TryGetHeaderValue (resp.Headers, "X-WNS-MSG-ID");
            var wnsNotificationStatus = TryGetHeaderValue (resp.Headers, "X-WNS-NOTIFICATIONSTATUS") ?? "";

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

        string TryGetHeaderValue (HttpResponseHeaders headers, string headerName)
        {
            IEnumerable<string> values;
            if (headers.TryGetValues (headerName, out values))
                return values.FirstOrDefault ();

            return null;
        }
    }
}

