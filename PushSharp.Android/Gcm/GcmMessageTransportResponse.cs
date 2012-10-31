using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		public long MulticastId { get; set; }
		public long NumberOfSuccesses { get; set; }
		public long NumberOfFailures { get; set; }
		public long NumberOfCanonicalIds { get; set; }
		public GcmNotification Message { get; set; }
		public List<GcmMessageResult> Results { get; set; }
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
