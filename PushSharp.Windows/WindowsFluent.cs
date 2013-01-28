using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
	public static class WindowsFluent
	{
		public static WindowsToastNotification ForChannelUri(this WindowsToastNotification notification, string channelUri)
		{
			ValidateUri(channelUri);	
			notification.ChannelUri = channelUri;
			return notification;
		}

		public static WindowsTileNotification ForChannelUri(this WindowsTileNotification notification, string channelUri)
		{
			ValidateUri(channelUri);	
			notification.ChannelUri = channelUri;
			return notification;
		}

		public static WindowsRawNotification ForChannelUri(this WindowsRawNotification notification, string channelUri)
		{
			ValidateUri(channelUri);	
			notification.ChannelUri = channelUri;
			return notification;
		}

		public static WindowsBadgeGlyphNotification ForChannelUri(this WindowsBadgeGlyphNotification notification, string channelUri)
		{
			ValidateUri(channelUri);	
			notification.ChannelUri = channelUri;
			return notification;
		}

		public static WindowsBadgeNumericNotification ForChannelUri(this WindowsBadgeNumericNotification notification, string channelUri)
		{
			ValidateUri(channelUri);
			notification.ChannelUri = channelUri;
			return notification;
		}

		static void ValidateUri(string url)
		{
			var uri = new Uri(url);
		}

		public static WindowsTileNotification WithTag(this WindowsTileNotification notification, string tag)
		{
			notification.NotificationTag = tag;
			return notification;
		}

		public static WindowsTileNotification WithCachePolicy(this WindowsTileNotification notification, WindowsNotificationCachePolicyType cachePolicy)
		{
			notification.CachePolicy = cachePolicy;
			return notification;
		}

		public static WindowsBadgeGlyphNotification WithCachePolicy(this WindowsBadgeGlyphNotification notification, WindowsNotificationCachePolicyType cachePolicy)
		{
			notification.CachePolicy = cachePolicy;
			return notification;
		}

		public static WindowsBadgeNumericNotification WithCachePolicy(this WindowsBadgeNumericNotification notification, WindowsNotificationCachePolicyType cachePolicy)
		{
			notification.CachePolicy = cachePolicy;
			return notification;
		}

		public static WindowsToastNotification WithRequestForStatus(this WindowsToastNotification notification, bool requestForStatus)
		{
			notification.RequestForStatus = requestForStatus;
			return notification;
		}

		public static WindowsTileNotification WithRequestForStatus(this WindowsTileNotification notification, bool requestForStatus)
		{
			notification.RequestForStatus = requestForStatus;
			return notification;
		}

		public static WindowsRawNotification WithRequestForStatus(this WindowsRawNotification notification, bool requestForStatus)
		{
			notification.RequestForStatus = requestForStatus;
			return notification;
		}

		public static WindowsBadgeGlyphNotification WithRequestForStatus(this WindowsBadgeGlyphNotification notification, bool requestForStatus)
		{
			notification.RequestForStatus = requestForStatus;
			return notification;
		}

		public static WindowsBadgeNumericNotification WithRequestForStatus(this WindowsBadgeNumericNotification notification, bool requestForStatus)
		{
			notification.RequestForStatus = requestForStatus;
			return notification;
		}

		public static WindowsToastNotification WithTimeToLive(this WindowsToastNotification notification, int timeToLive)
		{
			notification.TimeToLive = timeToLive;
			return notification;
		}

		public static WindowsTileNotification WithTimeToLive(this WindowsTileNotification notification, int timeToLive)
		{
			notification.TimeToLive = timeToLive;
			return notification;
		}

		public static WindowsRawNotification WithTimeToLive(this WindowsRawNotification notification, int timeToLive)
		{
			notification.TimeToLive = timeToLive;
			return notification;
		}

		public static WindowsBadgeGlyphNotification WithTimeToLive(this WindowsBadgeGlyphNotification notification, int timeToLive)
		{
			notification.TimeToLive = timeToLive;
			return notification;
		}

		public static WindowsBadgeNumericNotification WithTimeToLive(this WindowsBadgeNumericNotification notification, int timeToLive)
		{
			notification.TimeToLive = timeToLive;
			return notification;
		}

		public static WindowsBadgeGlyphNotification WithGlyph(this WindowsBadgeGlyphNotification notification, BadgeGlyphValue glyph)
		{
			notification.Glyph = glyph;
			return notification;
		}

		public static WindowsBadgeNumericNotification WithBadgeNumber(this WindowsBadgeNumericNotification notification, int badgeNumber)
		{
			notification.BadgeNumber = badgeNumber;
			return notification;
		}

		public static WindowsToastNotification WithVersion(this WindowsToastNotification notification, int version = 1)
		{
			notification.Version = version;
			return notification;
		}

		public static WindowsTileNotification WithVersion(this WindowsTileNotification notification, int version = 1)
		{
			notification.Version = version;
			return notification;
		}

		public static WindowsToastNotification WithLanguage(this WindowsToastNotification notification, string language)
		{
			notification.Language = language;
			return notification;
		}

		public static WindowsTileNotification WithLanguage(this WindowsTileNotification notification, string language)
		{
			notification.Language = language;
			return notification;
		}

		public static WindowsToastNotification WithBaseUri(this WindowsToastNotification notification, string baseUri)
		{
			notification.BaseUri = baseUri;
			return notification;
		}

		public static WindowsTileNotification WithBaseUri(this WindowsTileNotification notification, string baseUri)
		{
			notification.BaseUri = baseUri;
			return notification;
		}

		public static WindowsToastNotification WithAddImageQuery(this WindowsToastNotification notification, bool addImageQuery)
		{
			notification.AddImageQuery = addImageQuery;
			return notification;
		}

		public static WindowsTileNotification WithAddImageQuery(this WindowsTileNotification notification, bool addImageQuery)
		{
			notification.AddImageQuery = addImageQuery;
			return notification;
		}

		public static WindowsToastNotification WithLaunch(this WindowsToastNotification notification, string launch)
		{
			notification.Launch = launch;
			return notification;
		}

		public static WindowsToastNotification WithAudio(this WindowsToastNotification notification, ToastAudioType audioType, bool loop = false, ToastDuration duration = ToastDuration.Short)
		{
			notification.AudioType = audioType;
			notification.AudioLoop = loop;
			notification.Duration = duration;
			return notification;
		}

		public static WindowsTileNotification WithBranding(this WindowsTileNotification notification, TileBrandingType branding)
		{
			notification.Branding = branding;
			return notification;
		}

		public static WindowsToastNotification WithDuration(this WindowsToastNotification notification, ToastDuration duration)
		{
			notification.Duration = duration;
			return notification;
		}

		
		/// <summary>
		/// A single string wrapped across a maximum of three lines of text. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastText01(this WindowsToastNotification notification, string wrappedText)
		{
			notification.TextTemplate = ToastNotificationTemplate.ToastText01;
			notification.Texts.Add(wrappedText);
			return notification;
		}

		/// <summary>
		/// One string of bold text on the first line, one string of regular text wrapped across the second and third lines. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastText02(this WindowsToastNotification notification, string boldTextLine1, string wrappedTextLine2)
		{
			notification.TextTemplate = ToastNotificationTemplate.ToastText02;
			notification.Texts.Add(boldTextLine1);
			notification.Texts.Add(wrappedTextLine2);
			return notification;
		}

		/// <summary>
		/// One string of bold text wrapped across the first and second lines, one string of regular text on the third line. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastText03(this WindowsToastNotification notification, string boldWrappedTextLine1, string textLine2)
		{
			notification.TextTemplate = ToastNotificationTemplate.ToastText03;
			notification.Texts.Add(boldWrappedTextLine1);
			notification.Texts.Add(textLine2);
			return notification;
		}

		/// <summary>
		/// One string of bold text on the first line, one string of regular text on the second line, one string of regular text on the third line. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastText04(this WindowsToastNotification notification, string boldTextLine1, string textLine2, string textLine3)
		{
			notification.TextTemplate = ToastNotificationTemplate.ToastText04;
			notification.Texts.Add(boldTextLine1);
			notification.Texts.Add(textLine2);
			notification.Texts.Add(textLine3);
			return notification;
		}

		/// <summary>
		/// An image and a single string wrapped across a maximum of three lines of text. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastImageAndText01(this WindowsToastNotification notification, string wrappedText, string image, string imageAlt = null)
		{
			notification.TextTemplate = ToastNotificationTemplate.ToastImageAndText01;
			notification.Images.Add(image, imageAlt);
			notification.Texts.Add(wrappedText);
			return notification;
		}

		/// <summary>
		/// An image, one string of bold text on the first line, one string of regular text wrapped across the second and third lines. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastImageAndText02(this WindowsToastNotification notification, string boldTextLine1, string wrappedTextLine2, string image, string imageAlt = null)
		{
			notification.TextTemplate = ToastNotificationTemplate.ToastImageAndText02;
			notification.Images.Add(image, imageAlt);
			notification.Texts.Add(boldTextLine1);
			notification.Texts.Add(wrappedTextLine2);
			return notification;
		}

		/// <summary>
		/// An image, one string of bold text wrapped across the first two lines, one string of regular text on the third line.
		/// </summary>
		public static WindowsToastNotification AsToastImageAndText03(this WindowsToastNotification notification, string boldWrappedTextLine1, string textLine2, string image, string imageAlt = null)
		{
			notification.TextTemplate = ToastNotificationTemplate.ToastImageAndText03;
			notification.Images.Add(image, imageAlt);
			notification.Texts.Add(boldWrappedTextLine1);
			notification.Texts.Add(textLine2);
			return notification;
		}

		/// <summary>
		/// An image, one string of bold text on the first line, one string of regular text on the second line, one string of regular text on the third line. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastImageAndText04(this WindowsToastNotification notification, string boldTextLine1, string textLine2, string textLine3, string image, string imageAlt = null)
		{
			notification.TextTemplate = ToastNotificationTemplate.ToastImageAndText04;
			notification.Images.Add(image, imageAlt);
			notification.Texts.Add(boldTextLine1);
			notification.Texts.Add(textLine2);
			notification.Texts.Add(textLine3);
			return notification;
		}


		#region Tile Fluent Methods
		/// <summary>
		/// One short string of large block text over a single, short line of bold, regular text. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileSquareBlock(this WindowsTileNotification notification, string largeText, string smallBoldItalicText)
		{
			notification.TileTemplate = TileNotificationTemplate.TileSquareBlock;
			notification.Texts.Add(largeText);
			notification.Texts.Add(smallBoldItalicText);
			return notification;
		}

		/// <summary>
		/// One header string in larger text on the first line; three strings of regular text on each of the next three lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileSquareText01(this WindowsTileNotification notification, string largeText, string smallTextLine1, string smallTextLine2, string smallTextLine3)
		{
			notification.TileTemplate = TileNotificationTemplate.TileSquareText01;
			notification.Texts.Add(largeText);
			notification.Texts.Add(smallTextLine1);
			notification.Texts.Add(smallTextLine2);
			notification.Texts.Add(smallTextLine3);
			return notification;
		}

		/// <summary>
		/// One header string in larger text on the first line, over one string of regular text wrapped over a maximum of three lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileSquareText02(this WindowsTileNotification notification, string largeText, string smallTextWrapped)
		{
			notification.TileTemplate = TileNotificationTemplate.TileSquareText01;
			notification.Texts.Add(largeText);
			notification.Texts.Add(smallTextWrapped);
			return notification;
		}

		/// <summary>
		/// Four strings of regular text on four lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileSquareText03(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4)
		{
			notification.TileTemplate = TileNotificationTemplate.TileSquareText03;
			notification.Texts.Add(textLine1);
			notification.Texts.Add(textLine2);
			notification.Texts.Add(textLine3);
			notification.Texts.Add(textLine4);
			return notification;
		}

		/// <summary>
		/// One string of regular text wrapped over a maximum of four lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileSquareText04(this WindowsTileNotification notification, string wrappedText)
		{
			notification.TileTemplate = TileNotificationTemplate.TileSquareText04;
			notification.Texts.Add(wrappedText);
			return notification;
		}

		/// <summary>
		/// One square image that fills the entire tile, no text. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileSquareImage(this WindowsTileNotification notification, string image, string imageAlt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileSquareImage;
			notification.Images.Add(image, imageAlt);
			return notification;
		}

		/// <summary>
		/// Top: One square image, no text. Bottom: One header string in larger text on the first line, three strings of regular text on each of the next three lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileSquarePeakImageAndText01(this WindowsTileNotification notification, string image, string imageAlt, string largeTextLine1, string smallTextLine2, string smallTextLine3, string smallTextLine4)
		{
			notification.TileTemplate = TileNotificationTemplate.TileSquarePeekImageAndText01;
			notification.Images.Add(image, imageAlt);
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(smallTextLine2);
			notification.Texts.Add(smallTextLine3);
			notification.Texts.Add(smallTextLine4);
			return notification;
		}

		/// <summary>
		/// Top: Square image, no text. Bottom: One header string in larger text on the first line, over one string of regular text wrapped over a maximum of three lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileSquarePeakImageAndText02(this WindowsTileNotification notification, string image, string imageAlt, string largeTextLine1, string smallTextLine2)
		{
			notification.TileTemplate = TileNotificationTemplate.TileSquarePeekImageAndText02;
			notification.Images.Add(image, imageAlt);
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(smallTextLine2);
			return notification;
		}

		/// <summary>
		/// Top: Square image, no text. Bottom: Four strings of regular text on four lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileSquarePeakImageAndText03(this WindowsTileNotification notification, string image, string imageAlt, string textLine1, string textLine2, string textLine3, string textLine4)
		{
			notification.TileTemplate = TileNotificationTemplate.TileSquarePeekImageAndText03;
			notification.Images.Add(image, imageAlt);
			notification.Texts.Add(textLine1);
			notification.Texts.Add(textLine2);
			notification.Texts.Add(textLine3);
			notification.Texts.Add(textLine4);
			return notification;
		}

		/// <summary>
		/// Top: Square image, no text. Bottom: One string of regular text wrapped over a maximum of four lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileSquarePeakImageAndText04(this WindowsTileNotification notification, string image, string imageAlt, string wrappedText)
		{
			notification.TileTemplate = TileNotificationTemplate.TileSquarePeekImageAndText04;
			notification.Images.Add(image, imageAlt);
			notification.Texts.Add(wrappedText);
			return notification;
		}

		/// <summary>
		/// One header string in larger text on the first line, four strings of regular text on the next four lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideText01(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string textLine5)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideText01;
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(textLine2);
			notification.Texts.Add(textLine3);
			notification.Texts.Add(textLine4);
			notification.Texts.Add(textLine5);
			return notification;
		}

		/// <summary>
		/// One header string in larger text over eight short strings arranged in two columns of four lines each. Columns are of equal width. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideText02(this WindowsTileNotification notification, string largeTextLine1, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideText02;
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(textLine2Col1);
			notification.Texts.Add(textLine2Col2);
			notification.Texts.Add(textLine3Col1);
			notification.Texts.Add(textLine3Col2);
			notification.Texts.Add(textLine4Col1);
			notification.Texts.Add(textLine4Col2);
			return notification;
		}

		/// <summary>
		/// One string of large text wrapped over a maximum of three lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideText03(this WindowsTileNotification notification, string wrappedLargeTextLine1)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideText03;
			notification.Texts.Add(wrappedLargeTextLine1);
			return notification;
		}

		/// <summary>
		/// One string of regular text wrapped over a maximum of five lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideText04(this WindowsTileNotification notification, string wrappedTextLine1)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideText04;
			notification.Texts.Add(wrappedTextLine1);
			return notification;
		}

		/// <summary>
		/// Five strings of regular text on five lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideText05(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4, string textLine5)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideText05;
			notification.Texts.Add(textLine1);
			notification.Texts.Add(textLine2);
			notification.Texts.Add(textLine3);
			notification.Texts.Add(textLine4);
			notification.Texts.Add(textLine5);
			return notification;
		}

		/// <summary>
		/// Ten short strings of regular text, arranged in two columns of five lines each. Columns are of equal width. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideText06(this WindowsTileNotification notification, string textLine1Col1, string textLine1Col2, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string textLine5Col1, string textLine5Col2)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideText06;
			notification.Texts.Add(textLine1Col1);
			notification.Texts.Add(textLine1Col2);
			notification.Texts.Add(textLine2Col1);
			notification.Texts.Add(textLine2Col2);
			notification.Texts.Add(textLine3Col1);
			notification.Texts.Add(textLine3Col2);
			notification.Texts.Add(textLine4Col1);
			notification.Texts.Add(textLine4Col2);
			notification.Texts.Add(textLine5Col1);
			notification.Texts.Add(textLine5Col2);
			return notification;
		}

		/// <summary>
		/// One header string in larger text over eight short strings of regular text arranged in two columns of four lines each. The column widths are such that the first column acts as a label and the second column as the content. This template is similar to TileWideText10, but the first column is wider.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideText07(this WindowsTileNotification notification, string largeTextLine1, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideText07;
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(textLine2Col1);
			notification.Texts.Add(textLine2Col2);
			notification.Texts.Add(textLine3Col1);
			notification.Texts.Add(textLine3Col2);
			notification.Texts.Add(textLine4Col1);
			notification.Texts.Add(textLine4Col2);
			return notification;
		}

		/// <summary>
		/// Ten short strings of regular text arranged in two columns of five lines each. The column widths are such that the first column acts as a label and the second column as the content. This template is similar to TileWideText11, but the first column is wider.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideText08(this WindowsTileNotification notification, string textLine1Col1, string textLine1Col2, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideText08;
			notification.Texts.Add(textLine1Col1);
			notification.Texts.Add(textLine1Col2);
			notification.Texts.Add(textLine2Col1);
			notification.Texts.Add(textLine2Col2);
			notification.Texts.Add(textLine3Col1);
			notification.Texts.Add(textLine3Col2);
			notification.Texts.Add(textLine4Col1);
			notification.Texts.Add(textLine4Col2);
			return notification;
		}

		/// <summary>
		/// One header string in larger text over one string of regular text wrapped over a maximum of four lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideText09(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideText09;
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(wrappedTextLine2);
			return notification;
		}

		/// <summary>
		/// One header string in larger text over eight short strings of regular text arranged in two columns of four lines each. The column widths are such that the first column acts as a label and the second column as the content. This template is similar to TileWideText07, but the first column is narrower.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideText10(this WindowsTileNotification notification, string largeTextLine1, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideText10;
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(textLine2Col1);
			notification.Texts.Add(textLine2Col2);
			notification.Texts.Add(textLine3Col1);
			notification.Texts.Add(textLine3Col2);
			notification.Texts.Add(textLine4Col1);
			notification.Texts.Add(textLine4Col2);
			return notification;
		}

		/// <summary>
		/// Ten short strings of regular text arranged in two columns of five lines each. The column widths are such that the first column acts as a label and the second column as the content. This template is similar to TileWideText08, but the first column is narrower.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideText11(this WindowsTileNotification notification, string textLine1Col1, string textLine1Col2, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideText11;
			notification.Texts.Add(textLine1Col1);
			notification.Texts.Add(textLine1Col2);
			notification.Texts.Add(textLine2Col1);
			notification.Texts.Add(textLine2Col2);
			notification.Texts.Add(textLine3Col1);
			notification.Texts.Add(textLine3Col2);
			notification.Texts.Add(textLine4Col1);
			notification.Texts.Add(textLine4Col2);
			return notification;
		}

		/// <summary>
		/// One wide image that fills the entire tile, no text. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideImage(this WindowsTileNotification notification, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideImage;
			notification.Images.Add(image1, image1Alt);
			return notification;
		}

		/// <summary>
		/// One large square image with four smaller square images to its right, no text. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideImageCollection(this WindowsTileNotification notification, string largeImage1, string largeImage1Alt, string image2, string image2Alt, string image3, string image3Alt, string image4, string image4Alt, string image5, string image5Alt)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideImage;
			notification.Images.Add(largeImage1, largeImage1Alt);
			notification.Images.Add(image2, image2Alt);
			notification.Images.Add(image3, image3Alt);
			notification.Images.Add(image4, image4Alt);
			notification.Images.Add(image5, image5Alt);
			return notification;
		}

		/// <summary>
		/// One wide image over one string of regular text wrapped over a maximum of two lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideImageAndText01(this WindowsTileNotification notification, string wrappedText, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideImageAndText01;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(wrappedText);
			return notification;
		}

		/// <summary>
		/// One wide image over two strings of regular text on two lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideImageAndText02(this WindowsTileNotification notification, string textLine1, string textLine2, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideImageAndText02;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(textLine1);
			notification.Texts.Add(textLine2);
			return notification;
		}

		/// <summary>
		/// Four strings of regular, unwrapped text on the left; large block text over a single, short string of bold, regular text on the right. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideBlockAndText01(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4, string boldLargeTextRight, string boldTextBottomRight)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideImageAndText01;
			notification.Texts.Add(textLine1);
			notification.Texts.Add(textLine2);
			notification.Texts.Add(textLine3);
			notification.Texts.Add(textLine4);
			notification.Texts.Add(boldLargeTextRight);
			notification.Texts.Add(boldTextBottomRight);
			return notification;
		}

		/// <summary>
		/// One string of regular text wrapped over a maximum of four lines on the left; large block text over a single, short string of bold, regular text on the right.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideBlockAndText02(this WindowsTileNotification notification, string wrappedTextLeft, string boldLargeTextRight, string boldTextBottomRight)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideImageAndText02;
			notification.Texts.Add(wrappedTextLeft);
			notification.Texts.Add(boldLargeTextRight);
			notification.Texts.Add(boldTextBottomRight);
			return notification;
		}

		/// <summary>
		/// On the left, one small image; on the right, one string of large text wrapped over a maximum of three lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideSmallImageAndText01(this WindowsTileNotification notification, string wrappedTextRight, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideSmallImageAndText01;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(wrappedTextRight);
			return notification;
		}

		/// <summary>
		/// On the left, one small image; on the right, one header string in larger text on the first line over four strings of regular text on the next four lines. Text does not wrap.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideSmallImageAndText02(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideSmallImageAndText02;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(textLine2);
			notification.Texts.Add(textLine3);
			notification.Texts.Add(textLine4);
			return notification;
		}

		/// <summary>
		/// On the left, one small image; on the right, one string of regular text wrapped over a maximum of five lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideSmallImageAndText03(this WindowsTileNotification notification, string wrappedTextRight, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideSmallImageAndText03;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(wrappedTextRight);
			return notification;
		}

		/// <summary>
		/// On the left, one small image; on the right, one header string of larger text on the first line over one string of regular text wrapped over a maximum of four lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideSmallImageAndText04(this WindowsTileNotification notification, string largeTextRight, string wrappedTextRight, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideSmallImageAndText04;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(largeTextRight);
			notification.Texts.Add(wrappedTextRight);
			return notification;
		}

		/// <summary>
		/// On the left, one header string in larger text over one string of regular text wrapped over a maximum of four lines; on the right, one small image with 3:4 dimensions.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWideSmallImageAndText05(this WindowsTileNotification notification, string largeTextLeft, string wrappedTextLeft, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWideSmallImageAndText05;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(largeTextLeft);
			notification.Texts.Add(wrappedTextLeft);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: One header string in larger text over one string of regular text wrapped over a maximum of four lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImageCollection01(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2, string image1, string image1Alt, string image2, string image2Alt, string image3, string image3Alt, string image4, string image4Alt, string image5, string image5Alt)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection01;
			notification.Images.Add(image1, image1Alt);
			notification.Images.Add(image2, image2Alt);
			notification.Images.Add(image3, image3Alt);
			notification.Images.Add(image4, image4Alt);
			notification.Images.Add(image5, image5Alt);
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(wrappedTextLine2);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: One header string in larger text on the first line, four strings of regular text on the next four lines. Text does not wrap.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImageCollection02(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string image1, string image1Alt, string image2, string image2Alt, string image3, string image3Alt, string image4, string image4Alt, string image5, string image5Alt)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection02;
			notification.Images.Add(image1, image1Alt);
			notification.Images.Add(image2, image2Alt);
			notification.Images.Add(image3, image3Alt);
			notification.Images.Add(image4, image4Alt);
			notification.Images.Add(image5, image5Alt);
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(textLine2);
			notification.Texts.Add(textLine3);
			notification.Texts.Add(textLine4);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: One string of large text wrapped over a maximum of three lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImageCollection03(this WindowsTileNotification notification, string largeWrappedText, string largeImage1, string largeImage1Alt, string image2, string image2Alt, string image3, string image3Alt, string image4, string image4Alt, string image5, string image5Alt)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection03;
			notification.Images.Add(largeImage1, largeImage1Alt);
			notification.Images.Add(image2, image2Alt);
			notification.Images.Add(image3, image3Alt);
			notification.Images.Add(image4, image4Alt);
			notification.Images.Add(image5, image5Alt);
			notification.Texts.Add(largeWrappedText);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: One string of regular text wrapped over a maximum of five lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImageCollection04(this WindowsTileNotification notification, string wrappedText, string largeImage1, string largeImage1Alt, string image2, string image2Alt, string image3, string image3Alt, string image4, string image4Alt, string image5, string image5Alt)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection04;
			notification.Images.Add(largeImage1, largeImage1Alt);
			notification.Images.Add(image2, image2Alt);
			notification.Images.Add(image3, image3Alt);
			notification.Images.Add(image4, image4Alt);
			notification.Images.Add(image5, image5Alt);
			notification.Texts.Add(wrappedText);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: On the left, one small image; on the right, one header string of larger text on the first line over one string of regular text wrapped over a maximum of four lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImageCollection05(this WindowsTileNotification notification, string largeBottomTextLine1, string bottomTextLine2, string bottomImage, string bottomImageAlt, string largeImage1, string largeImage1Alt, string image2, string image2Alt, string image3, string image3Alt, string image4, string image4Alt, string image5, string image5Alt)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection05;
			notification.Images.Add(largeImage1, largeImage1Alt);
			notification.Images.Add(image2, image2Alt);
			notification.Images.Add(image3, image3Alt);
			notification.Images.Add(image4, image4Alt);
			notification.Images.Add(image5, image5Alt);
			notification.Images.Add(bottomImage, bottomImageAlt);
			notification.Texts.Add(largeBottomTextLine1);
			notification.Texts.Add(bottomTextLine2);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: On the left, one small image; on the right, one string of large text wrapped over a maximum of three lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImageCollection06(this WindowsTileNotification notification, string largeWrappedBottomText, string bottomImage, string bottomImageAlt, string largeImage1, string largeImage1Alt, string image2, string image2Alt, string image3, string image3Alt, string image4, string image4Alt, string image5, string image5Alt)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection06;
			notification.Images.Add(largeImage1, largeImage1Alt);
			notification.Images.Add(image2, image2Alt);
			notification.Images.Add(image3, image3Alt);
			notification.Images.Add(image4, image4Alt);
			notification.Images.Add(image5, image5Alt);
			notification.Images.Add(bottomImage, bottomImageAlt);
			notification.Texts.Add(largeWrappedBottomText);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: One string of regular text wrapped over a maximum of five lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImageAndText01(this WindowsTileNotification notification, string wrappedText, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImageAndText01;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(wrappedText);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: Five strings of regular text on five lines. Text does not wrap.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImageAndText02(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4, string textLine5, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImageAndText02;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(textLine1);
			notification.Texts.Add(textLine2);
			notification.Texts.Add(textLine3);
			notification.Texts.Add(textLine4);
			notification.Texts.Add(textLine5);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: One header string in larger text over one string of regular text that wraps over a maximum of four lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImage01(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImage01;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(wrappedTextLine2);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: One header string in larger text on the first line, four strings of regular text on the next four lines. Text does not wrap.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImage02(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string textLine5, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImage02;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(textLine2);
			notification.Texts.Add(textLine3);
			notification.Texts.Add(textLine4);
			notification.Texts.Add(textLine5);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: One string of large text wrapped over a maximum of three lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImage03(this WindowsTileNotification notification, string largeWrappedTextLine1, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImage03;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(largeWrappedTextLine1);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: One string of regular text wrapped over a maximum of five lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImage04(this WindowsTileNotification notification, string wrappedTextLine1, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImage04;
			notification.Images.Add(image1, image1Alt);
			notification.Texts.Add(wrappedTextLine1);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: On the left, one small image; on the right, one header string of larger text on the first line over one string of regular text wrapped over a maximum of four lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImage05(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2, string bottomImageLeft, string bottomImageLeftAlt, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImage05;
			notification.Images.Add(image1, image1Alt);
			notification.Images.Add(bottomImageLeft, bottomImageLeftAlt);
			notification.Texts.Add(largeTextLine1);
			notification.Texts.Add(wrappedTextLine2);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: On the left, one small image; on the right, one string of large text wrapped over a maximum of three lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification AsTileWidePeekImage06(this WindowsTileNotification notification, string largeTextLine1, string bottomImageLeft, string bottomImageLeftAlt, string image1, string image1Alt = null)
		{
			notification.TileTemplate = TileNotificationTemplate.TileWidePeekImage06;
			notification.Images.Add(image1, image1Alt);
			notification.Images.Add(bottomImageLeft, bottomImageLeftAlt);
			notification.Texts.Add(largeTextLine1);
			return notification;
		}
		#endregion

	}
}
