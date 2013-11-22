using System;
using PushSharp.Web.Business.Logging;
using PushSharp.Web.Domain.Concrete;
using PushSharp.Web.Interfaces.Settings;
using PushSharp.WindowsPhone;

namespace PushSharp.Web.Business.Services
{
    public interface IWindowsPhoneNotificationService
    {
        bool Send(WindowsPhoneApiNotificationPayLoad payLoad);
        OperationResult Result { get; set; }
    }

    public class WindowsPhoneNotificationService : NotificationServiceBase, IWindowsPhoneNotificationService
    {
        public WindowsPhoneNotificationService(IPushBroker pushBroker, ILogger logger, IWindowsPhoneServiceSettings settings)
        {
            PushBroker = pushBroker;
            Logger = logger;
            SetupPushSharpLogging(settings);
            Result = new OperationResult(false, string.Empty);
        }

        public bool Send(WindowsPhoneApiNotificationPayLoad payLoad)
        {
            HookEvents(PushBroker);

            try
            {
                PushBroker.RegisterService<WindowsPhoneNotification>(new WindowsPhonePushService());

                var notification = new WindowsPhoneToastNotification()
                    .ForEndpointUri(new Uri(payLoad.Token))
                    .ForOSVersion(WindowsPhoneDeviceOSVersion.MangoSevenPointFive)
                    .WithBatchingInterval(BatchingInterval.Immediate)
                    .WithNavigatePath(payLoad.NavigationPath)
                    .WithText1(payLoad.Message)
                    .WithText2(payLoad.TextMessage2);
                
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