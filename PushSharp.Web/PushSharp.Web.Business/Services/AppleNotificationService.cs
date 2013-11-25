using PushSharp.Apple;
using PushSharp.Web.Business.Logging;
using PushSharp.Web.Domain.Concrete;
using PushSharp.Web.Interfaces.Settings;

namespace PushSharp.Web.Business.Services
{
    public interface IAppleNotificationService
    {
        bool Send(AppleApiNotificationPayLoad payLoad);
        OperationResult Result { get; set; }
    }

    public class AppleNotificationService : NotificationServiceBase, IAppleNotificationService
    {
        private readonly IAppleServiceSettings _Settings;

        public AppleNotificationService(IPushBroker pushBroker, ILogger logger, IAppleServiceSettings settings)
        {
            PushBroker = pushBroker;
            _Settings = settings;
            Logger = logger;
            SetupPushSharpLogging(settings);
            Result = new OperationResult(false, string.Empty);
        }

        public bool Send(AppleApiNotificationPayLoad payLoad)
        {
            HookEvents(PushBroker);

            try
            {
                PushBroker.RegisterService<AppleNotification>(new ApplePushService(new ApplePushChannelSettings(_Settings.CertificateContents, _Settings.CertificatePassword)));

                var notification = new AppleNotification()
                    .ForDeviceToken(payLoad.Token)
                    .WithAlert(payLoad.Message)
                    .WithSound(payLoad.SoundFile)
                    .WithBadge(payLoad.BadgeNo);
                
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
