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
using Newtonsoft.Json.Linq;
using PushSharp.Core;

namespace PushSharp.Android
{
	public class GcmPushChannel : IPushChannel
	{
		GcmPushChannelSettings gcmSettings = null;
		long waitCounter = 0;

		public GcmPushChannel(GcmPushChannelSettings channelSettings)
		{
			gcmSettings = channelSettings as GcmPushChannelSettings;	
		}


		static GcmPushChannel()
		{
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, policyErrs) => { return true; };
		}

		private const string GCM_SEND_URL = "https://android.googleapis.com/gcm/send";
		
		public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			var msg = notification as GcmNotification;

			var result = new GcmMessageTransportResponse();
			result.Message = msg;
						
			var postData = msg.GetJson();

			var webReq = (HttpWebRequest)WebRequest.Create(GCM_SEND_URL);
			//webReq.ContentLength = postData.Length;
			webReq.Method = "POST";
			webReq.ContentType = "application/json";
			//webReq.ContentType = "application/x-www-form-urlencoded;charset=UTF-8   can be used for plaintext bodies
			webReq.UserAgent = "PushSharp (version: 1.0)";
			webReq.Headers.Add("Authorization: key=" + gcmSettings.SenderAuthToken);

			webReq.BeginGetRequestStream(new AsyncCallback(requestStreamCallback), new GcmAsyncParameters()
			{
				Callback = callback,
				WebRequest = webReq,
				WebResponse = null,
				Message = msg,
				SenderAuthToken = gcmSettings.SenderAuthToken,
				SenderId = gcmSettings.SenderID,
				ApplicationId = gcmSettings.ApplicationIdPackageName
			});
		}

		void requestStreamCallback(IAsyncResult result)
		{
			var asyncParam = result.AsyncState as GcmAsyncParameters;
		
			try
			{
				if (asyncParam != null)
				{
					var wrStream = asyncParam.WebRequest.EndGetRequestStream(result);

					using (var webReqStream = new StreamWriter(wrStream))
					{
						var data = asyncParam.Message.GetJson();
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
				//Raise individual failures for each registration id for the notification
				foreach (var r in asyncParam.Message.RegistrationIds)
					asyncParam.Callback(this, new SendNotificationResult(GcmNotification.ForSingleRegistrationId(asyncParam.Message, r), false, ex));

				Interlocked.Decrement(ref waitCounter);
			}
		}

		void responseCallback(IAsyncResult result)
		{
			var asyncParam = result.AsyncState as GcmAsyncParameters;

			try
			{
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
				//Raise individual failures for each registration id for the notification
				foreach (var r in asyncParam.Message.RegistrationIds)
					asyncParam.Callback(this, new SendNotificationResult(GcmNotification.ForSingleRegistrationId(asyncParam.Message, r), false, ex));

				Interlocked.Decrement(ref waitCounter);
			}
		}

		void processResponseOk(GcmAsyncParameters asyncParam)
		{
			var result = new GcmMessageTransportResponse()
			{
				ResponseCode = GcmMessageTransportResponseCode.Ok,
				Message = asyncParam.Message
			};

			//Get the response body
			var json = new JObject();

			try { json = JObject.Parse((new StreamReader(asyncParam.WebResponse.GetResponseStream())).ReadToEnd()); }
			catch { }


			result.NumberOfCanonicalIds = json.Value<long>("canonical_ids");
			result.NumberOfFailures = json.Value<long>("failure");
			result.NumberOfSuccesses = json.Value<long>("success");

		
			var jsonResults = json["results"] as JArray;

			if (jsonResults == null)
				jsonResults = new JArray();

			foreach (var r in json["results"])
			{
				var msgResult = new GcmMessageResult();
								
				msgResult.MessageId = r.Value<string>("message_id");
				msgResult.CanonicalRegistrationId = r.Value<string>("registration_id");

				if (!string.IsNullOrEmpty(msgResult.CanonicalRegistrationId))
				{
					msgResult.ResponseStatus = GcmMessageTransportResponseStatus.CanonicalRegistrationId;
				}
				else if (r["error"] != null)
				{
					var err = r.Value<string>("error") ?? "";

					switch (err.ToLower().Trim())
					{
						case "missingregistration":
							msgResult.ResponseStatus = GcmMessageTransportResponseStatus.MissingRegistrationId;
							break;
						case "unavailable":
							msgResult.ResponseStatus = GcmMessageTransportResponseStatus.Unavailable;
							break;
						case "notregistered":
							msgResult.ResponseStatus = GcmMessageTransportResponseStatus.NotRegistered;
							break;
						case "invalidregistration":
							msgResult.ResponseStatus = GcmMessageTransportResponseStatus.InvalidRegistration;
							break;
						case "mismatchsenderid":
							msgResult.ResponseStatus = GcmMessageTransportResponseStatus.MismatchSenderId;
							break;
						case "messagetoobig":
							msgResult.ResponseStatus = GcmMessageTransportResponseStatus.MessageTooBig;
							break;
                        case "invaliddatakey":
                            msgResult.ResponseStatus = GcmMessageTransportResponseStatus.InvalidDataKey;
                            break;
                        case "invalidttl":
                            msgResult.ResponseStatus = GcmMessageTransportResponseStatus.InvalidTtl;
                            break;
                        case "internalservererror":
                            msgResult.ResponseStatus = GcmMessageTransportResponseStatus.InternalServerError;
                            break;
						default:
							msgResult.ResponseStatus = GcmMessageTransportResponseStatus.Error;
							break;
					}
				}

				result.Results.Add(msgResult);
								
			}

			asyncParam.WebResponse.Close();

			int index = 0;

			var response = result;

			//Loop through every result in the response
			// We will raise events for each individual result so that the consumer of the library
			// can deal with individual registrationid's for the notification
			foreach (var r in response.Results)
			{
				var singleResultNotification = GcmNotification.ForSingleResult(response, index);

				if (r.ResponseStatus == GcmMessageTransportResponseStatus.Ok)
				{
					//It worked! Raise success
					asyncParam.Callback(this, new SendNotificationResult(singleResultNotification));
				}
				else if (r.ResponseStatus == GcmMessageTransportResponseStatus.CanonicalRegistrationId)
				{
					//Swap Registrations Id's
					var newRegistrationId = r.CanonicalRegistrationId;

					asyncParam.Callback(this, new SendNotificationResult(singleResultNotification, false, new DeviceSubscriptonExpiredException()) { NewSubscriptionId = newRegistrationId });
				}
				else if (r.ResponseStatus == GcmMessageTransportResponseStatus.Unavailable)
				{
					asyncParam.Callback(this, new SendNotificationResult(singleResultNotification, true, new Exception("Unavailable Response Status")));
				}
				else if (r.ResponseStatus == GcmMessageTransportResponseStatus.NotRegistered)
				{
					//Raise failure and device expired
					asyncParam.Callback(this, new SendNotificationResult(singleResultNotification, false, new DeviceSubscriptonExpiredException()));
				}
				else
				{
					//Raise failure, for unknown reason
					asyncParam.Callback(this, new SendNotificationResult(singleResultNotification, false, new GcmMessageTransportException(r.ResponseStatus.ToString(), response)));
				}

				index++;
			}

			Interlocked.Decrement(ref waitCounter);
		}

		void processResponseError(GcmAsyncParameters asyncParam)
		{
			try
			{
				var result = new GcmMessageTransportResponse();
				result.ResponseCode = GcmMessageTransportResponseCode.Error;

				if (asyncParam == null || asyncParam.WebResponse == null)
					throw new GcmMessageTransportException("Unknown Transport Error", result);

				if (asyncParam.WebResponse.StatusCode == HttpStatusCode.Unauthorized)
				{
					//401 bad auth token
					result.ResponseCode = GcmMessageTransportResponseCode.InvalidAuthToken;
					throw new GcmAuthenticationErrorTransportException(result);
				}
				else if (asyncParam.WebResponse.StatusCode == HttpStatusCode.BadRequest)
				{
					result.ResponseCode = GcmMessageTransportResponseCode.BadRequest;
					throw new GcmBadRequestTransportException(result);
				}
				else if (asyncParam.WebResponse.StatusCode == HttpStatusCode.InternalServerError)
				{
					result.ResponseCode = GcmMessageTransportResponseCode.InternalServiceError;
					throw new GcmMessageTransportException("Internal Service Error", result);
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
					result.ResponseCode = GcmMessageTransportResponseCode.ServiceUnavailable;

					throw new GcmServiceUnavailableTransportException(retryAfter, result);
				}

				throw new GcmMessageTransportException("Unknown Transport Error", result);
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

		class GcmAsyncParameters
		{
			public SendNotificationCallbackDelegate Callback { get; set; }

			public GcmNotification Message { get; set; }

			public HttpWebRequest WebRequest { get; set; }

			public HttpWebResponse WebResponse { get; set; }

			public string SenderAuthToken { get; set; }

			public string SenderId { get; set; }

			public string ApplicationId { get; set; }
		}
	}

	public class GcmMessageResult
	{
		public string MessageId { get; set; }

		public string CanonicalRegistrationId {	get; set; }

		public GcmMessageTransportResponseStatus ResponseStatus { get; set; }
	}
}
