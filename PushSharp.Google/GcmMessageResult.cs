using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace PushSharp.Google
{
    public class GcmMessageResult
    {
        [JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
        public string MessageId { get; set; }

        [JsonProperty("registration_id", NullValueHandling = NullValueHandling.Ignore)]
        public string CanonicalRegistrationId { get; set; }

        [JsonIgnore]
        public GcmResponseStatus ResponseStatus { get; set; }

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string Error
        {
            get 
            {
                switch (ResponseStatus)
                {
                case GcmResponseStatus.Ok:
                    return null;
                case GcmResponseStatus.Unavailable:
                    return "Unavailable";
                case GcmResponseStatus.QuotaExceeded:
                    return "QuotaExceeded";
                case GcmResponseStatus.NotRegistered:
                    return "NotRegistered";
                case GcmResponseStatus.MissingRegistrationId:
                    return "MissingRegistration";
                case GcmResponseStatus.MissingCollapseKey:
                    return "MissingCollapseKey";
                case GcmResponseStatus.MismatchSenderId:
                    return "MismatchSenderId";
                case GcmResponseStatus.MessageTooBig:
                    return "MessageTooBig";
                case GcmResponseStatus.InvalidTtl:
                    return "InvalidTtl";
                case GcmResponseStatus.InvalidRegistration:
                    return "InvalidRegistration";
                case GcmResponseStatus.InvalidDataKey:
                    return "InvalidDataKey";
                case GcmResponseStatus.InternalServerError:
                    return "InternalServerError";
                case GcmResponseStatus.DeviceQuotaExceeded:
                    return null;
                case GcmResponseStatus.CanonicalRegistrationId:
                    return null;
                case GcmResponseStatus.Error:
                    return "Error";
                default:
                    return null;
                }
            }
        }
    }
}
