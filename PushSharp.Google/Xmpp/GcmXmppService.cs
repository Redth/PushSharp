using System;
using PushSharp.Core;
using System.Threading.Tasks;

namespace PushSharp.Google
{
    public class GcmXmppServiceConnectionFactory : IServiceConnectionFactory<GcmXmppNotification>
    {
        public GcmXmppServiceConnectionFactory (GcmXmppConfiguration configuration)
        {
            Configuration = configuration;
        }

        public GcmXmppConfiguration Configuration { get; private set; }

        public IServiceConnection<GcmXmppNotification> Create()
        {
            return new GcmXmppServiceConnection (Configuration);
        }
    }

    public class GcmXmppServiceBroker : ServiceBroker<GcmXmppNotification>
    {
        public GcmXmppServiceBroker (GcmXmppConfiguration configuration) : base (new GcmXmppServiceConnectionFactory (configuration))
        {
        }
    }

    public class GcmXmppServiceConnection : IServiceConnection<GcmXmppNotification>
    {   
        readonly GcmXmppConnection connection;

        public GcmXmppServiceConnection (GcmXmppConfiguration configuration)
        {
            connection = new GcmXmppConnection (configuration);
        }

        public async Task Send (GcmXmppNotification notification)
        {
            var completableNotification = new GcmXmppConnection.CompletableNotification (notification);

            connection.Send (completableNotification);

            var ex = await completableNotification.WaitForComplete ().ConfigureAwait (false);

            if (ex != null)
                throw ex;
        }
    }
}
