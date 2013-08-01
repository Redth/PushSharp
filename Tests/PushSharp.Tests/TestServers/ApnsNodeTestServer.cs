using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PushSharp.Tests
{
	public class ApnsNodeTestServer
	{
		public ApnsNodeTestServer (string url)
		{
			Url = url;
		}

		public string Url { get;set; }

		public void Setup(params int[] failIds)
		{
			var wc = new WebClient ();
		
			var qs = new StringBuilder ();
			foreach (var id in failIds)
				qs.AppendFormat ("failId={0}&", id);

			var url = Url.TrimEnd ('/') + "/setup?" + qs.ToString ().TrimEnd ('&');

			var data = wc.DownloadString (url);

			var json = JObject.Parse (data);
		
		}

		public ApnsNodeTestServerInfo GetInfo()
		{
			var wc = new WebClient ();
			var url = Url.TrimEnd ('/') + "/info";
			var data = wc.DownloadString (url);

			return JsonConvert.DeserializeObject<ApnsNodeTestServerInfo> (data);
		}

		public void Reset()
		{
			var wc = new WebClient ();
			var url = Url.TrimEnd ('/') + "/reset";
			var data = wc.DownloadString (url);
			//var json = JObject.Parse (url);
		}
	}

	public class ApnsNodeTestServerInfo
	{
		[JsonProperty("failedIds")]
		public int[] FailedIds { get;set; }
		[JsonProperty("successIds")]
		public int[] SuccessIds { get;set; }
		[JsonProperty("discarded")]
		public int Discarded { get;set; }
		[JsonProperty("received")]
		public int Received { get;set; }
	}
}

