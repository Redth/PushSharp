using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		public WindowsPhoneNotification()
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

		public override string PayloadToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			sb.AppendLine("<wp:Notification xmlns:wp=\"WPNotification\">");
			sb.AppendLine("<wp:Toast>");

			if (!string.IsNullOrEmpty(Text1))
				sb.AppendLine("<wp:Text1>" + XmlEncode(Text1) + "</wp:Text1>");

			if (!string.IsNullOrEmpty(Text2))
				sb.AppendLine("<wp:Text2>" + XmlEncode(Text2) + "</wp:Text2>");
			
			if (this.OSVersion > WindowsPhoneDeviceOSVersion.Seven)
			{
				if (!string.IsNullOrEmpty(NavigatePath) || (Parameters != null && Parameters.Count > 0))
				{
					sb.Append("<wp:Param>");

					if (!string.IsNullOrEmpty(NavigatePath))
						sb.Append(XmlEncode("/" + NavigatePath.TrimStart('/')));

					if (Parameters != null && Parameters.Count > 0)
					{
						sb.Append("?");

						foreach (string key in Parameters.Keys)
							sb.Append(XmlEncode(key + "=" + Parameters[key].ToString()) + "&amp;");
					}

					sb.AppendLine("</wp:Param>");
				}
			}

			sb.AppendLine("</wp:Toast>");
			sb.AppendLine("</wp:Notification>");

			return sb.ToString();
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

		public override string PayloadToString()
		{
			var sb = new StringBuilder();

			sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			sb.AppendLine("<wp:Notification xmlns:wp=\"WPNotification\">");

			sb.Append("<wp:Tile");

			if (this.OSVersion > WindowsPhoneDeviceOSVersion.Seven)
			{
				if (!string.IsNullOrEmpty(this.TileId))
					sb.Append(" Id=\"" + XmlEncode(this.TileId) + "\"");
			}

			sb.AppendLine(">");

			if (!string.IsNullOrEmpty(BackgroundImage))
				sb.AppendLine("<wp:BackgroundImage>" + XmlEncode(this.BackgroundImage) + "</wp:BackgroundImage>");

			if (ClearCount)
				sb.AppendLine("<wp:Count Action=\"Clear\"></wp:Count>");
			else if (Count.HasValue)
				sb.AppendLine("<wp:Count>" + Count.ToString() + "</wp:Count>");

			if (ClearTitle)
				sb.AppendLine("<wp:Title Action=\"Clear\"></wp:Title>");
			else if (!string.IsNullOrEmpty(Title))
				sb.AppendLine("<wp:Title>" + XmlEncode(Title) + "</wp:Title>");

			if (this.OSVersion > WindowsPhoneDeviceOSVersion.Seven)
			{
				if (ClearBackBackgroundImage)
					sb.AppendLine("<wp:BackBackgroundImage Action=\"Clear\"></wp:BackBackgroundImage>");
				else if (!string.IsNullOrEmpty(BackBackgroundImage))
					sb.AppendLine("<wp:BackBackgroundImage>" + XmlEncode(BackBackgroundImage) + "</wp:BackBackgroundImage>");

				if (ClearBackTitle)
					sb.AppendLine("<wp:BackTitle Action=\"Clear\"></wp:BackTitle>");
				else if (!string.IsNullOrEmpty(BackTitle))
					sb.AppendLine("<wp:BackTitle>" + XmlEncode(BackTitle) + "</wp:BackTitle>");

				if (ClearBackContent)
					sb.AppendLine("<wp:BackContent Action=\"Clear\"></wp:BackContent>");
				else if (!string.IsNullOrEmpty(BackContent))
					sb.AppendLine("<wp:BackContent>" + XmlEncode(BackContent) + "</wp:BackContent>");
			}

			sb.AppendLine("</wp:Tile>");
			sb.AppendLine("</wp:Notification>");

			return sb.ToString();
		}
	}
}
