using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using PushSharp.Core;

namespace PushSharp.Amazon.Adm
{
	public class AdmNotification : Notification
	{
		public AdmNotification ()
		{
			Data = new Dictionary<string, string> ();
		}

		public Dictionary<string, string> Data { get; set; }

		public string ConsolidationKey { get;set; }

		public int? ExpiresAfter { get;set; }

		public string RegistrationId { get;set; }

		public string ToJson()
		{
			var json = new JObject ();

			var data = new JObject();

			if (Data != null && Data.Count > 0)
			{
				foreach (var key in Data.Keys)
					data [key] = Data [key];
			}

			json ["data"] = data;

			if (!string.IsNullOrEmpty (ConsolidationKey))
				json ["consolidationKey"] = ConsolidationKey;

			if (ExpiresAfter.HasValue && ExpiresAfter.Value >= 0)
				json ["expiresAfter"] = ExpiresAfter.Value;

			return json.ToString();
		}

		public override string ToString ()
		{
			return ToJson ();
		}
	}
}

