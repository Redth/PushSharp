using System;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Linq;
using System.Linq;
using PushSharp.Core;

namespace PushSharp.Blackberry
{
    public class BlackberryServiceConnectionFactory : IServiceConnectionFactory<BlackberryNotification>
    {
        public BlackberryServiceConnectionFactory (BlackberryConfiguration configuration)
        {
            Configuration = configuration;
        }

        public BlackberryConfiguration Configuration { get; private set; }

        public IServiceConnection<BlackberryNotification> Create()
        {
            return new BlackberryServiceConnection (Configuration);
        }
    }

    public class BlackberryServiceBroker : ServiceBroker<BlackberryNotification>
    {
        public BlackberryServiceBroker (BlackberryConfiguration configuration) : base (new BlackberryServiceConnectionFactory (configuration))
        {
        }
    }

    public class BlackberryServiceConnection : IServiceConnection<BlackberryNotification>
    {
        public BlackberryServiceConnection (BlackberryConfiguration configuration)
        {
            Configuration = configuration;
            http = new BlackberryHttpClient (Configuration);
        }

        public BlackberryConfiguration Configuration { get; private set; }

        readonly BlackberryHttpClient http;

        public async Task Send (BlackberryNotification notification)
        {                        
            var response = await http.PostNotification (notification);
            var description = string.Empty;

            var status = new BlackberryMessageStatus
            {
                Notification = notification,
                HttpStatus = HttpStatusCode.ServiceUnavailable
            };

            var bbNotStatus = string.Empty;
            status.HttpStatus = response.StatusCode;

            var xmlContent = await response.Content.ReadAsStreamAsync ();
            var doc = XDocument.Load (xmlContent);

            XElement result = doc.Descendants().FirstOrDefault(desc =>
                desc.Name == "response-result" ||
                desc.Name == "badmessage-response");
            if (result != null)
            {
                bbNotStatus = result.Attribute("code").Value;
                description = result.Attribute("desc").Value;
            }

            BlackberryNotificationStatus notStatus;
            Enum.TryParse(bbNotStatus, true, out notStatus);
            status.NotificationStatus = notStatus;

            if (status.NotificationStatus == BlackberryNotificationStatus.NoAppReceivePush)
                throw new DeviceSubscriptionExpiredException (notification);

            if (status.HttpStatus == HttpStatusCode.OK
                && status.NotificationStatus == BlackberryNotificationStatus.RequestAcceptedForProcessing)
                return;
            
            throw new BlackberryNotificationException (status, description, notification);
            
        }
    }
}

