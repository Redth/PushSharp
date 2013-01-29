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

	

		public static WindowsToastNotification WithLaunch(this WindowsToastNotification notification, string launch)
		{
			notification.Launch = launch;
			return notification;
		}

		public static WindowsToastNotification WithAudio(this WindowsToastNotification notification, ToastAudioSource source, bool loop = false)
		{
			notification.Audio = new ToastAudio() {Loop = loop, Source = source};
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
		public static WindowsToastNotification AsToastText01(this WindowsToastNotification notification, string wrappedText, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			notification.Visual.Binding = new ToastBinding()
				{
					ToastTemplate = ToastNotificationTemplate.ToastText01,
					Language = language,
					Fallback = fallback,
					BaseUri = baseUri,
					Branding = branding,
					AddImageQuery = addImageQuery
				};
			notification.Visual.Binding.Texts.Add(new ToastText() {Language = language, Text = wrappedText});

			return notification;
		}

		/// <summary>
		/// One string of bold text on the first line, one string of regular text wrapped across the second and third lines. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastText02(this WindowsToastNotification notification, string boldTextLine1, string wrappedTextLine2, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			notification.Visual.Binding = new ToastBinding()
			{
				ToastTemplate = ToastNotificationTemplate.ToastText02,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};
			notification.Visual.Binding.Texts.Add(new ToastText() {Language = language, Text = boldTextLine1});
			notification.Visual.Binding.Texts.Add(new ToastText() {Language = language, Text = wrappedTextLine2});

			return notification;
		}

		/// <summary>
		/// One string of bold text wrapped across the first and second lines, one string of regular text on the third line. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastText03(this WindowsToastNotification notification, string boldWrappedTextLine1, string textLine2, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			notification.Visual.Binding = new ToastBinding()
			{
				ToastTemplate = ToastNotificationTemplate.ToastText03,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = boldWrappedTextLine1 });
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = textLine2 });
			
			return notification;
		}

		/// <summary>
		/// One string of bold text on the first line, one string of regular text on the second line, one string of regular text on the third line. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastText04(this WindowsToastNotification notification, string boldTextLine1, string textLine2, string textLine3, string language = null, string fallback = null, BrandingType? branding = null)
		{
			notification.Visual.Binding = new ToastBinding()
			{
				ToastTemplate = ToastNotificationTemplate.ToastText04,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = boldTextLine1 });
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = textLine2 });
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = textLine3 });
			return notification;
		}

		/// <summary>
		/// An image and a single string wrapped across a maximum of three lines of text. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastImageAndText01(this WindowsToastNotification notification, string wrappedText, string imageSource, string imageAlt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			notification.Visual.Binding = new ToastBinding()
			{
				ToastTemplate = ToastNotificationTemplate.ToastImageAndText01,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = wrappedText });
			notification.Visual.Binding.Images.Add(new ToastImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });
			return notification;
		}

		/// <summary>
		/// An image, one string of bold text on the first line, one string of regular text wrapped across the second and third lines. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastImageAndText02(this WindowsToastNotification notification, string boldTextLine1, string wrappedTextLine2, string imageSource, string imageAlt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			notification.Visual.Binding = new ToastBinding()
			{
				ToastTemplate = ToastNotificationTemplate.ToastImageAndText02,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = boldTextLine1 });
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = wrappedTextLine2 });
			notification.Visual.Binding.Images.Add(new ToastImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });
			return notification;
		}

		/// <summary>
		/// An image, one string of bold text wrapped across the first two lines, one string of regular text on the third line.
		/// </summary>
		public static WindowsToastNotification AsToastImageAndText03(this WindowsToastNotification notification, string boldWrappedTextLine1, string textLine2, string imageSource, string imageAlt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			notification.Visual.Binding = new ToastBinding()
			{
				ToastTemplate = ToastNotificationTemplate.ToastImageAndText03,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = boldWrappedTextLine1 });
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = textLine2 });
			notification.Visual.Binding.Images.Add(new ToastImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });
			return notification;
		}

		/// <summary>
		/// An image, one string of bold text on the first line, one string of regular text on the second line, one string of regular text on the third line. For more information and example screenshots of the various Toast Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761494.aspx
		/// </summary>
		public static WindowsToastNotification AsToastImageAndText04(this WindowsToastNotification notification, string boldTextLine1, string textLine2, string textLine3, string imageSource, string imageAlt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			notification.Visual.Binding = new ToastBinding()
			{
				ToastTemplate = ToastNotificationTemplate.ToastImageAndText04,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = boldTextLine1 });
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = textLine2 });
			notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = textLine3 });
			notification.Visual.Binding.Images.Add(new ToastImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });
			return notification;
		}


		#region Tile Fluent Methods
		/// <summary>
		/// One short string of large block text over a single, short line of bold, regular text. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileSquareBlock(this WindowsTileNotification notification, string largeText, string smallBoldItalicText, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
				{
					TileTemplate = TileNotificationTemplate.TileSquareBlock,
					Language = language,
					Fallback = fallback,
					BaseUri = baseUri,
					Branding = branding,
					AddImageQuery = addImageQuery
				};

			binding.Texts.Add(new TileText() { Language = language, Text = largeText });
			binding.Texts.Add(new TileText() { Language = language, Text = smallBoldItalicText });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One header string in larger text on the first line; three strings of regular text on each of the next three lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileSquareText01(this WindowsTileNotification notification, string largeText, string smallTextLine1, string smallTextLine2, string smallTextLine3, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileSquareText01,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Texts.Add(new TileText() { Language = language, Text = largeText });
			binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2 });
			binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One header string in larger text on the first line, over one string of regular text wrapped over a maximum of three lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileSquareText02(this WindowsTileNotification notification, string largeText, string smallTextWrapped, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileSquareText02,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Texts.Add(new TileText() { Language = language, Text = largeText });
			binding.Texts.Add(new TileText() { Language = language, Text = smallTextWrapped });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Four strings of regular text on four lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileSquareText03(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileSquareText03,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Texts.Add(new TileText() { Language = language, Text = textLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One string of regular text wrapped over a maximum of four lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileSquareText04(this WindowsTileNotification notification, string wrappedText, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileSquareText04,
				Language = language,
				Fallback = fallback,
				Branding = branding
			};

			binding.Texts.Add(new TileText() { Language = language, Text = wrappedText });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One square image that fills the entire tile, no text. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileSquareImage(this WindowsTileNotification notification, string imageSource, string imageAlt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileSquareImage,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() {AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource});
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One square image, no text. Bottom: One header string in larger text on the first line, three strings of regular text on each of the next three lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileSquarePeakImageAndText01(this WindowsTileNotification notification, string imageSource, string imageAlt, string largeTextLine1, string smallTextLine2, string smallTextLine3, string smallTextLine4, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileSquarePeekImageAndText01,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2 });
			binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3 });
			binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: Square image, no text. Bottom: One header string in larger text on the first line, over one string of regular text wrapped over a maximum of three lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileSquarePeakImageAndText02(this WindowsTileNotification notification, string imageSource, string imageAlt, string largeTextLine1, string smallTextLine2, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileSquarePeekImageAndText02,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: Square image, no text. Bottom: Four strings of regular text on four lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileSquarePeakImageAndText03(this WindowsTileNotification notification, string imageSource, string imageAlt, string textLine1, string textLine2, string textLine3, string textLine4, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileSquarePeekImageAndText03,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: Square image, no text. Bottom: One string of regular text wrapped over a maximum of four lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileSquarePeakImageAndText04(this WindowsTileNotification notification, string imageSource, string imageAlt, string wrappedText, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileSquarePeekImageAndText04,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedText });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One header string in larger text on the first line, four strings of regular text on the next four lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideText01(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string textLine5, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideText01,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine5 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One header string in larger text over eight short strings arranged in two columns of four lines each. Columns are of equal width. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideText02(this WindowsTileNotification notification, string largeTextLine1, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideText02,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One string of large text wrapped over a maximum of three lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideText03(this WindowsTileNotification notification, string wrappedLargeTextLine1, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideText03,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = wrappedLargeTextLine1 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One string of regular text wrapped over a maximum of five lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideText04(this WindowsTileNotification notification, string wrappedTextLine1, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideText04,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextLine1 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Five strings of regular text on five lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideText05(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4, string textLine5, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideText05,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = textLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine5 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Ten short strings of regular text, arranged in two columns of five lines each. Columns are of equal width. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideText06(this WindowsTileNotification notification, string textLine1Col1, string textLine1Col2, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string textLine5Col1, string textLine5Col2, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideText06,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = textLine1Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine1Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine5Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine5Col2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One header string in larger text over eight short strings of regular text arranged in two columns of four lines each. The column widths are such that the first column acts as a label and the second column as the content. This template is similar to TileWideText10, but the first column is wider.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideText07(this WindowsTileNotification notification, string largeTextLine1, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideText07,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Ten short strings of regular text arranged in two columns of five lines each. The column widths are such that the first column acts as a label and the second column as the content. This template is similar to TileWideText11, but the first column is wider.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideText08(this WindowsTileNotification notification, string textLine1Col1, string textLine1Col2, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideText08,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = textLine1Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine1Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One header string in larger text over one string of regular text wrapped over a maximum of four lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideText09(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideText09,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextLine2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One header string in larger text over eight short strings of regular text arranged in two columns of four lines each. The column widths are such that the first column acts as a label and the second column as the content. This template is similar to TileWideText07, but the first column is narrower.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideText10(this WindowsTileNotification notification, string largeTextLine1, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideText10,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Ten short strings of regular text arranged in two columns of five lines each. The column widths are such that the first column acts as a label and the second column as the content. This template is similar to TileWideText08, but the first column is narrower.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideText11(this WindowsTileNotification notification, string textLine1Col1, string textLine1Col2, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideText11,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = textLine1Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine1Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3Col2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4Col2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One wide image that fills the entire tile, no text. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideImage(this WindowsTileNotification notification, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideImage,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One large square image with four smaller square images to its right, no text. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideImageCollection(this WindowsTileNotification notification, string largeImage1Source, string largeImage1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideImageCollection,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = largeImage1Alt, Source = largeImage1Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image2Alt, Source = image2Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image3Alt, Source = image3Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image4Alt, Source = image4Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image5Alt, Source = image5Source });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One wide image over one string of regular text wrapped over a maximum of two lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideImageAndText01(this WindowsTileNotification notification, string wrappedText, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideImageAndText01,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedText });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One wide image over two strings of regular text on two lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideImageAndText02(this WindowsTileNotification notification, string textLine1, string textLine2, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideImageAndText02,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Four strings of regular, unwrapped text on the left; large block text over a single, short string of bold, regular text on the right. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideBlockAndText01(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4, string boldLargeTextRight, string boldTextBottomRight, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideBlockAndText01,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = textLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4 });
			binding.Texts.Add(new TileText() { Language = language, Text = boldLargeTextRight });
			binding.Texts.Add(new TileText() { Language = language, Text = boldTextBottomRight });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// One string of regular text wrapped over a maximum of four lines on the left; large block text over a single, short string of bold, regular text on the right.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideBlockAndText02(this WindowsTileNotification notification, string wrappedTextLeft, string boldLargeTextRight, string boldTextBottomRight, string language = null, string fallback = null, BrandingType? branding = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideBlockAndText02,
				Language = language,
				Fallback = fallback,
				Branding = branding,
			};

			binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextLeft });
			binding.Texts.Add(new TileText() { Language = language, Text = boldLargeTextRight });
			binding.Texts.Add(new TileText() { Language = language, Text = boldTextBottomRight });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// On the left, one small image; on the right, one string of large text wrapped over a maximum of three lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideSmallImageAndText01(this WindowsTileNotification notification, string wrappedTextRight, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideSmallImageAndText01,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() {AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source});
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextRight });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// On the left, one small image; on the right, one header string in larger text on the first line over four strings of regular text on the next four lines. Text does not wrap.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideSmallImageAndText02(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideSmallImageAndText02,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// On the left, one small image; on the right, one string of regular text wrapped over a maximum of five lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideSmallImageAndText03(this WindowsTileNotification notification, string wrappedTextRight, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideSmallImageAndText03,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextRight });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// On the left, one small image; on the right, one header string of larger text on the first line over one string of regular text wrapped over a maximum of four lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideSmallImageAndText04(this WindowsTileNotification notification, string largeTextRight, string wrappedTextRight, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideSmallImageAndText04,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextRight });
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextRight });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// On the left, one header string in larger text over one string of regular text wrapped over a maximum of four lines; on the right, one small image with 3:4 dimensions.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWideSmallImageAndText05(this WindowsTileNotification notification, string largeTextLeft, string wrappedTextLeft, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWideSmallImageAndText05,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLeft });
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextLeft });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: One header string in larger text over one string of regular text wrapped over a maximum of four lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImageCollection01(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2, string image1Source, string image1Alt, string image2, string image2Alt, string image3, string image3Alt, string image4, string image4Alt, string image5, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection01,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: One header string in larger text on the first line, four strings of regular text on the next four lines. Text does not wrap.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImageCollection02(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string image1Source, string image1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection02,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image2Alt, Source = image2Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image3Alt, Source = image3Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image4Alt, Source = image4Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image5Alt, Source = image5Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: One string of large text wrapped over a maximum of three lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImageCollection03(this WindowsTileNotification notification, string largeWrappedText, string largeImage1Source, string largeImage1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection03,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = largeImage1Alt, Source = largeImage1Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image2Alt, Source = image2Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image3Alt, Source = image3Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image4Alt, Source = image4Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image5Alt, Source = image5Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeWrappedText });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: One string of regular text wrapped over a maximum of five lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImageCollection04(this WindowsTileNotification notification, string wrappedText, string largeImage1Source, string largeImage1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection04,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = largeImage1Alt, Source = largeImage1Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image2Alt, Source = image2Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image3Alt, Source = image3Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image4Alt, Source = image4Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image5Alt, Source = image5Source });
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedText });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: On the left, one small image; on the right, one header string of larger text on the first line over one string of regular text wrapped over a maximum of four lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImageCollection05(this WindowsTileNotification notification, string largeBottomTextLine1, string bottomTextLine2, string bottomImageSource, string bottomImageAlt, string largeImage1Source, string largeImage1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection05,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = bottomImageAlt, Source = bottomImageSource });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = largeImage1Alt, Source = largeImage1Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image2Alt, Source = image2Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image3Alt, Source = image3Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image4Alt, Source = image4Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image5Alt, Source = image5Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeBottomTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text =  bottomTextLine2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One large square image with four smaller square images to its right, no text. Bottom: On the left, one small image; on the right, one string of large text wrapped over a maximum of three lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImageCollection06(this WindowsTileNotification notification, string largeWrappedBottomText, string bottomImageSource, string bottomImageAlt, string largeImage1Source, string largeImage1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImageCollection06,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = bottomImageAlt, Source = bottomImageSource });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = largeImage1Alt, Source = largeImage1Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image2Alt, Source = image2Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image3Alt, Source = image3Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image4Alt, Source = image4Source });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image5Alt, Source = image5Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeWrappedBottomText });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: One string of regular text wrapped over a maximum of five lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImageAndText01(this WindowsTileNotification notification, string wrappedText, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImageAndText01,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedText });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: Five strings of regular text on five lines. Text does not wrap.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImageAndText02(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4, string textLine5, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImageAndText02,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine5 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: One header string in larger text over one string of regular text that wraps over a maximum of four lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImage01(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImage01,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextLine2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: One header string in larger text on the first line, four strings of regular text on the next four lines. Text does not wrap.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImage02(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string textLine5, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImage02,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine3 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine4 });
			binding.Texts.Add(new TileText() { Language = language, Text = textLine5 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: One string of large text wrapped over a maximum of three lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImage03(this WindowsTileNotification notification, string largeWrappedTextLine1, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImage03,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeWrappedTextLine1 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: One string of regular text wrapped over a maximum of five lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImage04(this WindowsTileNotification notification, string wrappedTextLine1, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImage04,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextLine1 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: On the left, one small image; on the right, one header string of larger text on the first line over one string of regular text wrapped over a maximum of four lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImage05(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2, string bottomImageLeftSource, string bottomImageLeftAlt, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImage05,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = bottomImageLeftAlt, Source = bottomImageLeftSource });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextLine2 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}

		/// <summary>
		/// Top: One wide image. Bottom: On the left, one small image; on the right, one string of large text wrapped over a maximum of three lines.
		///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
		/// </summary>
		public static WindowsTileNotification WithTileWidePeekImage06(this WindowsTileNotification notification, string largeTextLine1, string bottomImageLeftSource, string bottomImageLeftAlt, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
		{
			var binding = new TileBinding()
			{
				TileTemplate = TileNotificationTemplate.TileWidePeekImage06,
				Language = language,
				Fallback = fallback,
				BaseUri = baseUri,
				Branding = branding,
				AddImageQuery = addImageQuery
			};

			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = bottomImageLeftAlt, Source = bottomImageLeftSource });
			binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
			binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
			notification.Visual.Bindings.Add(binding);
			return notification;
		}
		#endregion

	}
}
