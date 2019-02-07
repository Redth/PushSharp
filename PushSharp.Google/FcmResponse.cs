using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PushSharp.Google
{
    public class FcmResponse
    {
        public FcmResponse()
        {
            MulticastId = -1;
            NumberOfSuccesses = 0;
            NumberOfFailures = 0;
            NumberOfCanonicalIds = 0;
            OriginalNotification = null;
            Results = new List<FcmMessageResult>();
            ResponseCode = FcmResponseCode.Ok;
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
        public FcmNotification OriginalNotification { get; set; }

        [JsonProperty("results")]
        public List<FcmMessageResult> Results { get; set; }

        [JsonIgnore]
        public FcmResponseCode ResponseCode { get; set; }
    }

    public enum FcmResponseCode
    {
        Ok,
        Error,
        BadRequest,
        ServiceUnavailable,
        InvalidAuthToken,
        InternalServiceError
    }

    public enum FcmResponseStatus
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

