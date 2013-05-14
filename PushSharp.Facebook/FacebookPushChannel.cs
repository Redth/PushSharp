using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.Net.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Core;
using Facebook;
using System.Threading.Tasks;

namespace PushSharp.Facebook
{
	public class FacebookPushChannel : IPushChannel
	{
		FacebookPushChannelSettings FacebookSettings = null;
		long waitCounter = 0;

		public FacebookPushChannel(FacebookPushChannelSettings channelSettings)
		{
			FacebookSettings = channelSettings as FacebookPushChannelSettings;
		}


		static FacebookPushChannel()
		{
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, policyErrs) => { return true; };
		}


		private string GetAppAccessToken()
		{
			//bool v_Method = true;
			string v_AccessToken = string.Empty;
			string facebookAppId = FacebookSettings.AppId;
			string facebookAppSecret = FacebookSettings.AppSecret;

			//if (v_Method)
			//{
			FacebookClient v_FacebookClient = new FacebookClient();
			IDictionary<string, object> v_FacebookClientResult = v_FacebookClient.Get("oauth/access_token"
				, new
				{
					client_id = facebookAppId,
					client_secret = facebookAppSecret,
					grant_type = "client_credentials"
				}) as IDictionary<string, object>;

			v_AccessToken = v_FacebookClientResult["access_token"].ToString();

			//}
			//else
			//{
			//	HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
			//			String.Format("https://graph.facebook.com/oauth/access_token?grant_type=client_credentials&client_id={0}&client_secret={1}",
			//			facebookAppId, facebookAppSecret));
			//	HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			//	var responseStream = new StreamReader(response.GetResponseStream());
			//	var responseString = responseStream.ReadToEnd();

			//	if (responseString.Contains("access_token="))
			//	{
			//		v_AccessToken = responseString.Replace("access_token=", string.Empty);
			//	}
			//}
			return v_AccessToken;
		}


		public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			FacebookNotification msg = notification as FacebookNotification;

			FacebookMessageTransportResponse result = new FacebookMessageTransportResponse();
			result.Message = msg;

			FacebookClient v_FacebookClient = new FacebookClient();
			if (!string.IsNullOrEmpty(this.FacebookSettings.AppAccessToken))
			{
				v_FacebookClient.AccessToken = this.FacebookSettings.AppAccessToken;
			}
			else
			{
				v_FacebookClient.AccessToken = this.GetAppAccessToken();
			}
			Dictionary<string, object> parameters = new Dictionary<string, object>();
			switch (msg.NotificationType)
			{
				case FacebookNotificationType.ApplicationRequest:
					parameters["message"] = msg.Message;
					parameters["title"] = msg.Title;
					break;
				case FacebookNotificationType.Wall:
					parameters["message"] = msg.Message;
					break;
				case FacebookNotificationType.Notification:
				default:
					parameters["href"] = msg.CallbackUrl;
					parameters["template"] = msg.Message;
					parameters["ref"] = msg.Category;
					break;
			}
			// Prepare object in call back
			FacebookAsyncParameters v_FacebookAsyncParameters = new FacebookAsyncParameters()
			{
				Callback = callback,
				WebRequest = null,
				WebResponse = null,
				Message = msg,
				AppAccessToken = FacebookSettings.AppAccessToken,
				AppId = FacebookSettings.AppId,
				AppSecret = FacebookSettings.AppSecret
			};


			CancellationToken v_CancellationToken = new CancellationToken();

			v_FacebookClient.PostCompleted += FacebookClient_PostCompleted;
			IDictionary<string, object> v_FacebookClientResult = v_FacebookClient.PostTaskAsync
				(string.Format("{0}/{1}", msg.RegistrationIds[0], msg.CommandNotification)
				, parameters
				, v_FacebookAsyncParameters
				, v_CancellationToken) as IDictionary<string, object>;




			//var postData = msg.GetJson();

			//var webReq = (HttpWebRequest)WebRequest.Create(FacebookSettings.FacebookUrl);
			////webReq.ContentLength = postData.Length;
			//webReq.Method = "POST";
			//webReq.ContentType = "application/json";
			////webReq.ContentType = "application/x-www-form-urlencoded;charset=UTF-8   can be used for plaintext bodies
			//webReq.UserAgent = "PushSharp (version: 1.0)";
			//webReq.Headers.Add("Authorization: key=" + FacebookSettings.SenderAuthToken);

			//webReq.BeginGetRequestStream(new AsyncCallback(requestStreamCallback), new FacebookAsyncParameters()
			//{
			//	Callback = callback,
			//	WebRequest = webReq,
			//	WebResponse = null,
			//	Message = msg,
			//	SenderAuthToken = FacebookSettings.SenderAuthToken,
			//	SenderId = FacebookSettings.SenderID,
			//	ApplicationId = FacebookSettings.ApplicationIdPackageName
			//});
		}

		void FacebookClient_PostCompleted(object sender, FacebookApiEventArgs e)
		{
			FacebookAsyncParameters asyncParam = null;
			TaskCompletionSource<object> v_TaskCompletionSource = null;
			//Task<FacebookAsyncParameters> v_Task = null;
			try
			{
				if ((e.UserState != null)
					&& ((e.UserState) is TaskCompletionSource<object>))
				{
					v_TaskCompletionSource = ((e.UserState) as TaskCompletionSource<object>);
					//v_Task = v_TaskCompletionSource.Task as Task<FacebookAsyncParameters>;

					asyncParam = v_TaskCompletionSource.Task.AsyncState as FacebookAsyncParameters;
				}

				if (e.Error == null)
				{
					//asyncParam.Message
					processResponseOk(asyncParam);
				}
				else
				{
					processResponseError(asyncParam, e.Error);
				}
				//throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				//Raise individual failures for each registration id for the notification
				foreach (var r in asyncParam.Message.RegistrationIds)
					asyncParam.Callback(this, new SendNotificationResult(FacebookNotification.ForSingleRegistrationId(asyncParam.Message, r), false, ex));

				Interlocked.Decrement(ref waitCounter);
			}
		}

		void processResponseOk(FacebookAsyncParameters asyncParam)
		{
			var result = new FacebookMessageTransportResponse()
			{
				ResponseCode = FacebookMessageTransportResponseCode.Ok,
				Message = asyncParam.Message
			};

			FacebookMessageResult msgResult = new FacebookMessageResult();
			msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.Ok;
			result.Results.Add(msgResult);

			////Get the response body
			//var json = new JObject();

			//try { json = JObject.Parse((new StreamReader(asyncParam.WebResponse.GetResponseStream())).ReadToEnd()); }
			//catch { }


			//result.NumberOfCanonicalIds = json.Value<long>("canonical_ids");
			//result.NumberOfFailures = json.Value<long>("failure");
			//result.NumberOfSuccesses = json.Value<long>("success");


			//var jsonResults = json["results"] as JArray;

			//if (jsonResults == null)
			//	jsonResults = new JArray();

			//foreach (var r in jsonResults)
			//{
			//	var msgResult = new FacebookMessageResult();

			//	msgResult.MessageId = r.Value<string>("message_id");
			//	msgResult.CanonicalRegistrationId = r.Value<string>("registration_id");
			//	msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.Ok;

			//	if (!string.IsNullOrEmpty(msgResult.CanonicalRegistrationId))
			//	{
			//		msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.CanonicalRegistrationId;
			//	}
			//	else if (r["error"] != null)
			//	{
			//		var err = r.Value<string>("error") ?? "";

			//		switch (err.ToLower().Trim())
			//		{
			//			case "ok":
			//				msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.Ok;
			//				break;
			//			case "missingregistration":
			//				msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.MissingRegistrationId;
			//				break;
			//			case "unavailable":
			//				msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.Unavailable;
			//				break;
			//			case "notregistered":
			//				msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.NotRegistered;
			//				break;
			//			case "invalidregistration":
			//				msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.InvalidRegistration;
			//				break;
			//			case "mismatchsenderid":
			//				msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.MismatchSenderId;
			//				break;
			//			case "messagetoobig":
			//				msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.MessageTooBig;
			//				break;
			//			case "invaliddatakey":
			//				msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.InvalidDataKey;
			//				break;
			//			case "invalidttl":
			//				msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.InvalidTtl;
			//				break;
			//			case "internalservererror":
			//				msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.InternalServerError;
			//				break;
			//			default:
			//				msgResult.ResponseStatus = FacebookMessageTransportResponseStatus.Error;
			//				break;
			//		}
			//	}

			//	result.Results.Add(msgResult);

			//}

			//asyncParam.WebResponse.Close();

			int index = 0;

			var response = result;

			//Loop through every result in the response
			// We will raise events for each individual result so that the consumer of the library
			// can deal with individual registrationid's for the notification
			foreach (var r in response.Results)
			{
				var singleResultNotification = FacebookNotification.ForSingleResult(response, index);

				if (r.ResponseStatus == FacebookMessageTransportResponseStatus.Ok)
				{
					//It worked! Raise success
					asyncParam.Callback(this, new SendNotificationResult(singleResultNotification));
				}
				else if (r.ResponseStatus == FacebookMessageTransportResponseStatus.CanonicalRegistrationId)
				{
					//Swap Registrations Id's
					var newRegistrationId = r.CanonicalRegistrationId;
					var oldRegistrationId = string.Empty;

					if (singleResultNotification.RegistrationIds != null && singleResultNotification.RegistrationIds.Count > 0)
						oldRegistrationId = singleResultNotification.RegistrationIds[0];

					asyncParam.Callback(this, new SendNotificationResult(singleResultNotification, false, new DeviceSubscriptonExpiredException()) { OldSubscriptionId = oldRegistrationId, NewSubscriptionId = newRegistrationId, IsSubscriptionExpired = true });
				}
				else if (r.ResponseStatus == FacebookMessageTransportResponseStatus.Unavailable)
				{
					asyncParam.Callback(this, new SendNotificationResult(singleResultNotification, true, new Exception("Unavailable Response Status")));
				}
				else if (r.ResponseStatus == FacebookMessageTransportResponseStatus.NotRegistered)
				{
					var oldRegistrationId = string.Empty;

					if (singleResultNotification.RegistrationIds != null && singleResultNotification.RegistrationIds.Count > 0)
						oldRegistrationId = singleResultNotification.RegistrationIds[0];

					//Raise failure and device expired
					asyncParam.Callback(this, new SendNotificationResult(singleResultNotification, false, new DeviceSubscriptonExpiredException()) { OldSubscriptionId = oldRegistrationId, IsSubscriptionExpired = true, SubscriptionExpiryUtc = DateTime.UtcNow });
				}
				else
				{
					//Raise failure, for unknown reason
					asyncParam.Callback(this, new SendNotificationResult(singleResultNotification, false, new FacebookMessageTransportException(r.ResponseStatus.ToString(), response)));
				}

				index++;
			}

			Interlocked.Decrement(ref waitCounter);
		}

		void processResponseError(FacebookAsyncParameters asyncParam, Exception p_Exception)
		{
			try
			{
				var result = new FacebookMessageTransportResponse();
				result.ResponseCode = FacebookMessageTransportResponseCode.Error;

				if (p_Exception != null)
				{
					throw new FacebookMessageTransportException(p_Exception.Message, result);
				}
				else
				{
					//	if (asyncParam == null || asyncParam.WebResponse == null)
					throw new FacebookMessageTransportException("Unknown Transport Error", result);
				}

				//if (asyncParam.WebResponse.StatusCode == HttpStatusCode.Unauthorized)
				//{
				//	//401 bad auth token
				//	result.ResponseCode = FacebookMessageTransportResponseCode.InvalidAuthToken;
				//	throw new FacebookAuthenticationErrorTransportException(result);
				//}
				//else if (asyncParam.WebResponse.StatusCode == HttpStatusCode.BadRequest)
				//{
				//	result.ResponseCode = FacebookMessageTransportResponseCode.BadRequest;
				//	throw new FacebookBadRequestTransportException(result);
				//}
				//else if (asyncParam.WebResponse.StatusCode == HttpStatusCode.InternalServerError)
				//{
				//	result.ResponseCode = FacebookMessageTransportResponseCode.InternalServiceError;
				//	throw new FacebookMessageTransportException("Internal Service Error", result);
				//}
				//else if (asyncParam.WebResponse.StatusCode == HttpStatusCode.ServiceUnavailable)
				//{
				//	//First try grabbing the retry-after header and parsing it.
				//	TimeSpan retryAfter = new TimeSpan(0, 0, 120);

				//	var wrRetryAfter = asyncParam.WebResponse.GetResponseHeader("Retry-After");

				//	if (!string.IsNullOrEmpty(wrRetryAfter))
				//	{
				//		DateTime wrRetryAfterDate = DateTime.UtcNow;

				//		if (DateTime.TryParse(wrRetryAfter, out wrRetryAfterDate))
				//			retryAfter = wrRetryAfterDate - DateTime.UtcNow;
				//		else
				//		{
				//			int wrRetryAfterSeconds = 120;
				//			if (int.TryParse(wrRetryAfter, out wrRetryAfterSeconds))
				//				retryAfter = new TimeSpan(0, 0, wrRetryAfterSeconds);
				//		}
				//	}

				//	//503 exponential backoff, get retry-after header
				//	result.ResponseCode = FacebookMessageTransportResponseCode.ServiceUnavailable;

				//	throw new FacebookServiceUnavailableTransportException(retryAfter, result);
				//}

				//throw new FacebookMessageTransportException("Unknown Transport Error", result);
			}
			finally
			{
				if (asyncParam != null && asyncParam.WebResponse != null)
					asyncParam.WebResponse.Close();
			}
		}

		public void Dispose()
		{
			var slept = 0;
			while (Interlocked.Read(ref waitCounter) > 0 && slept <= 5000)
			{
				slept += 100;
				Thread.Sleep(100);
			}
		}

		class FacebookAsyncParameters
		{
			public SendNotificationCallbackDelegate Callback { get; set; }

			public FacebookNotification Message { get; set; }

			public HttpWebRequest WebRequest { get; set; }

			public HttpWebResponse WebResponse { get; set; }

			public string AppId { get; set; }
			public string AppSecret { get; set; }
			public string AppAccessToken { get; set; }

		}
	}

	public class FacebookMessageResult
	{
		[JsonProperty("message_id", NullValueHandling = NullValueHandling.Ignore)]
		public string MessageId { get; set; }

		[JsonProperty("registration_id", NullValueHandling = NullValueHandling.Ignore)]
		public string CanonicalRegistrationId { get; set; }

		[JsonIgnore]
		public FacebookMessageTransportResponseStatus ResponseStatus { get; set; }

		[JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
		public string Error
		{
			get
			{
				switch (ResponseStatus)
				{
					case FacebookMessageTransportResponseStatus.Ok:
						return null;
					case FacebookMessageTransportResponseStatus.Unavailable:
						return "Unavailable";
					case FacebookMessageTransportResponseStatus.QuotaExceeded:
						return "QuotaExceeded";
					case FacebookMessageTransportResponseStatus.NotRegistered:
						return "NotRegistered";
					case FacebookMessageTransportResponseStatus.MissingRegistrationId:
						return "MissingRegistration";
					case FacebookMessageTransportResponseStatus.MissingCollapseKey:
						return "MissingCollapseKey";
					case FacebookMessageTransportResponseStatus.MismatchSenderId:
						return "MismatchSenderId";
					case FacebookMessageTransportResponseStatus.MessageTooBig:
						return "MessageTooBig";
					case FacebookMessageTransportResponseStatus.InvalidTtl:
						return "InvalidTtl";
					case FacebookMessageTransportResponseStatus.InvalidRegistration:
						return "InvalidRegistration";
					case FacebookMessageTransportResponseStatus.InvalidDataKey:
						return "InvalidDataKey";
					case FacebookMessageTransportResponseStatus.InternalServerError:
						return "InternalServerError";
					case FacebookMessageTransportResponseStatus.DeviceQuotaExceeded:
						return null;
					case FacebookMessageTransportResponseStatus.CanonicalRegistrationId:
						return null;
					case FacebookMessageTransportResponseStatus.Error:
						return "Error";
					default:
						return null;
				}
			}
		}
	}
}
