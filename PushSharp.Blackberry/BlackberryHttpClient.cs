using System;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PushSharp.Blackberry
{
    public class BlackberryHttpClient : HttpClient
    {
        public BlackberryConfiguration Configuration { get; private set; }

        public BlackberryHttpClient (BlackberryConfiguration configuration) : base()
        {
            Configuration = configuration;

            var authInfo = Configuration.ApplicationId + ":" + Configuration.Password;
            authInfo = Convert.ToBase64String (Encoding.Default.GetBytes(authInfo));

            this.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue ("Basic", authInfo);
            this.DefaultRequestHeaders.ConnectionClose = true;

            this.DefaultRequestHeaders.Remove("connection");
        }

        public Task<HttpResponseMessage> PostNotification (BlackberryNotification notification)
        {
            var c = new MultipartContent ("related", Configuration.Boundary);
            c.Headers.Remove("Content-Type");
            c.Headers.TryAddWithoutValidation("Content-Type", "multipart/related; boundary=" + Configuration.Boundary + "; type=application/xml");

            var xml = notification.ToPapXml ();


            c.Add (new StringContent (xml, Encoding.UTF8, "application/xml"));

            var bc = new ByteArrayContent(notification.Content.Content);
            bc.Headers.Add("Content-Type", notification.Content.ContentType);

            foreach (var header in notification.Content.Headers)
                bc.Headers.Add(header.Key, header.Value);

            c.Add(bc);

            return PostAsync (Configuration.SendUrl, c);
        }
    }

}

