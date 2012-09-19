using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
	public abstract class WindowsNotification : Common.Notification
	{
		protected WindowsNotification()
		{
			this.Platform = Common.PlatformType.Windows;
			
		}

		public string ChannelUri { get; set; }

		public bool? RequestForStatus { get; set; }
		public int? TimeToLive { get; set; }
		

		public abstract string PayloadToString();

		public abstract WindowsNotificationType Type { get; }

		protected string XmlEncode(string text)
		{
			return System.Security.SecurityElement.Escape(text);
		}
	}

	public class WindowsTileNotification : WindowsNotification
	{
		public WindowsTileNotification()
			: base()
		{
			this.Texts = new List<string>();
			this.Images = new Dictionary<string, string>();
			this.TileTemplate = TileNotificationTemplate.TileSquareBlock;
		}

		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Tile; }
		}

		public WindowsNotificationCachePolicyType? CachePolicy { get; set; }
		public string NotificationTag { get; set; }

		public TileNotificationTemplate TileTemplate { get; set; }
		public Dictionary<string, string> Images { get; set; }
		public List<string> Texts { get; set; }

		public override string PayloadToString()
		{
			var xml = new StringBuilder();

			xml.Append("<tile>");
			xml.Append("<visual>");
			xml.AppendFormat("<binding template=\"{0}\">", this.TileTemplate.ToString());

			int idOn = 1;

			foreach (var imgSrc in Images.Keys)
			{
				var alt = Images[imgSrc];

				if (!string.IsNullOrEmpty(alt))
					xml.AppendFormat("<image id=\"{0}\" src=\"{1}\" alt=\"{2}\"/>", idOn, XmlEncode(imgSrc), XmlEncode(alt));
				else
					xml.AppendFormat("<image id=\"{0}\" src=\"{1}\"/>", idOn, XmlEncode(imgSrc));

				idOn++;
			}

			idOn = 1;

			foreach (var text in Texts)
			{
				xml.AppendFormat("<text id=\"{0}\">{1}</text>", idOn, XmlEncode(text));
				idOn++;
			}

			xml.Append("</binding>");
			xml.Append("</visual>");
			xml.Append("</tile>");

			return xml.ToString();
		}
	}

	public class WindowsToastNotification : WindowsNotification
	{
		public WindowsToastNotification()
			: base()
		{
			this.Texts = new List<string>();
			this.Images = new Dictionary<string, string>();
			this.TextTemplate = ToastNotificationTemplate.ToastImageAndText01;
		}

		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Toast; }
		}

		public ToastNotificationTemplate TextTemplate { get; set; }
		public Dictionary<string, string> Images { get; set; }
		public List<string> Texts { get; set; }

		public override string PayloadToString()
		{
			var xml = new StringBuilder();

			xml.Append("<toast>");
			xml.Append("<visual>");
			xml.AppendFormat("<binding template=\"{0}\">", this.TextTemplate.ToString());

			int idOn = 1;

			foreach (var imgSrc in Images.Keys)
			{
				var alt = Images[imgSrc];

				if (!string.IsNullOrEmpty(alt))
					xml.AppendFormat("<image id=\"{0}\" src=\"{1}\" alt=\"{2}\"/>", idOn, XmlEncode(imgSrc), XmlEncode(alt));
				else
					xml.AppendFormat("<image id=\"{0}\" src=\"{1}\"/>", idOn, XmlEncode(imgSrc));

				idOn++;
			}

			idOn = 1;

			foreach (var text in Texts)
			{
				xml.AppendFormat("<text id=\"{0}\">{1}</text>", idOn, XmlEncode(text));
				idOn++;
			}

			xml.Append("</binding>");
			xml.Append("</visual>");
			xml.Append("</toast>");

			return xml.ToString();
		}
	}

	public class WindowsBadgeNotification : WindowsNotification
	{
		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Badge; }
		}

		public WindowsNotificationCachePolicyType? CachePolicy { get; set; }

		public override string PayloadToString()
		{
			throw new NotImplementedException();
		}
	}

	public class WindowsRawNotification : WindowsNotification
	{
		public override WindowsNotificationType Type
		{
			get { return WindowsNotificationType.Raw; }
		}

		public string RawXml { get; set; }

		public override string PayloadToString()
		{
			return RawXml;
		}
	}

	public enum WindowsNotificationCachePolicyType
	{
		Cache,
		NoCache
	}

	public enum WindowsNotificationType
	{
		Badge,
		Tile,
		Toast,
		Raw
	}

	public enum ToastNotificationTemplate
	{
		ToastText01,
		ToastText02,
		ToastText03,
		ToastText04,
		ToastImageAndText01,
		ToastImageAndText02,
		ToastImageAndText03,
		ToastImageAndText04
	}
	
}
