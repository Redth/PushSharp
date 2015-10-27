using System;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace PushSharp.Core
{
    public static class PushHttpClient
    {
        static PushHttpClient ()
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.Expect100Continue = false;
        }

        public static async Task<PushHttpResponse> RequestAsync (PushHttpRequest request)
        {
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

