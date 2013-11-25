using PushSharp.Web.Business.Services;
using PushSharp.Web.Domain.Concrete;

namespace PushSharp.Web.Controllers
{
    public class NotificationsController : PushSharpController
    {
        private readonly IAppleNotificationService _AppleService;
        private readonly ISecurityService _SecurityService;

        public NotificationsController(IAppleNotificationService appleService, ISecurityService securityService)
        {
            _AppleService = appleService;
            _SecurityService = securityService;
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
        public ApiCallResult GoogleGcm()
        {
            return new ApiCallResult(false, "Method is not implemented yet");
        }

        [System.Web.Http.HttpGet]
        public ApiCallResult WindowsPhone()
        {
            return new ApiCallResult(false, "Method is not implemented yet");
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

        #endregion

    }
}
