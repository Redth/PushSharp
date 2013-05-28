using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Globalization;

namespace PushSharp.Blackberry
{
    public enum QualityOfServiceLevel
	{
		NotSpecified,
		Unconfirmed,
		PreferConfirmed,
		Confirmed
	}

    public class BlackberryNotification : Core.Notification
	{
		public BlackberryNotification()
		{
			PushId = Guid.NewGuid ().ToString ();
			Recipients = new List<BlackberryRecipient> ();
			ContentType = "text/plain";
		}

		public string PushId { get; private set; }
		public QualityOfServiceLevel? QualityOfService { get;set; }
		public string PpgNotifyRequestedTo { get; set; }
		public DateTime? DeliverBeforeTimestamp { get; set; }
		public DateTime? DeliverAfterTimestamp { get; set; }
		public List<BlackberryRecipient> Recipients { get;set; }

		public string ContentType { get; set; }
		public byte[] Message { get; set; }
      
		public string ToPapXml()
		{
			var doc = new XDocument ();

			var docType = new XDocumentType ("pap", "//WAPFORUM//DTD PAP 2.0//EN", "http://www.wapforum.org/DTD/pap_2.0.dtd", "[<?wap-pap-ver supported-versions=\"2.0\"?>]");

			doc.AddFirst (docType);

			var pap = new XElement ("pap");

			var pushMsg = new XElement ("push-message");

			pushMsg.Add (new XAttribute ("push-id", this.PushId));

			if (this.QualityOfService.HasValue && !string.IsNullOrEmpty (this.PpgNotifyRequestedTo))
				pushMsg.Add(new XAttribute("ppg-notify-requested-to", this.PpgNotifyRequestedTo));

			if (this.DeliverAfterTimestamp.HasValue)
					pushMsg.Add (new XAttribute ("deliver-after-timestamp", this.DeliverAfterTimestamp.Value.ToUniversalTime ().ToString("s", CultureInfo.InvariantCulture) + "Z"));
			if (this.DeliverBeforeTimestamp.HasValue)
				pushMsg.Add (new XAttribute ("deliver-before-timestamp", this.DeliverBeforeTimestamp.Value.ToUniversalTime ().ToString("s", CultureInfo.InvariantCulture) + "Z"));

			//Add all the recipients
			foreach (var r in Recipients)
			{
				var address = new XElement ("address");
				var addrValue = string.Format ("WAPPUSH={0}%3A{1}/TYPE={2}", System.Web.HttpUtility.UrlEncode(r.Recipient), r.Port, r.RecipientType);

				address.Add(new XAttribute("address-value", addrValue));
				pushMsg.Add (address);
			}

			if (this.QualityOfService.HasValue)
				pushMsg.Add (new XElement ("quality-of-service", new XAttribute ("delivery-method", this.QualityOfService.Value.ToString ().ToLowerInvariant ())));

			doc.Add (pap);

			return doc.ToString (SaveOptions.DisableFormatting);
		}

	
		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
		}

	}

	public class BlackberryRecipient
	{
		public string Recipient { get;set; }
		public int Port { get;set; }
		public string RecipientType { get;set; }
	}
}
