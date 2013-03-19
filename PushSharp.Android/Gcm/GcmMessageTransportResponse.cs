using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PushSharp.Android
{
	public class GcmMessageTransportResponse
	{
		public GcmMessageTransportResponse()
		{
			this.MulticastId = -1;
			this.NumberOfSuccesses = 0;
			this.NumberOfFailures = 0;
			this.NumberOfCanonicalIds = 0;
			this.Message = null;
			this.Results = new List<GcmMessageResult>();
			this.ResponseCode = GcmMessageTransportResponseCode.Ok;
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
		public GcmNotification Message { get; set; }

		[JsonProperty("results")]
		public List<GcmMessageResult> Results { get; set; }

		[JsonIgnore]
		public GcmMessageTransportResponseCode ResponseCode { get; set; }
	}

    public enum GcmMessageTransportResponseCode
    {
        Ok,
        Error,
        BadRequest,
        ServiceUnavailable,
        InvalidAuthToken,
        InternalServiceError
    }

    public enum GcmMessageTransportResponseStatus
    {
        Ok,
        Error,
        QuotaExceeded,
        DeviceQuotaExceeded,
        InvalidRegistration,
        NotRegistered,
        MessageTooBig,
        MissingCollapseKey,
        MissingRegistrationId,
        Unavailable,
        MismatchSenderId,
        CanonicalRegistrationId,
        InvalidDataKey,
        InvalidTtl,
        InternalServerError
    }
}
