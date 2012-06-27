using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.Net.Security;
using Newtonsoft.Json.Linq;

namespace PushSharp.Android
{
	internal class GcmMessageTransportAsync
	{
		public event Action<string> UpdateGoogleClientAuthToken;
		public event Action<C2dmMessageTransportResponse> MessageResponseReceived;
		public event Action<AndroidNotification, Exception> UnhandledException;

		static GcmMessageTransportAsync()
		{
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, policyErrs) => { return true; };
		}

		private const string GCM_SEND_URL = "https://android.googleapis.com/gcm/send";

		public void Send(AndroidNotification msg, string googleLoginAuthorizationToken, string senderID, string applicationID)
		{
			try
			{
				send(msg, googleLoginAuthorizationToken, senderID, applicationID);
			}
			catch (Exception ex)
			{
				if (UnhandledException != null)
					UnhandledException(msg, ex);
				else
					throw ex;
			}
		}

		void send(AndroidNotification msg, string applicationApiKey, string senderID, string applicationID)
		{
			C2dmMessageTransportResponse result = new C2dmMessageTransportResponse();
			result.Message = msg;

			var postData = msg.GetPostData();

			var webReq = (HttpWebRequest)WebRequest.Create(GCM_SEND_URL);
			//webReq.ContentLength = postData.Length;
			webReq.Method = "POST";
			webReq.ContentType = "application/json";
			//webReq.ContentType = "application/x-www-form-urlencoded;charset=UTF-8   can be used for plaintext bodies
			webReq.UserAgent = "PushSharp (version: 1.0)";
			webReq.Headers.Add("Authorization: key=" + applicationApiKey);

			webReq.BeginGetRequestStream(new AsyncCallback(requestStreamCallback), new GcmAsyncParameters()
			{
				WebRequest = webReq,
				WebResponse = null,
				Message = msg,
				ApplicationApiKey = applicationApiKey,
				SenderId = senderID,
				ApplicationId = applicationID
			});
		}

		void requestStreamCallback(IAsyncResult result)
		{
			var msg = new AndroidNotification();

			try
			{
				var asyncParam = result.AsyncState as GcmAsyncParameters;
				msg = asyncParam.Message;

				if (asyncParam != null)
				{
					var wrStream = asyncParam.WebRequest.EndGetRequestStream(result);

					using (var webReqStream = new StreamWriter(wrStream))
					{
						var data = asyncParam.Message.GetPostData();
						webReqStream.Write(data);
						webReqStream.Close();
					}

					try
					{
						asyncParam.WebRequest.BeginGetResponse(new AsyncCallback(responseCallback), asyncParam);
					}
					catch (WebException wex)
					{
						asyncParam.WebResponse = wex.Response as HttpWebResponse;
						processResponseError(asyncParam);
					}
				}
			}
			catch (Exception ex)
			{
				if (UnhandledException != null)
					UnhandledException(msg, ex);
				else
					throw ex;
			}
		}

		void responseCallback(IAsyncResult result)
		{
			var msg = new AndroidNotification();

			try
			{
				var asyncParam = result.AsyncState as GcmAsyncParameters;
				msg = asyncParam.Message;

				try
				{
					asyncParam.WebResponse = asyncParam.WebRequest.EndGetResponse(result) as HttpWebResponse;
					processResponseOk(asyncParam);
				}
				catch (WebException wex)
				{
					asyncParam.WebResponse = wex.Response as HttpWebResponse;
					processResponseError(asyncParam);
				}
			}
			catch (Exception ex)
			{
				if (UnhandledException != null)
					UnhandledException(msg, ex);
				else
					throw ex;
			}
		}

		void processResponseOk(GcmAsyncParameters asyncParam)
		{
			var result = new C2dmMessageTransportResponse()
			{
				ResponseCode = MessageTransportResponseCode.Ok,
				ResponseStatus = MessageTransportResponseStatus.Ok,
				Message = asyncParam.Message,
				MessageId = string.Empty
			};

			var updateClientAuth = asyncParam.WebResponse.GetResponseHeader("Update-Client-Auth");

			if (!string.IsNullOrEmpty(updateClientAuth) && UpdateGoogleClientAuthToken != null)
				UpdateGoogleClientAuthToken(updateClientAuth);

			//Get the response body
			var json = new JObject();

			try { json = JObject.Parse((new StreamReader(asyncParam.WebResponse.GetResponseStream())).ReadToEnd()); }
			catch { }


			var jsonResults = json["results"] as JArray;

			if (jsonResults == null)
				jsonResults = new JArray();

			foreach (var r in json["results"])
			{
				var msgResult = new GcmMessageResult();

				msgResult.MessageId = r.Value<string>("message_id");
				msgResult.CanonicalRegistrationId = r.Value<string>("registration_id");
				msgResult.Error = r.Value<string>("error");
				

			}
			

			////Handle the type of error
			//if (json.StartsWith("Error="))
			//{
			//    var wrErr = responseBody.Substring(responseBody.IndexOf("Error=") + 6);
			//    switch (wrErr.ToLower().Trim())
			//    {
			//        case "quotaexceeded":
			//            result.ResponseStatus = MessageTransportResponseStatus.QuotaExceeded;
			//            break;
			//        case "devicequotaexceeded":
			//            result.ResponseStatus = MessageTransportResponseStatus.DeviceQuotaExceeded;
			//            break;
			//        case "invalidregistration":
			//            result.ResponseStatus = MessageTransportResponseStatus.InvalidRegistration;
			//            break;
			//        case "notregistered":
			//            result.ResponseStatus = MessageTransportResponseStatus.NotRegistered;
			//            break;
			//        case "messagetoobig":
			//            result.ResponseStatus = MessageTransportResponseStatus.MessageTooBig;
			//            break;
			//        case "missingcollapsekey":
			//            result.ResponseStatus = MessageTransportResponseStatus.MissingCollapseKey;
			//            break;
			//        default:
			//            result.ResponseStatus = MessageTransportResponseStatus.Error;
			//            break;
			//    }

			//    throw new MessageTransportException(wrErr, result);
			//}
			//else
			//{
			//    //Get the message ID
			//    if (responseBody.StartsWith("id="))
			//        result.MessageId = responseBody.Substring(3).Trim();
			//}

			asyncParam.WebResponse.Close();

			if (MessageResponseReceived != null)
				MessageResponseReceived(result);
		}

		void processResponseError(GcmAsyncParameters asyncParam)
		{
			var result = new C2dmMessageTransportResponse()
			{
				ResponseCode = MessageTransportResponseCode.Error,
				ResponseStatus = MessageTransportResponseStatus.Error,
				Message = asyncParam.Message,
				MessageId = string.Empty
			};

			if (asyncParam.WebResponse.StatusCode == HttpStatusCode.Unauthorized)
			{
				//401 bad auth token
				result.ResponseCode = MessageTransportResponseCode.InvalidAuthToken;
				result.ResponseStatus = MessageTransportResponseStatus.Error;
				throw new InvalidAuthenticationTokenTransportException(result);
			}
			else if (asyncParam.WebResponse.StatusCode == HttpStatusCode.ServiceUnavailable)
			{
				//First try grabbing the retry-after header and parsing it.
				TimeSpan retryAfter = new TimeSpan(0, 0, 120);

				var wrRetryAfter = asyncParam.WebResponse.GetResponseHeader("Retry-After");

				if (!string.IsNullOrEmpty(wrRetryAfter))
				{
					DateTime wrRetryAfterDate = DateTime.UtcNow;

					if (DateTime.TryParse(wrRetryAfter, out wrRetryAfterDate))
						retryAfter = wrRetryAfterDate - DateTime.UtcNow;
					else
					{
						int wrRetryAfterSeconds = 120;
						if (int.TryParse(wrRetryAfter, out wrRetryAfterSeconds))
							retryAfter = new TimeSpan(0, 0, wrRetryAfterSeconds);
					}
				}

				//503 exponential backoff, get retry-after header
				result.ResponseCode = MessageTransportResponseCode.ServiceUnavailable;
				result.ResponseStatus = MessageTransportResponseStatus.Error;

				throw new ServiceUnavailableTransportException(retryAfter, result);
			}

			asyncParam.WebResponse.Close();

			if (MessageResponseReceived != null)
				MessageResponseReceived(result);
		}

		class GcmAsyncParameters
		{
			public AndroidNotification Message
			{
				get;
				set;
			}

			public HttpWebRequest WebRequest
			{
				get;
				set;
			}

			public HttpWebResponse WebResponse
			{
				get;
				set;
			}

			public string ApplicationApiKey
			{
				get;
				set;
			}

			public string SenderId
			{
				get;
				set;
			}

			public string ApplicationId
			{
				get;
				set;
			}
		}
	}

	public class GcmMessageResult
	{
		public string MessageId { get; set; }

		public string CanonicalRegistrationId {	get; set; }

		public string Error { get; set; }
	}
}
