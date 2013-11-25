using PushSharp.Android;
using PushSharp.Web.Business.Logging;
using PushSharp.Web.Domain.Concrete;
using PushSharp.Web.Interfaces.Settings;

namespace PushSharp.Web.Business.Services
{
    public interface IGoogleGcmNotificationService
    {
        bool Send(GoogleGcmApiNotificationPayLoad payLoad);
        OperationResult Result { get; set; }
    }

    public class GoogleGcmNotificationService : NotificationServiceBase, IGoogleGcmNotificationService
    {
        private readonly IGoogleServiceSettings _Settings;

        public GoogleGcmNotificationService(IPushBroker pushBroker, ILogger logger, IGoogleServiceSettings settings)
        {
            PushBroker = pushBroker;
            _Settings = settings;
            Logger = logger;
            SetupPushSharpLogging(settings);
            Result = new OperationResult(false, string.Empty);
        }

        public bool Send(GoogleGcmApiNotificationPayLoad payLoad)
        {
            HookEvents(PushBroker);

            try
            {
                PushBroker.RegisterService<GcmNotification>(new GcmPushService(new GcmPushChannelSettings(_Settings.GoogleApiAccessKey)));

                var notification = new GcmNotification()
                                    .ForDeviceRegistrationId(payLoad.Token)
                                    .WithJson(payLoad.GetGoogleFormattedJson());
                
                PushBroker.QueueNotification(notification);
            }
            finally 
            {
                StopBroker(); 
            }

            return true;
        }
    }
}