using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace PushSharp.Blackberry
{
    public enum BISNotificationType
    {
        Text,
        Html//,
        //JpegImage
    }

    public class BISNotification : Core.Notification
	{
		public BISNotification()
		{

            DeviceToken = string.Empty;
		    Message = string.Empty;
            PayLoad=new Dictionary<string, string>();
		}

        public BISNotificationType BISNoType { get; set; }

        public string DeviceToken { get; set; }

		public string Message { get; set; }

        public Dictionary<string, string> PayLoad { get; set; }

        public string BISContentType
        {
            get { 
                switch (BISNoType)
                {
                    case BISNotificationType.Html:
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

        public string PayLoadToString()
	    {
	        var payLoadStr=new StringBuilder();
	        if (PayLoad == null)
	            return string.Empty;

	        foreach (var payLoadItem in PayLoad)
	        {
	            payLoadStr.AppendLine(payLoadItem.Key + ":" + payLoadItem.Value);
	        }
            return payLoadStr.ToString();
	    }
	}
}
