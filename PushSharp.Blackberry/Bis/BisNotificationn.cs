using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace PushSharp.Blackberry
{
    public enum NotificationType
    {
        Text,
        Html//,
        //JpegImage
    }

    public class BisNotification : Core.Notification
	{
		public BisNotification()
		{
            DeviceToken = string.Empty;
		    Message = string.Empty;
            Payload = new Dictionary<string, string>();
		}

        public NotificationType NotificationType { get; set; }

        public string DeviceToken { get; set; }

		public string Message { get; set; }

        public Dictionary<string, string> Payload { get; set; }

        public string ContentType
        {
            get { 
                switch (NotificationType)
                {
                    case NotificationType.Html:
                        return "text/html";
                    //case BISNotificationType.JpegImage:
                        //return "image/jpeg";
                    default:
                        return "text/plain";
                }
            }
        }

        //public byte[] JpegImage;

        //public string GetJpegImageBytes()
        //{
        //    return JpegImage == null ? string.Empty : Encoding.Default.GetString(JpegImage);
        //}

        public string PayloadToString()
	    {
	        var payLoadStr=new StringBuilder();
	        if (Payload == null)
	            return string.Empty;

	        foreach (var payLoadItem in Payload)
	        {
	            payLoadStr.AppendLine(payLoadItem.Key + ":" + payLoadItem.Value);
	        }
            return payLoadStr.ToString();
	    }
	}
}
