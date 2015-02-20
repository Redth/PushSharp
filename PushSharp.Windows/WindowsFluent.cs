using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
    public static class WindowsFluent
    {
        #region Toast Fluent Methods
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
            new Uri(url);
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
            notification.Visual.Version = version;
            return notification;
        }

        public static WindowsTileNotification WithVersion(this WindowsTileNotification notification, int version = 1)
        {
            notification.Visual.Version = version;
            return notification;
        }



        public static WindowsToastNotification WithLaunch(this WindowsToastNotification notification, string launch)
        {
            notification.Launch = launch;
            return notification;
        }

        public static WindowsToastNotification WithAudio(this WindowsToastNotification notification, ToastAudioSource source, bool loop = false)
        {
            notification.Audio = new ToastAudio() { Loop = loop, Source = source };
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
            notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = wrappedText });

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
            notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = boldTextLine1 });
            notification.Visual.Binding.Texts.Add(new ToastText() { Language = language, Text = wrappedTextLine2 });

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
        #endregion

        #region Tile Fluent Methods
        #region Version 1 (windows 8.0 supported tiles)
        /// <summary>
        /// One short string of large block text over a single, short line of bold, regular text. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        [Obsolete("TileSquareBlock may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Block.")]
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
        [Obsolete("TileSquareText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Text01.")]
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
        [Obsolete("TileSquareText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Text02.")]
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
        [Obsolete("TileSquareText03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Text03.")]
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
        [Obsolete("TileSquareText04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Text04.")]
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
        [Obsolete("TileSquareImage may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Image.")]
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

            binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// Top: One square image, no text. Bottom: One header string in larger text on the first line, three strings of regular text on each of the next three lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        [Obsolete("TileSquarePeekImageAndText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150PeekImageAndText01.")]
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
        [Obsolete("TileSquarePeekImageAndText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150PeekImageAndText02.")]
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
        [Obsolete("TileSquarePeekImageAndText03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150PeekImageAndText03.")]
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
        [Obsolete("TileSquarePeekImageAndText04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150PeekImageAndText04.")]
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
        [Obsolete("TileWideText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text01.")]
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
        [Obsolete("TileWideText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text02.")]
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
        [Obsolete("TileWideText03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text03.")]
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
        [Obsolete("TileWideText04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text04.")]
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
        [Obsolete("TileWideText05 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text05.")]
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
        [Obsolete("TileWideText06 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text06.")]
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
        [Obsolete("TileWideText07 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text07.")]
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
        [Obsolete("TileWideText08 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text08.")]
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
        [Obsolete("TileWideText09 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text09.")]
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
        [Obsolete("TileWideText10 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text10.")]
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
        [Obsolete("TileWideText11 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text11.")]
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
        [Obsolete("TileWideImage may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Image.")]
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
        [Obsolete("TileWideImageCollection may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150ImageCollection.")]
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
        [Obsolete("TileWideImageAndText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150ImageAndText01.")]
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
        [Obsolete("TileWideImageAndText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150ImageAndText02.")]
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
        [Obsolete("TileWideBlockAndText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150BlockAndText01.")]
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
        [Obsolete("TileWideBlockAndText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150BlockAndText02.")]
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
        [Obsolete("TileWideSmallImageAndText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150SmallImageAndText01.")]
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

            binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = image1Alt, Source = image1Source });
            binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextRight });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// On the left, one small image; on the right, one header string in larger text on the first line over four strings of regular text on the next four lines. Text does not wrap.
        ///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        [Obsolete("TileWideSmallImageAndText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150SmallImageAndText02.")]
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
        [Obsolete("TileWideSmallImageAndText03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150SmallImageAndText03.")]
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
        [Obsolete("TileWideSmallImageAndText04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150SmallImageAndText04.")]
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
        [Obsolete("TileWideSmallImageAndText05 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150SmallImageAndText05.")]
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
        [Obsolete("TileWidePeekImageCollection01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection01.")]
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
        [Obsolete("TileWidePeekImageCollection02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection02.")]
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
        [Obsolete("TileWidePeekImageCollection03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection03.")]
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
        [Obsolete("TileWidePeekImageCollection04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection04.")]
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
        [Obsolete("TileWidePeekImageCollection05 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection05.")]
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
            binding.Texts.Add(new TileText() { Language = language, Text = bottomTextLine2 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// Top: One large square image with four smaller square images to its right, no text. Bottom: On the left, one small image; on the right, one string of large text wrapped over a maximum of three lines.
        ///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        [Obsolete("TileWidePeekImageCollection06 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection06.")]
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
        [Obsolete("TileWidePeekImageAndText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageAndText01.")]
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
        [Obsolete("TileWidePeekImageAndText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageAndText02.")]
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
        [Obsolete("TileWidePeekImage01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImage01.")]
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
        [Obsolete("TileWidePeekImage02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImage02.")]
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
        [Obsolete("TileWidePeekImage03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImage03.")]
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
        [Obsolete("TileWidePeekImage04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImage04.")]
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
        [Obsolete("TileWidePeekImage05 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImage05.")]
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
        #endregion
        #region Version 2 (Windows 8.1 supported original Tiles)
        /// <summary>
        /// One short string of large block text over a single, short line of bold, regular text. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare150x150Block(this WindowsTileNotification notification, string largeText, string smallBoldItalicText, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare150x150Block,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileSquareBlock.ToString(),
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
        public static WindowsTileNotification WithTileSquare150x150Text01(this WindowsTileNotification notification, string largeText, string smallTextLine1, string smallTextLine2, string smallTextLine3, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare150x150Text01,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileSquareText01.ToString(),
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
        public static WindowsTileNotification WithTileSquare150x150Text02(this WindowsTileNotification notification, string largeText, string smallTextWrapped, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare150x150Text02,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileSquareText02.ToString(),
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
        public static WindowsTileNotification WithTileSquare150x150Text03(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare150x150Text03,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileSquareText03.ToString(),
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
        public static WindowsTileNotification WithTileSquare150x150Text04(this WindowsTileNotification notification, string wrappedText, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare150x150Text04,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileSquareText04.ToString(),
                Branding = branding
            };

            binding.Texts.Add(new TileText() { Language = language, Text = wrappedText });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// One square image that fills the entire tile, no text. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare150x150Image(this WindowsTileNotification notification, string imageSource, string imageAlt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare150x150Image,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileSquareImage.ToString(),
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// Top: One square image, no text. Bottom: One header string in larger text on the first line, three strings of regular text on each of the next three lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare150x150PeekImageAndText01(this WindowsTileNotification notification, string imageSource, string imageAlt, string largeTextLine1, string smallTextLine2, string smallTextLine3, string smallTextLine4, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare150x150PeekImageAndText01,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileSquarePeekImageAndText01.ToString(),
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
        public static WindowsTileNotification WithTileSquare150x150PeekImageAndText02(this WindowsTileNotification notification, string imageSource, string imageAlt, string largeTextLine1, string smallTextLine2, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare150x150PeekImageAndText02,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileSquarePeekImageAndText02.ToString(),
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
        public static WindowsTileNotification WithTileSquare150x150PeekImageAndText03(this WindowsTileNotification notification, string imageSource, string imageAlt, string textLine1, string textLine2, string textLine3, string textLine4, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare150x150PeekImageAndText03,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileSquarePeekImageAndText03.ToString(),
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
        public static WindowsTileNotification WithTileSquare150x150PeekImageAndText04(this WindowsTileNotification notification, string imageSource, string imageAlt, string wrappedText, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare150x150PeekImageAndText04,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileSquarePeekImageAndText04.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150Text01(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string textLine5, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Text01,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideText01.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150Text02(this WindowsTileNotification notification, string largeTextLine1, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Text02,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideText02.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150Text03(this WindowsTileNotification notification, string wrappedLargeTextLine1, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Text03,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideText03.ToString(),
                Branding = branding,
            };

            binding.Texts.Add(new TileText() { Language = language, Text = wrappedLargeTextLine1 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// One string of regular text wrapped over a maximum of five lines. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileWide310x150Text04(this WindowsTileNotification notification, string wrappedTextLine1, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Text04,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideText04.ToString(),
                Branding = branding,
            };

            binding.Texts.Add(new TileText() { Language = language, Text = wrappedTextLine1 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// Five strings of regular text on five lines. Text does not wrap. For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileWide310x150Text05(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4, string textLine5, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Text05,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideText05.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150Text06(this WindowsTileNotification notification, string textLine1Col1, string textLine1Col2, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string textLine5Col1, string textLine5Col2, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Text06,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideText06.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150Text07(this WindowsTileNotification notification, string largeTextLine1, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Text07,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideText07.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150Text08(this WindowsTileNotification notification, string textLine1Col1, string textLine1Col2, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Text08,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideText08.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150Text09(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Text09,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideText09.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150Text10(this WindowsTileNotification notification, string largeTextLine1, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Text10,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideText10.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150Text11(this WindowsTileNotification notification, string textLine1Col1, string textLine1Col2, string textLine2Col1, string textLine2Col2, string textLine3Col1, string textLine3Col2, string textLine4Col1, string textLine4Col2, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Text11,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideText11.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150Image(this WindowsTileNotification notification, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150Image,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideImage.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150ImageCollection(this WindowsTileNotification notification, string largeImage1Source, string largeImage1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150ImageCollection,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideImageCollection.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150ImageAndText01(this WindowsTileNotification notification, string wrappedText, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150ImageAndText01,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideImageAndText01.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150ImageAndText02(this WindowsTileNotification notification, string textLine1, string textLine2, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150ImageAndText02,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideImageAndText02.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150BlockAndText01(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4, string boldLargeTextRight, string boldTextBottomRight, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150BlockAndText01,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideBlockAndText01.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150BlockAndText02(this WindowsTileNotification notification, string wrappedTextLeft, string boldLargeTextRight, string boldTextBottomRight, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150BlockAndText02,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideBlockAndText02.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150SmallImageAndText01(this WindowsTileNotification notification, string wrappedTextRight, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150SmallImageAndText01,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideSmallImageAndText01.ToString(),
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
        /// On the left, one small image; on the right, one header string in larger text on the first line over four strings of regular text on the next four lines. Text does not wrap.
        ///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileWide310x150SmallImageAndText02(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150SmallImageAndText02,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideSmallImageAndText02.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150SmallImageAndText03(this WindowsTileNotification notification, string wrappedTextRight, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150SmallImageAndText03,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideSmallImageAndText03.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150SmallImageAndText04(this WindowsTileNotification notification, string largeTextRight, string wrappedTextRight, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150SmallImageAndText04,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideSmallImageAndText04.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150SmallImageAndText05(this WindowsTileNotification notification, string largeTextLeft, string wrappedTextLeft, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150SmallImageAndText05,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWideSmallImageAndText05.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImageCollection01(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2, string image1Source, string image1Alt, string image2, string image2Alt, string image3, string image3Alt, string image4, string image4Alt, string image5, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImageCollection01,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImageCollection01.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImageCollection02(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string image1Source, string image1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImageCollection02,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImageCollection02.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImageCollection03(this WindowsTileNotification notification, string largeWrappedText, string largeImage1Source, string largeImage1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImageCollection03,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImageCollection03.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImageCollection04(this WindowsTileNotification notification, string wrappedText, string largeImage1Source, string largeImage1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImageCollection04,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImageCollection04.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImageCollection05(this WindowsTileNotification notification, string largeBottomTextLine1, string bottomTextLine2, string bottomImageSource, string bottomImageAlt, string largeImage1Source, string largeImage1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImageCollection05,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImageCollection05.ToString(),
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
            binding.Texts.Add(new TileText() { Language = language, Text = bottomTextLine2 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// Top: One large square image with four smaller square images to its right, no text. Bottom: On the left, one small image; on the right, one string of large text wrapped over a maximum of three lines.
        ///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileWide310x150PeekImageCollection06(this WindowsTileNotification notification, string largeWrappedBottomText, string bottomImageSource, string bottomImageAlt, string largeImage1Source, string largeImage1Alt, string image2Source, string image2Alt, string image3Source, string image3Alt, string image4Source, string image4Alt, string image5Source, string image5Alt, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImageCollection06,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImageCollection06.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImageAndText01(this WindowsTileNotification notification, string wrappedText, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImageAndText01,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImageAndText01.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImageAndText02(this WindowsTileNotification notification, string textLine1, string textLine2, string textLine3, string textLine4, string textLine5, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImageAndText02,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImageAndText02.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImage01(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImage01,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImage01.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImage02(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string textLine4, string textLine5, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImage02,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImage02.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImage03(this WindowsTileNotification notification, string largeWrappedTextLine1, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImage03,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImage03.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImage04(this WindowsTileNotification notification, string wrappedTextLine1, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImage04,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImage04.ToString(),
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
        public static WindowsTileNotification WithTileWide310x150PeekImage05(this WindowsTileNotification notification, string largeTextLine1, string wrappedTextLine2, string bottomImageLeftSource, string bottomImageLeftAlt, string image1Source, string image1Alt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150PeekImage05,
                Language = language,
                Fallback = fallback ?? TileNotificationTemplate.TileWidePeekImage05.ToString(),
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
        #endregion
        #region Version 2 (windows 8.1 large tile support. Not backwards compatible with 8.0)
        #region Large text-only templates
        /// <summary>
        ///Two main text groups separated by a blank area:
        ///One string of large text, which can wrap over up to two lines, sitting over two strings of unwrapped regular text on two lines.
        ///Four strings on four lines, separated slightly into two sets. To the side is one string of large block text (this should be numerical) over a single, short line of regular text.
        ///Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310BlockAndText01(this WindowsTileNotification notification,
            string largeText,
            string topSmallTextLine1,
            string topSmallTextLine2,
            string bottomLeftSmallTextLine1,
            string bottomLeftSmallTextLine2,
            string bottomLeftSmallTextLine3,
            string bottomLeftSmallTextLine4,
            string blockNumericalText,
            string bottomRightSmallTextLine,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310BlockAndText01,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Texts.Add(new TileText() { Language = language, Text = largeText });
            binding.Texts.Add(new TileText() { Language = language, Text = topSmallTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = topSmallTextLine2 });
            binding.Texts.Add(new TileText() { Language = language, Text = bottomLeftSmallTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = bottomLeftSmallTextLine2 });
            binding.Texts.Add(new TileText() { Language = language, Text = bottomLeftSmallTextLine3 });
            binding.Texts.Add(new TileText() { Language = language, Text = bottomLeftSmallTextLine4 });
            binding.Texts.Add(new TileText() { Language = language, Text = bottomLeftSmallTextLine4 });
            binding.Texts.Add(new TileText() { Language = language, Text = blockNumericalText });
            binding.Texts.Add(new TileText() { Language = language, Text = bottomRightSmallTextLine });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// One header string in larger text on the first line, nine strings of regular text on the next nine lines. Text does not wrap.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310Text01(
            this WindowsTileNotification notification,
            string largeText,
            string smallTextLine1,
            string smallTextLine2,
            string smallTextLine3,
            string smallTextLine4,
            string smallTextLine5,
            string smallTextLine6,
            string smallTextLine7,
            string smallTextLine8,
            string smallTextLine9,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310Text01,
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
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// One header string in larger text over eighteen short strings arranged in two columns of nine lines each. Columns are of equal width. 
        /// This template is similar to TileSquare310x310Text05 and TileSquare310x310Text07, but those templates use columns of unequal width.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310Text02(
            this WindowsTileNotification notification,
            string largeText,
            string smallTextLine1Col1,
            string smallTextLine2Col1,
            string smallTextLine3Col1,
            string smallTextLine4Col1,
            string smallTextLine5Col1,
            string smallTextLine6Col1,
            string smallTextLine7Col1,
            string smallTextLine8Col1,
            string smallTextLine9Col1,
            string smallTextLine1Col2,
            string smallTextLine2Col2,
            string smallTextLine3Col2,
            string smallTextLine4Col2,
            string smallTextLine5Col2,
            string smallTextLine6Col2,
            string smallTextLine7Col2,
            string smallTextLine8Col2,
            string smallTextLine9Col2,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310Text02,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Texts.Add(new TileText() { Language = language, Text = largeText });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col2 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// Eleven strings of regular text on eleven lines. Text does not wrap.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310Text03(
            this WindowsTileNotification notification,
            string smallTextLine1,
            string smallTextLine2,
            string smallTextLine3,
            string smallTextLine4,
            string smallTextLine5,
            string smallTextLine6,
            string smallTextLine7,
            string smallTextLine8,
            string smallTextLine9,
                        string smallTextLine10,
                        string smallTextLine11,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310Text03,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };


            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine10 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine11 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// Twenty-two short strings of regular text, arranged in two columns of eleven lines each. Columns are of equal width. 
        /// This template is similar to TileSquare310x310Text06 and TileSquare310x310Text08, but those templates use columns of unequal width.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310Text04(
            this WindowsTileNotification notification,
            string smallTextLine1Col1,
            string smallTextLine2Col1,
            string smallTextLine3Col1,
            string smallTextLine4Col1,
            string smallTextLine5Col1,
            string smallTextLine6Col1,
            string smallTextLine7Col1,
            string smallTextLine8Col1,
            string smallTextLine9Col1,
            string smallTextLine10Col1,
            string smallTextLine11Col1,
            string smallTextLine1Col2,
            string smallTextLine2Col2,
            string smallTextLine3Col2,
            string smallTextLine4Col2,
            string smallTextLine5Col2,
            string smallTextLine6Col2,
            string smallTextLine7Col2,
            string smallTextLine8Col2,
            string smallTextLine9Col2,
            string smallTextLine10Col2,
            string smallTextLine11Col2,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310Text04,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col2 });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// One header string in larger text over eighteen short strings of regular text arranged in two columns of nine lines each.
        /// The column widths are such that the first column acts as a label and the second column as the content. 
        /// This template is similar to TileSquare310x310Text07, which has an even narrower first column, and TileSquare310x310Text02, which has columns of equal width.
        /// Note  This template requires the visual element to declare version="2". 
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310Text05(
            this WindowsTileNotification notification,
            string largeText,
            string smallTextLine1Col1,
            string smallTextLine2Col1,
            string smallTextLine3Col1,
            string smallTextLine4Col1,
            string smallTextLine5Col1,
            string smallTextLine6Col1,
            string smallTextLine7Col1,
            string smallTextLine8Col1,
            string smallTextLine9Col1,
            string smallTextLine1Col2,
            string smallTextLine2Col2,
            string smallTextLine3Col2,
            string smallTextLine4Col2,
            string smallTextLine5Col2,
            string smallTextLine6Col2,
            string smallTextLine7Col2,
            string smallTextLine8Col2,
            string smallTextLine9Col2,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310Text05,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Texts.Add(new TileText() { Language = language, Text = largeText });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col2 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Twenty-two short strings of regular text arranged in two columns of eleven lines each.
        /// The column widths are such that the first column acts as a label and the second column as the content.
        /// This template is similar to TileSquare310x310Text08, which has an even narrower first column, and TileSquare310x310Text04, which has columns of equal width.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310Text06(
            this WindowsTileNotification notification,
            string smallTextLine1Col1,
            string smallTextLine2Col1,
            string smallTextLine3Col1,
            string smallTextLine4Col1,
            string smallTextLine5Col1,
            string smallTextLine6Col1,
            string smallTextLine7Col1,
            string smallTextLine8Col1,
            string smallTextLine9Col1,
            string smallTextLine10Col1,
            string smallTextLine11Col1,
            string smallTextLine1Col2,
            string smallTextLine2Col2,
            string smallTextLine3Col2,
            string smallTextLine4Col2,
            string smallTextLine5Col2,
            string smallTextLine6Col2,
            string smallTextLine7Col2,
            string smallTextLine8Col2,
            string smallTextLine9Col2,
            string smallTextLine10Col2,
            string smallTextLine11Col2,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310Text06,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col2 });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// One header string in larger text over eighteen short strings of regular text arranged in two columns of nine lines each.
        /// The column widths are such that the first column acts as a label and the second column as the content.
        /// This template is similar to TileSquare310x310Text05, which has a wider first column, and TileSquare310x310Text02, which has columns of equal width.
        /// Note  This template requires the visual element to declare version="2". 
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310Text07(
            this WindowsTileNotification notification,
            string largeText,
            string smallTextLine1Col1,
            string smallTextLine2Col1,
            string smallTextLine3Col1,
            string smallTextLine4Col1,
            string smallTextLine5Col1,
            string smallTextLine6Col1,
            string smallTextLine7Col1,
            string smallTextLine8Col1,
            string smallTextLine9Col1,
            string smallTextLine1Col2,
            string smallTextLine2Col2,
            string smallTextLine3Col2,
            string smallTextLine4Col2,
            string smallTextLine5Col2,
            string smallTextLine6Col2,
            string smallTextLine7Col2,
            string smallTextLine8Col2,
            string smallTextLine9Col2,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310Text07,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Texts.Add(new TileText() { Language = language, Text = largeText });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col2 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Twenty-two short strings of regular text arranged in two columns of eleven lines each. The column widths are such that the first column acts as a label and the second column as the content.
        /// This template is similar to TileSquare310x310Text06, which has a wider first column, and TileSquare310x310Text04, which has columns of equal width.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310Text08(
            this WindowsTileNotification notification,
            string smallTextLine1Col1,
            string smallTextLine2Col1,
            string smallTextLine3Col1,
            string smallTextLine4Col1,
            string smallTextLine5Col1,
            string smallTextLine6Col1,
            string smallTextLine7Col1,
            string smallTextLine8Col1,
            string smallTextLine9Col1,
            string smallTextLine10Col1,
            string smallTextLine11Col1,
            string smallTextLine1Col2,
            string smallTextLine2Col2,
            string smallTextLine3Col2,
            string smallTextLine4Col2,
            string smallTextLine5Col2,
            string smallTextLine6Col2,
            string smallTextLine7Col2,
            string smallTextLine8Col2,
            string smallTextLine9Col2,
            string smallTextLine10Col2,
            string smallTextLine11Col2,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310Text08,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine7Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine8Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine9Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine10Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine10Col2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine11Col1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine11Col2 });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// One line of header text wrapped over a maximum of two lines. Beneath are two more, slightly separated lines of header text, each one line only.
        /// At the bottom are two lines of regular text, each of one line only.
        /// Note  This template requires the visual element to declare version="2". 
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310Text09(
            this WindowsTileNotification notification,
            string topWrappedText,
            string largeText1,
            string largeText2,
            string smallTextLine1,
            string smallTextLine2,
            string language = null,
            string fallback = null,
            BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310Text09,
                Language = language,
                Fallback = fallback,
                Branding = branding
            };

            binding.Texts.Add(new TileText() { Language = language, Text = topWrappedText });
            binding.Texts.Add(new TileText() { Language = language, Text = largeText1 });
            binding.Texts.Add(new TileText() { Language = language, Text = largeText2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Three stacked notices, each containing one header string in larger text on the first line, two strings of regular text on the next two lines. Text does not wrap.
        /// Note  This template requires the visual element to declare version="2". 
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310TextList01(
            this WindowsTileNotification notification,
            string HeaderText1,
            string smallTextLine1,
            string smallTextLine2,
            string HeaderText2,
            string smallTextLine3,
            string smallTextLine4,
            string HeaderText3,
            string smallTextLine5,
            string smallTextLine6,
            string language = null,
            string fallback = null,
            BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310TextList01,
                Language = language,
                Fallback = fallback,
                Branding = branding
            };

            binding.Texts.Add(new TileText() { Language = language, Text = HeaderText1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2 });
            binding.Texts.Add(new TileText() { Language = language, Text = HeaderText2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4 });
            binding.Texts.Add(new TileText() { Language = language, Text = HeaderText3 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine5 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine6 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Three stacked notices, each containing one string of regular text wrapped over a maximum of three lines.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310TextList02(this WindowsTileNotification notification, string wrappedText1, string wrappedText2, string wrappedText3, string language = null, string fallback = null, BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310TextList02,
                Language = language,
                Fallback = fallback,
                Branding = branding
            };

            binding.Texts.Add(new TileText() { Language = language, Text = wrappedText1 });
            binding.Texts.Add(new TileText() { Language = language, Text = wrappedText2 });
            binding.Texts.Add(new TileText() { Language = language, Text = wrappedText3 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Three stacked notices, each containing one header string in larger text over one string of regular text wrapped over a maximum of two lines.
        /// Note  This template requires the visual element to declare version="2". 
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310TextList03(
            this WindowsTileNotification notification,
            string HeaderText1,
            string wrappedText1,
            string HeaderText2,
            string wrappedText2,
            string HeaderText3,
            string wrappedText3,
            string language = null,
            string fallback = null,
            BrandingType? branding = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310TextList03,
                Language = language,
                Fallback = fallback,
                Branding = branding
            };

            binding.Texts.Add(new TileText() { Language = language, Text = HeaderText1 });
            binding.Texts.Add(new TileText() { Language = language, Text = wrappedText1 });
            binding.Texts.Add(new TileText() { Language = language, Text = HeaderText2 });
            binding.Texts.Add(new TileText() { Language = language, Text = wrappedText2 });
            binding.Texts.Add(new TileText() { Language = language, Text = HeaderText1 });
            binding.Texts.Add(new TileText() { Language = language, Text = wrappedText3 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        #endregion
        #region Large image-only templates
        /// <summary>
        /// One image that fills the entire tile; no text.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310Image(this WindowsTileNotification notification,
            string imageSource,
            string imageAlt = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310Image,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = imageSource, AddImageQuery = addImageQuery });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Four small square images overlaid across the top one large, full-tile, square image. Note that the small images cut off the top of the large image.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310ImageCollection(this WindowsTileNotification notification,
            string mainImage,
            string subImage1,
            string subImage2,
            string subImage3,
            string subImage4,
            string imageAltMainImage = null,
            string imageAltsubImage1 = null,
            string imageAltsubImage2 = null,
            string imageAltsubImage3 = null,
            string imageAltsubImage4 = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310ImageCollection,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = mainImage, AddImageQuery = addImageQuery, Alt = imageAltMainImage });
            binding.Images.Add(new TileImage() { Source = subImage1, AddImageQuery = addImageQuery, Alt = imageAltsubImage1 });
            binding.Images.Add(new TileImage() { Source = subImage2, AddImageQuery = addImageQuery, Alt = imageAltsubImage2 });
            binding.Images.Add(new TileImage() { Source = subImage3, AddImageQuery = addImageQuery, Alt = imageAltsubImage3 });
            binding.Images.Add(new TileImage() { Source = subImage4, AddImageQuery = addImageQuery, Alt = imageAltsubImage4 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        #endregion
        #region Large image-and-text templates
        /// <summary>
        /// One string of large block text (this should be numerical); two lines of large header text (no wrap); two sets of two strings in two lines (no wrap).
        /// Image in the background. If the text color is light, the image is darkened a bit to improve the text visibility.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310BlockAndText02(this WindowsTileNotification notification,
            string imageSource,
            string blockText,
            string largeText1,
            string largeText2,
            string smallTextLine1,
            string smallTextLine2,
            string smallTextLine3,
            string smallTextLine4,
            string imageAlt = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310BlockAndText02,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = imageSource, AddImageQuery = addImageQuery, Alt = imageAlt });
            binding.Texts.Add(new TileText() { Language = language, Text = blockText });
            binding.Texts.Add(new TileText() { Language = language, Text = largeText1 });
            binding.Texts.Add(new TileText() { Language = language, Text = largeText2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine4 });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// One image above one string of regular text wrapped over a maximum of two lines. The width of the text area depends on whether a logo is displayed.
        /// Note  This template allows branding only as "logo" or "none", but not "name". If you set the branding attribute to "name", it will automatically revert to "logo".
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310ImageAndText01(this WindowsTileNotification notification,
            string imageSource,
            string wrappedText,
            string imageAlt = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310ImageAndText01,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = imageSource, AddImageQuery = addImageQuery, Alt = imageAlt });
            binding.Texts.Add(new TileText() { Language = language, Text = wrappedText });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// One image above two strings of regular text on two lines. Text does not wrap. The width of the text area depends on whether a logo is displayed.
        /// Note  This template allows branding only as "logo" or "none", but not "name". If you set the branding attribute to "name", it will automatically revert to "logo".
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310ImageAndText02(this WindowsTileNotification notification,
            string imageSource,
            string smallTextLine1,
            string smallTextLine2,
            string imageAlt = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310ImageAndText02,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = imageSource, AddImageQuery = addImageQuery, Alt = imageAlt });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2 });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Background: a single image that fills the entire tile. Foreground: One string of text wrapped over a maximum of three lines.
        /// If the text color is light, the image is darkened a bit to improve the text visibility.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310ImageAndTextOverlay01(this WindowsTileNotification notification,
            string imageSource,
            string wrappedText,
            string imageAlt = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310ImageAndTextOverlay01,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = imageSource, AddImageQuery = addImageQuery, Alt = imageAlt });
            binding.Texts.Add(new TileText() { Language = language, Text = wrappedText });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Background: a single image that fills the entire tile. Foreground: At the top, one string of large text wrapped over a maximum of two lines;
        /// at the bottom, one string of regular text wrapped over a maximum of three lines. If the text color is light, the image is darkened a bit to improve the text visibility.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310ImageAndTextOverlay02(this WindowsTileNotification notification,
            string imageSource,
            string topLargeWrappedText,
            string wrappedText,
            string imageAlt = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310ImageAndTextOverlay02,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = imageSource, AddImageQuery = addImageQuery, Alt = imageAlt });
            binding.Texts.Add(new TileText() { Language = language, Text = topLargeWrappedText });
            binding.Texts.Add(new TileText() { Language = language, Text = wrappedText });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Background: a single image that fills the entire tile. Foreground: At the top, one string of large text wrapped over a maximum of two lines;
        /// at the bottom, three strings of regular text on three lines that do not wrap. If the text color is light, the image is darkened a bit to improve the text visibility.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310ImageAndTextOverlay03(this WindowsTileNotification notification,
            string imageSource,
            string topLargeWrappedText,
            string smallTextLine1,
            string smallTextLine2,
            string smallTextLine3,
            string imageAlt = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310ImageAndTextOverlay03,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = imageSource, AddImageQuery = addImageQuery, Alt = imageAlt });
            binding.Texts.Add(new TileText() { Language = language, Text = topLargeWrappedText });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine2 });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine3 });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// TileSquare310x310ImageCollection with the addition of a text ribbon across the bottom of the tile. 
        /// The text area contains one string of regular text wrapped over a maximum of two lines.
        /// Note that the small images cut off the top of the large image while the text area cuts off the bottom of the image. 
        /// The width of the text area depends on whether a logo is displayed.
        /// Note  This template allows branding only as "logo" or "none", but not "name". If you set the branding attribute to "name", it will automatically revert to "logo".
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310ImageCollectionAndText01(this WindowsTileNotification notification,
            string mainImage,
            string subImage1,
            string subImage2,
            string subImage3,
            string subImage4,
            string text,
            string imageAltMainImage = null,
            string imageAltsubImage1 = null,
            string imageAltsubImage2 = null,
            string imageAltsubImage3 = null,
            string imageAltsubImage4 = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310ImageCollectionAndText01,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = mainImage, AddImageQuery = addImageQuery, Alt = imageAltMainImage });
            binding.Images.Add(new TileImage() { Source = subImage1, AddImageQuery = addImageQuery, Alt = imageAltsubImage1 });
            binding.Images.Add(new TileImage() { Source = subImage2, AddImageQuery = addImageQuery, Alt = imageAltsubImage2 });
            binding.Images.Add(new TileImage() { Source = subImage3, AddImageQuery = addImageQuery, Alt = imageAltsubImage3 });
            binding.Images.Add(new TileImage() { Source = subImage4, AddImageQuery = addImageQuery, Alt = imageAltsubImage4 });

            binding.Texts.Add(new TileText() { Language = language, Text = text });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// TileSquare310x310ImageCollection with the addition of a text ribbon across the bottom of the tile.
        /// The text area contains two strings of regular text on two lines. 
        /// Text does not wrap. Note that the small images cut off the top of the large image while the text area cuts off the bottom of the image. 
        /// The width of the text area depends on whether a logo is displayed.
        /// Note  This template allows branding only as "logo" or "none", but not "name". If you set the branding attribute to "name", it will automatically revert to "logo".
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310ImageCollectionAndText02(this WindowsTileNotification notification,
            string mainImage,
            string subImage1,
            string subImage2,
            string subImage3,
            string subImage4,
            string textLine1,
            string textLine2,
            string imageAltMainImage = null,
            string imageAltsubImage1 = null,
            string imageAltsubImage2 = null,
            string imageAltsubImage3 = null,
            string imageAltsubImage4 = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310ImageCollectionAndText02,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = mainImage, AddImageQuery = addImageQuery, Alt = imageAltMainImage });
            binding.Images.Add(new TileImage() { Source = subImage1, AddImageQuery = addImageQuery, Alt = imageAltsubImage1 });
            binding.Images.Add(new TileImage() { Source = subImage2, AddImageQuery = addImageQuery, Alt = imageAltsubImage2 });
            binding.Images.Add(new TileImage() { Source = subImage3, AddImageQuery = addImageQuery, Alt = imageAltsubImage3 });
            binding.Images.Add(new TileImage() { Source = subImage4, AddImageQuery = addImageQuery, Alt = imageAltsubImage4 });

            binding.Texts.Add(new TileText() { Language = language, Text = textLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Three sets of information, each of which consists of one small square image to the left of one header string in larger text over two strings of regular text on the next two lines. Text does not wrap.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310SmallImagesAndTextList01(this WindowsTileNotification notification,
            string image1,
            string image2,
            string image3,
            string image1HeaderText,
            string image1SubText1,
            string image1SubText2,
            string image2HeaderText,
            string image2SubText1,
            string image2SubText2,
            string image3HeaderText,
            string image3SubText1,
            string image3SubText2,
            string imageAltMainImage = null,
            string imageAltsubImage1 = null,
            string imageAltsubImage2 = null,
            string imageAltsubImage3 = null,
            string imageAltsubImage4 = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310SmallImagesAndTextList01,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltMainImage });
            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltsubImage1 });
            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltsubImage2 });


            binding.Texts.Add(new TileText() { Language = language, Text = image1HeaderText });
            binding.Texts.Add(new TileText() { Language = language, Text = image1SubText1 });
            binding.Texts.Add(new TileText() { Language = language, Text = image1SubText2 });
            binding.Texts.Add(new TileText() { Language = language, Text = image2HeaderText });
            binding.Texts.Add(new TileText() { Language = language, Text = image2SubText1 });
            binding.Texts.Add(new TileText() { Language = language, Text = image2SubText2 });
            binding.Texts.Add(new TileText() { Language = language, Text = image3HeaderText });
            binding.Texts.Add(new TileText() { Language = language, Text = image3SubText1 });
            binding.Texts.Add(new TileText() { Language = language, Text = image3SubText2 });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Up to three sets of information, each of which consists of one small square image to the left of one string of regular text wrapped over a maximum of three lines.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310SmallImagesAndTextList02(this WindowsTileNotification notification,
            string image1,
            string image2,
            string image3,
            string image1WrappedText,
            string image2WrappedText,
            string image3WrappedText,
            string imageAltMainImage = null,
            string imageAltsubImage1 = null,
            string imageAltsubImage2 = null,
            string imageAltsubImage3 = null,
            string imageAltsubImage4 = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310SmallImagesAndTextList02,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltMainImage });
            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltsubImage1 });
            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltsubImage2 });


            binding.Texts.Add(new TileText() { Language = language, Text = image1WrappedText });
            binding.Texts.Add(new TileText() { Language = language, Text = image2WrappedText });
            binding.Texts.Add(new TileText() { Language = language, Text = image3WrappedText });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Up to three sets of information, each of which consists of one small square image to the left of one string of large text over one string of regular text wrapped over a maximum of two lines.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310SmallImagesAndTextList03(this WindowsTileNotification notification,
            string image1,
            string image2,
            string image3,
            string image1HeaderText,
            string image1WrappedText,
            string image2HeaderText,
            string image2WrappedText,
            string image3HeaderText,
            string image3WrappedText,
            string imageAltMainImage = null,
            string imageAltsubImage1 = null,
            string imageAltsubImage2 = null,
            string imageAltsubImage3 = null,
            string imageAltsubImage4 = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310SmallImagesAndTextList03,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltMainImage });
            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltsubImage1 });
            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltsubImage2 });

            binding.Texts.Add(new TileText() { Language = language, Text = image1HeaderText });
            binding.Texts.Add(new TileText() { Language = language, Text = image1WrappedText });
            binding.Texts.Add(new TileText() { Language = language, Text = image2HeaderText });
            binding.Texts.Add(new TileText() { Language = language, Text = image2WrappedText });
            binding.Texts.Add(new TileText() { Language = language, Text = image3HeaderText });
            binding.Texts.Add(new TileText() { Language = language, Text = image3WrappedText });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// Up to three sets of information, each of which consists of one small rectangular image to the right of one string of large text over one string of regular text wrapped over a maximum of two lines.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310SmallImagesAndTextList04(this WindowsTileNotification notification,
            string image1,
            string image2,
            string image3,
            string image1HeaderText,
            string image1WrappedText,
            string image2HeaderText,
            string image2WrappedText,
            string image3HeaderText,
            string image3WrappedText,
            string imageAltMainImage = null,
            string imageAltsubImage1 = null,
            string imageAltsubImage2 = null,
            string imageAltsubImage3 = null,
            string imageAltsubImage4 = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310SmallImagesAndTextList04,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltMainImage });
            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltsubImage1 });
            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltsubImage2 });

            binding.Texts.Add(new TileText() { Language = language, Text = image1HeaderText });
            binding.Texts.Add(new TileText() { Language = language, Text = image1WrappedText });
            binding.Texts.Add(new TileText() { Language = language, Text = image2HeaderText });
            binding.Texts.Add(new TileText() { Language = language, Text = image2WrappedText });
            binding.Texts.Add(new TileText() { Language = language, Text = image3HeaderText });
            binding.Texts.Add(new TileText() { Language = language, Text = image3WrappedText });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// One line of header text across the top, over three sets of information, 
        /// each of which consists of one small square image to the left of one header string in larger text over two strings of regular text on the next two lines. Text does not wrap.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310SmallImagesAndTextList05(this WindowsTileNotification notification,
            string image1,
            string image2,
            string image3,
            string largeHeaderText,
            string image1SmallTextLine1,
            string image1SmallTextLine2,
            string image2SmallTextLine1,
            string image2SmallTextLine2,
            string image3SmallTextLine1,
            string image3SmallTextLine2,
            string imageAltMainImage = null,
            string imageAltsubImage1 = null,
            string imageAltsubImage2 = null,
            string imageAltsubImage3 = null,
            string imageAltsubImage4 = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310SmallImagesAndTextList05,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltMainImage });
            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltsubImage1 });
            binding.Images.Add(new TileImage() { Source = image1, AddImageQuery = addImageQuery, Alt = imageAltsubImage2 });

            binding.Texts.Add(new TileText() { Language = language, Text = largeHeaderText });
            binding.Texts.Add(new TileText() { Language = language, Text = image1SmallTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = image1SmallTextLine2 });
            binding.Texts.Add(new TileText() { Language = language, Text = image2SmallTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = image2SmallTextLine2 });
            binding.Texts.Add(new TileText() { Language = language, Text = image3SmallTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = image3SmallTextLine2 });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// One medium-sized image in the upper left corner (upper right on right-to-left systems) over a single, unwrapped header string. 
        /// Beneath this are two sets of regular text: the first wrapped over a maximum of two lines, the second a single line only.
        /// Note  This template requires the visual element to declare version="2".
        /// For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare310x310SmallImageAndText01(this WindowsTileNotification notification,
            string imageSource,
            string largeHeaderText,
            string smallWrappedText,
            string smallTextLine,
            string imageAltMainImage = null,
            string imageAltsubImage1 = null,
            string imageAltsubImage2 = null,
            string imageAltsubImage3 = null,
            string imageAltsubImage4 = null,
            string language = null,
            string fallback = null,
            string baseUri = null,
            BrandingType? branding = null,
            bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare310x310SmallImageAndText01,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };

            binding.Images.Add(new TileImage() { Source = imageSource, AddImageQuery = addImageQuery, Alt = imageAltMainImage });

            binding.Texts.Add(new TileText() { Language = language, Text = largeHeaderText });
            binding.Texts.Add(new TileText() { Language = language, Text = smallWrappedText });
            binding.Texts.Add(new TileText() { Language = language, Text = smallTextLine });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        #endregion
        #endregion
        #region version 3 (Windows phone 8.1 only tiles)
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
        /// <summary>
        /// A single icon image. The icon image's background should be transparent to allow the tile color to show through. 
        /// A badge is shown to the right of the image, and the image should be sized to allow that badge to have enough room. 
        /// This template, being the small tile size, cannot display the app name or logo declared in your manifest.
        /// This template only works on Windows Phone runtime apps, and requires the version to be set to "3".
        ///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare71x71IconWithBadge(this WindowsTileNotification notification, string imageSource, string imageAlt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare71x71IconWithBadge,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };


            binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }

        /// <summary>
        /// A single icon image. The icon image's background should be transparent to allow the tile color to show through. 
        /// A badge is shown to the right of the image, and the image should be sized to allow that badge to have enough room. 
        /// This template only works on Windows Phone runtime apps, and requires the version to be set to "3".
        ///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileSquare150x150IconWithBadge(this WindowsTileNotification notification, string imageSource, string imageAlt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileSquare150x150IconWithBadge,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };


            binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });

            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        /// <summary>
        /// In the lower right corner, a single icon image. The icon image's background should be transparent to allow the tile color to show through. 
        /// A badge is shown to the right of the image. If no text elements are specified in this template, the icon image and badge are centered in the tile.
        /// This template only works on Windows Phone runtime apps, and requires the version to be set to "3".
        ///  For more information and example screenshots of the various Tile Layouts see http://msdn.microsoft.com/en-us/library/windows/apps/hh761491.aspx
        /// </summary>
        public static WindowsTileNotification WithTileWide310x150IconWithBadgeAndText(this WindowsTileNotification notification, string largeTextLine1, string textLine2, string textLine3, string imageSource, string imageAlt = null, string language = null, string fallback = null, string baseUri = null, BrandingType? branding = null, bool? addImageQuery = null)
        {
            var binding = new TileBinding()
            {
                TileTemplate = TileNotificationTemplate.TileWide310x150IconWithBadgeAndText,
                Language = language,
                Fallback = fallback,
                BaseUri = baseUri,
                Branding = branding,
                AddImageQuery = addImageQuery
            };


            binding.Images.Add(new TileImage() { AddImageQuery = addImageQuery, Alt = imageAlt, Source = imageSource });
            binding.Texts.Add(new TileText() { Language = language, Text = largeTextLine1 });
            binding.Texts.Add(new TileText() { Language = language, Text = textLine2 });
            binding.Texts.Add(new TileText() { Language = language, Text = textLine3 });
            notification.Visual.Bindings.Add(binding);
            return notification;
        }
        #endregion
        #endregion


    }
}