using System;
using PushSharp.Core;
using PushSharp.Web.Business.Logging;
using PushSharp.Web.Domain.Concrete;
using PushSharp.Web.Interfaces.Settings;
using ILogger = PushSharp.Web.Business.Logging.ILogger;

namespace PushSharp.Web.Business.Services
{
    public class NotificationServiceBase
    {
        protected IPushBroker PushBroker;
        public ILogger Logger { get; set; }

        public OperationResult Result { get; set; }

        protected void HookEvents(IPushBroker pushBroker)
        {
            pushBroker.OnNotificationSent += NotificationSent;
            pushBroker.OnChannelException += ChannelException;
            pushBroker.OnServiceException += ServiceException;
            pushBroker.OnNotificationFailed += NotificationFailed;
            pushBroker.OnDeviceSubscriptionExpired += DeviceSubscriptionExpired;
            pushBroker.OnDeviceSubscriptionChanged += DeviceSubscriptionChanged;
            pushBroker.OnChannelCreated += ChannelCreated;
            pushBroker.OnChannelDestroyed += ChannelDestroyed;
        }

        protected void UnHookEvents(IPushBroker pushBroker)
        {
            pushBroker.OnNotificationSent -= NotificationSent;
            pushBroker.OnChannelException -= ChannelException;
            pushBroker.OnServiceException -= ServiceException;
            pushBroker.OnNotificationFailed -= NotificationFailed;
            pushBroker.OnDeviceSubscriptionExpired -= DeviceSubscriptionExpired;
            pushBroker.OnDeviceSubscriptionChanged -= DeviceSubscriptionChanged;
            pushBroker.OnChannelCreated -= ChannelCreated;
            pushBroker.OnChannelDestroyed -= ChannelDestroyed;
        }

        protected void SetupPushSharpLogging(IServiceSettings settings)
        {
            Log.Logger = new PushSharpLogger();
            Log.Level = settings.LogLevel;
        }

        protected void StopBroker()
        {
            PushBroker.StopAllServices();
            UnHookEvents(PushBroker);
            PushBroker.Dispose();
        }

        #region Implementation
        private void DeviceSubscriptionChanged(object sender, string oldSubscriptionId, string newSubscriptionId, INotification notification)
        {
            //Currently this event will only ever happen for Android GCM
            Logger.Info("Device Registration Changed:  Old-> " + oldSubscriptionId + "  New-> " + newSubscriptionId + " -> " + notification);
            Result = new ApiCallResult(false, "Device Subscription Changed");
        }

        private void NotificationSent(object sender, INotification notification)
        {
            Logger.Info("Sent: " + sender + " -> " + notification);
            Result = new OperationResult(true, "Notification Sent");
        }

        private void NotificationFailed(object sender, INotification notification, Exception notificationFailureException)
        {
            Logger.Error("Failure: " + sender + " -> " + notificationFailureException.Message + " -> " + notification, notificationFailureException);
            Result = new ApiCallResult(false, notificationFailureException.Message);
        }

        private void ChannelException(object sender, IPushChannel channel, Exception exception)
        {
            Logger.Error("Channel Exception: " + sender + " -> " + exception, exception);
            Result = new ApiCallResult(false, "Channel Exception: " + exception.Message);
        }

        private void ServiceException(object sender, Exception exception)
        {
            Logger.Error("Service Exception: " + sender + " -> " + exception, exception);
            Result = new ApiCallResult(false, "Service Exception: " + exception.Message);
        }

        private void DeviceSubscriptionExpired(object sender, string expiredDeviceSubscriptionId, DateTime timestamp, INotification notification)
        {
            string errorMessage = "Device Subscription Expired: " + sender + " -> " + expiredDeviceSubscriptionId;
            Logger.Error(errorMessage);
            Result = new ApiCallResult(false, errorMessage);
        }

        private void ChannelDestroyed(object sender)
        {
            Logger.Info("Channel Destroyed for: " + sender);
        }

        private void ChannelCreated(object sender, IPushChannel pushChannel)
        {
            Logger.Info("Channel Created for: " + sender);
        } 
        #endregion
    }
}