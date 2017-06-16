using System;
using PushSharp.Core;

namespace PushSharp.Apple
{
    public enum ApnsNotificationErrorStatusCode
    {
        NoErrors = 0,
        ProcessingError = 1,
        MissingDeviceToken = 2,
        MissingTopic = 3,
        MissingPayload = 4,
        InvalidTokenSize = 5,
        InvalidTopicSize = 6,
        InvalidPayloadSize = 7,
        InvalidToken = 8,
        Shutdown = 10,
        ConnectionError = 254,
        Unknown = 255
    }

    public class ApnsNotificationException : NotificationException
    {
        public ApnsNotificationException(byte errorStatusCode, ApnsNotification notification)
            : this(ToErrorStatusCode(errorStatusCode), notification)
        { }

        public ApnsNotificationException (ApnsNotificationErrorStatusCode errorStatusCode, ApnsNotification notification)
            : base ("Apns notification error: '" + errorStatusCode + "'", notification)
        {
            Notification = notification;
            ErrorStatusCode = errorStatusCode;
        }

        public ApnsNotificationException (ApnsNotificationErrorStatusCode errorStatusCode, ApnsNotification notification, Exception innerException)
            : base ("Apns notification error: '" + errorStatusCode + "'", notification, innerException)
        {
            Notification = notification;
            ErrorStatusCode = errorStatusCode;
        }

        public new ApnsNotification Notification { get; set; }
        public ApnsNotificationErrorStatusCode ErrorStatusCode { get; private set; }
        
        private static ApnsNotificationErrorStatusCode ToErrorStatusCode(byte errorStatusCode)
        {
            var s = ApnsNotificationErrorStatusCode.Unknown;
            Enum.TryParse<ApnsNotificationErrorStatusCode>(errorStatusCode.ToString(), out s);
            return s;
        }
    }

    public class ApnsConnectionException : Exception
    {
        public ApnsConnectionException (string message) : base (message)
        {
        }

        public ApnsConnectionException (string message, Exception innerException) : base (message, innerException)
        {
        }
    }
}
