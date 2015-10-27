using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PushSharp.Google
{
    public class GcmResponse
    {
        public GcmResponse()
        {
            MulticastId = -1;
            NumberOfSuccesses = 0;
            NumberOfFailures = 0;
            NumberOfCanonicalIds = 0;
            OriginalNotification = null;
            Results = new List<GcmMessageResult>();
            ResponseCode = GcmResponseCode.Ok;
        }

        [JsonProperty("multicast_id")]
        public long MulticastId { get; set; }

        [JsonProperty("success")]
        public long NumberOfSuccesses { get; set; }

        [JsonProperty("failure")]
        public long NumberOfFailures { get; set; }

        [JsonProperty("canonical_ids")]
        public long NumberOfCanonicalIds { get; set; }

        [JsonIgnore]
        public GcmNotification OriginalNotification { get; set; }

        [JsonProperty("results")]
        public List<GcmMessageResult> Results { get; set; }

        [JsonIgnore]
        public GcmResponseCode ResponseCode { get; set; }
    }

    public enum GcmResponseCode
    {
        Ok,
        Error,
        BadRequest,
        ServiceUnavailable,
        InvalidAuthToken,
        InternalServiceError
    }

    public enum GcmResponseStatus
    {
        [EnumMember (Value="Ok")]
        Ok,

        [EnumMember (Value="Error")]
        Error,

        [EnumMember (Value="QuotaExceeded")]
        QuotaExceeded,

        [EnumMember (Value="DeviceQuotaExceeded")]
        DeviceQuotaExceeded,

        [EnumMember (Value="InvalidRegistration")]
        InvalidRegistration,

        [EnumMember (Value="NotRegistered")]
        NotRegistered,

        [EnumMember (Value="MessageTooBig")]
        MessageTooBig,

        [EnumMember (Value="MissingCollapseKey")]
        MissingCollapseKey,

        [EnumMember (Value="MissingRegistration")]
        MissingRegistrationId,

        [EnumMember (Value="Unavailable")]
        Unavailable,

        [EnumMember (Value="MismatchSenderId")]
        MismatchSenderId,

        [EnumMember (Value="CanonicalRegistrationId")]
        CanonicalRegistrationId,

        [EnumMember (Value="InvalidDataKey")]
        InvalidDataKey,

        [EnumMember (Value="InvalidTtl")]
        InvalidTtl,

        [EnumMember (Value="InternalServerError")]
        InternalServerError,

        [EnumMember (Value="InvalidPackageName")]
        InvalidPackageName
    }
}

