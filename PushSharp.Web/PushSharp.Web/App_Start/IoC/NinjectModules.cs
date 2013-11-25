using Ninject.Modules;
using PushSharp.Web.Business.Logging;
using PushSharp.Web.Business.Services;
using PushSharp.Web.Business.Settings;
using PushSharp.Web.Interfaces.Settings;

namespace PushSharp.Web.App_Start.IoC
{

    // List and Describe Necessary HttpModules
    // This class is optional if you already Have NinjectMvc
    public class NinjectModules
    {
        public static NinjectModule[] Modules
        {
            get
            {
                return new[] { new MainModule() };
            }
        }

        public class MainModule : NinjectModule
        {
            public override void Load()
            {
                Bind<IPushBroker>().To<PushBroker>();
                Bind<IAppleNotificationService>().To<AppleNotificationService>();
                Bind<IAppleServiceSettings>().To<AppleServiceSettings>();
                Bind<IGoogleGcmNotificationService>().To<GoogleGcmNotificationService>();
                Bind<IGoogleServiceSettings>().To<GoogleServiceSettings>();
                Bind<IWindowsPhoneNotificationService>().To<WindowsPhoneNotificationService>();
                Bind<IWindowsPhoneServiceSettings>().To<WindowsPhoneServiceSettings>();
                Bind<ISecurityService>().To<SecurityService>();
                Bind<ILogger>().To<ApiLogger>();
                Bind<IConfigurationManager>().To<WebApiConfigurationManager>();
            }
        }
    }
}