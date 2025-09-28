using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

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
            if (x is < 0)
            {
                w += x;
                x = 0;
            }
            if (y is < 0)
            {
                h += y;
                y = 0;
            }
            w = Math.Min(w, width - x);
            h = Math.Min(h, height - y);
            if (w is > 0 && h is > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Adjust(ref Rectangle rect, int width, int height)
        {
            var (x, y, w, h) = rect;
            if (Adjust(ref x, ref y, ref w, ref h, width, height))
            {
                rect = new(x, y, w, h);
                return true;
            }
            else
            {
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Adjust(ref int x, ref int y, ref int width, ref int height, LnBitmapData bitmap) => Adjust(ref x, ref y, ref width, ref height, bitmap.Width, bitmap.Height);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Adjust(ref Rectangle rect, LnBitmapData bitmap) => Adjust(ref rect, bitmap.Width, bitmap.Height);
    }
}
