using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace PushSharp.Apple
{
	public class AppleNotificationPayload
	{
		public AppleNotificationAlert Alert { get; set; }

		public int? ContentAvailable { get; set; }

		public int? Badge { get; set; }

		public string Sound { get; set; }

		public bool HideActionButton { get; set; }

		public Dictionary<string, object[]> CustomItems
		{
			get;
			private set;
		}

		public AppleNotificationPayload()
		{
			HideActionButton = false;
			Alert = new AppleNotificationAlert();
			CustomItems = new Dictionary<string, object[]>();
		}

		public AppleNotificationPayload(string alert)
		{
			HideActionButton = false;
			Alert = new AppleNotificationAlert() { Body = alert };
			CustomItems = new Dictionary<string, object[]>();
		}

		public AppleNotificationPayload(string alert, int badge)
		{
			HideActionButton = false;
			Alert = new AppleNotificationAlert() { Body = alert };
			Badge = badge;
			CustomItems = new Dictionary<string, object[]>();
		}

		public AppleNotificationPayload(string alert, int badge, string sound)
		{
			HideActionButton = false;
			Alert = new AppleNotificationAlert() { Body = alert };
			Badge = badge;
			Sound = sound;
			CustomItems = new Dictionary<string, object[]>();
		}

		public void AddCustom(string key, params object[] values)
		{
			if (values != null)
				this.CustomItems.Add(key, values);
		}

		public string ToJson()
		{
			JObject json = new JObject();

			JObject aps = new JObject();

			if (!this.Alert.IsEmpty)
			{
                if (!string.IsNullOrEmpty(this.Alert.Body)
					&& string.IsNullOrEmpty(this.Alert.LocalizedKey)
					&& string.IsNullOrEmpty(this.Alert.ActionLocalizedKey)
					&& (this.Alert.LocalizedArgs == null || this.Alert.LocalizedArgs.Count <= 0)
					&& string.IsNullOrEmpty(this.Alert.LaunchImage)
                    && !this.HideActionButton)
				{
					aps["alert"] = new JValue(this.Alert.Body);
				}
				else
				{
					JObject jsonAlert = new JObject();

					if (!string.IsNullOrEmpty(this.Alert.LocalizedKey))
						jsonAlert["loc-key"] = new JValue(this.Alert.LocalizedKey);

					if (this.Alert.LocalizedArgs != null && this.Alert.LocalizedArgs.Count > 0)
						jsonAlert["loc-args"] = new JArray(this.Alert.LocalizedArgs.ToArray());

					if (!string.IsNullOrEmpty(this.Alert.Body))
						jsonAlert["body"] = new JValue(this.Alert.Body);

					if (this.HideActionButton)
						jsonAlert["action-loc-key"] = new JValue((string)null);
					else if (!string.IsNullOrEmpty(this.Alert.ActionLocalizedKey))
						jsonAlert["action-loc-key"] = new JValue(this.Alert.ActionLocalizedKey);

                    if (!string.IsNullOrEmpty(this.Alert.LaunchImage))
                        jsonAlert["launch-image"] = new JValue(this.Alert.LaunchImage);

					aps["alert"] = jsonAlert;
				}
			}

			if (this.Badge.HasValue)
				aps["badge"] = new JValue(this.Badge.Value);

			if (!string.IsNullOrEmpty(this.Sound))
				aps["sound"] = new JValue(this.Sound);

			if (this.ContentAvailable.HasValue)
				aps["content-available"] = new JValue(this.ContentAvailable.Value);

			if (aps.Count > 0)
				json["aps"] = aps;

			foreach (string key in this.CustomItems.Keys)
			{
				if (this.CustomItems[key].Length == 1)
					json[key] = new JValue(this.CustomItems[key][0]);
				else if (this.CustomItems[key].Length > 1)
					json[key] = new JArray(this.CustomItems[key]);
			}

			string rawString = json.ToString(Newtonsoft.Json.Formatting.None, null);

			StringBuilder encodedString = new StringBuilder();
			foreach (char c in rawString)
			{
				if ((int)c < 32 || (int)c > 127)
					encodedString.Append("\\u" + String.Format("{0:x4}", Convert.ToUInt32(c)));
				else
					encodedString.Append(c);
			}
			return rawString;// encodedString.ToString();
		}

		//public string ToJson()
		//{
		//    bool tmpBool;
		//    double tmpDbl;

		//    var json = new StringBuilder();

		//    var aps = new StringBuilder();

		//    if (!this.Alert.IsEmpty)
		//    {
		//        if (!string.IsNullOrEmpty(this.Alert.Body)
		//            && string.IsNullOrEmpty(this.Alert.LocalizedKey)
		//            && string.IsNullOrEmpty(this.Alert.ActionLocalizedKey)
		//            && (this.Alert.LocalizedArgs == null || this.Alert.LocalizedArgs.Count <= 0)
		//            && !this.HideActionButton)
		//        {
		//            aps.AppendFormat("\"alert\":\"{0}\",", this.Alert.Body);
		//        }
		//        else
		//        {
		//            var jsonAlert = new StringBuilder();

		//            if (!string.IsNullOrEmpty(this.Alert.LocalizedKey))
		//                jsonAlert.AppendFormat("\"loc-key\":\"{0}\",", this.Alert.LocalizedKey);

		//            if (this.Alert.LocalizedArgs != null && this.Alert.LocalizedArgs.Count > 0)
		//            {
		//                var locArgs = new StringBuilder();

		//                foreach (var larg in this.Alert.LocalizedArgs)
		//                {
		//                    if (double.TryParse(larg.ToString(), out tmpDbl)
		//                        || bool.TryParse(larg.ToString(), out tmpBool))
		//                        locArgs.AppendFormat("{0},", larg.ToString());
		//                    else
		//                        locArgs.AppendFormat("\"{0}\",", larg.ToString());
		//                }

		//                jsonAlert.AppendFormat("\"loc-args\":[{0}],", locArgs.ToString().TrimEnd(','));
		//            }

		//            if (!string.IsNullOrEmpty(this.Alert.Body))
		//                jsonAlert.AppendFormat("\body\":\"{0}\",", this.Alert.Body);

		//            if (this.HideActionButton)
		//                jsonAlert.AppendFormat("\"action-loc-key\":null,");
		//            else if (!string.IsNullOrEmpty(this.Alert.ActionLocalizedKey))
		//                jsonAlert.AppendFormat("\action-loc-key\":\"{0}\",", this.Alert.ActionLocalizedKey);

		//            aps.Append("\"alert\":{");
		//            aps.Append(jsonAlert.ToString().TrimEnd(','));
		//            aps.Append("},");
		//        }
		//    }

		//    if (this.Badge.HasValue)
		//        aps.AppendFormat("\"badge\":{0},", this.Badge.Value.ToString());

		//    if (!string.IsNullOrEmpty(this.Sound))
		//        aps.AppendFormat("\"sound\":\"{0}\",", this.Sound);

		//    if (this.ContentAvailable.HasValue)
		//        aps.AppendFormat("\"content-available\":{0},", this.ContentAvailable.Value.ToString());

		//    json.Append("\"aps\":{");
		//    json.Append(aps.ToString().TrimEnd(','));
		//    json.Append("},");

		//    foreach (string key in this.CustomItems.Keys)
		//    {
		//        if (this.CustomItems[key].Length == 1)
		//        {
		//            if (double.TryParse(this.CustomItems[key].ToString(), out tmpDbl)
		//                || bool.TryParse(this.CustomItems[key].ToString(), out tmpBool))
		//                json.AppendFormat("\"{0}\":[{1}],", key, this.CustomItems[key][0].ToString());
		//            else
		//                json.AppendFormat("\"{0}\":[\"{1}\",", key, this.CustomItems[key][0].ToString());
		//        }
		//        else if (this.CustomItems[key].Length > 1)
		//        {
		//            var jarr = new StringBuilder();

		//            foreach (var item in this.CustomItems[key])
		//            {
		//                if (double.TryParse(item.ToString(), out tmpDbl)
		//                    || bool.TryParse(item.ToString(), out tmpBool))
		//                    jarr.AppendFormat("{0},", item.ToString());
		//                else
		//                    jarr.AppendFormat("\"{0}\",", item.ToString());
		//            }

		//            json.AppendFormat("\"{0}\":[{1}],", key, jarr.ToString().Trim(','));
		//        }
		//    }

		//    string rawString = "{" + json.ToString().TrimEnd(',') + "}";

		//    StringBuilder encodedString = new StringBuilder();
		//    foreach (char c in rawString)
		//    {
		//        if ((int)c < 32 || (int)c > 127)
		//            encodedString.Append("\\u" + String.Format("{0:x4}", Convert.ToUInt32(c)));
		//        else
		//            encodedString.Append(c);
		//    }
		//    return rawString;// encodedString.ToString();
		//}

		public override string ToString()
		{
			return ToJson();
		}
	}
}
