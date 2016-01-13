using System;

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
        Unknown = 255
    }

    public class ApnsNotificationException : Exception
    {
        public ApnsNotificationException(byte errorStatusCode, ApnsNotification notification)
            : this(ToErrorStatusCode(errorStatusCode), notification)
        { }

        public ApnsNotificationException (ApnsNotificationErrorStatusCode errorStatusCode, ApnsNotification notification)
            : base ($"Apns notification error: '{errorStatusCode}'")
        {
            Notification = notification;
            ErrorStatusCode = errorStatusCode;
        }

        public ApnsNotification Notification { get; set; }

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


    public class ApnsHttp2NotificationException : Exception
    {
        public ApnsHttp2NotificationException (ApnsHttp2FailureReason reason, ApnsHttp2Notification notification)
            : base (reason.ToString ())
        {
            Notification = notification;
            Reason = reason;
        }


        public ApnsHttp2Notification Notification { get; set; }

        public ApnsHttp2FailureReason Reason { get; set; }
    }

    public enum ApnsHttp2FailureReason
    {
        /// <summary>
        /// The message payload was empty.
        /// </summary>
        PayloadEmpty,

        /// <summary>
        /// The message payload was too large. The maximum payload size is 4096 bytes.
        /// </summary>
        PayloadTooLarge,

        /// <summary>
        /// The apns-topic was invalid.
        /// </summary>
        BadTopic,

        /// <summary>
        /// Pushing to this topic is not allowed.
        /// </summary>
        TopicDisallowed,

        /// <summary>
        /// The apns-id value is bad.
        /// </summary>
        BadMessageId,

        /// <summary>
        /// The apns-expiration value is bad.
        /// </summary>
        BadExpirationDate,

        /// <summary>
        /// The apns-priority value is bad.
        /// </summary>
        BadPriority,

        /// <summary>
        /// The device token is not specified in the request :path. Verify that the :path header contains the device token.
        /// </summary>
        MissingDeviceToken,

        /// <summary>
        /// The specified device token was bad. Verify that the request contains a valid token and that the token matches the environment.
        /// </summary>
        BadDeviceToken,

        /// <summary>
        /// The device token does not match the specified topic.
        /// </summary>
        DeviceTokenNotForTopic,

        /// <summary>
        /// The device token is inactive for the specified topic.
        /// </summary>
        Unregistered,

        /// <summary>
        /// One or more headers were repeated.
        /// </summary>
        DuplicateHeaders,

        /// <summary>
        /// The client certificate was for the wrong environment.
        /// </summary>
        BadCertificateEnvironment,

        /// <summary>
        /// The certificate was bad.
        /// </summary>
        BadCertificate,

        /// <summary>
        /// The specified action is not allowed.
        /// </summary>
        Forbidden,

        /// <summary>
        /// The request contained a bad :path value.
        /// </summary>
        BadPath,

        /// <summary>
        /// The specified :method was not POST.
        /// </summary>
        MethodNotAllowed,

        /// <summary>
        /// Too many requests were made consecutively to the same device token.
        /// </summary>
        TooManyRequests,

        /// <summary>
        /// Idle time out.
        /// </summary>
        IdleTimeout,

        /// <summary>
        /// The server is shutting down.
        /// </summary>
        Shutdown,

        /// <summary>
        /// An internal server error occurred.
        /// </summary>
        InternalServerError,

        /// <summary>
        /// The service is unavailable.
        /// </summary>
        ServiceUnavailable,

        /// <summary>
        /// The apns-topic header of the request was not specified and was required. The apns-topic header is mandatory when the client is connected using a certificate that supports multiple topics.
        /// </summary>
        MissingTopic,
    }
}

