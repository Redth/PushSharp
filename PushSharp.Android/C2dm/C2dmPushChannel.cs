using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Security;
using System.Net.Security;
using System.Threading;
using PushSharp.Core;

namespace PushSharp.Android
{
	[Obsolete("Google has Deprecated C2DM, and you should now use GCM Instead.")]
	public class C2dmPushChannel : IPushChannel
	{
		C2dmPushChannelSettings androidSettings = null;
		string googleAuthToken = string.Empty;		
		long waitCounter = 0;

		static C2dmPushChannel()
		{
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, policyErrs) => true;
		}

		public C2dmPushChannel(IPushChannelSettings channelSettings)
		{
			androidSettings = channelSettings as C2dmPushChannelSettings;
		}

		private const string C2DM_SEND_URL = "https://android.apis.google.com/c2dm/send";

		public void SendNotification(INotification notification, SendNotificationCallbackDelegate callback)
		{
			if (string.IsNullOrEmpty(googleAuthToken))
				RefreshGoogleAuthToken();

			var msg = notification as C2dmNotification;

			var result = new C2dmMessageTransportResponse();
			result.Message = msg;

			var postData = msg.GetPostData();

			var webReq = (HttpWebRequest)WebRequest.Create(C2DM_SEND_URL);
			//webReq.ContentLength = postData.Length;
			webReq.Method = "POST";
			webReq.ContentType = "application/x-www-form-urlencoded";
			webReq.UserAgent = "PushSharp (version: 1.0)";
			webReq.Headers.Add("Authorization: GoogleLogin auth=" + googleAuthToken);

			webReq.BeginGetRequestStream(requestStreamCallback, new C2dmAsyncParameters()
			{
				WebRequest = webReq,
				WebResponse = null,
				Message = msg,
				GoogleAuthToken = googleAuthToken,
				SenderId = androidSettings.SenderID,
				ApplicationId = androidSettings.ApplicationID
			});
		}

		void requestStreamCallback(IAsyncResult result)
		{
			var asyncParam = result.AsyncState as C2dmAsyncParameters;
			
			try
			{	
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
				if (asyncParam.Callback != null)
					asyncParam.Callback(this, new SendNotificationResult(asyncParam.Message, true, new Exception("Unknown Network Failure")));
			}
		}

		void responseCallback(IAsyncResult result)
		{
			var asyncParam = result.AsyncState as C2dmAsyncParameters;
		
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
				if (asyncParam.Callback != null)
					asyncParam.Callback(this, new SendNotificationResult(asyncParam.Message, true, new Exception("Unknown Network Failure")));
			}
		}

		void processResponseOk(C2dmAsyncParameters asyncParam)
		{
			var result = new C2dmMessageTransportResponse()
			{
				ResponseCode = MessageTransportResponseCode.Ok,
				ResponseStatus = MessageTransportResponseStatus.Ok,
				Message = asyncParam.Message,
				MessageId = string.Empty
			};

			var updateClientAuth = asyncParam.WebResponse.GetResponseHeader("Update-Client-Auth");

			//Update our client auth if necessary
			if (!string.IsNullOrEmpty(updateClientAuth))
				googleAuthToken = updateClientAuth;

			//Get the response body
			var responseBody = "Error=";
			try { responseBody = (new StreamReader(asyncParam.WebResponse.GetResponseStream())).ReadToEnd(); }
			catch { }

			//Handle the type of error
			if (responseBody.StartsWith("Error="))
			{
				var wrErr = responseBody.Substring(responseBody.IndexOf("Error=") + 6);
				switch (wrErr.ToLower().Trim())
				{
					case "quotaexceeded":
						result.ResponseStatus = MessageTransportResponseStatus.QuotaExceeded;
						break;
					case "devicequotaexceeded":
						result.ResponseStatus = MessageTransportResponseStatus.DeviceQuotaExceeded;
						break;
					case "invalidregistration":
						result.ResponseStatus = MessageTransportResponseStatus.InvalidRegistration;
						break;
					case "notregistered":
						result.ResponseStatus = MessageTransportResponseStatus.NotRegistered;
						break;
					case "messagetoobig":
						result.ResponseStatus = MessageTransportResponseStatus.MessageTooBig;
						break;
					case "missingcollapsekey":
						result.ResponseStatus = MessageTransportResponseStatus.MissingCollapseKey;
						break;
					default:
						result.ResponseStatus = MessageTransportResponseStatus.Error;
						break;
				}

				result.ResponseCode = MessageTransportResponseCode.Error;
				//throw new MessageTransportException(wrErr, result);
			}
			else
			{
				//Get the message ID
				if (responseBody.StartsWith("id="))
					result.MessageId = responseBody.Substring(3).Trim();
			}

			asyncParam.WebResponse.Close();

			ProcessResponse(asyncParam, result);
		}

		void processResponseError(C2dmAsyncParameters asyncParam)
		{
			var result = new C2dmMessageTransportResponse()
			{
				ResponseCode = MessageTransportResponseCode.Error,
				ResponseStatus = MessageTransportResponseStatus.Error,
				Message = asyncParam != null ? asyncParam.Message : null,
				MessageId = string.Empty
			};

			try
			{
				if (asyncParam.WebResponse.StatusCode == HttpStatusCode.Unauthorized)
				{
					//401 bad auth token
					result.ResponseCode = MessageTransportResponseCode.InvalidAuthToken;
					result.ResponseStatus = MessageTransportResponseStatus.Error;
					//throw new InvalidAuthenticationTokenTransportException(result);
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
			}
			finally
			{
				if (asyncParam != null && asyncParam.WebResponse != null)
					asyncParam.WebResponse.Close();
			}

			ProcessResponse(asyncParam, result);
		}

		private void ProcessResponse(C2dmAsyncParameters asyncParam, C2dmMessageTransportResponse response)
		{
			//Check if our token was expired and refresh/requeue if need be
			if (response.ResponseCode == MessageTransportResponseCode.InvalidAuthToken)
			{
				Interlocked.Decrement(ref waitCounter);
				asyncParam.Callback(this, new SendNotificationResult(asyncParam.Message, true, new Exception("Invalid Auth Token Response")));
				this.googleAuthToken = string.Empty;
				return;
			}

			if (response.ResponseStatus == MessageTransportResponseStatus.Ok)
				asyncParam.Callback(this, new SendNotificationResult(response.Message)); //Msg ok!
			else if (response.ResponseStatus == MessageTransportResponseStatus.InvalidRegistration || response.ResponseStatus == MessageTransportResponseStatus.NotRegistered)
			{
				//Device subscription is no good!
				asyncParam.Callback(this, new SendNotificationResult(response.Message, false, new DeviceSubscriptonExpiredException()) { IsSubscriptionExpired = true});
			}
			else
			{
				//Message Failed some other way
				asyncParam.Callback(this, new SendNotificationResult(response.Message, false, new Exception(response.ResponseStatus.ToString())));
			}

			Interlocked.Decrement(ref waitCounter);
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

		/// <summary>
		/// Explicitly refreshes the Google Auth Token.  Usually not necessary.
		/// </summary>
		public void RefreshGoogleAuthToken()
		{
			string authUrl = "https://www.google.com/accounts/ClientLogin";

			var data = new NameValueCollection();

			data.Add("Email", this.androidSettings.SenderID);
			data.Add("Passwd", this.androidSettings.Password);
			data.Add("accountType", "GOOGLE_OR_HOSTED");
			data.Add("service", "ac2dm");
			data.Add("source", this.androidSettings.ApplicationID);

			var wc = new WebClient();

			try
			{
				var authStr = Encoding.ASCII.GetString(wc.UploadValues(authUrl, data));

				//Only care about the Auth= part at the end
				if (authStr.Contains("Auth="))
					googleAuthToken = authStr.Substring(authStr.IndexOf("Auth=") + 5);
				else
					throw new GoogleLoginAuthorizationException("Missing Auth Token");
			}
			catch (WebException ex)
			{
				var result = "Unknown Error";
				try { result = (new System.IO.StreamReader(ex.Response.GetResponseStream())).ReadToEnd(); }
				catch { }

				throw new GoogleLoginAuthorizationException(result);
			}
		}

		class C2dmAsyncParameters
		{
			public SendNotificationCallbackDelegate Callback { get; set; }

			public C2dmNotification Message { get; set; }

			public HttpWebRequest WebRequest { get; set; }

			public HttpWebResponse WebResponse { get; set; }

			public string GoogleAuthToken { get; set; }

			public string SenderId { get; set; }

			public string ApplicationId { get; set; }
		}
	}
}
