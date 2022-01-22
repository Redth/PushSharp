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
    public class ApnsHttp2NotificationException : NotificationException
    {
        public ApnsHttp2NotificationException(byte errorStatusCode, IApnsHttp2Notification notification)
            : this(ToErrorStatusCode(errorStatusCode), notification)
        { }

        public ApnsHttp2NotificationException(ApnsNotificationErrorStatusCode errorStatusCode, IApnsHttp2Notification notification)
            : base("Apns notification error: '" + errorStatusCode + "'", notification)
        {
            Notification = notification;
            ErrorStatusCode = errorStatusCode;
        }

        public ApnsHttp2NotificationException(ApnsNotificationErrorStatusCode errorStatusCode, IApnsHttp2Notification notification, Exception innerException)
            : base("Apns notification error: '" + errorStatusCode + "'", notification, innerException)
        {
            Notification = notification;
            ErrorStatusCode = errorStatusCode;
        }

        public new IApnsHttp2Notification Notification { get; set; }
        public ApnsNotificationErrorStatusCode ErrorStatusCode { get; private set; }

        internal static ApnsNotificationErrorStatusCode ToErrorStatusCode(byte errorStatusCode)
        {
            ApnsNotificationErrorStatusCode s;
            Enum.TryParse(errorStatusCode.ToString(), out s);
            return s;
        }
    }
    public class ApnsNotificationException : NotificationException
    {
        public ApnsNotificationException(byte errorStatusCode, IApnsNotification notification)
            : this(ToErrorStatusCode(errorStatusCode), notification)
        { }

        public ApnsNotificationException (ApnsNotificationErrorStatusCode errorStatusCode, IApnsNotification notification)
            : base ("Apns notification error: '" + errorStatusCode + "'", notification)
        {
            Notification = notification;
            ErrorStatusCode = errorStatusCode;
        }

        public ApnsNotificationException (ApnsNotificationErrorStatusCode errorStatusCode, IApnsNotification notification, Exception innerException)
            : base ("Apns notification error: '" + errorStatusCode + "'", notification, innerException)
        {
            Notification = notification;
            ErrorStatusCode = errorStatusCode;
        }

        public new IApnsNotification Notification { get; set; }
        public ApnsNotificationErrorStatusCode ErrorStatusCode { get; private set; }
        
        internal static ApnsNotificationErrorStatusCode ToErrorStatusCode(byte errorStatusCode)
        {
            ApnsNotificationErrorStatusCode s;
            Enum.TryParse(errorStatusCode.ToString(), out s);
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
