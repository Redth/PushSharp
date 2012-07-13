using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using PushSharp.Common;

namespace PushSharp.Windows
{
	public class WindowsPushChannel : Common.PushChannelBase
	{
		public string AccessToken { get; private set; }
		public string TokenType { get; private set; }

		WindowsPushChannelSettings channelSettings;

		public WindowsPushChannel(WindowsPushChannelSettings channelSettings, PushServiceSettings serviceSettings) : base(channelSettings, serviceSettings)
		{
			this.channelSettings = channelSettings;
		}

		void RenewAccessToken()
		{
			var postData = new StringBuilder();

			postData.AppendFormat("{0}={1}&", "grant_type", "client_credentials");
			postData.AppendFormat("{0}={1}&", "client_id", HttpUtility.UrlEncode(this.channelSettings.PackageSecurityIdentifier));
			postData.AppendFormat("{0}={1}&", "client_secret", HttpUtility.UrlEncode(this.channelSettings.ClientSecret));
			postData.AppendFormat("{0}={1}", "scope", "notify.windows.com");
			
			var wc = new WebClient();
			wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

			var response = string.Empty;

			try { response = wc.UploadString("https://login.live.com/accesstoken.srf", "POST", postData.ToString()); }
			catch (Exception ex) { this.Events.RaiseChannelException(ex); }

			var json = new JObject();

			try { json = JObject.Parse(response); }
			catch { }

			var accessToken = json.Value<string>("access_token");
			var tokenType = json.Value<string>("token_type");

			if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(tokenType))
			{
				this.AccessToken = accessToken;
				this.TokenType = tokenType;
			}
			else
			{
				this.Events.RaiseChannelException(new UnauthorizedAccessException("Could not retrieve access token for the supplied Package Security Identifier (SID) and client secret"));
			}
		}

		protected override void SendNotification(Common.Notification notification)
		{
			//See if we need an access token
			if (string.IsNullOrEmpty(AccessToken))
				RenewAccessToken();

			var winNotification = notification as WindowsNotification;
			
			//https://cloud.notify.windows.com/?token=.....
			//Authorization: Bearer {AccessToken}
			//

			var wnsType = string.Empty;
			switch (winNotification.Type)
			{
				case WindowsNotificationType.Tile:
					wnsType = "wns/tile";
					break;
				case WindowsNotificationType.Badge:
					wnsType = "wns/badge";
					break;
				case WindowsNotificationType.Toast:
					wnsType = "wns/toast";
					break;
				default:
					wnsType = "wns/raw";
					break;
			}

			var request = (HttpWebRequest)HttpWebRequest.Create("https://cloud.notify.windows.com");
			request.Method = "POST";
			request.Headers.Add("X-WNS-Type", wnsType);
			request.Headers.Add("Authorization", string.Format("Bearer {0}", this.AccessToken));
			request.ContentType = "text/xml";

			if (winNotification.Type == WindowsNotificationType.Tile)
			{
				var winTileNot = winNotification as WindowsTileNotification;

				if (winTileNot != null && winTileNot.CachePolicy.HasValue)
					request.Headers.Add("X-WNS-Cache-Policy", winTileNot.CachePolicy == WindowsNotificationCachePolicyType.Cache ? "cache" : "no-cache");
			}
			else if (winNotification.Type == WindowsNotificationType.Badge)
			{
				var winTileBadge = winNotification as WindowsBadgeNotification;

				if (winTileBadge != null && winTileBadge.CachePolicy.HasValue)
					request.Headers.Add("X-WNS-Cache-Policy", winTileBadge.CachePolicy == WindowsNotificationCachePolicyType.Cache ? "cache" : "no-cache");
			}
			
			if (winNotification.RequestForStatus.HasValue)
				request.Headers.Add("X-WNS-RequestForStatus", winNotification.RequestForStatus.Value.ToString().ToLower());

			if (winNotification.Type == WindowsNotificationType.Tile)
			{
				var winTileNot = winNotification as WindowsTileNotification;

				if (winTileNot != null && !string.IsNullOrEmpty(winTileNot.Tag))
					request.Headers.Add("X-WNS-Tag", winTileNot.Tag); // TILE only
			}

			if (winNotification.TimeToLive.HasValue)
				request.Headers.Add("X-WNS-TTL", winNotification.TimeToLive.Value.ToString()); //Time to live in seconds

			//Microsoft recommends we disable expect-100 to improve latency
			request.ServicePoint.Expect100Continue = false;


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
			// 404	The channel URI is not valid or is not recognized by WNS. - Raise Expiry
			// 405	Invalid Method - never will get
			// 406	The cloud service exceeded its throttle limit.
			// 410	The channel expired. - Raise Expiry
			// 413	The notification payload exceeds the 5000 byte size limit.
			// 500	An internal failure caused notification delivery to fail.
			// 503	The server is currently unavailable.
		}






	
	}
}
