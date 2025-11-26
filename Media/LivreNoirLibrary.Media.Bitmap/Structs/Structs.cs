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

        public static void Deconstruct(this in Rectangle rect, out Point point, out Size size)
        {
            point = rect.Location;
            size = rect.Size;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Adjust(ref int x, ref int y, ref int w, ref int h, int width, int height)
        {
            var x1 = Math.Max(x, 0);
            var y1 = Math.Max(y, 0);
            w = Math.Min(x + w, width) - x1;
            h = Math.Min(y + h, height) - y1;
            if (w is > 0 && h is > 0)
            {
                x = x1;
                y = y1;
                return true;
            }
            else
            {
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Adjust(
            ref int sx, ref int sy, ref int sw, ref int sh, 
            ref int dx, ref int dy,
            int sourceWidth, int sourceHeight, int destWidth, int destHeight)
        {
            // コピー先の実際の範囲
            var x1 = Math.Max(dx, 0);
            var y1 = Math.Max(dy, 0);
            sw = Math.Min(dx + sw, destWidth) - x1;
            sh = Math.Min(dy + sh, destHeight) - y1;
            if (sw is <= 0 || sh is <= 0)
            {
                return false;
            }
            // コピーする座標を実際の範囲に合わせてシフト
            sx += x1 - dx;
            sy += y1 - dy;
            dx = x1;
            dy = y1;
            return Adjust(ref sx, ref sy, ref sw, ref sh, sourceWidth, sourceHeight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Adjust(ref Rectangle rect, int width, int height)
        {
            var (x, y, w, h) = rect;
            var result = Adjust(ref x, ref y, ref w, ref h, width, height);
            rect = new(x, y, w, h);
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Adjust(ref Rectangle sourceRect, ref Point destLocation, int sourceWidth, int sourceHeight, int destWidth, int destHeight)
        {
            var (sx, sy, sw, sh) = sourceRect;
            var (dx, dy) = destLocation;
            var result = Adjust(ref sx, ref sy, ref sw, ref sh, ref dx, ref dy, sourceWidth, sourceHeight, destWidth, destHeight);
            sourceRect = new(sx, sy, sw, sh);
            destLocation = new(dx, dy);
            return result;
        }
    }
}
