using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace PushSharp.Google
{
    public class FcmMessageResult
    {
        [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageId { get; set; }

        [JsonProperty("registration_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CanonicalRegistrationId { get; set; }

        [JsonIgnore]
        public FcmResponseStatus ResponseStatus { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error
        {
            get 
            {
                switch (ResponseStatus)
                {
                case FcmResponseStatus.Ok:
                    return null;
                case FcmResponseStatus.Unavailable:
                    return "Unavailable";
                case FcmResponseStatus.QuotaExceeded:
                    return "QuotaExceeded";
                case FcmResponseStatus.NotRegistered:
                    return "NotRegistered";
                case FcmResponseStatus.MissingRegistrationId:
                    return "MissingRegistration";
                case FcmResponseStatus.MissingCollapseKey:
                    return "MissingCollapseKey";
                case FcmResponseStatus.MismatchSenderId:
                    return "MismatchSenderId";
                case FcmResponseStatus.MessageTooBig:
                    return "MessageTooBig";
                case FcmResponseStatus.InvalidTtl:
                    return "InvalidTtl";
                case FcmResponseStatus.InvalidRegistration:
                    return "InvalidRegistration";
                case FcmResponseStatus.InvalidDataKey:
                    return "InvalidDataKey";
                case FcmResponseStatus.InternalServerError:
                    return "InternalServerError";
                case FcmResponseStatus.DeviceQuotaExceeded:
                    return null;
                case FcmResponseStatus.CanonicalRegistrationId:
                    return null;
                case FcmResponseStatus.Error:
                    return "Error";
                default:
                    return null;
                }
            }
        }
    }
}
