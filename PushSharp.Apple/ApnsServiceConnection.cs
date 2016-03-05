using System;
using PushSharp.Core;
using System.Threading.Tasks;

namespace PushSharp.Apple
{
    public class ApnsServiceConnectionFactory : IServiceConnectionFactory<ApnsNotification>
    {
        public ApnsServiceConnectionFactory (ApnsConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ApnsConfiguration Configuration { get; private set; }

        public IServiceConnection<ApnsNotification> Create()
        {
            return new ApnsServiceConnection (Configuration);
        }
    }

    public class ApnsServiceBroker : ServiceBroker<ApnsNotification>
    {
        public ApnsServiceBroker (ApnsConfiguration configuration) : base (new ApnsServiceConnectionFactory (configuration))
        {
        }
    }

    public class ApnsServiceConnection : IServiceConnection<ApnsNotification>
    {
        readonly ApnsConnection connection;

        public ApnsServiceConnection (ApnsConfiguration configuration)
        {
            connection = new ApnsConnection (configuration);
        }
        
        public async Task Send (ApnsNotification notification)
        {
            var completableNotification = new ApnsConnection.CompletableApnsNotification (notification);

            connection.Send (completableNotification);

            var ex = await completableNotification.WaitForComplete ().ConfigureAwait (false);

            //Log.Info ("Finished Waiting for Notification: {0} (Had Exception? {1})", notification.Identifier, ex != null);

            if (ex != null) {
                throw ex;
            }
        }
    }
}
