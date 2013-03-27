using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using PushSharp.Core;

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

	public class WindowsPhoneNotificationFactory : Notification
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

		public WindowsPhoneCycleTileNotification CycleTile()
		{
			return new WindowsPhoneCycleTileNotification();
		}

		public WindowsPhoneFlipTileNotification FlipTile()
		{
			return new WindowsPhoneFlipTileNotification();
		}

		public WindowsPhoneIconicTileNotification IconicTile()
		{
			return new WindowsPhoneIconicTileNotification();
		}
	}

	public abstract class WindowsPhoneNotification : Notification
	{
		protected WindowsPhoneNotification()
		{
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
		{
			NotificationType = NotificationType.Toast;
		}

		public string Text1 { get; set; }
		public string Text2 { get; set; }

		public string NavigatePath { get; set; }

		public System.Collections.Specialized.NameValueCollection Parameters { get; set; }

		public override string PayloadToString()
		{
			XNamespace wp = "WPNotification";
			var notification = new XElement(wp + "Notification", new XAttribute(XNamespace.Xmlns + "wp", "WPNotification"));

			var toast = new XElement(wp + "Toast");

			if (!string.IsNullOrEmpty(Text1))
				toast.Add(new XElement(wp + "Text1", Text1));

			if (!string.IsNullOrEmpty(Text2))
				toast.Add(new XElement(wp + "Text2", Text2));


			if (this.OSVersion > WindowsPhoneDeviceOSVersion.Seven)
			{
				if (!string.IsNullOrEmpty(NavigatePath) || (Parameters != null && Parameters.Count > 0))
				{
					var sb = new StringBuilder();

					if (!string.IsNullOrEmpty(NavigatePath))
						sb.Append(XmlEncode("/" + NavigatePath.TrimStart('/')));

					if (Parameters != null && Parameters.Count > 0)
					{
						sb.Append("?");

						foreach (string key in Parameters.Keys)
							sb.Append(XmlEncode(key + "=" + Parameters[key].ToString()) + "&amp;");
					}

					var paramValue = sb.ToString();

					if (!string.IsNullOrEmpty(paramValue) && paramValue.EndsWith("&amp;"))
						paramValue.Substring(0, paramValue.Length - "&amp;".Length);

					if (!string.IsNullOrEmpty(paramValue))
						toast.Add(new XElement(wp + "Param", paramValue));
				}
			}

			notification.Add(toast);
			return notification.ToString();
		}
	}

	public class WindowsPhoneRawNotification : WindowsPhoneNotification
	{
		public WindowsPhoneRawNotification()
		{
			NotificationType = NotificationType.Raw;
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
		{
			NotificationType = NotificationType.Tile;
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
			XNamespace wp = "WPNotification";
			var notification = new XElement(wp + "Notification", new XAttribute(XNamespace.Xmlns + "wp", "WPNotification"));

			var tile = new XElement(wp + "Tile");
			
			if (this.OSVersion > WindowsPhoneDeviceOSVersion.Seven && !string.IsNullOrEmpty(this.TileId))
				tile.Add(new XAttribute("Id", XmlEncode(this.TileId)));

			if (!string.IsNullOrEmpty(BackgroundImage))
				tile.Add(new XElement(wp + "BackgroundImage", XmlEncode(BackgroundImage)));
			
			if (ClearCount)
				tile.Add(new XElement(wp + "Count", new XAttribute("Action", "Clear")));
			else if (Count.HasValue)
				tile.Add(new XElement(wp + "Count", XmlEncode(Count.ToString())));

			if (ClearTitle)
				tile.Add(new XElement(wp + "Title", new XAttribute("Action", "Clear")));
			else if (!string.IsNullOrEmpty(Title))
				tile.Add(new XElement(wp + "Title", XmlEncode(Title)));
			
			if (OSVersion > WindowsPhoneDeviceOSVersion.Seven)
			{
				if (ClearBackTitle)
					tile.Add(new XElement(wp + "BackTitle", new XAttribute("Action", "Clear")));
                else if (!string.IsNullOrEmpty(BackTitle))
					tile.Add(new XElement(wp + "BackTitle", XmlEncode(BackTitle)));

				if (ClearBackBackgroundImage)
					tile.Add(new XElement(wp + "BackBackgroundImage", new XAttribute("Action", "Clear")));
                else if (!string.IsNullOrEmpty(BackBackgroundImage))
					tile.Add(new XElement(wp + "BackBackgroundImage", XmlEncode(BackBackgroundImage)));

				if (ClearBackContent)
					tile.Add(new XElement(wp + "BackContent", new XAttribute("Action", "Clear")));
                else if (!string.IsNullOrEmpty(BackContent))
					tile.Add(new XElement(wp + "BackContent", XmlEncode(BackContent)));
			
			}

			notification.Add(tile);
			return notification.ToString();
		}
	}

	public class WindowsPhoneFlipTileNotification : WindowsPhoneNotification
	{
		public WindowsPhoneFlipTileNotification()
		{
			NotificationType = NotificationType.Tile;
		}

		public string Title { get; set; }
		public bool ClearTitle { get; set; }

	    public string TileId { get; set; }

		public string BackTitle { get; set; }
		public bool ClearBackTitle { get; set; }

		public string BackContent { get; set; }
		public bool ClearBackContent { get; set; }

		public string WideBackContent { get; set; }
		public bool ClearWideBackContent { get; set; }

		public int? Count { get; set; }
		public bool ClearCount { get; set; }

		public string SmallBackgroundImage { get; set; }
		public bool ClearSmallBackgroundImage { get; set; }

		public string BackgroundImage { get; set; }
		public bool ClearBackgroundImage { get; set; }

		public string BackBackgroundImage { get; set; }
		public bool ClearBackBackgroundImage { get; set; }

		public string WideBackgroundImage { get; set; }
		public bool ClearWideBackgroundImage { get; set; }

		public string WideBackBackgroundImage { get; set; }
		public bool ClearWideBackBackgroundImage { get; set; }

		public override string PayloadToString()
		{
			XNamespace wp = "WPNotification";
			var notification = new XElement(wp + "Notification", new XAttribute(XNamespace.Xmlns + "wp", "WPNotification"));

			var tile = new XElement(wp + "Tile", new XAttribute("Template", "FlipTile"));

            if (!string.IsNullOrEmpty(this.TileId))
                tile.Add(new XAttribute("Id", XmlEncode(this.TileId)));

			if (ClearTitle)
				tile.Add(new XElement(wp + "Title", new XAttribute("Action", "Clear")));
			else if (!string.IsNullOrEmpty(Title))
				tile.Add(new XElement(wp + "Title", XmlEncode(Title)));

			if (ClearBackTitle)
				tile.Add(new XElement(wp + "BackTitle", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(BackTitle))
				tile.Add(new XElement(wp + "BackTitle", XmlEncode(BackTitle)));
			
			if (ClearBackContent)
				tile.Add(new XElement(wp + "BackContent", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(BackContent))
				tile.Add(new XElement(wp + "BackContent", XmlEncode(BackContent)));
			
			if (ClearWideBackContent)
				tile.Add(new XElement(wp + "WideBackContent", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(WideBackContent))
				tile.Add(new XElement(wp + "WideBackContent", XmlEncode(WideBackContent)));
			
			if (ClearCount)
				tile.Add(new XElement(wp + "Count", new XAttribute("Action", "Clear")));
			else if (Count.HasValue)
				tile.Add(new XElement(wp + "Count", XmlEncode(Count.Value.ToString())));
			
			if (ClearSmallBackgroundImage)
				tile.Add(new XElement(wp + "SmallBackgroundImage", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(SmallBackgroundImage))
				tile.Add(new XElement(wp + "SmallBackgroundImage", XmlEncode(SmallBackgroundImage)));
			
			if (ClearBackgroundImage)
				tile.Add(new XElement(wp + "BackgroundImage", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(BackgroundImage))
				tile.Add(new XElement(wp + "BackgroundImage", XmlEncode(BackgroundImage)));
			
			if (ClearBackBackgroundImage)
				tile.Add(new XElement(wp + "BackBackgroundImage", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(BackBackgroundImage))
				tile.Add(new XElement(wp + "BackBackgroundImage", XmlEncode(BackBackgroundImage)));
			
			if (ClearWideBackgroundImage)
				tile.Add(new XElement(wp + "WideBackgroundImage", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(WideBackgroundImage))
				tile.Add(new XElement(wp + "WideBackgroundImage", XmlEncode(WideBackgroundImage)));
			
			if (ClearWideBackBackgroundImage)
				tile.Add(new XElement(wp + "WideBackBackgroundImage", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(WideBackBackgroundImage))
				tile.Add(new XElement(wp + "WideBackBackgroundImage", XmlEncode(WideBackBackgroundImage)));

			notification.Add(tile);

			return notification.ToString();
		}
	}
	
	public class WindowsPhoneIconicTileNotification : WindowsPhoneNotification
	{
		public WindowsPhoneIconicTileNotification()
		{
			NotificationType = NotificationType.Tile;
		}

		public string Title { get; set; }
		public bool ClearTitle { get; set; }

	    public string TileId { get; set; }

		public string WideContent1 { get; set; }
		public bool ClearWideContent1 { get; set; }

		public string WideContent2 { get; set; }
		public bool ClearWideContent2 { get; set; }

		public string WideContent3 { get; set; }
		public bool ClearWideContent3 { get; set; }

		public int? Count { get; set; }
		public bool ClearCount { get; set; }

		public string SmallIconImage { get; set; }
		public bool ClearSmallIconImage { get; set; }

		public string IconImage { get; set; }
		public bool ClearIconImage { get; set; }

		public string BackgroundColor { get; set; }
		public bool ClearBackgroundColor { get; set; }

		
		public override string PayloadToString()
		{
			XNamespace wp = "WPNotification";
			var notification = new XElement(wp + "Notification", new XAttribute(XNamespace.Xmlns + "wp", "WPNotification"));

			var tile = new XElement(wp + "Tile", new XAttribute("Template", "IconicTile"));

            if (!string.IsNullOrEmpty(this.TileId))
                tile.Add(new XAttribute("Id", XmlEncode(this.TileId)));

			if (ClearTitle)
				tile.Add(new XElement(wp + "Title", new XAttribute("Action", "Clear")));
			else if (!string.IsNullOrEmpty(Title))
				tile.Add(new XElement(wp + "Title", XmlEncode(Title)));

			if (ClearWideContent1)
				tile.Add(new XElement(wp + "WideContent1", new XAttribute("Action", "Clear")));
			else if (!string.IsNullOrEmpty(WideContent1))
				tile.Add(new XElement(wp + "WideContent1", XmlEncode(WideContent1)));

			if (ClearWideContent2)
				tile.Add(new XElement(wp + "WideContent2", new XAttribute("Action", "Clear")));
			else if (!string.IsNullOrEmpty(WideContent2))
				tile.Add(new XElement(wp + "WideContent2", XmlEncode(WideContent2)));

			if (ClearWideContent3)
				tile.Add(new XElement(wp + "WideContent3", new XAttribute("Action", "Clear")));
			else if (!string.IsNullOrEmpty(WideContent3))
				tile.Add(new XElement(wp + "WideContent3", XmlEncode(WideContent3)));

			if (ClearCount)
				tile.Add(new XElement(wp + "Count", new XAttribute("Action", "Clear")));
			else if (Count.HasValue)
				tile.Add(new XElement(wp + "Count", XmlEncode(Count.Value.ToString())));

			if (ClearSmallIconImage)
				tile.Add(new XElement(wp + "SmallIconImage", new XAttribute("Action", "Clear")));
			else if (!string.IsNullOrEmpty(SmallIconImage))
				tile.Add(new XElement(wp + "SmallIconImage", XmlEncode(SmallIconImage)));

			if (ClearIconImage)
				tile.Add(new XElement(wp + "IconImage", new XAttribute("Action", "Clear")));
			else if (!string.IsNullOrEmpty(IconImage))
				tile.Add(new XElement(wp + "IconImage", XmlEncode(IconImage)));

			if (ClearBackgroundColor)
				tile.Add(new XElement(wp + "BackgroundColor", new XAttribute("Action", "Clear")));
			else if (!string.IsNullOrEmpty(BackgroundColor))
				tile.Add(new XElement(wp + "BackgroundColor", XmlEncode(BackgroundColor)));

			notification.Add(tile);

			return notification.ToString();
		}
	}

	public class WindowsPhoneCycleTileNotification : WindowsPhoneNotification
	{
		public WindowsPhoneCycleTileNotification()
		{
			NotificationType = NotificationType.Tile;
		}

		public string Title { get; set; }
		public bool ClearTitle { get; set; }

		public int? Count { get; set; }
		public bool ClearCount { get; set; }

	    public string TileId { get; set; }

		public string CycleImage1 { get; set; }
		public bool ClearCycleImage1 { get; set; }

		public string CycleImage2 { get; set; }
		public bool ClearCycleImage2 { get; set; }

		public string CycleImage3 { get; set; }
		public bool ClearCycleImage3 { get; set; }
		
		public string CycleImage4 { get; set; }
		public bool ClearCycleImage4 { get; set; }

		public string CycleImage5 { get; set; }
		public bool ClearCycleImage5 { get; set; }

		public string CycleImage6 { get; set; }
		public bool ClearCycleImage6 { get; set; }

		public string CycleImage7 { get; set; }
		public bool ClearCycleImage7 { get; set; }

		public string CycleImage8 { get; set; }
		public bool ClearCycleImage8 { get; set; }

		public string CycleImage9 { get; set; }
		public bool ClearCycleImage9 { get; set; }

		public override string PayloadToString()
		{
			XNamespace wp = "WPNotification";
			var notification = new XElement(wp + "Notification", new XAttribute(XNamespace.Xmlns + "wp", "WPNotification"));

			var tile = new XElement(wp + "Tile", new XAttribute("Template", "CycleTile"));

            if (!string.IsNullOrEmpty(this.TileId))
                tile.Add(new XAttribute("Id", XmlEncode(this.TileId)));

			if (ClearTitle)
				tile.Add(new XElement(wp + "Title", new XAttribute("Action", "Clear")));
			else if (!string.IsNullOrEmpty(Title))
				tile.Add(new XElement(wp + "Title", XmlEncode(Title)));

			if (ClearCount)
				tile.Add(new XElement(wp + "Count", new XAttribute("Action", "Clear")));
			else if (Count.HasValue)
				tile.Add(new XElement(wp + "Count", XmlEncode(Count.Value.ToString())));
			
			if (ClearCycleImage1)
				tile.Add(new XElement(wp + "CycleImage1", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(CycleImage1))
				tile.Add(new XElement(wp + "CycleImage1", XmlEncode(CycleImage1)));

			if (ClearCycleImage2)
				tile.Add(new XElement(wp + "CycleImage2", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(CycleImage2))
				tile.Add(new XElement(wp + "CycleImage2", XmlEncode(CycleImage2)));
			
			if (ClearCycleImage3)
				tile.Add(new XElement(wp + "CycleImage3", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(CycleImage3))
				tile.Add(new XElement(wp + "CycleImage3", XmlEncode(CycleImage3)));
			
			if (ClearCycleImage4)
				tile.Add(new XElement(wp + "CycleImage4", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(CycleImage4))
				tile.Add(new XElement(wp + "CycleImage4", XmlEncode(CycleImage4)));
			
			if (ClearCycleImage5)
				tile.Add(new XElement(wp + "CycleImage5", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(CycleImage5))
				tile.Add(new XElement(wp + "CycleImage5", XmlEncode(CycleImage5)));
		
			if (ClearCycleImage6)
				tile.Add(new XElement(wp + "CycleImage6", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(CycleImage6))
				tile.Add(new XElement(wp + "CycleImage6", XmlEncode(CycleImage6)));
			
			if (ClearCycleImage7)
				tile.Add(new XElement(wp + "CycleImage7", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(CycleImage7))
				tile.Add(new XElement(wp + "CycleImage7", XmlEncode(CycleImage7)));
			
			if (ClearCycleImage8)
				tile.Add(new XElement(wp + "CycleImage8", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(CycleImage8))
				tile.Add(new XElement(wp + "CycleImage8", XmlEncode(CycleImage8)));
			
			if (ClearCycleImage9)
				tile.Add(new XElement(wp + "CycleImage9", new XAttribute("Action", "Clear")));
            else if (!string.IsNullOrEmpty(CycleImage9))
				tile.Add(new XElement(wp + "CycleImage9", XmlEncode(CycleImage9)));

			notification.Add(tile);

			return notification.ToString();
		}
	}
}
