using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace PushSharp.Google
{
    public class GcmXmppResponse
    {
        public GcmXmppResponse ()
        {
        }

        [JsonProperty ("message_type", ItemConverterType = typeof (StringEnumConverter))]
        public ResponseMessageType MessageType { get;set; }

        [JsonProperty ("message_id")]
        public string MessageId { get;set; }

        [JsonProperty ("from")]
        public string From { get;set; }

        [JsonProperty ("error", ItemConverterType = typeof (StringEnumConverter))] 
        public ResponseErrorType Error { get; set; }

        [JsonProperty ("error_description")]
        public string ErrorDescription { get; set; }

    }

    public enum ResponseMessageType 
    {
        [EnumMember (Value="ack")]
        Ack,

        [EnumMember (Value="nack")]
        Nack,

        [EnumMember (Value="control")]
        Control
    }

    public enum ResponseErrorType
    {
        /// <summary>
        /// The ACK message is improperly formed.
        /// </summary>
        [EnumMember (Value="BAD_ACK")]
        BadAck,

        /// <summary>
        /// The device has a registration ID, but it's invalid or expired.
        /// </summary>
        [EnumMember (Value="BAD_REGISTRATION")]
        BadRegistration,

        /// <summary>
        /// The message couldn't be processed because the connection is draining. The message should be immediately retried over another connection.
        /// </summary>
        [EnumMember (Value="CONNECTION_DRAINING")]
        ConnectionDraining,

        /// <summary>
        /// The device is not registered.
        /// </summary>
        [EnumMember (Value="DEVICE_UNREGISTERED")]
        DeviceUnregistered,

        /// <summary>
        /// The server encountered an error while trying to process the request.
        /// </summary>
        [EnumMember (Value="INTERNAL_SERVER_ERROR")]
        InternalServerError,

        /// <summary>
        /// The JSON message payload is not valid.
        /// </summary>
        [EnumMember (Value="INVALID_JSON")]
        InvalidJson,

        /// <summary>
        /// The rate of messages to a particular registration ID (in other words, to a sender/device pair) is too high. If you want to retry the message, try using a slower rate.
        /// </summary>
        [EnumMember (Value="QUOTA_EXCEEDED")]
        QuotaExceeded,

        /// <summary>
        /// CCS is not currently able to process the message. The message should be r
        /// </summary>
        [EnumMember (Value="SERVICE_UNAVAILABLE")]
        ServiceUnavailable
    }
}
