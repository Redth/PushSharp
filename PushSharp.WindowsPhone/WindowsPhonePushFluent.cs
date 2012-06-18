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

		public static WindowsPhoneRawNotification WithMessageID(this WindowsPhoneRawNotification n, string messageID)
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

		public static WindowsPhoneToastNotification WithMessageID(this WindowsPhoneToastNotification n, string messageID)
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

		public static WindowsPhoneTileNotification WithMessageID(this WindowsPhoneTileNotification n, string messageID)
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
	}
}
