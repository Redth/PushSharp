using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PushSharp.WindowsPhone;

namespace PushSharp
{
	public static class WindowsPhoneRawPushFluent
	{
		public static WindowsPhoneRawNotification ForEndpointUri(this WindowsPhoneRawNotification n, Uri endpointUri)
		{
			n.EndPointUrl = endpointUri.ToString();
			return n;
		}

		public static WindowsPhoneRawNotification WithCallbackUri(this WindowsPhoneRawNotification n, Uri callbackUri)
		{
			n.CallbackURI = callbackUri.ToString();
			return n;
		}

		public static WindowsPhoneRawNotification WithMessageID(this WindowsPhoneRawNotification n, Guid messageID)
		{
			n.MessageID = messageID;
			return n;
		}
		public static WindowsPhoneRawNotification WithBatchingInterval(this WindowsPhoneRawNotification n, BatchingInterval batchingInterval)
		{
			n.NotificationClass = batchingInterval;
			return n;
		}

		public static WindowsPhoneRawNotification ForOSVersion(this WindowsPhoneRawNotification n, WindowsPhoneDeviceOSVersion osVersion)
		{
			n.OSVersion = osVersion;
			return n;
		}

		public static WindowsPhoneRawNotification WithRaw(this WindowsPhoneRawNotification n, string raw)
		{
			n.Raw = raw;
			return n;
		}

        public static WindowsPhoneRawNotification WithTag(this WindowsPhoneRawNotification n, object tag)
        {
            n.Tag = tag;
            return n;
        }
	}

	public static class WindowsPhoneToastPushFluent
	{
		public static WindowsPhoneToastNotification ForEndpointUri(this WindowsPhoneToastNotification n, Uri endpointUri)
		{
			n.EndPointUrl = endpointUri.ToString();
			return n;
		}

		public static WindowsPhoneToastNotification WithCallbackUri(this WindowsPhoneToastNotification n, Uri callbackUri)
		{
			n.CallbackURI = callbackUri.ToString();
			return n;
		}

		public static WindowsPhoneToastNotification WithMessageID(this WindowsPhoneToastNotification n, Guid messageID)
		{
			n.MessageID = messageID;
			return n;
		}
		public static WindowsPhoneToastNotification WithBatchingInterval(this WindowsPhoneToastNotification n, BatchingInterval batchingInterval)
		{
			n.NotificationClass = batchingInterval;
			return n;
		}

		public static WindowsPhoneToastNotification ForOSVersion(this WindowsPhoneToastNotification n, WindowsPhoneDeviceOSVersion osVersion)
		{
			n.OSVersion = osVersion;
			return n;
		}

		public static WindowsPhoneToastNotification WithNavigatePath(this WindowsPhoneToastNotification n, string navigatePath)
		{
			n.NavigatePath = navigatePath;
			return n;
		}

		public static WindowsPhoneToastNotification WithText1(this WindowsPhoneToastNotification n, string text1)
		{
			n.Text1 = text1;
			return n;
		}

		public static WindowsPhoneToastNotification WithText2(this WindowsPhoneToastNotification n, string text2)
		{
			n.Text2 = text2;
			return n;
		}

		public static WindowsPhoneToastNotification WithParameter(this WindowsPhoneToastNotification n, string key, string value)
		{
			if (n.Parameters == null)
				n.Parameters = new System.Collections.Specialized.NameValueCollection();

			n.Parameters.Add(key, value);
			return n;
		}

        public static WindowsPhoneToastNotification WithTag(this WindowsPhoneToastNotification n, object tag)
        {
            n.Tag = tag;
            return n;
        }
	}

	public static class WindowsPhoneTilePushFluent
	{
		public static WindowsPhoneTileNotification ForEndpointUri(this WindowsPhoneTileNotification n, Uri endpointUri)
		{
			n.EndPointUrl = endpointUri.ToString();
			return n;
		}

		public static WindowsPhoneTileNotification WithCallbackUri(this WindowsPhoneTileNotification n, Uri callbackUri)
		{
			n.CallbackURI = callbackUri.ToString();
			return n;
		}

		public static WindowsPhoneTileNotification WithMessageID(this WindowsPhoneTileNotification n, Guid messageID)
		{
			n.MessageID = messageID;
			return n;
		}
		public static WindowsPhoneTileNotification WithBatchingInterval(this WindowsPhoneTileNotification n, BatchingInterval batchingInterval)
		{
			n.NotificationClass = batchingInterval;
			return n;
		}

		public static WindowsPhoneTileNotification ForOSVersion(this WindowsPhoneTileNotification n, WindowsPhoneDeviceOSVersion osVersion)
		{
			n.OSVersion = osVersion;
			return n;
		}

		public static WindowsPhoneTileNotification WithBackBackgroundImage(this WindowsPhoneTileNotification n, string backBackgroundImage)
		{
			n.BackBackgroundImage = backBackgroundImage;
			return n;
		}

		public static WindowsPhoneTileNotification WithBackgroundImage(this WindowsPhoneTileNotification n, string backgroundImage)
		{
			n.BackgroundImage = backgroundImage;
			return n;
		}

		public static WindowsPhoneTileNotification WithBackContent(this WindowsPhoneTileNotification n, string backContent)
		{
			n.BackContent = backContent;
			return n;
		}

		public static WindowsPhoneTileNotification WithBackTitle(this WindowsPhoneTileNotification n, string backTitle)
		{
			n.BackTitle = backTitle;
			return n;
		}

		public static WindowsPhoneTileNotification WithCount(this WindowsPhoneTileNotification n, int count)
		{
			n.Count = count;
			return n;
		}

		public static WindowsPhoneTileNotification WithTileId(this WindowsPhoneTileNotification n, string tileId)
		{
			n.TileId = tileId;
			return n;
		}

		public static WindowsPhoneTileNotification WithTitle(this WindowsPhoneTileNotification n, string title)
		{
			n.Title = title;
			return n;
		}

		public static WindowsPhoneTileNotification ClearBackBackgroundImage(this WindowsPhoneTileNotification n)
		{
			n.ClearBackBackgroundImage = true;
			return n;
		}

		public static WindowsPhoneTileNotification ClearBackContent(this WindowsPhoneTileNotification n)
		{
			n.ClearBackContent = true;
			return n;
		}

		public static WindowsPhoneTileNotification ClearBackTitle(this WindowsPhoneTileNotification n)
		{
			n.ClearBackTitle = true;
			return n;
		}

		public static WindowsPhoneTileNotification ClearCount(this WindowsPhoneTileNotification n)
		{
			n.ClearCount = true;
			return n;
		}

		public static WindowsPhoneTileNotification ClearTitle(this WindowsPhoneTileNotification n)
		{
			n.ClearTitle = true;
			return n;
		}

        public static WindowsPhoneTileNotification WithTag(this WindowsPhoneTileNotification n, object tag)
        {
            n.Tag = tag;
            return n;
        }
	}


	public static class WindowsPhoneFlipTilePushFluent
	{
		public static WindowsPhoneFlipTileNotification ForEndpointUri(this WindowsPhoneFlipTileNotification n, Uri endpointUri)
		{
			n.EndPointUrl = endpointUri.ToString();
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithCallbackUri(this WindowsPhoneFlipTileNotification n, Uri callbackUri)
		{
			n.CallbackURI = callbackUri.ToString();
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithMessageID(this WindowsPhoneFlipTileNotification n, Guid messageID)
		{
			n.MessageID = messageID;
			return n;
		}
		public static WindowsPhoneFlipTileNotification WithBatchingInterval(this WindowsPhoneFlipTileNotification n, BatchingInterval batchingInterval)
		{
			n.NotificationClass = batchingInterval;
			return n;
		}

		public static WindowsPhoneFlipTileNotification ForOSVersion(this WindowsPhoneFlipTileNotification n, WindowsPhoneDeviceOSVersion osVersion)
		{
			n.OSVersion = osVersion;
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithTitle(this WindowsPhoneFlipTileNotification n, string title)
		{
			n.Title = title;
			return n;
		}

        public static WindowsPhoneTileNotification WithTileId(this WindowsPhoneTileNotification n, string tileId)
        {
            n.TileId = tileId;
            return n;
        }

		public static WindowsPhoneFlipTileNotification WithCount(this WindowsPhoneFlipTileNotification n, int count)
		{
			n.Count = count;
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithBackTitle(this WindowsPhoneFlipTileNotification n, string backTitle)
		{
			n.BackTitle = backTitle;
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithBackContent(this WindowsPhoneFlipTileNotification n, string backContent)
		{
			n.BackContent = backContent;
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithWideBackContent(this WindowsPhoneFlipTileNotification n, string wideBackContent)
		{
			n.WideBackContent = wideBackContent;
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithSmallBackgroundImage(this WindowsPhoneFlipTileNotification n, string smallBackgroundImage)
		{
			n.SmallBackgroundImage = smallBackgroundImage;
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithBackgroundImage(this WindowsPhoneFlipTileNotification n, string backgroundImage)
		{
			n.BackgroundImage = backgroundImage;
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithBackBackgroundImage(this WindowsPhoneFlipTileNotification n, string backBackgroundImage)
		{
			n.BackBackgroundImage = backBackgroundImage;
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithWideBackgroundImage(this WindowsPhoneFlipTileNotification n, string wideBackgroundImage)
		{
			n.WideBackgroundImage = wideBackgroundImage;
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithWideBackBackgroundImage(this WindowsPhoneFlipTileNotification n, string wideBackBackgroundImage)
		{
			n.WideBackBackgroundImage = wideBackBackgroundImage;
			return n;
		}


		public static WindowsPhoneFlipTileNotification ClearTitle(this WindowsPhoneFlipTileNotification n)
		{
			n.ClearTitle = true;
			return n;
		}

		public static WindowsPhoneFlipTileNotification ClearBackTitle(this WindowsPhoneFlipTileNotification n)
		{
			n.ClearBackTitle = true;
			return n;
		}

		public static WindowsPhoneFlipTileNotification ClearBackContent(this WindowsPhoneFlipTileNotification n)
		{
			n.ClearBackContent = true;
			return n;
		}

		public static WindowsPhoneFlipTileNotification ClearWideBackContent(this WindowsPhoneFlipTileNotification n)
		{
			n.ClearWideBackContent = true;
			return n;
		}

		public static WindowsPhoneFlipTileNotification ClearCount(this WindowsPhoneFlipTileNotification n)
		{
			n.ClearCount = true;
			return n;
		}

		public static WindowsPhoneFlipTileNotification ClearSmallBackgroundImage(this WindowsPhoneFlipTileNotification n)
		{
			n.ClearSmallBackgroundImage = true;
			return n;
		}

		public static WindowsPhoneFlipTileNotification ClearBackgroundImage(this WindowsPhoneFlipTileNotification n)
		{
			n.ClearBackgroundImage = true;
			return n;
		}

		public static WindowsPhoneFlipTileNotification ClearBackBackgroundImage(this WindowsPhoneFlipTileNotification n)
		{
			n.ClearBackBackgroundImage = true;
			return n;
		}

		public static WindowsPhoneFlipTileNotification ClearWideBackgroundImage(this WindowsPhoneFlipTileNotification n)
		{
			n.ClearWideBackgroundImage = true;
			return n;
		}

		public static WindowsPhoneFlipTileNotification ClearWideBackBackgroundImage(this WindowsPhoneFlipTileNotification n)
		{
			n.ClearWideBackBackgroundImage = true;
			return n;
		}

		public static WindowsPhoneFlipTileNotification WithTag(this WindowsPhoneFlipTileNotification n, object tag)
		{
			n.Tag = tag;
			return n;
		}
	}
	
	public static class WindowsPhoneIconicTilePushFluent
	{
		public static WindowsPhoneIconicTileNotification ForEndpointUri(this WindowsPhoneIconicTileNotification n, Uri endpointUri)
		{
			n.EndPointUrl = endpointUri.ToString();
			return n;
		}

        public static WindowsPhoneTileNotification WithTileId(this WindowsPhoneTileNotification n, string tileId)
        {
            n.TileId = tileId;
            return n;
        }

		public static WindowsPhoneIconicTileNotification WithCallbackUri(this WindowsPhoneIconicTileNotification n, Uri callbackUri)
		{
			n.CallbackURI = callbackUri.ToString();
			return n;
		}

		public static WindowsPhoneIconicTileNotification WithMessageID(this WindowsPhoneIconicTileNotification n, Guid messageID)
		{
			n.MessageID = messageID;
			return n;
		}
		public static WindowsPhoneIconicTileNotification WithBatchingInterval(this WindowsPhoneIconicTileNotification n, BatchingInterval batchingInterval)
		{
			n.NotificationClass = batchingInterval;
			return n;
		}

		public static WindowsPhoneIconicTileNotification ForOSVersion(this WindowsPhoneIconicTileNotification n, WindowsPhoneDeviceOSVersion osVersion)
		{
			n.OSVersion = osVersion;
			return n;
		}

		public static WindowsPhoneIconicTileNotification WithTitle(this WindowsPhoneIconicTileNotification n, string title)
		{
			n.Title = title;
			return n;
		}

		public static WindowsPhoneIconicTileNotification WithCount(this WindowsPhoneIconicTileNotification n, int count)
		{
			n.Count = count;
			return n;
		}

		public static WindowsPhoneIconicTileNotification WithWideContent1(this WindowsPhoneIconicTileNotification n, string wideContent1)
		{
			n.WideContent1 = wideContent1;
			return n;
		}

		public static WindowsPhoneIconicTileNotification WithWideContent2(this WindowsPhoneIconicTileNotification n, string wideContent2)
		{
			n.WideContent2 = wideContent2;
			return n;
		}

		public static WindowsPhoneIconicTileNotification WithWideContent3(this WindowsPhoneIconicTileNotification n, string wideContent3)
		{
			n.WideContent3 = wideContent3;
			return n;
		}

		public static WindowsPhoneIconicTileNotification WithSmallIconImage(this WindowsPhoneIconicTileNotification n, string smallIconImage)
		{
			n.SmallIconImage = smallIconImage;
			return n;
		}

		public static WindowsPhoneIconicTileNotification WithIconImage(this WindowsPhoneIconicTileNotification n, string iconImage)
		{
			n.IconImage = iconImage;
			return n;
		}

		public static WindowsPhoneIconicTileNotification WithBackgroundColor(this WindowsPhoneIconicTileNotification n, string backgroundColor)
		{
			n.BackgroundColor = backgroundColor;
			return n;
		}
		
		public static WindowsPhoneIconicTileNotification ClearTitle(this WindowsPhoneIconicTileNotification n)
		{
			n.ClearTitle = true;
			return n;
		}

		public static WindowsPhoneIconicTileNotification ClearWideContent1(this WindowsPhoneIconicTileNotification n)
		{
			n.ClearWideContent1 = true;
			return n;
		}

		public static WindowsPhoneIconicTileNotification ClearWideContent2(this WindowsPhoneIconicTileNotification n)
		{
			n.ClearWideContent2 = true;
			return n;
		}

		public static WindowsPhoneIconicTileNotification ClearWideContent3(this WindowsPhoneIconicTileNotification n)
		{
			n.ClearWideContent3 = true;
			return n;
		}

		public static WindowsPhoneIconicTileNotification ClearCount(this WindowsPhoneIconicTileNotification n)
		{
			n.ClearCount = true;
			return n;
		}

		public static WindowsPhoneIconicTileNotification ClearSmallIconImage(this WindowsPhoneIconicTileNotification n)
		{
			n.ClearSmallIconImage = true;
			return n;
		}

		public static WindowsPhoneIconicTileNotification ClearIconImage(this WindowsPhoneIconicTileNotification n)
		{
			n.ClearIconImage = true;
			return n;
		}

		public static WindowsPhoneIconicTileNotification ClearBackgroundColor(this WindowsPhoneIconicTileNotification n)
		{
			n.ClearBackgroundColor = true;
			return n;
		}
		
		public static WindowsPhoneIconicTileNotification WithTag(this WindowsPhoneIconicTileNotification n, object tag)
		{
			n.Tag = tag;
			return n;
		}
	}

	public static class WindowsPhoneCycleTilePushFluent
	{
		public static WindowsPhoneCycleTileNotification ForEndpointUri(this WindowsPhoneCycleTileNotification n, Uri endpointUri)
		{
			n.EndPointUrl = endpointUri.ToString();
			return n;
		}

        public static WindowsPhoneTileNotification WithTileId(this WindowsPhoneTileNotification n, string tileId)
        {
            n.TileId = tileId;
            return n;
        }

		public static WindowsPhoneCycleTileNotification WithCallbackUri(this WindowsPhoneCycleTileNotification n, Uri callbackUri)
		{
			n.CallbackURI = callbackUri.ToString();
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithMessageID(this WindowsPhoneCycleTileNotification n, Guid messageID)
		{
			n.MessageID = messageID;
			return n;
		}
		public static WindowsPhoneCycleTileNotification WithBatchingInterval(this WindowsPhoneCycleTileNotification n, BatchingInterval batchingInterval)
		{
			n.NotificationClass = batchingInterval;
			return n;
		}

		public static WindowsPhoneCycleTileNotification ForOSVersion(this WindowsPhoneCycleTileNotification n, WindowsPhoneDeviceOSVersion osVersion)
		{
			n.OSVersion = osVersion;
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithTitle(this WindowsPhoneCycleTileNotification n, string title)
		{
			n.Title = title;
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithCount(this WindowsPhoneCycleTileNotification n, int count)
		{
			n.Count = count;
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithCycleImage1(this WindowsPhoneCycleTileNotification n, string cycleImage1)
		{
			n.CycleImage1 = cycleImage1;
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithCycleImage2(this WindowsPhoneCycleTileNotification n, string cycleImage2)
		{
			n.CycleImage2 = cycleImage2;
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithCycleImage3(this WindowsPhoneCycleTileNotification n, string cycleImage3)
		{
			n.CycleImage3 = cycleImage3;
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithCycleImage4(this WindowsPhoneCycleTileNotification n, string cycleImage4)
		{
			n.CycleImage4 = cycleImage4;
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithCycleImage5(this WindowsPhoneCycleTileNotification n, string cycleImage5)
		{
			n.CycleImage5 = cycleImage5;
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithCycleImage6(this WindowsPhoneCycleTileNotification n, string cycleImage6)
		{
			n.CycleImage6 = cycleImage6;
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithCycleImage7(this WindowsPhoneCycleTileNotification n, string cycleImage7)
		{
			n.CycleImage6 = cycleImage7;
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithCycleImage8(this WindowsPhoneCycleTileNotification n, string cycleImage8)
		{
			n.CycleImage6 = cycleImage8;
			return n;
		}

		public static WindowsPhoneCycleTileNotification WithCycleImage9(this WindowsPhoneCycleTileNotification n, string cycleImage9)
		{
			n.CycleImage6 = cycleImage9;
			return n;
		}

		public static WindowsPhoneCycleTileNotification ClearTitle(this WindowsPhoneCycleTileNotification n)
		{
			n.ClearTitle = true;
			return n;
		}

		public static WindowsPhoneCycleTileNotification ClearCount(this WindowsPhoneCycleTileNotification n)
		{
			n.ClearCount = true;
			return n;
		}
		
		public static WindowsPhoneCycleTileNotification ClearCycleImage1(this WindowsPhoneCycleTileNotification n)
		{
			n.ClearCycleImage1 = true;
			return n;
		}

		public static WindowsPhoneCycleTileNotification ClearCycleImage2(this WindowsPhoneCycleTileNotification n)
		{
			n.ClearCycleImage2 = true;
			return n;
		}

		public static WindowsPhoneCycleTileNotification ClearCycleImage3(this WindowsPhoneCycleTileNotification n)
		{
			n.ClearCycleImage3 = true;
			return n;
		}

		public static WindowsPhoneCycleTileNotification ClearCycleImage4(this WindowsPhoneCycleTileNotification n)
		{
			n.ClearCycleImage4 = true;
			return n;
		}

		public static WindowsPhoneCycleTileNotification ClearCycleImage5(this WindowsPhoneCycleTileNotification n)
		{
			n.ClearCycleImage5 = true;
			return n;
		}

		public static WindowsPhoneCycleTileNotification ClearCycleImage6(this WindowsPhoneCycleTileNotification n)
		{
			n.ClearCycleImage6 = true;
			return n;
		}

		public static WindowsPhoneCycleTileNotification ClearCycleImage7(this WindowsPhoneCycleTileNotification n)
		{
			n.ClearCycleImage7 = true;
			return n;
		}

		public static WindowsPhoneCycleTileNotification ClearCycleImage8(this WindowsPhoneCycleTileNotification n)
		{
			n.ClearCycleImage8 = true;
			return n;
		}

		public static WindowsPhoneCycleTileNotification ClearCycleImage9(this WindowsPhoneCycleTileNotification n)
		{
			n.ClearCycleImage9 = true;
			return n;
		}
		public static WindowsPhoneCycleTileNotification WithTag(this WindowsPhoneCycleTileNotification n, object tag)
		{
			n.Tag = tag;
			return n;
		}
	}
}
