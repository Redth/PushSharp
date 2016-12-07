using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace PushSharp.Firebase
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

		public string GetSingleRegistrationID(int resultIndex)
		{
			if (OriginalNotification.RegistrationIds != null && OriginalNotification.RegistrationIds.Count >= (resultIndex + 1))
				return OriginalNotification.RegistrationIds[resultIndex];

			return string.Empty;
		}
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
		[EnumMember(Value = "Ok")]
		Ok,

		[EnumMember(Value = "Error")]
		Error,

		//[EnumMember(Value = "QuotaExceeded")]
		//QuotaExceeded,

		//[EnumMember(Value = "DeviceQuotaExceeded")]
		//DeviceQuotaExceeded,

		//[EnumMember(Value = "MissingCollapseKey")]
		//MissingCollapseKey,

		[EnumMember(Value = "CanonicalRegistrationId")]
		CanonicalRegistrationId,

		/// <summary>
		/// Check the format of the registration token you pass to the server.
		/// Make sure it matches the registration token the client app receives from registering with Firebase Notifications.
		/// Do not truncate or add additional characters.
		/// </summary>
		[EnumMember(Value = "InvalidRegistration")]
		InvalidRegistration,

		/// <summary>
		/// An existing registration token may cease to be valid in a number of scenarios, including:
		///	<para>	- If the client app unregisters with FCM.</para>
		///	<para>	- If the client app is automatically unregistered, which can happen if the user uninstalls the application. For example, on iOS, if the APNS Feedback Service reported the APNS token as invalid.</para>
		///	<para>	- If the registration token expires (for example, Google might decide to refresh registration tokens, or the APNS token has expired for iOS devices).</para>
		///	<para>	- If the client app is updated but the new version is not configured to receive messages.</para>
		/// <para></para>
		/// <para>For all these cases, remove this registration token from the app server and stop using it to send messages.</para>
		/// </summary>
		[EnumMember(Value = "NotRegistered")]
		NotRegistered,

		/// <summary>
		/// Check that the total size of the payload data included in a message does not exceed FCM limits: 4096 bytes for most messages, 
		/// or 2048 bytes in the case of messages to topics or notification messages on iOS. This includes both the keys and the values.
		/// </summary>
		[EnumMember(Value = "MessageTooBig")]
		MessageTooBig,

		/// <summary>
		/// Check that the request contains a registration token (in the registration_id in a plain text message, 
		/// or in the to or registration_ids field in JSON).
		/// </summary>
		[EnumMember(Value = "MissingRegistration")]
		MissingRegistrationId,

		/// <summary>
		/// The server couldn't process the request in time. Retry the same request, but you must:
		///	<para>	-Honor the Retry-After header if it is included in the response from the FCM Connection Server.</para>
		///	<para>	-Implement exponential back-off in your retry mechanism. (e.g. if you waited one second before the first retry, 
		///	wait at least two second before the next one, then 4 seconds and so on). If you're sending multiple messages, 
		///	delay each one independently by an additional random amount to avoid issuing a new request for all messages at the same time.</para>
		///	<para></para>
		///	<para>Senders that cause problems risk being blacklisted.</para>
		/// </summary>
		[EnumMember(Value = "Unavailable")]
		Unavailable,

		/// <summary>
		/// A registration token is tied to a certain group of senders. When a client app registers for FCM, 
		/// it must specify which senders are allowed to send messages. You should use one of those sender IDs when sending messages to the client app.
		/// If you switch to a different sender, the existing registration tokens won't work.
		/// </summary>
		[EnumMember(Value = "MismatchSenderId")]
		MismatchSenderId,

		/// <summary>
		/// Check that the payload data does not contain a key (such as from, or gcm, or any value prefixed by google) that is used internally by FCM.
		/// Note that some words (such as collapse_key) are also used by FCM but are allowed in the payload, 
		/// in which case the payload value will be overridden by the FCM value.
		/// </summary>
		[EnumMember(Value = "InvalidDataKey")]
		InvalidDataKey,

		/// <summary>
		/// Check that the value used in time_to_live is an integer representing a duration in seconds between 0 and 2,419,200 (4 weeks).
		/// </summary>
		[EnumMember(Value = "InvalidTtl")]
		InvalidTtl,

		/// <summary>
		/// The server encountered an error while trying to process the request. You could retry the same request following the requirements listed in "Timeout" (see row above). 
		/// If the error persists, please report the problem in the android-gcm group.
		/// </summary>
		[EnumMember(Value = "InternalServerError")]
		InternalServerError,

		/// <summary>
		/// Make sure the message was addressed to a registration token whose package name matches the value passed in the request.
		/// </summary>
		[EnumMember(Value = "InvalidPackageName")]
		InvalidPackageName,

		/// <summary>
		/// The rate of messages to a particular device is too high. 
		/// Reduce the number of messages sent to this device and do not immediately retry sending to this device.
		/// </summary>
		[EnumMember(Value = "DeviceMessageRateExceeded")]
		DeviceMessageRateExceeded,

		/// <summary>
		/// The rate of messages to subscribers to a particular topic is too high. 
		/// Reduce the number of messages sent for this topic, and do not immediately retry sending.
		/// </summary>
		[EnumMember(Value = "TopicsMessageRateExceeded")]
		TopicsMessageRateExceeded
	}
}