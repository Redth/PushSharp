using System;
using PushSharp.Core;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace PushSharp.Firefox
{
    public class FirefoxServiceConnectionFactory : IServiceConnectionFactory<FirefoxNotification>
    {
        public FirefoxServiceConnectionFactory (FirefoxConfiguration configuration)
        {
            Configuration = configuration;
        }

        public FirefoxConfiguration Configuration { get; private set; }

        public IServiceConnection<FirefoxNotification> Create()
        {
            return new FirefoxServiceConnection ();
        }
    }

    public class FirefoxServiceBroker : ServiceBroker<FirefoxNotification>
    {
        public FirefoxServiceBroker (FirefoxConfiguration configuration) : base (new FirefoxServiceConnectionFactory (configuration))
        {
        }
    }

    public class FirefoxServiceConnection : IServiceConnection<FirefoxNotification>
    {   
        HttpClient http = new HttpClient ();

        public async Task Send (FirefoxNotification notification)
        {            
            var data = notification.ToString ();

            http.DefaultRequestHeaders.UserAgent.Clear ();
            http.DefaultRequestHeaders.UserAgent.Add (new ProductInfoHeaderValue ("PushSharp", "3.0"));

            var result = await http.PutAsync (notification.EndPointUrl, new StringContent (data));

            if (result.StatusCode != HttpStatusCode.OK && result.StatusCode != HttpStatusCode.NoContent) {
                throw new FirefoxNotificationException (notification, "HTTP Status: " + result.StatusCode);
            }
        }
    }
}
