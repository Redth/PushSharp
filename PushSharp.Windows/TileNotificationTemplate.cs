using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PushSharp.Windows
{
    public enum TileNotificationTemplate
    {
        /// <summary>
        ///     One image that fills the entire tile; no text.
        /// </summary>
        [Obsolete("TileSquareImage may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Image.")]
        TileSquareImage,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileSquareImage One square image that fills the entire tile,
        ///     no text.
        /// </summary>
        TileSquare150x150Image,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileSquareBlock One string of large block text (this should
        ///     be numerical) over a single, short line of bold, regular text.
        /// </summary>
        TileSquare150x150Block,
        /// <summary>
        ///     Windows: One string of large block text (generally numerical) over a single,
        ///     short line of bold, regular text. Windows Phone 8.1: One short string of
        ///     large block text (generally numerical) in the lower right corner, to the
        ///     left of a single, very short line of bold, regular text.
        /// </summary>
        [Obsolete("TileSquareBlock may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Block.")]
        TileSquareBlock,
        /// <summary>
        ///     One header string in larger text on the first line; three strings of regular
        ///     text on each of the next three lines. Text does not wrap.
        /// </summary>
        [Obsolete("TileSquareText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Text01.")]
        TileSquareText01,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileSquareText01 One header string in larger text on the
        ///     first line; three strings of regular text on each of the next three lines.
        ///     Text does not wrap.
        /// </summary>
        TileSquare150x150Text01,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileSquareText02 One header string in larger text on the
        ///     first line, over one string of regular text wrapped over a maximum of three
        ///     lines.
        /// </summary>
        TileSquare150x150Text02,
        /// <summary>
        ///     One header string in larger text on the first line, over one string of regular
        ///     text wrapped over a maximum of three lines.
        /// </summary>
        [Obsolete("TileSquareText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Text02.")]
        TileSquareText02,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileSquareText03 Four strings of regular text on four lines.
        ///     Text does not wrap.
        /// </summary>
        TileSquare150x150Text03,
        /// <summary>
        ///     Four strings of regular text on four lines. Text does not wrap.
        /// </summary>
        [Obsolete("TileSquareText03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Text03.")]
        TileSquareText03,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileSquareText04 One string of regular text wrapped over
        ///     a maximum of four lines.
        /// </summary>
        TileSquare150x150Text04,
        /// <summary>
        ///     One string of regular text wrapped over a maximum of four lines.
        /// </summary>
        [Obsolete("TileSquareText04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150Text04.")]
        TileSquareText04,
        /// <summary>
        ///     Top/Front: One square image, no text. Bottom/Back: One header string in larger
        ///     text on the first line, three strings of regular text on each of the next
        ///     three lines. Text does not wrap.
        /// </summary>
        [Obsolete("TileSquarePeekImageAndText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150PeekImageAndText01.")]
        TileSquarePeekImageAndText01,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileSquarePeekImageAndText01 Top/Front: One square image,
        ///     no text. Bottom/Back: One header string in larger text on the first line,
        ///     three strings of regular text on each of the next three lines. Text does
        ///     not wrap.
        /// </summary>
        TileSquare150x150PeekImageAndText01,
        /// <summary>
        ///     Top/Front: Square image, no text. Bottom/Back: One header string in larger
        ///     text on the first line, over one string of regular text wrapped over a maximum
        ///     of three lines.
        /// </summary>
        [Obsolete("TileSquarePeekImageAndText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150PeekImageAndText02.")]
        TileSquarePeekImageAndText02,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileSquarePeekImageAndText02 Top/Front: Square image, no
        ///     text. Bottom/Back: One header string in larger text on the first line, over
        ///     one string of regular text wrapped over a maximum of three lines.
        TileSquare150x150PeekImageAndText02,
        /// <summary>
        ///     Top/Front: Square image, no text. Bottom/Back: Four strings of regular text
        ///     on four lines. Text does not wrap.
        /// </summary>
        [Obsolete("TileSquarePeekImageAndText03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150PeekImageAndText03.")]
        TileSquarePeekImageAndText03,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileSquarePeekImageAndText03 Top/Front: Square image, no
        ///     text. Bottom/Back: Four strings of regular text on four lines. Text does
        ///     not wrap.
        /// </summary>
        TileSquare150x150PeekImageAndText03,
        /// <summary>
        ///     Top/Front: Square image, no text. Bottom/Back: One string of regular text
        ///     wrapped over a maximum of four lines.
        /// </summary>
        [Obsolete("TileSquarePeekImageAndText04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileSquare150x150PeekImageAndText04.")]
        TileSquarePeekImageAndText04,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileSquarePeekImageAndText04 Top/Front: Square image, no
        ///     text. Bottom/Back: One string of regular text wrapped over a maximum of four
        ///     lines.
        /// </summary>
        TileSquare150x150PeekImageAndText04,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideImage One wide image that fills the entire tile,
        ///     no text.
        /// </summary>
        TileWide310x150Image,
        /// <summary>
        ///     One wide image that fills the entire tile, no text.
        /// </summary>
        [Obsolete("TileWideImage may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Image.")]
        TileWideImage,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideImageCollection Windows: One large square image
        ///     with four smaller square images to its right, no text. Windows Phone 8.1:
        ///     The images appear in a 6x3 set of tessellated, flipping blocks. An image
        ///     might take up one block or four. Images are shown randomly, moving among
        ///     blocks of various solid colors.
        /// </summary>
        TileWide310x150ImageCollection,
        /// <summary>
        ///     Windows: One large square image with four smaller square images to its right,
        ///     no text. Windows Phone 8.1: The images appear in a 6x3 set of tessellated,
        ///     flipping blocks. An image might take up one block or four. Images are shown
        ///     randomly, moving among blocks of various solid colors.
        /// </summary>
        [Obsolete("TileWideImageCollection may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150ImageCollection.")]
        TileWideImageCollection,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideImageAndText01 One wide image over one string of
        ///     regular text wrapped over a maximum of two lines (one line on Windows Phone
        ///     8.1). The width of the text area depends on whether a logo is displayed.
        ///     This template allows branding only as "logo" or "none", but not "name". If
        ///     you set the branding attribute to "name", it will automatically revert to
        ///     "logo" on Windows or "none" on Windows Phone 8.1.
        /// </summary>
        TileWide310x150ImageAndText01,
        /// <summary>
        ///     One wide image over one string of regular text wrapped over a maximum of
        ///     two lines (one line on Windows Phone 8.1). The width of the text area depends
        ///     on whether a logo is displayed. This template allows branding only as "logo"
        ///     or "none", but not "name". If you set the branding attribute to "name", it
        ///     will automatically revert to "logo" on Windows or "none" on Windows Phone
        ///     8.1.
        /// </summary>
        [Obsolete("TileWideImageAndText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150ImageAndText01.")]
        TileWideImageAndText01,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideImageAndText02 Windows: One wide image over two
        ///     strings of regular text on two lines. Text does not wrap. The width of the
        ///     text area depends on whether a logo is displayed. Windows Phone 8.1: One
        ///     wide image over one string of regular text on one line. Text does not wrap.
        ///     The second string is ignored. The width of the text area depends on whether
        ///     a logo is displayed. This template allows branding only as "logo" or "none",
        ///     but not "name". If you set the branding attribute to "name", it will automatically
        ///     revert to "logo" on Windows or "none" on Windows Phone 8.1.
        /// </summary>
        TileWide310x150ImageAndText02,
        /// <summary>
        ///     Windows: One wide image over two strings of regular text on two lines. Text
        ///     does not wrap. The width of the text area depends on whether a logo is displayed.
        ///     Windows Phone 8.1: One wide image over one string of regular text on one
        ///     line. Text does not wrap. The second string is ignored. The width of the
        ///     text area depends on whether a logo is displayed. This template allows branding
        ///     only as "logo" or "none", but not "name". If you set the branding attribute
        ///     to "name", it will automatically revert to "logo" on Windows or "none" on
        ///     Windows Phone 8.1..
        /// </summary>
        [Obsolete("TileWideImageAndText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150ImageAndText02.")]
        TileWideImageAndText02,
        /// <summary>
        ///     Four strings of regular, unwrapped text on the left; large block text (this
        ///     should be numerical) over a single, short string of bold, regular text on
        ///     the right. The last of the four strings on the left is ignored in Windows
        ///     Phone 8.1.
        /// </summary>
        [Obsolete("TileWideBlockAndText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150BlockAndText01.")]
        TileWideBlockAndText01,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideBlockAndText01 Four strings of regular, unwrapped
        ///     text on the left; large block text (this should be numerical) over a single,
        ///     short string of bold, regular text on the right. The last of the four strings
        ///     on the left is ignored in Windows Phone 8.1.
        /// </summary>
        TileWide310x150BlockAndText01,
        /// <summary>
        ///     One string of regular text wrapped over a maximum of four lines on the left;
        ///     large block text (this should be numerical) over a single, short string of
        ///     bold, regular text on the right.
        /// </summary>
        [Obsolete("TileWideBlockAndText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150BlockAndText02.")]
        TileWideBlockAndText02,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideBlockAndText02 One string of regular text wrapped
        ///     over a maximum of four lines on the left; large block text (this should be
        ///     numerical) over a single, short string of bold, regular text on the right.
        /// </summary>
        TileWide310x150BlockAndText02,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImageCollection01 Windows: Top: One large square
        ///     image with four smaller square images to its right, no text. Note that the
        ///     large image is not quite square; it is slightly wider than it is tall. If
        ///     you supply a square image, the image will be scaled for width and slightly
        ///     cropped on the top and bottom. Windows Phone 8.1: Front: The images appear
        ///     in a 6x3 set of tessellated, flipping blocks. An image might take up one
        ///     block or four. Images are shown randomly, moving among blocks of various
        ///     solid colors. Bottom/Back: One header string in larger text over one string
        ///     of regular text wrapped over a maximum of four lines.
        /// </summary>
        TileWide310x150PeekImageCollection01,
        /// <summary>
        ///     Windows: Top: One large square image with four smaller square images to its
        ///     right, no text. Windows Phone 8.1: Front: The images appear in a 6x3 set
        ///     of tessellated, flipping blocks. An image might take up one block or four.
        ///     Images are shown randomly, moving among blocks of various solid colors. Bottom/Back:
        ///     One header string in larger text over one string of regular text wrapped
        ///     over a maximum of four lines.
        /// </summary>
        [Obsolete("TileWidePeekImageCollection01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection01.")]
        TileWidePeekImageCollection01,
        /// <summary>
        ///     Windows: Top: One large square image with four smaller square images to its
        ///     right, no text. Windows Phone 8.1: Front: The images appear in a 6x3 set
        ///     of tessellated, flipping blocks. An image might take up one block or four.
        ///     Images are shown randomly, moving among blocks of various solid colors. Bottom/Back:
        ///     One header string in larger text on the first line, four strings of regular
        ///     text on the next four lines. Text does not wrap.
        /// </summary>
        [Obsolete("TileWidePeekImageCollection02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection02.")]
        TileWidePeekImageCollection02,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImageCollection02 Windows: Top: One large square
        ///     image with four smaller square images to its right, no text. Note that the
        ///     large image is not quite square; it is slightly wider than it is tall. If
        ///     you supply a square image, the image will be scaled for width and slightly
        ///     cropped on the top and bottom. Windows Phone 8.1: Front: The images appear
        ///     in a 6x3 set of tessellated, flipping blocks. An image might take up one
        ///     block or four. Images are shown randomly, moving among blocks of various
        ///     solid colors. Bottom/Back: One header string in larger text on the first
        ///     line, four strings of regular text on the next four lines. Text does not
        ///     wrap.
        /// </summary>
        TileWide310x150PeekImageCollection02,
        /// <summary>
        ///     Windows: Top: One large square image with four smaller square images to its
        ///     right, no text. Windows Phone 8.1: Front: The images appear in a 6x3 set
        ///     of tessellated, flipping blocks. An image might take up one block or four.
        ///     Images are shown randomly, moving among blocks of various solid colors. Bottom/Back:
        ///     One string of large text wrapped over a maximum of three lines.
        /// </summary>
        [Obsolete("TileWidePeekImageCollection03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection03.")]
        TileWidePeekImageCollection03,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImageCollection03 Windows: Top: One large square
        ///     image with four smaller square images to its right, no text. Note that the
        ///     large image is not quite square; it is slightly wider than it is tall. If
        ///     you supply a square image, the image will be scaled for width and slightly
        ///     cropped on the top and bottom. Windows Phone 8.1: Front: The images appear
        ///     in a 6x3 set of tessellated, flipping blocks. An image might take up one
        ///     block or four. Images are shown randomly, moving among blocks of various
        ///     solid colors. Bottom/Back: One string of large text wrapped over a maximum
        ///     of three lines.
        /// </summary>
        TileWide310x150PeekImageCollection03,
        /// <summary>
        ///     Windows: Top: One large square image with four smaller square images to its
        ///     right, no text. Windows Phone 8.1: Front: The images appear in a 6x3 set
        ///     of tessellated, flipping blocks. An image might take up one block or four.
        ///     Images are shown randomly, moving among blocks of various solid colors. Bottom/Back:
        ///     One string of regular text wrapped over a maximum of five lines.
        /// </summary>
        [Obsolete("TileWidePeekImageCollection04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection04.")]
        TileWidePeekImageCollection04,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImageCollection04 Windows: Top: One large square
        ///     image with four smaller square images to its right, no text. Note that the
        ///     large image is not quite square; it is slightly wider than it is tall. If
        ///     you supply a square image, the image will be scaled for width and slightly
        ///     cropped on the top and bottom. Windows Phone 8.1: Front: The images appear
        ///     in a 6x3 set of tessellated, flipping blocks. An image might take up one
        ///     block or four. Images are shown randomly, moving among blocks of various
        ///     solid colors. Bottom/Back: One string of regular text wrapped over a maximum
        ///     of five lines.
        /// </summary>
        TileWide310x150PeekImageCollection04,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImageCollection05 Windows Top: One large square
        ///     image with four smaller square images to its right, no text. Note that the
        ///     large image is not quite square; it is slightly wider than it is tall. If
        ///     you supply a square image, the image will be scaled for width and slightly
        ///     cropped on the top and bottom. Bottom: On the left, one small image; on the
        ///     right, one header string of larger text on the first line over one string
        ///     of regular text wrapped over a maximum of four lines. Windows Phone 8.1 Front:
        ///     The images appear in a 6x3 set of tessellated, flipping blocks. An image
        ///     might take up one block or four. Images are shown randomly, moving among
        ///     blocks of various solid colors. Back: One header string of larger text on
        ///     the first line over one string of regular text wrapped over a maximum of
        ///     four lines.
        /// </summary>
        TileWide310x150PeekImageCollection05,
        /// <summary>
        ///     Windows: Top: One large square image with four smaller square images to its
        ///     right, no text. Windows Phone 8.1: Front: The images appear in a 6x3 set
        ///     of tessellated, flipping blocks. An image might take up one block or four.
        ///     Images are shown randomly, moving among blocks of various solid colors. Bottom/Back:
        ///     On the left, one small image; on the right, one header string of larger text
        ///     on the first line over one string of regular text wrapped over a maximum
        ///     of four lines. On Windows Phone 8.1, the small image that accompanies the
        ///     text is not shown.
        /// </summary>
        [Obsolete("TileWidePeekImageCollection05 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection05.")]
        TileWidePeekImageCollection05,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImageCollection06 Windows Top: One large square
        ///     image with four smaller square images to its right, no text. Note that the
        ///     large image is not quite square; it is slightly wider than it is tall. If
        ///     you supply a square image, the image will be scaled for width and slightly
        ///     cropped on the top and bottom. Bottom: On the left, one small image; on the
        ///     right, one string of large text wrapped over a maximum of three lines. Windows
        ///     Phone 8.1 Front: The images appear in a 6x3 set of tessellated, flipping
        ///     blocks. An image might take up one block or four. Images are shown randomly,
        ///     moving among blocks of various solid colors. Back: One string of large text
        ///     wrapped over a maximum of three lines.
        /// </summary>
        TileWide310x150PeekImageCollection06,
        /// <summary>
        ///     Windows: Top: One large square image with four smaller square images to its
        ///     right, no text. Windows Phone 8.1: Front: The images appear in a 6x3 set
        ///     of tessellated, flipping blocks. An image might take up one block or four.
        ///     Images are shown randomly, moving among blocks of various solid colors. Bottom/Back:
        ///     On the left, one small image; on the right, one string of large text wrapped
        ///     over a maximum of three lines. On Windows Phone 8.1, the small image that
        ///     accompanies the text is not shown.
        /// </summary>
        [Obsolete("TileWidePeekImageCollection06 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageCollection06.")]
        TileWidePeekImageCollection06,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImageAndText01 Windows: Top: One wide image,
        ///     with a shorter height than a full-bleed wide image. Windows Phone 8.1: Front:
        ///     One wide image that fills the entire tile. Bottom/Back: One string of regular
        ///     text wrapped over a maximum of five lines.
        /// </summary>
        TileWide310x150PeekImageAndText01,
        /// <summary>
        ///     Windows: Top: One wide image, with a shorter height than a full-bleed wide
        ///     image. Windows Phone 8.1: Front: One wide image that fills the entire tile.
        ///     Bottom/Back: One string of regular text wrapped over a maximum of five lines.
        /// </summary>
        [Obsolete("TileWidePeekImageAndText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageAndText01.")]
        TileWidePeekImageAndText01,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImageAndText02 Top/Front: One wide image, with
        ///     a shorter height than a full-bleed wide image. Bottom/Back: Five strings
        ///     of regular text on five lines. Text does not wrap.
        /// </summary>
        TileWide310x150PeekImageAndText02,
        /// <summary>
        ///     Top/Front: One wide image, with a shorter height than a full-bleed wide image.
        ///     Bottom/Back: Five strings of regular text on five lines. Text does not wrap.
        /// </summary>
        [Obsolete("TileWidePeekImageAndText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImageAndText02.")]
        TileWidePeekImageAndText02,
        /// <summary>
        ///     Top/Front: One wide image. Bottom/Back: One header string in larger text
        ///     over one string of regular text that wraps over a maximum of four lines.
        /// </summary>
        [Obsolete("TileWidePeekImage01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImage01.")]
        TileWidePeekImage01,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImage01 Top/Front: One wide image. Bottom/Back:
        ///     One header string in larger text over one string of regular text that wraps
        ///     over a maximum of four lines.
        /// </summary>
        TileWide310x150PeekImage01,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImage02 Top/Front: One wide image. Bottom/Back:
        ///     One header string in larger text on the first line, four strings of regular
        ///     text on the next four lines. Text does not wrap.
        /// </summary>
        TileWide310x150PeekImage02,
        /// <summary>
        ///     Top/Front: One wide image. Bottom/Back: One header string in larger text
        ///     on the first line, four strings of regular text on the next four lines. Text
        ///     does not wrap.
        /// </summary>
        [Obsolete("TileWidePeekImage02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImage02.")]
        TileWidePeekImage02,
        /// <summary>
        ///     Top/Front: One wide image. Bottom/Back: One string of large text wrapped
        ///     over a maximum of three lines.
        /// </summary>
        [Obsolete("TileWidePeekImage03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImage03.")]
        TileWidePeekImage03,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImage03 Top/Front: One wide image. Bottom/Back:
        ///     One string of large text wrapped over a maximum of three lines.
        /// </summary>
        TileWide310x150PeekImage03,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImage04 Top/Front: One wide image. Bottom/Back:
        ///     One string of regular text wrapped over a maximum of five lines.
        /// </summary>
        TileWide310x150PeekImage04,
        /// <summary>
        ///     Top/Front: One wide image. Bottom/Back: One string of regular text wrapped
        ///     over a maximum of five lines.
        /// </summary>
        [Obsolete("TileWidePeekImage04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImage04.")]
        TileWidePeekImage04,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImage05 Top/Front: One wide image. Windows:
        ///     Bottom: On the left, one small image; on the right, one header string of
        ///     larger text on the first line over one string of regular text wrapped over
        ///     a maximum of four lines. Windows Phone 8.1: Back: One header string of larger
        ///     text on the first line over one string of regular text wrapped over a maximum
        ///     of four lines.
        /// </summary>
        TileWide310x150PeekImage05,
        /// <summary>
        ///     Top/Front: One wide image. Windows: Bottom: On the left, one small image;
        ///     on the right, one header string of larger text on the first line over one
        ///     string of regular text wrapped over a maximum of four lines. Windows Phone
        ///     8.1: Back: One header string of larger text on the first line over one string
        ///     of regular text wrapped over a maximum of four lines.
        /// </summary>
        [Obsolete("TileWidePeekImage05 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImage05.")]
        TileWidePeekImage05,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWidePeekImage06 Top/Front: One wide image. Windows:
        ///     Bottom: On the left, one small image; on the right, one string of large text
        ///     wrapped over a maximum of three lines. Windows Phone 8.1: Back: One string
        ///     of large text wrapped over a maximum of three lines.
        /// </summary>
        TileWide310x150PeekImage06,
        /// <summary>
        ///     Top/Front: One wide image. Windows: Bottom: On the left, one small image;
        ///     on the right, one string of large text wrapped over a maximum of three lines.
        ///     Windows Phone 8.1: Back: One string of large text wrapped over a maximum
        ///     of three lines.
        /// </summary>
        [Obsolete("TileWidePeekImage06 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150PeekImage06.")]
        TileWidePeekImage06,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideSmallImageAndText01 On the left, one small image;
        ///     on the right, one string of large text wrapped over a maximum of three lines.
        ///     On Windows Phone 8.1, the image is not shown.
        /// </summary>
        TileWide310x150SmallImageAndText01,
        /// <summary>
        ///     On the left, one small image; on the right, one string of large text wrapped
        ///     over a maximum of three lines. On Windows Phone 8.1, the image is not shown.
        /// </summary>
        [Obsolete("TileWideSmallImageAndText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150SmallImageAndText01.")]
        TileWideSmallImageAndText01,
        /// <summary>
        ///     On the left, one small image; on the right, one header string in larger text
        ///     on the first line, four strings of regular text on the next four lines. Text
        ///     does not wrap. On Windows Phone 8.1, the image is not shown.
        /// </summary>
        [Obsolete("TileWideSmallImageAndText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150SmallImageAndText02.")]
        TileWideSmallImageAndText02,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideSmallImageAndText02 On the left, one small image;
        ///     on the right, one header string in larger text on the first line, four strings
        ///     of regular text on the next four lines. Text does not wrap. On Windows Phone
        ///     8.1, the image is not shown.
        /// </summary>
        TileWide310x150SmallImageAndText02,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideSmallImageAndText03 On the left, one small image;
        ///     on the right, one string of regular text wrapped over a maximum of five lines.
        ///     On Windows Phone 8.1, the image is not shown.
        /// </summary>
        TileWide310x150SmallImageAndText03,
        /// <summary>
        ///     On the left, one small image; on the right, one string of regular text wrapped
        ///     over a maximum of five lines. On Windows Phone 8.1, the image is not shown.
        /// </summary>
        [Obsolete("TileWideSmallImageAndText03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150SmallImageAndText03.")]
        TileWideSmallImageAndText03,
        /// <summary>
        ///     On the left, one small image; on the right, one header string of larger text
        ///     on the first line over one string of regular text wrapped over a maximum
        ///     of four lines. On Windows Phone 8.1, the image is not shown.
        /// </summary>
        [Obsolete("TileWideSmallImageAndText04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150SmallImageAndText04.")]
        TileWideSmallImageAndText04,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideSmallImageAndText04 On the left, one small image;
        ///     on the right, one header string of larger text on the first line over one
        ///     string of regular text wrapped over a maximum of four lines. On Windows Phone
        ///     8.1, the image is not shown.
        /// </summary>
        TileWide310x150SmallImageAndText04,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideSmallImageAndText05 On the left, one header string
        ///     in larger text over one string of regular text wrapped over a maximum of
        ///     four lines; on the right, one small image with 3:4 dimensions. On Windows
        ///     Phone 8.1, the image is not shown.
        /// </summary>
        TileWide310x150SmallImageAndText05,
        /// <summary>
        ///     On the left, one header string in larger text over one string of regular
        ///     text wrapped over a maximum of four lines; on the right, one small image
        ///     with 3:4 dimensions. On Windows Phone 8.1, the image is not shown.
        /// </summary>
        [Obsolete("TileWideSmallImageAndText05 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150SmallImageAndText05.")]
        TileWideSmallImageAndText05,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideText01 One header string in larger text on the first
        ///     line, four strings of regular text on the next four lines. Text does not
        ///     wrap.
        /// </summary>
        TileWide310x150Text01,
        /// <summary>
        ///     One header string in larger text on the first line, four strings of regular
        ///     text on the next four lines. Text does not wrap.
        /// </summary>
        [Obsolete("TileWideText01 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text01.")]
        TileWideText01,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Windows 8 (Version 1) name/Windows 8.1 fallback attribute value: TileWideText02
        ///     One header string in larger text over eight short strings arranged in two
        ///     columns of four lines each. Columns are of equal width.
        /// </summary>
        TileWide310x150Text02,
        /// <summary>
        ///     One header string in larger text over eight short strings arranged in two
        ///     columns of four lines each. Columns are of equal width. This template is
        ///     similar to TileWideText07 and TileWideText10, but those templates use columns
        ///     of unequal width.
        /// </summary>
        [Obsolete("TileWideText02 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text02.")]
        TileWideText02,
        /// <summary>
        ///     One string of large text wrapped over a maximum of three lines.
        /// </summary>
        [Obsolete("TileWideText03 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text03.")]
        TileWideText03,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideText03 One string of large text wrapped over a maximum
        ///     of three lines.
        /// </summary>
        TileWide310x150Text03,
        /// <summary>
        ///     One string of regular text wrapped over a maximum of five lines.
        /// </summary>
        [Obsolete("TileWideText04 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text04.")]
        TileWideText04,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideText04 One string of regular text wrapped over a
        ///     maximum of five lines.
        /// </summary>
        TileWide310x150Text04,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideText05 Five strings of regular text on five lines.
        ///     Text does not wrap.
        /// </summary>
        TileWide310x150Text05,
        /// <summary>
        ///     Five strings of regular text on five lines. Text does not wrap.
        /// </summary>
        [Obsolete("TileWideText05 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text05.")]
        TileWideText05,
        /// <summary>
        ///     Ten short strings of regular text, arranged in two columns of five lines
        ///     each. Columns are of equal width. This template is similar to TileWideText08
        ///     and TileWideText11, but those templates use columns of unequal width.
        /// </summary>
        [Obsolete("TileWideText06 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text06.")]
        TileWideText06,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Windows 8 (Version 1) name/Windows 8.1 fallback attribute value: TileWideText06
        ///     Ten short strings of regular text, arranged in two columns of five lines
        ///     each. Columns are of equal width.
        /// </summary>
        TileWide310x150Text06,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Windows 8 (Version 1) name/Windows 8.1 fallback attribute value: TileWideText07
        ///     One header string in larger text over eight short strings of regular text
        ///     arranged in two columns of four lines each. The column widths are such that
        ///     the first column acts as a label and the second column as the content. This
        ///     template is similar to TileWideText10, but in that template the first column
        ///     is narrower.
        /// </summary>
        TileWide310x150Text07,
        /// <summary>
        ///     One header string in larger text over eight short strings of regular text
        ///     arranged in two columns of four lines each. The column widths are such that
        ///     the first column acts as a label and the second column as the content. This
        ///     template is similar to TileWideText10, which has an even narrower first column,
        ///     and TileWideText02, which has columns of equal width.
        /// </summary>
        [Obsolete("TileWideText07 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text07.")]
        TileWideText07,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Windows 8 (Version 1) name/Windows 8.1 fallback attribute value: TileWideText08
        ///     Ten short strings of regular text arranged in two columns of five lines each.
        ///     The column widths are such that the first column acts as a label and the
        ///     second column as the content. This template is similar to TileWideText11,
        ///     but in that template the first column is narrower.
        ///  </summary>
        TileWide310x150Text08,
        /// <summary>
        ///     Ten short strings of regular text arranged in two columns of five lines each.
        ///     The column widths are such that the first column acts as a label and the
        ///     second column as the content. This template is similar to TileWideText11,
        ///     which has an even narrower first column, and TileWideText06, which has columns
        ///     of equal width.
        /// </summary>
        [Obsolete("TileWideText08 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text08.")]
        TileWideText08,
        /// <summary>
        ///     One header string in larger text over one string of regular text wrapped
        ///     over a maximum of four lines.
        /// </summary>
        [Obsolete("TileWideText09 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text09.")]
        TileWideText09,
        /// <summary>
        ///     This value requires the version attribute of the template's visual element
        ///     to be set to at least "2". Windows 8 (Version 1) name/Windows 8.1 fallback
        ///     attribute value: TileWideText09 One header string in larger text over one
        ///     string of regular text wrapped over a maximum of four lines.
        /// </summary>
        TileWide310x150Text09,
        /// <summary>
        ///     One header string in larger text over eight short strings of regular text
        ///     arranged in two columns of four lines each. The column widths are such that
        ///     the first column acts as a label and the second column as the content. This
        ///     template is similar to TileWideText07, which has a wider first column, and
        ///     TileWideText02, which has columns of equal width.
        /// </summary>
        [Obsolete("TileWideText10 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text10.")]
        TileWideText10,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Windows 8 (Version 1) name/Windows 8.1 fallback attribute value: TileWideText10
        ///     One header string in larger text over eight short strings of regular text
        ///     arranged in two columns of four lines each. The column widths are such that
        ///     the first column acts as a label and the second column as the content. This
        ///     template is similar to TileWideText07, but in that template the first column
        ///     is wider.
        /// </summary>
        TileWide310x150Text10,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Windows 8 (Version 1) name/Windows 8.1 fallback attribute value: TileWideText11
        ///     Ten short strings of regular text arranged in two columns of five lines each.
        ///     The column widths are such that the first column acts as a label and the
        ///     second column as the content. This template is similar to TileWideText08,
        ///     but in that template the first column is wider.
        /// </summary>
        TileWide310x150Text11,
        /// <summary>
        ///     Ten short strings of regular text arranged in two columns of five lines each.
        ///     The column widths are such that the first column acts as a label and the
        ///     second column as the content. This template is similar to TileWideText08,
        ///     which has a wider first column, and TileWideText06, which has columns of
        ///     equal width.
        ///</summary>
        [Obsolete("TileWideText11 may be altered or unavailable for releases after Windows 8.1. Instead, use TileWide310x150Text11.")]
        TileWideText11,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Two main text groups separated by a blank area: One string of large
        ///     text, which can wrap over up to two lines, sitting over two strings of unwrapped
        ///     regular text on two lines. Four strings on four lines, separated slightly
        ///     into two sets. To the side is one string of large block text (this should
        ///     be numerical) over a single, short line of bold, regular text.
        /// </summary>
        TileSquare310x310BlockAndText01,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". One string of large block text (this should be numerical); two lines
        ///     of large header text (no wrap); two sets of two strings in two lines (no
        ///     wrap). Image in the background. If the text color is light, the image is
        ///     darkened a bit to improve the text visibility.
        /// </summary>
        TileSquare310x310BlockAndText02,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". One image that fills the entire tile; no text.
        /// </summary>
        TileSquare310x310Image,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". One image above one string of regular text wrapped over a maximum of
        ///     two lines. This template allows branding only as "logo" or "none", but not
        ///     "name". If you set the branding attribute to "name", it will automatically
        ///     revert to "logo" on Windows or "none" on Windows Phone 8.1.
        /// </summary>
        TileSquare310x310ImageAndText01,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". One image above two strings of regular text on two lines. Text does
        ///     not wrap. This template allows branding only as "logo" or "none", but not
        ///     "name". If you set the branding attribute to "name", it will automatically
        ///     revert to "logo" on Windows or "none" on Windows Phone 8.1.
        /// </summary>
        TileSquare310x310ImageAndText02,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Background: a single image that fills the entire tile. Foreground: One
        ///     string of text wrapped over a maximum of three lines. If the text color is
        ///     light, the image is darkened a bit to improve the text visibility.
        TileSquare310x310ImageAndTextOverlay01,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Background: a single image that fills the entire tile. Foreground: At
        ///     the top, one string of large text wrapped over a maximum of two lines; at
        ///     the bottom, one string of regular text wrapped over a maximum of three lines.
        ///     If the text color is light, the image is darkened a bit to improve the text
        ///     visibility.
        /// </summary>
        TileSquare310x310ImageAndTextOverlay02,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Background: a single image that fills the entire tile. Foreground: At
        ///     the top, one string of large text wrapped over a maximum of two lines; at
        ///     the bottom, three strings of regular text on three lines that do not wrap.
        ///     If the text color is light, the image is darkened a bit to improve the text
        ///     visibility.
        /// </summary>
        TileSquare310x310ImageAndTextOverlay03,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". TileSquare310x310ImageCollection with the addition of a text ribbon
        ///     across the bottom of the tile. The text area contains one string of regular
        ///     text wrapped over a maximum of two lines. Note that the small images cut
        ///     off the top of the large image while the text area cuts off the bottom of
        ///     the image. This template allows branding only as "logo" or "none", but not
        ///     "name". If you set the branding attribute to "name", it will automatically
        ///     revert to "logo" on Windows or "none" on Windows Phone 8.1.
        /// </summary>
        TileSquare310x310ImageCollectionAndText01,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". TileSquare310x310ImageCollection with the addition of a text ribbon
        ///     across the bottom of the tile. The text area contains two strings of regular
        ///     text on two lines. Text does not wrap. Note that the small images cut off
        ///     the top of the large image while the text area cuts off the bottom of the
        ///     image. This template allows branding only as "logo" or "none", but not "name".
        ///     If you set the branding attribute to "name", it will automatically revert
        ///     to "logo" on Windows or "none" on Windows Phone 8.1.
        /// </summary>
        TileSquare310x310ImageCollectionAndText02,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Four small square images overlaid across the top one large, full-tile,
        ///     square image. Note that the small images cut off the top of the large image.
        /// </summary>
        TileSquare310x310ImageCollection,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Three sets of information, each of which consists of one small square
        ///     image to the left of one header string in larger text over two strings of
        ///     regular text on the next two lines. Text does not wrap.
        /// </summary>
        TileSquare310x310SmallImagesAndTextList01,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Up to three sets of information, each of which consists of one small
        ///     square image to the left of one string of regular text wrapped over a maximum
        ///     of three lines.
        /// </summary>
        TileSquare310x310SmallImagesAndTextList02,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Up to three sets of information, each of which consists of one small
        ///     square image to the left of one string of large text over one string of regular
        ///     text wrapped over a maximum of two lines.
        /// </summary>
        TileSquare310x310SmallImagesAndTextList03,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Up to three sets of information, each of which consists of one small
        ///     rectangular image to the right of one string of large text over one string
        ///     of regular text wrapped over a maximum of two lines.
        /// </summary>
        TileSquare310x310SmallImagesAndTextList04,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". One header string in larger text on the first line, nine strings of
        ///     regular text on the next nine lines. Text does not wrap.
        /// </summary>
        TileSquare310x310Text01,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". One header string in larger text over eighteen short strings arranged
        ///     in two columns of nine lines each. Columns are of equal width.
        /// </summary>
        TileSquare310x310Text02,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Eleven strings of regular text on eleven lines. Text does not wrap.
        /// </summary>
        TileSquare310x310Text03,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Twenty-two short strings of regular text, arranged in two columns of
        ///     eleven lines each. Columns are of equal width. This template is similar to
        ///     TileSquare310x310Text06 and TileSquare310x310Text08, but those templates
        ///     have columns of unequal width.
        /// </summary>
        TileSquare310x310Text04,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". One header string in larger text over eighteen short strings of regular
        ///     text arranged in two columns of nine lines each. The column widths are such
        ///     that the first column acts as a label and the second column as the content.
        ///     This template is similar to TileSquare310x310Text07, which has an even narrower
        ///     first column, and TileSquare310x310Text02, which has columns of equal width.
        /// </summary>
        TileSquare310x310Text05,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Twenty-two short strings of regular text arranged in two columns of
        ///     eleven lines each. The column widths are such that the first column acts
        ///     as a label and the second column as the content. This template is similar
        ///     to TileSquare310x310Text08, which has an even narrower first column, and
        ///     TileSquare310x310Text04, which has columns of equal width.
        /// </summary>
        TileSquare310x310Text06,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". One header string in larger text over eighteen short strings of regular
        ///     text arranged in two columns of nine lines each. The column widths are such
        ///     that the first column acts as a label and the second column as the content.
        ///     This template is similar to TileSquare310x310Text05, which has a wider first
        ///     column, and TileSquare310x310Text02, which has columns of equal width.
        /// </summary>
        TileSquare310x310Text07,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Twenty-two short strings of regular text arranged in two columns of
        ///     eleven lines each. The column widths are such that the first column acts
        ///     as a label and the second column as the content. This template is similar
        ///     to TileSquare310x310Text06, which has a wider first column, and TileSquare310x310Text04,
        ///     which has columns of equal width.
        /// </summary>
        TileSquare310x310Text08,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Three stacked notices, each containing one header string in larger text
        ///     on the first line, two strings of regular text on the next two lines. Text
        ///     does not wrap.
        /// </summary>
        TileSquare310x310TextList01,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Three stacked notices, each containing one string of regular text wrapped
        ///     over a maximum of three lines.
        /// </summary>
        TileSquare310x310TextList02,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". Three stacked notices, each containing one header string in larger text
        ///     over one string of regular text wrapped over a maximum of two lines.
        /// </summary>
        TileSquare310x310TextList03,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". One medium-sized image in the upper left corner (upper right on right-to-left
        ///     systems) over a single, unwrapped header string. Beneath this are two sets
        ///     of regular text: the first wrapped over a maximum of two lines, the second
        ///     a single line only.
        /// </summary>
        TileSquare310x310SmallImageAndText01,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". One line of header text across the top, over three sets of information,
        ///     each of which consists of one small square image to the left of one header
        ///     string in larger text over two strings of regular text on the next two lines.
        ///     Text does not wrap.
        /// </summary>
        TileSquare310x310SmallImagesAndTextList05,
        /// <summary>
        ///     Windows only; not supported on Windows Phone 8.1 This value requires the
        ///     version attribute of the template's visual element to be set to at least
        ///     "2". One line of header text wrapped over a maximum of two lines. Beneath
        ///     are two more, slightly separated lines of header text, each one line only.
        ///     At the bottom are two lines of regular text, each of one line only.
        /// </summary>
        TileSquare310x310Text09,
        /// <summary>
        ///     Windows Phone only. This value requires the version attribute of the template's
        ///     visual element to be set to "3". A single icon image. The icon image's background
        ///     should be transparent to allow the tile color to show through. A badge, sent
        ///     through a separate notification, is shown to the right of the image. Unlike
        ///     other example images on this page, the badge (in this case, the number 13)
        ///     is included here. This template, being the small tile size, cannot display
        ///     the app name or logo declared in your manifest.
        /// </summary>
        TileSquare71x71IconWithBadge,
        /// <summary>
        /// Windows Phone only This value requires the version attribute of the template's
        ///     visual element to be set to "3". A single icon image. The icon image's background
        ///     should be transparent to allow the tile color to show through. A badge, sent
        ///     through a separate notification, is shown to the right of the image. Unlike
        ///     other example images on this page, the badge (in this case, the number 13)
        ///     is included here.
        /// </summary>
        TileSquare150x150IconWithBadge,
        /// <summary>
        ///     Windows Phone only This value requires the version attribute of the template's
        ///     visual element to be set to "3". In the lower right corner, a single icon
        ///     image. Treat this image as a logo image. The icon image's background should
        ///     be transparent to allow the tile color to show through. A badge, sent through
        ///     a separate notification, is shown to the right of the image. Unlike other
        ///     example images on this page, the badge (in this case, the number 13) is included
        ///     here. In the upper right corner, one header string in larger text on the
        ///     first line, two strings of regular text on the next two lines. Text does
        ///     not wrap. If no text elements are specified in this template, the icon image
        ///     and badge are centered in the tile.
        /// </summary>
        TileWide310x150IconWithBadgeAndText,
        /// <summary>
        ///     Windows Phone only This value requires the version attribute of the template's
        ///     visual element to be set to "3". One square image that fills the entire tile,
        ///     no text.
        /// </summary>
        TileSquare71x71Image,
    }
}
