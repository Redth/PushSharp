using System;
using PushSharp.Core;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
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

    public class BlackberryNotification : INotification
    {
        public BlackberryNotification()
        {
            PushId = Guid.NewGuid ().ToString ();
            Recipients = new List<BlackberryRecipient> ();
            DeliverBeforeTimestamp = DateTime.UtcNow.AddMinutes(5);
            QualityOfService = QualityOfServiceLevel.Unconfirmed;
        }

        public bool IsDeviceRegistrationIdValid ()
        {
            return true;
        }

        public object Tag { get;set; }

        public string PushId { get; private set; }

        public QualityOfServiceLevel QualityOfService { get;set; }

        /// <summary>
        /// Address (e.g. URL) that Blackberry push service could use for notification 
        /// of results related to the message
        /// </summary>
        public string PpgNotifyRequestedTo { get; set; }

        /// <summary>
        /// Date and time by which the content must be delivered,expressed as UTC
        /// Message that has aged beyond this date will not be transmitted
        /// </summary>
        public DateTime? DeliverBeforeTimestamp { get; set; }

        /// <summary>
        /// Date and time after which the content should be delivered,expressed as UTC
        /// Message will not be transmitted before this date
        /// </summary>
        public DateTime? DeliverAfterTimestamp { get; set; }

        public List<BlackberryRecipient> Recipients { get;set; }

        public string SourceReference { get; set; }

        public BlackberryMessageContent Content { get; set; }

        public string ToPapXml()
        {
            var doc = new XDocument ();

            var docType = new XDocumentType("pap", "-//WAPFORUM//DTD PAP 2.1//EN", "http://www.openmobilealliance.org/tech/DTD/pap_2.1.dtd", "<?wap-pap-ver supported-versions=\"2.0\"?>");

            doc.AddFirst (docType);

            var pap = new XElement ("pap");

            var pushMsg = new XElement ("push-message");

            pushMsg.Add (new XAttribute ("push-id", this.PushId));
            pushMsg.Add(new XAttribute("source-reference", this.SourceReference));

            if (!string.IsNullOrEmpty (this.PpgNotifyRequestedTo))
                pushMsg.Add(new XAttribute("ppg-notify-requested-to", this.PpgNotifyRequestedTo));

            if (this.DeliverAfterTimestamp.HasValue)
                pushMsg.Add (new XAttribute ("deliver-after-timestamp", this.DeliverAfterTimestamp.Value.ToUniversalTime ().ToString("s", CultureInfo.InvariantCulture) + "Z"));
            if (this.DeliverBeforeTimestamp.HasValue)
                pushMsg.Add (new XAttribute ("deliver-before-timestamp", this.DeliverBeforeTimestamp.Value.ToUniversalTime ().ToString("s", CultureInfo.InvariantCulture) + "Z"));

            //Add all the recipients
            foreach (var r in Recipients)
            {
                var address = new XElement("address");

                var addrValue = r.Recipient;

                if (!string.IsNullOrEmpty(r.RecipientType))
                {
                    addrValue = string.Format("WAPPUSH={0}%3A{1}/TYPE={2}", System.Web.HttpUtility.UrlEncode(r.Recipient),
                        r.Port, r.RecipientType);
                }

                address.Add(new XAttribute("address-value", addrValue));
                pushMsg.Add (address);
            }

            pushMsg.Add (new XElement ("quality-of-service", new XAttribute ("delivery-method", this.QualityOfService.ToString ().ToLowerInvariant ())));

            pap.Add(pushMsg);
            doc.Add (pap);

            return "<?xml version=\"1.0\"?>" + Environment.NewLine + doc.ToString (SaveOptions.None);
        }


        protected string XmlEncode(string text)
        {
            return System.Security.SecurityElement.Escape(text);
        }

    }

    public class BlackberryRecipient
    {
        public BlackberryRecipient(string recipient)
        {
            Recipient = recipient;
        }

        public BlackberryRecipient(string recipient, int port, string recipientType)
        {
            Recipient = recipient;
            Port = port;
            RecipientType = recipientType;
        }

        public string Recipient { get;set; }
        public int Port { get;set; }
        public string RecipientType { get;set; }
    }

    public class BlackberryMessageContent
    {

        public BlackberryMessageContent(string contentType, string content)
        {
            this.Headers = new Dictionary<string, string>();
            this.ContentType = contentType;
            this.Content = Encoding.UTF8.GetBytes(content);
        }

        public BlackberryMessageContent(string content)
        {
            this.Headers = new Dictionary<string, string>();
            this.ContentType = "text/plain";
            this.Content = Encoding.UTF8.GetBytes(content);
        }

        public BlackberryMessageContent(string contentType, byte[] content)
        {
            this.Headers = new Dictionary<string, string>();
            this.ContentType = contentType;
            this.Content = content;
        }

        public string ContentType { get; private set; }
        public byte[] Content { get; private set; }

        public Dictionary<string, string> Headers { get; private set; } 
    }
}

