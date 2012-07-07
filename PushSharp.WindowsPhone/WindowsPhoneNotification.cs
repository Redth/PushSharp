using System;
using System.Text;
using System.Xml.Linq;

namespace PushSharp.WindowsPhone
{
	public enum WindowsPhoneDeviceOSVersion
	{
		Seven = 7,
		MangoSevenPointFive = 8,
		Eight = 9
	}

	public enum BatchingInterval
	{
		Immediate,
		Medium,
		Slow
	}

	public enum NotificationType
	{
		Tile,
		Toast,
		Raw
	}

	public class WindowsPhoneNotificationFactory : Common.Notification
	{
		public WindowsPhoneRawNotification Raw()
		{
			return new WindowsPhoneRawNotification();
		}

		public WindowsPhoneTileNotification Tile()
		{
			return new WindowsPhoneTileNotification();
		}

		public WindowsPhoneToastNotification Toast()
		{
			return new WindowsPhoneToastNotification();
		}
	}

	public abstract class WindowsPhoneNotification : Common.Notification
	{
	    protected WindowsPhoneNotification()
		{
			this.Platform = Common.PlatformType.WindowsPhone;
			this.MessageID = Guid.NewGuid();
		}

		public string EndPointUrl { get; set; }

		public Guid MessageID { get; set; }

		public BatchingInterval? NotificationClass { get; set; } //Batching interval   2 = immediate, 12 = 450 seconds, 22 = 900 seconds

		public NotificationType NotificationType { get; set; } //Tile, toast, raw  (raw default)

		public string CallbackURI { get; set; }

		public abstract string PayloadToString();

		public override string ToString()
		{
			return PayloadToString();
		}

		public WindowsPhoneDeviceOSVersion OSVersion { get; set; }

		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
		}

        protected XElement StringElement(XName tagName, string value)
        {
            if (null == tagName) return null;
            return string.IsNullOrEmpty(value) ? null : new XElement(tagName, XmlEncode(value));
        }
	}

	public class WindowsPhoneToastNotification : WindowsPhoneNotification
	{
		public WindowsPhoneToastNotification()
			: base()
		{
		}

		public string Text1 { get; set; }
		public string Text2 { get; set; }

		public string NavigatePath { get; set; }

		public System.Collections.Specialized.NameValueCollection Parameters { get; set; }

        private XElement ToastParam(XName tagName, string navigatePath, System.Collections.Specialized.NameValueCollection parameters)
        {
            if (OSVersion > WindowsPhoneDeviceOSVersion.Seven)
            {
                if (!string.IsNullOrEmpty(NavigatePath) || (null != Parameters && Parameters.Count > 0))
                {
                    var sb = new StringBuilder();

                    if (!string.IsNullOrEmpty(navigatePath))
                        sb.Append(XmlEncode("/" + navigatePath.TrimStart('/')));

                    if (parameters != null && parameters.Count > 0)
                    {
                        sb.Append("?");

                        foreach (string key in parameters.Keys)
                            sb.Append(XmlEncode(key + "=" + parameters[key]) + "&amp;");
                    }

                    return new XElement(tagName, sb.ToString());
                }
            }
            return null;
        }

		public override string PayloadToString()
		{
            XNamespace wp = "WPNotification";
            var notification = new XElement(wp + "Notification",
                new XAttribute(XNamespace.Xmlns + "wp", "WPNotification"),
                new XElement(wp + "Toast", 
                    StringElement(wp + "Text1", Text1),
                    StringElement(wp + "Text2", Text2),
                    ToastParam(wp + "Param", NavigatePath, Parameters)
                    ));

		    return notification.ToString();
		}
	}

	public class WindowsPhoneRawNotification : WindowsPhoneNotification
	{
		public WindowsPhoneRawNotification()
			: base()
		{
		}


		public string Raw { get; set; }

		public override string PayloadToString()
		{
			return Raw;
		}
	}

	public class WindowsPhoneTileNotification : WindowsPhoneNotification
	{
		public WindowsPhoneTileNotification()
			: base()
		{
		}

		public string TileId { get; set; } //Secondary tile id, leave blank for application tile

		public string BackgroundImage { get; set; }

		public int? Count { get; set; }
		public bool ClearCount { get; set; }

		public string Title { get; set; }
		public bool ClearTitle { get; set; }

		public string BackBackgroundImage { get; set; }
		public bool ClearBackBackgroundImage { get; set; }

		public string BackTitle { get; set; }
		public bool ClearBackTitle { get; set; }

		public string BackContent { get; set; }
		public bool ClearBackContent { get; set; }

        private XElement ClearAttribute(XName tagName, bool clear, bool back, object value)
        {
            if (null == tagName) return null;

            if (clear)
                return new XElement(tagName, new XAttribute("Action", "Clear"));

            if (value is int?) //Count
            {
                return new XElement(tagName, XmlEncode(string.Format("{0}", value as int?)));
            }
            if (value is string) //Others
            {
                var strVal = value as string;
                if (!string.IsNullOrEmpty(strVal) && !(back && OSVersion > WindowsPhoneDeviceOSVersion.Seven))
                    return new XElement(tagName, XmlEncode(strVal));
            }
            return null;
        }

		public override string PayloadToString()
		{
            XNamespace wp = "WPNotification";
            var notification = new XElement(wp + "Notification",
                new XAttribute(XNamespace.Xmlns + "wp", "WPNotification"),
                new XElement(wp + "Tile", 
                    OSVersion > WindowsPhoneDeviceOSVersion.Seven ? 
                        (!string.IsNullOrEmpty(TileId) ? new XAttribute("Id", XmlEncode(TileId)) : null ) : null,
                    StringElement(wp + "BackgroundImage", BackgroundImage),
                    ClearAttribute(wp + "Count", ClearCount, false, Count),
                    ClearAttribute(wp + "Title", ClearTitle, false, Title),
                    ClearAttribute(wp + "BackBackgroundImage", ClearBackBackgroundImage, true, BackBackgroundImage),
                    ClearAttribute(wp + "BackTitle", ClearBackTitle, true, BackTitle),
                    ClearAttribute(wp + "BackContent", ClearBackContent, true, BackContent)
                    ));

            return notification.ToString();
		}
	}
}
