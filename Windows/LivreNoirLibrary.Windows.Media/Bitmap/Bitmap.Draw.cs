using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public static partial class Bitmap
    {
        public static unsafe void FillRect(this WriteableBitmap bitmap, int x, int y, int width, int height, Color color)
        {
            using BitmapPointer p = new(bitmap);
            LivreNoirLibrary.Media.BitmapOperation.FillRect(p, new(x, y, width, height), color.ToLnColor());
        }

        public static void FillRect(this WriteableBitmap bitmap, Int32Rect rect, Color color) => FillRect(bitmap, rect.X, rect.Y, rect.Width, rect.Height, color);

        public static unsafe void FillRect(this WriteableBitmap bitmap, int x, int y, int width, int height, Color color1, Color color2, bool vertical = false)
        {
            using BitmapPointer p = new(bitmap);
            LivreNoirLibrary.Media.BitmapOperation.FillRect(p, new(x, y, width, height), color1.ToLnColor(), color2.ToLnColor(), vertical);
        }

        public static void FillRect(this WriteableBitmap bitmap, Int32Rect rect, Color color1, Color color2, bool vertical = false) 
            => FillRect(bitmap, rect.X, rect.Y, rect.Width, rect.Height, color1, color2, vertical);

        public static unsafe void FillTriangle(this WriteableBitmap bitmap, int x0, int y0, int x1, int y1, int x2, int y2, Color color)
        {
            using BitmapPointer p = new(bitmap);
            LivreNoirLibrary.Media.BitmapOperation.FillTriangle(p, new(x0, y0, x1, y1, x2, y2), color.ToLnColor());
        }

        public static unsafe void FillTriangle(this WriteableBitmap bitmap, int x0, int y0, int x1, int y1, int x2, int y2, Color color1, Color color2, bool radial = false)
        {
            using BitmapPointer p = new(bitmap);
            LivreNoirLibrary.Media.BitmapOperation.FillTriangle(p, new(x0, y0, x1, y1, x2, y2), color1.ToLnColor(), color2.ToLnColor(), radial);
        }

        public static unsafe void DrawBorder(this WriteableBitmap bitmap, int thickness, Color color, bool keepSource = false)
        {
            using BitmapPointer p = new(bitmap);
            LivreNoirLibrary.Media.BitmapOperation.DrawBorder(p, thickness, color.ToLnColor(), keepSource);
        }

        public static unsafe void DrawBorderSimple(this WriteableBitmap bitmap, int thickness, Color color)
        {
            using BitmapPointer p = new(bitmap);
            LivreNoirLibrary.Media.BitmapOperation.DrawBorderSimple(p, thickness, color.ToLnColor());
        }
    }
}
