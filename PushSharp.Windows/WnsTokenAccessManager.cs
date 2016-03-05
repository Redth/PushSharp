using System;
using System.Threading.Tasks;
using System.Net.Http;
using PushSharp.Core;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace PushSharp.Windows
{
    public class WnsAccessTokenManager
    {
        Task renewAccessTokenTask = null;
        string accessToken = null;
        HttpClient http;

        public WnsAccessTokenManager (WnsConfiguration configuration)
        {
            http = new HttpClient ();
            Configuration = configuration;
        }

        public WnsConfiguration Configuration { get; private set; }

        public async Task<string> GetAccessToken ()
        {
            if (accessToken == null) {
                if (renewAccessTokenTask == null) {
                    Log.Info ("Renewing Access Token");
                    renewAccessTokenTask = RenewAccessToken ();
                    await renewAccessTokenTask;
                } else {
                    Log.Info ("Waiting for access token");
                    await renewAccessTokenTask;
                }
            }

            return accessToken;
        }

        public void InvalidateAccessToken (string currentAccessToken)
        {
            if (accessToken == currentAccessToken)
                accessToken = null;
        }

        async Task RenewAccessToken ()
        {
            var p = new Dictionary<string, string> {
                { "grant_type", "client_credentials" },
                { "client_id", Configuration.PackageSecurityIdentifier },
                { "client_secret", Configuration.ClientSecret },
                { "scope", "notify.windows.com" }
            };

            var result = await http.PostAsync ("https://login.live.com/accesstoken.srf", new FormUrlEncodedContent (p));

            var data = await result.Content.ReadAsStringAsync ();

            var token = string.Empty;
            var tokenType = string.Empty;

            try {
                var json = JObject.Parse (data);
                token = json.Value<string> ("access_token");
                tokenType = json.Value<string> ("token_type");
            } catch {
            }

            if (!string.IsNullOrEmpty (token) && !string.IsNullOrEmpty (tokenType)) {
                accessToken = token;
            } else {
                accessToken = null;
                throw new UnauthorizedAccessException ("Could not retrieve access token for the supplied Package Security Identifier (SID) and client secret");
            }
        }
    }
}

