using System;
using System.Net;
using System.Text;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
#if NETSTANDARD
using System.Net.Http;
using System.Security.Authentication;
#endif

namespace PushSharp.Core
{
    public static class PushHttpClient
    {
        static PushHttpClient ()
        {
#if !NETSTANDARD
			ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.Expect100Continue = false;
#endif
        }

        public static async Task<PushHttpResponse> RequestAsync (PushHttpRequest request)
        {
#if NETSTANDARD
			WinHttpHandler httpHandler = new WinHttpHandler ();
			httpHandler.SslProtocols = SslProtocols.Tls12;
			HttpClient client = new HttpClient(httpHandler);

			client.DefaultRequestHeaders.Clear ();
	        foreach (var headerKey in request.Headers.AllKeys)
	        {
		        client.DefaultRequestHeaders.TryAddWithoutValidation (headerKey, request.Headers[headerKey]);
	        }

			HttpMethod method = HttpMethod.Get;
			switch (request.Method)
			{
				case "GET":
					method = HttpMethod.Get;
					break;
				case "POST":
					method = HttpMethod.Post;
					break;
				case "PUT":
					method = HttpMethod.Put;
					break;
			}

			HttpRequestMessage message = new HttpRequestMessage (method, request.Url);
	        HttpContent content = new StringContent (request.Body, request.Encoding);
	        message.Content = content;

			HttpResponseMessage httpResponse = await client.SendAsync (message);

			WebHeaderCollection responseHeaderCollection = new WebHeaderCollection ();
	        foreach (var responseHeader in httpResponse.Headers)
	        {
				responseHeaderCollection[responseHeader.Key] = responseHeader.Value.FirstOrDefault ();
	        }

			PushHttpResponse response = new PushHttpResponse
			{
				Body = await httpResponse.Content.ReadAsStringAsync (),
				Headers = responseHeaderCollection,
				Uri = httpResponse.RequestMessage.RequestUri,
				Encoding = Encoding.GetEncoding (httpResponse.Content.Headers.ContentEncoding.First ()),
				LastModified = httpResponse.Content.Headers.LastModified == null ? DateTime.Now : httpResponse.Content.Headers.LastModified.Value.DateTime,
				StatusCode = httpResponse.StatusCode
			};

			client.Dispose ();
			return response;
#else

			var httpRequest = HttpWebRequest.CreateHttp (request.Url);
            httpRequest.Proxy = null;

            httpRequest.Headers = request.Headers;

            if (!string.IsNullOrEmpty (request.Body)) {
                var requestStream = await httpRequest.GetRequestStreamAsync ();

                var requestBody = request.Encoding.GetBytes (request.Body);

                await requestStream.WriteAsync (requestBody, 0, requestBody.Length);
            }

            HttpWebResponse httpResponse;
            Stream responseStream;

            try {
                httpResponse = await httpRequest.GetResponseAsync () as HttpWebResponse;

                responseStream = httpResponse.GetResponseStream ();
            } catch (WebException webEx) {
                httpResponse = webEx.Response as HttpWebResponse;

                responseStream = httpResponse.GetResponseStream ();
            }

            var body = string.Empty;

            using (var sr = new StreamReader (responseStream))
                body = await sr.ReadToEndAsync ();
                
            var responseEncoding = Encoding.ASCII;
            try {
                responseEncoding = Encoding.GetEncoding (httpResponse.ContentEncoding);
            } catch {
            }

            var response = new PushHttpResponse {
                Body = body,
                Headers = httpResponse.Headers,
                Uri = httpResponse.ResponseUri,
                Encoding = responseEncoding,
                LastModified = httpResponse.LastModified,
                StatusCode = httpResponse.StatusCode
            };

            httpResponse.Close ();
            httpResponse.Dispose ();

            return response;
#endif
        }

	}

    public class PushHttpRequest
    {
        public PushHttpRequest ()
        {
            Encoding = Encoding.ASCII;
            Headers = new WebHeaderCollection ();
            Method = "GET";
            Body = string.Empty;
        }

        public string Url { get;set; }
        public string Method { get;set; }
        public string Body { get;set; }
        public WebHeaderCollection Headers { get;set; }
        public Encoding Encoding { get;set; }
    }

    public class PushHttpResponse 
    {
        public HttpStatusCode StatusCode { get;set; }
        public string Body { get;set; }
        public WebHeaderCollection Headers { get;set; }
        public Uri Uri { get;set; }
        public Encoding Encoding { get;set; }
        public DateTime LastModified { get;set; }
    }
}

