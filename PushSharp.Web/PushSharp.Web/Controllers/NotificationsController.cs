using PushSharp.Web.Business.Services;
using PushSharp.Web.Domain.Concrete;

namespace PushSharp.Web.Controllers
{
    public class NotificationsController : PushSharpController
    {
        private readonly IAppleNotificationService _AppleService;
        private readonly ISecurityService _SecurityService;
        private readonly IGoogleGcmNotificationService _GoogleGcmService;
        private readonly IWindowsPhoneNotificationService _WindowsPhoneService;

        public NotificationsController(IAppleNotificationService appleService, ISecurityService securityService, IGoogleGcmNotificationService googleGcmService, IWindowsPhoneNotificationService windowsPhoneService)
        {
            _AppleService = appleService;
            _SecurityService = securityService;
            _GoogleGcmService = googleGcmService;
            _WindowsPhoneService = windowsPhoneService;
        }

        [System.Web.Http.HttpGet]
        public ApiCallResult Apple(string token, string message, int badge, string soundFile, string key, string uuid)
        {
            var payLoad = new AppleApiNotificationPayLoad(token, message, badge, soundFile, key, uuid);
            return ProcessAppleNotification(payLoad);
        }

        [System.Web.Http.HttpPost]
        public ApiCallResult Apple(AppleApiNotificationPayLoad payLoad)
        {
            return ProcessAppleNotification(payLoad);
        }

        [System.Web.Http.HttpGet]
        public ApiCallResult GoogleGcm(GoogleGcmApiNotificationPayLoad payLoad)
        {
            return ProcessGoogleGcmNotification(payLoad);
        }

        [System.Web.Http.HttpGet]
        public ApiCallResult WindowsPhone(WindowsPhoneApiNotificationPayLoad payLoad)
        {
            return ProcessWindowsPhoneNotification(payLoad);
        }

        #region Implementation

        private ApiCallResult ProcessAppleNotification(AppleApiNotificationPayLoad payLoad)
        {
            var result = new PushNotificationApiCallResult(false, "Notificaiton failed.", payLoad.DeviceUuid, payLoad.Message);
            var isRequestValid = _SecurityService.ValidateRequestKey(payLoad.AuthenticationKey);
            if (!isRequestValid)
            {
                result.Message = "Invalid Authentication Key";
            }
            else if (_AppleService.Send(payLoad))
            {
                result.IsSuccessful = _AppleService.Result.IsSuccessful;
                result.Message = _AppleService.Result.Message;
            }

            return result;
        }

        private ApiCallResult ProcessGoogleGcmNotification(GoogleGcmApiNotificationPayLoad payLoad)
        {
            var result = new PushNotificationApiCallResult(false, "Notificaiton failed.", payLoad.DeviceUuid, payLoad.Message);
            var isRequestValid = _SecurityService.ValidateRequestKey(payLoad.AuthenticationKey);
            if (!isRequestValid)
            {
                result.Message = "Invalid Authentication Key";
            }
            else if (_GoogleGcmService.Send(payLoad))
            {
                result.IsSuccessful = _GoogleGcmService.Result.IsSuccessful;
                result.Message = _GoogleGcmService.Result.Message;
            }

            return result;
        }

        private ApiCallResult ProcessWindowsPhoneNotification(WindowsPhoneApiNotificationPayLoad payLoad)
        {
            var result = new PushNotificationApiCallResult(false, "Notificaiton failed.", payLoad.DeviceUuid, payLoad.Message);
            var isRequestValid = _SecurityService.ValidateRequestKey(payLoad.AuthenticationKey);
            if (!isRequestValid)
            {
                result.Message = "Invalid Authentication Key";
            }
            else if (_WindowsPhoneService.Send(payLoad))
            {
                result.IsSuccessful = _WindowsPhoneService.Result.IsSuccessful;
                result.Message = _WindowsPhoneService.Result.Message;
            }

            return result;
        }

        #endregion

    }
}
