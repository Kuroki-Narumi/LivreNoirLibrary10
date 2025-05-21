using System;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public static partial class WindowsExtensions
    {
        public static Int32Rect ToInt32Rect(this Rect rect)
        {
            return new((int)rect.X, (int)rect.Y, (int)Math.Ceiling(rect.Width), (int)Math.Ceiling(rect.Height));
        }

        public static Rect ToRect(this Int32Rect rect)
        {
            return new(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Point Offset(this Point point, double offsetX, double offsetY)
        {
            return new(point.X + offsetX, point.Y + offsetY);
        }

        public static Point Round(this Point point)
        {
            return new(Math.Round(point.X), Math.Round(point.Y));
        }

        public static Point Ceiling(this Point point)
        {
            return new(Math.Ceiling(point.X), Math.Ceiling(point.Y));
        }

        public static Point Floor(this Point point)
        {
            return new(Math.Floor(point.X), Math.Floor(point.Y));
        }

        public static Point Truncate(this Point point)
        {
            return new(Math.Truncate(point.X), Math.Truncate(point.Y));
        }

        public static void Deconstruct(this Rect rect, out double x, out double y, out double width, out double height)
        {
            x = rect.X;
            y = rect.Y;
            width = rect.Width;
            height = rect.Height;
        }

        public static void Deconstruct(this Int32Rect rect, out double x, out double y, out double width, out double height)
        {
            x = rect.X;
            y = rect.Y;
            width = rect.Width;
            height = rect.Height;
        }

        public static void Deconstruct(this System.Drawing.Rectangle rect, out double x, out double y, out double width, out double height)
        {
            x = rect.X;
            y = rect.Y;
            width = rect.Width;
            height = rect.Height;
        }

        public static void Deconstruct(this Point point, out double x, out double y)
        {
            x = point.X;
            y = point.Y;
        }

        public static void Deconstruct(this System.Drawing.Point point, out double x, out double y)
        {
            x = point.X;
            y = point.Y;
        }

        public static void Deconstruct(this Size size, out double width, out double height)
        {
            width = size.Width;
            height = size.Height;
        }
    }
}
