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
		public static WindowsPhoneFlipTile ForEndpointUri(this WindowsPhoneFlipTile n, Uri endpointUri)
		{
			n.EndPointUrl = endpointUri.ToString();
			return n;
		}

		public static WindowsPhoneFlipTile WithCallbackUri(this WindowsPhoneFlipTile n, Uri callbackUri)
		{
			n.CallbackURI = callbackUri.ToString();
			return n;
		}

		public static WindowsPhoneFlipTile WithMessageID(this WindowsPhoneFlipTile n, Guid messageID)
		{
			n.MessageID = messageID;
			return n;
		}
		public static WindowsPhoneFlipTile WithBatchingInterval(this WindowsPhoneFlipTile n, BatchingInterval batchingInterval)
		{
			n.NotificationClass = batchingInterval;
			return n;
		}

		public static WindowsPhoneFlipTile ForOSVersion(this WindowsPhoneFlipTile n, WindowsPhoneDeviceOSVersion osVersion)
		{
			n.OSVersion = osVersion;
			return n;
		}

		public static WindowsPhoneFlipTile WithTitle(this WindowsPhoneFlipTile n, string title)
		{
			n.Title = title;
			return n;
		}

        public static WindowsPhoneTileNotification WithTileId(this WindowsPhoneTileNotification n, string tileId)
        {
            n.TileId = tileId;
            return n;
        }

		public static WindowsPhoneFlipTile WithCount(this WindowsPhoneFlipTile n, int count)
		{
			n.Count = count;
			return n;
		}

		public static WindowsPhoneFlipTile WithBackTitle(this WindowsPhoneFlipTile n, string backTitle)
		{
			n.BackTitle = backTitle;
			return n;
		}

		public static WindowsPhoneFlipTile WithBackContent(this WindowsPhoneFlipTile n, string backContent)
		{
			n.BackContent = backContent;
			return n;
		}

		public static WindowsPhoneFlipTile WithWideBackContent(this WindowsPhoneFlipTile n, string wideBackContent)
		{
			n.WideBackContent = wideBackContent;
			return n;
		}

		public static WindowsPhoneFlipTile WithSmallBackgroundImage(this WindowsPhoneFlipTile n, string smallBackgroundImage)
		{
			n.SmallBackgroundImage = smallBackgroundImage;
			return n;
		}

		public static WindowsPhoneFlipTile WithBackgroundImage(this WindowsPhoneFlipTile n, string backgroundImage)
		{
			n.BackgroundImage = backgroundImage;
			return n;
		}

		public static WindowsPhoneFlipTile WithBackBackgroundImage(this WindowsPhoneFlipTile n, string backBackgroundImage)
		{
			n.BackBackgroundImage = backBackgroundImage;
			return n;
		}

		public static WindowsPhoneFlipTile WithWideBackgroundImage(this WindowsPhoneFlipTile n, string wideBackgroundImage)
		{
			n.WideBackgroundImage = wideBackgroundImage;
			return n;
		}

		public static WindowsPhoneFlipTile WithWideBackBackgroundImage(this WindowsPhoneFlipTile n, string wideBackBackgroundImage)
		{
			n.WideBackBackgroundImage = wideBackBackgroundImage;
			return n;
		}


		public static WindowsPhoneFlipTile ClearTitle(this WindowsPhoneFlipTile n)
		{
			n.ClearTitle = true;
			return n;
		}

		public static WindowsPhoneFlipTile ClearBackTitle(this WindowsPhoneFlipTile n)
		{
			n.ClearBackTitle = true;
			return n;
		}

		public static WindowsPhoneFlipTile ClearBackContent(this WindowsPhoneFlipTile n)
		{
			n.ClearBackContent = true;
			return n;
		}

		public static WindowsPhoneFlipTile ClearWideBackContent(this WindowsPhoneFlipTile n)
		{
			n.ClearWideBackContent = true;
			return n;
		}

		public static WindowsPhoneFlipTile ClearCount(this WindowsPhoneFlipTile n)
		{
			n.ClearCount = true;
			return n;
		}

		public static WindowsPhoneFlipTile ClearSmallBackgroundImage(this WindowsPhoneFlipTile n)
		{
			n.ClearSmallBackgroundImage = true;
			return n;
		}

		public static WindowsPhoneFlipTile ClearBackgroundImage(this WindowsPhoneFlipTile n)
		{
			n.ClearBackgroundImage = true;
			return n;
		}

		public static WindowsPhoneFlipTile ClearBackBackgroundImage(this WindowsPhoneFlipTile n)
		{
			n.ClearBackBackgroundImage = true;
			return n;
		}

		public static WindowsPhoneFlipTile ClearWideBackgroundImage(this WindowsPhoneFlipTile n)
		{
			n.ClearWideBackgroundImage = true;
			return n;
		}

		public static WindowsPhoneFlipTile ClearWideBackBackgroundImage(this WindowsPhoneFlipTile n)
		{
			n.ClearWideBackBackgroundImage = true;
			return n;
		}

		public static WindowsPhoneFlipTile WithTag(this WindowsPhoneFlipTile n, object tag)
		{
			n.Tag = tag;
			return n;
		}
	}
	
	public static class WindowsPhoneIconicTilePushFluent
	{
		public static WindowsPhoneIconicTile ForEndpointUri(this WindowsPhoneIconicTile n, Uri endpointUri)
		{
			n.EndPointUrl = endpointUri.ToString();
			return n;
		}

        public static WindowsPhoneTileNotification WithTileId(this WindowsPhoneTileNotification n, string tileId)
        {
            n.TileId = tileId;
            return n;
        }

		public static WindowsPhoneIconicTile WithCallbackUri(this WindowsPhoneIconicTile n, Uri callbackUri)
		{
			n.CallbackURI = callbackUri.ToString();
			return n;
		}

		public static WindowsPhoneIconicTile WithMessageID(this WindowsPhoneIconicTile n, Guid messageID)
		{
			n.MessageID = messageID;
			return n;
		}
		public static WindowsPhoneIconicTile WithBatchingInterval(this WindowsPhoneIconicTile n, BatchingInterval batchingInterval)
		{
			n.NotificationClass = batchingInterval;
			return n;
		}

		public static WindowsPhoneIconicTile ForOSVersion(this WindowsPhoneIconicTile n, WindowsPhoneDeviceOSVersion osVersion)
		{
			n.OSVersion = osVersion;
			return n;
		}

		public static WindowsPhoneIconicTile WithTitle(this WindowsPhoneIconicTile n, string title)
		{
			n.Title = title;
			return n;
		}

		public static WindowsPhoneIconicTile WithCount(this WindowsPhoneIconicTile n, int count)
		{
			n.Count = count;
			return n;
		}

		public static WindowsPhoneIconicTile WithWideContent1(this WindowsPhoneIconicTile n, string wideContent1)
		{
			n.WideContent1 = wideContent1;
			return n;
		}

		public static WindowsPhoneIconicTile WithWideContent2(this WindowsPhoneIconicTile n, string wideContent2)
		{
			n.WideContent2 = wideContent2;
			return n;
		}

		public static WindowsPhoneIconicTile WithWideContent3(this WindowsPhoneIconicTile n, string wideContent3)
		{
			n.WideContent3 = wideContent3;
			return n;
		}

		public static WindowsPhoneIconicTile WithSmallIconImage(this WindowsPhoneIconicTile n, string smallIconImage)
		{
			n.SmallIconImage = smallIconImage;
			return n;
		}

		public static WindowsPhoneIconicTile WithIconImage(this WindowsPhoneIconicTile n, string iconImage)
		{
			n.IconImage = iconImage;
			return n;
		}

		public static WindowsPhoneIconicTile WithBackgroundColor(this WindowsPhoneIconicTile n, string backgroundColor)
		{
			n.BackgroundColor = backgroundColor;
			return n;
		}
		
		public static WindowsPhoneIconicTile ClearTitle(this WindowsPhoneIconicTile n)
		{
			n.ClearTitle = true;
			return n;
		}

		public static WindowsPhoneIconicTile ClearWideContent1(this WindowsPhoneIconicTile n)
		{
			n.ClearWideContent1 = true;
			return n;
		}

		public static WindowsPhoneIconicTile ClearWideContent2(this WindowsPhoneIconicTile n)
		{
			n.ClearWideContent2 = true;
			return n;
		}

		public static WindowsPhoneIconicTile ClearWideContent3(this WindowsPhoneIconicTile n)
		{
			n.ClearWideContent3 = true;
			return n;
		}

		public static WindowsPhoneIconicTile ClearCount(this WindowsPhoneIconicTile n)
		{
			n.ClearCount = true;
			return n;
		}

		public static WindowsPhoneIconicTile ClearSmallIconImage(this WindowsPhoneIconicTile n)
		{
			n.ClearSmallIconImage = true;
			return n;
		}

		public static WindowsPhoneIconicTile ClearIconImage(this WindowsPhoneIconicTile n)
		{
			n.ClearIconImage = true;
			return n;
		}

		public static WindowsPhoneIconicTile ClearBackgroundColor(this WindowsPhoneIconicTile n)
		{
			n.ClearBackgroundColor = true;
			return n;
		}
		
		public static WindowsPhoneIconicTile WithTag(this WindowsPhoneIconicTile n, object tag)
		{
			n.Tag = tag;
			return n;
		}
	}

	public static class WindowsPhoneCycleTilePushFluent
	{
		public static WindowsPhoneCycleTile ForEndpointUri(this WindowsPhoneCycleTile n, Uri endpointUri)
		{
			n.EndPointUrl = endpointUri.ToString();
			return n;
		}

        public static WindowsPhoneTileNotification WithTileId(this WindowsPhoneTileNotification n, string tileId)
        {
            n.TileId = tileId;
            return n;
        }

		public static WindowsPhoneCycleTile WithCallbackUri(this WindowsPhoneCycleTile n, Uri callbackUri)
		{
			n.CallbackURI = callbackUri.ToString();
			return n;
		}

		public static WindowsPhoneCycleTile WithMessageID(this WindowsPhoneCycleTile n, Guid messageID)
		{
			n.MessageID = messageID;
			return n;
		}
		public static WindowsPhoneCycleTile WithBatchingInterval(this WindowsPhoneCycleTile n, BatchingInterval batchingInterval)
		{
			n.NotificationClass = batchingInterval;
			return n;
		}

		public static WindowsPhoneCycleTile ForOSVersion(this WindowsPhoneCycleTile n, WindowsPhoneDeviceOSVersion osVersion)
		{
			n.OSVersion = osVersion;
			return n;
		}

		public static WindowsPhoneCycleTile WithTitle(this WindowsPhoneCycleTile n, string title)
		{
			n.Title = title;
			return n;
		}

		public static WindowsPhoneCycleTile WithCount(this WindowsPhoneCycleTile n, int count)
		{
			n.Count = count;
			return n;
		}

		public static WindowsPhoneCycleTile WithCycleImage1(this WindowsPhoneCycleTile n, string cycleImage1)
		{
			n.CycleImage1 = cycleImage1;
			return n;
		}

		public static WindowsPhoneCycleTile WithCycleImage2(this WindowsPhoneCycleTile n, string cycleImage2)
		{
			n.CycleImage2 = cycleImage2;
			return n;
		}

		public static WindowsPhoneCycleTile WithCycleImage3(this WindowsPhoneCycleTile n, string cycleImage3)
		{
			n.CycleImage3 = cycleImage3;
			return n;
		}

		public static WindowsPhoneCycleTile WithCycleImage4(this WindowsPhoneCycleTile n, string cycleImage4)
		{
			n.CycleImage4 = cycleImage4;
			return n;
		}

		public static WindowsPhoneCycleTile WithCycleImage5(this WindowsPhoneCycleTile n, string cycleImage5)
		{
			n.CycleImage5 = cycleImage5;
			return n;
		}

		public static WindowsPhoneCycleTile WithCycleImage6(this WindowsPhoneCycleTile n, string cycleImage6)
		{
			n.CycleImage6 = cycleImage6;
			return n;
		}

		public static WindowsPhoneCycleTile WithCycleImage7(this WindowsPhoneCycleTile n, string cycleImage7)
		{
			n.CycleImage6 = cycleImage7;
			return n;
		}

		public static WindowsPhoneCycleTile WithCycleImage8(this WindowsPhoneCycleTile n, string cycleImage8)
		{
			n.CycleImage6 = cycleImage8;
			return n;
		}

		public static WindowsPhoneCycleTile WithCycleImage9(this WindowsPhoneCycleTile n, string cycleImage9)
		{
			n.CycleImage6 = cycleImage9;
			return n;
		}

		public static WindowsPhoneCycleTile ClearTitle(this WindowsPhoneCycleTile n)
		{
			n.ClearTitle = true;
			return n;
		}

		public static WindowsPhoneCycleTile ClearCount(this WindowsPhoneCycleTile n)
		{
			n.ClearCount = true;
			return n;
		}
		
		public static WindowsPhoneCycleTile ClearCycleImage1(this WindowsPhoneCycleTile n)
		{
			n.ClearCycleImage1 = true;
			return n;
		}

		public static WindowsPhoneCycleTile ClearCycleImage2(this WindowsPhoneCycleTile n)
		{
			n.ClearCycleImage2 = true;
			return n;
		}

		public static WindowsPhoneCycleTile ClearCycleImage3(this WindowsPhoneCycleTile n)
		{
			n.ClearCycleImage3 = true;
			return n;
		}

		public static WindowsPhoneCycleTile ClearCycleImage4(this WindowsPhoneCycleTile n)
		{
			n.ClearCycleImage4 = true;
			return n;
		}

		public static WindowsPhoneCycleTile ClearCycleImage5(this WindowsPhoneCycleTile n)
		{
			n.ClearCycleImage5 = true;
			return n;
		}

		public static WindowsPhoneCycleTile ClearCycleImage6(this WindowsPhoneCycleTile n)
		{
			n.ClearCycleImage6 = true;
			return n;
		}

		public static WindowsPhoneCycleTile ClearCycleImage7(this WindowsPhoneCycleTile n)
		{
			n.ClearCycleImage7 = true;
			return n;
		}

		public static WindowsPhoneCycleTile ClearCycleImage8(this WindowsPhoneCycleTile n)
		{
			n.ClearCycleImage8 = true;
			return n;
		}

		public static WindowsPhoneCycleTile ClearCycleImage9(this WindowsPhoneCycleTile n)
		{
			n.ClearCycleImage9 = true;
			return n;
		}
		public static WindowsPhoneCycleTile WithTag(this WindowsPhoneCycleTile n, object tag)
		{
			n.Tag = tag;
			return n;
		}
	}
}
