using System;

namespace PushSharp.Blackberry
{
    public class BlackberryMessageStatus
    {
        public BlackberryNotificationStatus NotificationStatus { get; set; }

        public BlackberryNotification Notification { get; set; }

        public System.Net.HttpStatusCode HttpStatus { get; set; }
    }

    public enum BlackberryNotificationStatus
    {
        NotAvailable=0,
        /// <summary>
        /// The request was completed successfully
        /// </summary>
        RequestCompleted = 1000,
        /// <summary>
        /// The request was accepted for processing
        /// </summary>
        RequestAcceptedForProcessing = 1001,
        /// <summary>
        /// The request was accepted for processing, but the daily push count quota was exceeded  
        /// for the push application and future pushes to the application might start being rejected. 
        /// Future pushes should be delayed until the next day when more quota is available
        /// </summary>
        RequestAcceptedButPushQuotaExceeded = 1500,
        /// <summary>
        /// The request is invalid
        /// </summary>
        InvalidRequest = 2000,
        /// <summary>
        /// The requested action is forbidden
        /// </summary>
        ForbiddenRequestAction = 2001,
        /// <summary>
        /// The specified PIN or token is not recognized
        /// </summary>
        PinOrTokenNotRecognized = 2002,
        /// <summary>
        /// Could not find the specified Push ID
        /// </summary>
        PushIdNotFound = 2004,
        /// <summary>
        /// The supplied Push ID is not unique
        /// </summary>
        PushIdNotUnique = 2007,
        /// <summary>
        /// The Push ID is valid, but the push request could not be cancelled
        /// </summary>
        PushCantBeCancelled = 2008,
        /// <summary>
        /// The Push ID is valid, but the corresponding PINs or tokens are still being processed. 
        /// Status query is not possible at this time and should be tried again later
        /// </summary>
        StatusCodeNotPossible = 2009,
        /// <summary>
        /// The request was rejected because the daily push count quota was exceeded for the push application. 
        /// Future pushes should be delayed until the next day when more quota is available
        /// </summary>
        PushQuotaExceeded = 2500,
        /// <summary>
        /// The PPG could not complete the request due to an internal error
        /// </summary>
        InternalError = 3000,
        /// <summary>
        /// The server does not support the operation that was requested
        /// </summary>
        OperationNotSupported = 3001,
        /// <summary>
        /// The server does not support the PAP version specified in the request
        /// </summary>
        ProvidedPapVersionNotSupported = 3002,
        /// <summary>
        /// The PPG could not deliver the message using the specified method
        /// </summary>
        DeliveryFailed = 3007,
        /// <summary>
        /// The service failed
        /// </summary>
        ServiceFailed = 4000,
        /// <summary>
        /// The server is busy
        /// </summary>
        ServerBusy = 4001,
        /// <summary>
        /// The request expired
        /// </summary>
        RequestExpired = 4500,
        /// <summary>
        /// The request failed
        /// </summary>
        RequestFailed = 4501,
        /// <summary>
        /// The request failed because no application on the device is listening to receive the push 
        /// (either the application is closed and cannot be launched or it was removed from the device)"
        /// </summary>
        NoAppReceivePush = 4502,
        /// <summary>
        /// The device is unable to receive the push due to the push service being blocked
        /// </summary>
        PushServiceBlocked = 4503,
        /// <summary>
        /// Specific request was completed successfully
        /// </summary>
        SpecifiedRequestCompleted = 21000,
        /// <summary>
        /// Specific request is badly formed
        /// </summary>
        SpecifiedRequestMalformed = 22000,
        /// <summary>
        /// Could not find the specified application ID for the specific request
        /// </summary>
        SpecifiedRequestAppIdNotFound = 22001,
        /// <summary>
        /// The specified PIN or token in the specific request is invalid
        /// </summary>
        SpecifiedRequestInvalidPinOrToken = 22002,
        /// <summary>
        /// The specific request provides an incorrect status
        /// </summary>
        SpecifiedRequestIncorrectStatus = 22003,
        /// <summary>
        /// The specific request produces no results
        /// </summary>
        SpecifiedRequestNoResults = 22004,
        /// <summary>
        /// The specific request exceeds the number of calls allowed 
        /// within the specified time period
        /// </summary>
        SpecifiedRequestNumCallsExceeded = 22005,
        /// <summary>
        /// Internal error has prevented the request from being completed
        /// </summary>
        SpecifiedRequestInternalError = 23000
    }
}

