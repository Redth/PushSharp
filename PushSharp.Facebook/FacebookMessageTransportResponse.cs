using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace PushSharp.Facebook
{
	public class FacebookMessageTransportResponse
	{
		public FacebookMessageTransportResponse()
		{
			this.MulticastId = -1;
			this.NumberOfSuccesses = 0;
			this.NumberOfFailures = 0;
			this.NumberOfCanonicalIds = 0;
			this.Message = null;
			this.Results = new List<FacebookMessageResult>();
			this.ResponseCode = FacebookMessageTransportResponseCode.Ok;
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
		public FacebookNotification Message { get; set; }

		[JsonProperty("results")]
		public List<FacebookMessageResult> Results { get; set; }

		[JsonIgnore]
		public FacebookMessageTransportResponseCode ResponseCode { get; set; }
	}

    public enum FacebookMessageTransportResponseCode
    {
        Ok,
        Error,
        BadRequest,
        ServiceUnavailable,
        InvalidAuthToken,
        InternalServiceError
    }

    public enum FacebookMessageTransportResponseStatus
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
