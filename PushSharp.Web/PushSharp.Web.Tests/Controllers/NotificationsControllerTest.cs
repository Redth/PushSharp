using System;
using Moq;
using NUnit.Framework;
using PushSharp.Apple;
using PushSharp.Web.Business.Logging;
using PushSharp.Web.Business.Services;
using PushSharp.Web.Business.Settings;
using PushSharp.Web.Controllers;
using PushSharp.Web.Domain.Concrete;

namespace PushSharp.Web.Tests.Controllers
{
    [TestFixture]
    public class NotificationsControllerTest
    {
        [Test]
        public void AppleNotificationsTest()
        {
            var payLoad = new AppleApiNotificationPayLoad("token", "hello World", 0, "default", "wrong key", "deviceUuid");
            var testConfigurationManager = new TestConfigurationManager();
            var securityService = new SecurityService(testConfigurationManager);
            var settings = new AppleServiceSettings(testConfigurationManager);

            var broker = new Mock<IPushBroker>();
            broker.Setup(b => b.Dispose()).Verifiable();
            broker.Setup(b => b.RegisterService<AppleNotification>(It.IsAny<ApplePushService>(), It.IsAny<bool>()));
            broker.Setup(b => b.QueueNotification(It.IsAny<AppleNotification>()));
            var appleService = new AppleNotificationService(broker.Object, new ApiLogger(), settings);
            var controller = new NotificationsController(appleService, securityService);
            var exception = new Exception("Test Exception");

            // senario 1: invalid Key
            var result = controller.Apple(payLoad);
            Assert.IsFalse(result.IsSuccessful, "Should fail, wrong api key");
            Assert.AreEqual("Invalid Authentication Key", result.Message);
            AssertPushNotificationReturnedInfo(payLoad, result);

            // senario 2: send successful 
            payLoad.AuthenticationKey = "LeopardValidApiKey";
            broker.Setup(b => b.StopAllServices(It.IsAny<bool>())).Raises(b => b.OnNotificationSent += null, this, null);
            result = controller.Apple(payLoad);
            Assert.IsTrue(result.IsSuccessful);
            Assert.AreEqual("Notification Sent", result.Message);
            AssertPushNotificationReturnedInfo(payLoad, result);

            // senario 3: send failed
            broker.Setup(b => b.StopAllServices(It.IsAny<bool>())).Raises(b => b.OnNotificationFailed += null, this, null, exception);
            result = controller.Apple(payLoad);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("Test Exception", result.Message, "should get the exception message in the result");
            AssertPushNotificationReturnedInfo(payLoad, result);

            // senario 4: device subuscription changed
            broker.Setup(b => b.StopAllServices(It.IsAny<bool>())).Raises(b => b.OnDeviceSubscriptionChanged += null, this, string.Empty, string.Empty, null);
            result = controller.Apple(payLoad);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("Device Subscription Changed", result.Message);
            AssertPushNotificationReturnedInfo(payLoad, result);

            // senario 5: device subscription expired
            broker.Setup(b => b.StopAllServices(It.IsAny<bool>())).Raises(b => b.OnDeviceSubscriptionExpired += null, this, string.Empty, DateTime.Now, null);
            result = controller.Apple(payLoad);
            Assert.IsFalse(result.IsSuccessful);
            Assert.IsTrue(result.Message.Contains("Device Subscription Expired: "));
            AssertPushNotificationReturnedInfo(payLoad, result);

            // senario 6: Service Exception
            broker.Setup(b => b.StopAllServices(It.IsAny<bool>())).Raises(b => b.OnServiceException += null, this, exception );
            result = controller.Apple(payLoad);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("Service Exception: Test Exception", result.Message);
            AssertPushNotificationReturnedInfo(payLoad, result);

            // senario 7: channel Exception
            broker.Setup(b => b.StopAllServices(It.IsAny<bool>())).Raises(b => b.OnChannelException += null, this, null, exception);
            result = controller.Apple(payLoad);
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("Channel Exception: Test Exception", result.Message);
            AssertPushNotificationReturnedInfo(payLoad, result);
        }

        [Test]
        public void GoogleGcmTest()
        {
            var controller = new NotificationsController(null, null);
            var result = controller.GoogleGcm();
            
            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("Method is not implemented yet", result.Message);
        }

        [Test]
        public void WindowsPhoneTest()
        {
            var controller = new NotificationsController(null, null);
            var result = controller.WindowsPhone();

            Assert.IsFalse(result.IsSuccessful);
            Assert.AreEqual("Method is not implemented yet", result.Message);
        }

        #region Implementation

        private void AssertPushNotificationReturnedInfo(AppleApiNotificationPayLoad payLoad, ApiCallResult result)
        {
            var pushResult = result as PushNotificationApiCallResult;
            Assert.AreEqual(payLoad.DeviceUuid, pushResult.DeviceUuid);
            Assert.AreEqual(payLoad.Message, pushResult.NotificationMessage);
        }

        #endregion

    }
}
