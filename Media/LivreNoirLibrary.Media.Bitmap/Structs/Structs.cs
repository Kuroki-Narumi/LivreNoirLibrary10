using LivreNoirLibrary.Numerics;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media
{
    public static class Structs
    {
        public static void Deconstruct(this in Point point, out int x, out int y)
        {
            x = point.X;
            y = point.Y;
        }

        public static void Deconstruct(this in Size size, out int width, out int height)
        {
            width = size.Width;
            height = size.Height;
        }

        public static void Deconstruct(this in Rectangle rect, out int x, out int y, out int width, out int height)
        {
            x = rect.X;
            y = rect.Y;
            width = rect.Width;
            height = rect.Height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Adjust(ref Rectangle rect, int width, int height)
        {
            var (x, y, w, h) = rect;
            var x1 = Math.Max(x, 0);
            var y1 = Math.Max(y, 0);
            w = Math.Min(x + w, width) - x1;
            h = Math.Min(y + h, height) - y1;
            if (w is > 0 && h is > 0)
            {
                rect = new(x1, y1, w, h);
                return true;
            }
            else
            {
                rect = default;
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Adjust(ref Rectangle sourceRect, ref Point destLocation, int sourceWidth, int sourceHeight, int destWidth, int destHeight)
        {
            var (sx, sy, w, h) = sourceRect;
            var (dx, dy) = destLocation;

            // 実際のクリップ範囲
            // [クリップ元 左上] と [クリップ有効範囲 左上] のうち、より右下にあるほう
            var sourceLeft = Math.Max(sx, 0);
            var sourceTop = Math.Max(sy, 0);
            // [クリップ元 右下] と [クリップ有効範囲 右下] のうち、より左上にあるほう
            var sourceRight = Math.Min(sx + w, sourceWidth);
            var sourceBottom = Math.Min(sx + h, sourceHeight);

            // 実際の貼り付け範囲
            // [貼り付け先 左上] と [貼り付け有効範囲 左上] のうち、より右下にあるほう
            var destLeft = Math.Max(dx, 0);
            var destTop = Math.Max(dy, 0);
            // [貼り付け先 右下] と [貼り付け有効範囲 右下] のうち、より左上にあるほう
            var destRight = Math.Min(dx + w, destWidth);
            var destBottom = Math.Min(dy + h, destHeight);

            // [クリップ元 範囲] を [貼り付け先 範囲] に合わせる
            sourceLeft = Math.Max(sourceLeft, sx + (destLeft - dx));
            sourceTop = Math.Max(sourceTop, sy + (destTop - dy));
            // [貼り付け先 範囲] を [クリップ元 範囲] に合わせる
            destLeft = Math.Max(destLeft, dx + (sourceLeft - sx));
            destTop = Math.Max(destTop, dy + (sourceTop - sy));
            // 実際の貼り付けサイズ
            w = Math.Min(sourceRight - sourceLeft, destRight - destLeft);
            h = Math.Min(sourceBottom - sourceTop, destBottom - destTop);

            sourceRect = new(sourceLeft, sourceTop, w, h);
            destLocation = new(destLeft, destTop);
            return w is > 0 && h is > 0;
        }

        /// <summary>
        /// <paramref name="sourceValidRect"/>で規定された有効範囲から<paramref name="sourceRect"/>で指定された範囲を切り抜き、
        /// <paramref name="destValidRect"/>で規定された有効範囲内の<paramref name="destRect"/>で指定された範囲に貼り付けようとした際の、
        /// 実際に参照するべき切り抜き元範囲を<paramref name="actualSourceRect"/>、実際の貼り付け先範囲を<paramref name="actualDestRect"/>へ格納し、
        /// 切り抜き元及び貼り付け先の範囲が全て有効であるかどうかを返します。
        /// </summary>
        /// <param name="sourceValidRect">クリップ有効範囲</param>
        /// <param name="sourceRect">クリップ元範囲</param>
        /// <param name="destValidRect">貼り付け有効範囲</param>
        /// <param name="destRect">貼り付け先範囲</param>
        /// <param name="actualSourceRect">正規化されたクリップ元範囲</param>
        /// <param name="actualDestRect">正規化された貼り付け先範囲</param>
        /// <returns><see langword="true"/> if both <paramref name="actualSourceRect"/> and <paramref name="actualDestRect"/> are valid; otherwise, <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AdjustStretch(
            in DoubleRect sourceValidRect, in DoubleRect sourceRect, 
            in DoubleRect destValidRect, in DoubleRect destRect, 
            out Rectangle actualSourceRect, out Rectangle actualDestRect)
        {
            var (sourceOrigX, sourceOrigY, sourceOrigWidth, sourceOrigHeight) = sourceValidRect;
            var (sourceX, sourceY, sourceWidth, sourceHeight) = sourceRect;
            var (destOrigX, destOrigY, destOrigWidth, destOrigHeight) = destValidRect;
            var (destX, destY, destWidth, destHeight) = destRect;
            // いずれかの領域がゼロである場合は無効
            if (sourceOrigWidth is <= 0 || sourceOrigHeight is <= 0 || sourceWidth is <= 0 || sourceHeight is <= 0 || 
                destOrigWidth is <= 0 || destOrigHeight is <= 0 || destWidth is <= 0 || destHeight is <= 0)
            {
                actualSourceRect = default;
                actualDestRect = default;
                return false;
            }

            // 拡大率
            var scaleX = destWidth / sourceWidth;
            var scaleY = destHeight / sourceHeight;

            // 実際のクリップ範囲
            // [クリップ元 左上] と [クリップ有効範囲 左上] のうち、より右下にあるほう
            var sourceLeft = Math.Max(sourceX, sourceOrigX);
            var sourceTop = Math.Max(sourceY, sourceOrigY);
            // [クリップ元 右下] と [クリップ有効範囲 右下] のうち、より左上にあるほう
            var sourceRight = Math.Min(sourceX + sourceWidth, sourceOrigX + sourceOrigWidth);
            var sourceBottom = Math.Min(sourceY + sourceHeight, sourceOrigY + sourceOrigHeight);

            // 実際の貼り付け範囲
            // [貼り付け先 左上] と [貼り付け有効範囲 左上] のうち、より右下にあるほう
            var destLeft = Math.Max(destX, destOrigX);
            var destTop = Math.Max(destY, destOrigY);
            // [貼り付け先 右下] と [貼り付け有効範囲 右下] のうち、より左上にあるほう
            var destRight = Math.Min(destX + destWidth, destOrigX + destOrigWidth);
            var destBottom = Math.Min(destY + destHeight, destOrigY + destOrigHeight);

            // [クリップ元 範囲] を [貼り付け先 範囲] に合わせる
            sourceLeft = Math.Max(sourceLeft, sourceX + (destLeft - destX) / scaleX);
            sourceTop = Math.Max(sourceTop, sourceY + (destTop - destY) / scaleY);
            sourceWidth = Math.Min(sourceRight - sourceLeft, (destRight - destLeft) / scaleX);
            sourceHeight = Math.Min(sourceBottom - sourceTop, (destBottom - destTop) / scaleY);

            // [貼り付け先 範囲] を [クリップ元 範囲] に合わせる
            destLeft = Math.Max(destLeft, destX + (sourceLeft - sourceX) * scaleX);
            destTop = Math.Max(destTop, destY + (sourceTop - sourceY) * scaleY);
            destWidth = sourceWidth * scaleX;
            destHeight = sourceHeight * scaleY;

            actualSourceRect = new((int)sourceLeft, (int)sourceTop, (int)Math.Ceiling(sourceWidth), (int)Math.Ceiling(sourceHeight));
            actualDestRect = new(destLeft.RoundToInt(), destTop.RoundToInt(), destWidth.RoundToInt(), destHeight.RoundToInt());

            // 範囲矯正の結果幅や高さが有効でなくなる可能性がある
            return actualSourceRect.Width is > 0 && actualSourceRect.Height is > 0 && actualDestRect.Width is > 0 && actualDestRect.Height is > 0;
        }
    }
}
