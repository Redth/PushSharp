using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Android;

namespace PushSharp.Tests.TestServers
{
	public delegate void GcmNotificationReceivedDelegate(GcmMessageTransportResponse result);

	internal class GcmTestServer : IDisposable
	{
		private static int msgId = 1000;

		public List<GcmMessageResponseFilter> MessageResponseFilters  = new List<GcmMessageResponseFilter>();
		public List<GcmTransportResponseFilter> TransportResponseFilters = new List<GcmTransportResponseFilter>();
 
		private HttpListener listener;
		private CancellationTokenSource cancelServer = new CancellationTokenSource();

		private GcmNotificationReceivedDelegate notificationReceived;

		public void Start(int port, GcmNotificationReceivedDelegate notificationReceived)
		{
			this.notificationReceived = notificationReceived;

			listener = new HttpListener();
			listener.Prefixes.Add("http://localhost:" + port + "/");
			

			Task.Factory.StartNew(ListenerWorker).ContinueWith(t =>
				{
					var ex = t.Exception;
					Console.WriteLine("GcmTestServer Failed: " + ex);
				}, TaskContinuationOptions.OnlyOnFaulted);
		}

		public void Dispose()
		{
			cancelServer.Cancel();
			listener.Stop();
		}

		private void ListenerWorker()
		{
			listener.Start();
			listener.BeginGetContext(ProcessRequest, listener);
		}

		private void ProcessRequest(IAsyncResult result)
		{
			try
			{

				var context = listener.EndGetContext(result);

				var request = context.Request;
				var response = context.Response;

				var wrd = string.Empty;

				using (var sr = new StreamReader(request.InputStream))
					wrd = sr.ReadToEnd();


				var gcm = JsonConvert.DeserializeObject<GcmRequest>(wrd);

				var transportResponse = new GcmMessageTransportResponse();

				foreach (var id in gcm.RegistrationIds)
				{

					GcmMessageResult singleResult = null;
					bool matchedFilter = false;

					foreach (var filter in this.MessageResponseFilters)
					{
						if (filter.IsMatch(gcm, id))
						{
							singleResult = filter.Status;
							singleResult.MessageId = "1:" + msgId++;
							transportResponse.Results.Add(singleResult);
							matchedFilter = true;
							break;
						}
					}

					if (!matchedFilter)
					{
						singleResult = new GcmMessageResult();
						singleResult.ResponseStatus = GcmMessageTransportResponseStatus.Ok;
						singleResult.MessageId = "1:" + msgId++;
						transportResponse.Results.Add(singleResult);
					}

					if (singleResult.ResponseStatus == GcmMessageTransportResponseStatus.Ok)
					{
						transportResponse.NumberOfSuccesses++;
					}
					else
						transportResponse.NumberOfFailures++;

					transportResponse.NumberOfCanonicalIds++;
					
				}

				transportResponse.ResponseCode = GcmMessageTransportResponseCode.Ok;


				foreach (var filter in TransportResponseFilters)
				{
					if (filter.IsMatch(gcm))
					{
						transportResponse.ResponseCode = filter.Code;
						break;
					}
				}


				switch (transportResponse.ResponseCode)
				{
					case GcmMessageTransportResponseCode.Ok:
						response.StatusCode = (int)HttpStatusCode.OK;
						break;
					case GcmMessageTransportResponseCode.InvalidAuthToken:
						response.StatusCode = (int)HttpStatusCode.Unauthorized;
						break;
					case GcmMessageTransportResponseCode.ServiceUnavailable:
						response.Headers.Add("Retry-After", "120");
						response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
						break;
					case GcmMessageTransportResponseCode.InternalServiceError:
						response.StatusCode = (int)HttpStatusCode.InternalServerError;
						break;
					case GcmMessageTransportResponseCode.BadRequest:
						response.StatusCode = (int)HttpStatusCode.BadRequest;
						break;
					default:
						response.StatusCode = (int)HttpStatusCode.InternalServerError;
						break;
				}

				var responseString = JsonConvert.SerializeObject(transportResponse);

				//Console.WriteLine(responseString);

				var buffer = Encoding.UTF8.GetBytes(responseString);
				// Get a response stream and write the response to it.
				response.ContentLength64 = buffer.Length;
				System.IO.Stream output = response.OutputStream;
				output.Write(buffer, 0, buffer.Length);
				// You must close the output stream.
				output.Close();

				notificationReceived(transportResponse);
			}
			catch (Exception)
			{
				
			}
			
			if (listener.IsListening && !cancelServer.IsCancellationRequested)
				listener.BeginGetContext(ProcessRequest, listener);		
		}
	}

	public class GcmMessageResponseFilter
	{
		public Func<GcmRequest, string, bool> IsMatch { get; set; }

		public GcmMessageResult Status { get; set; }
	}

	public class GcmTransportResponseFilter
	{
		public Func<GcmRequest, bool> IsMatch { get; set; }

		public GcmMessageTransportResponseCode Code { get; set; }
	}

	public class GcmRequest
	{
		[JsonProperty("dry_run")]
		public bool? DryRun { get; set; }

		[JsonProperty("restricted_package_name")]
		public string RestrictedPackageName { get; set; }

		[JsonProperty("time_to_live")]
		public int? TimeToLive { get; set; }

		[JsonProperty("delay_while_idle")]
		public bool? DelayWhileIdle { get; set; }

		[JsonProperty("data")]
		public JObject Data { get; set; }

		[JsonProperty("collapse_key")]
		public string CollapseKey { get; set; }

		[JsonProperty("registration_ids")]
		public List<string> RegistrationIds { get; set; }
	}
	
}
