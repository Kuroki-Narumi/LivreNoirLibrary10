using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public static partial class DependencyObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect ToRect(this in Int32Rect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect ToInt32Rect(this in Rect rect) => new((int)rect.X, (int)rect.Y, (int)Math.Ceiling(rect.Width), (int)Math.Ceiling(rect.Height));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect ToRect(this in System.Drawing.Rectangle rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect ToInt32Rect(this in System.Drawing.Rectangle rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Drawing.Rectangle ToFormsRect(this in Rect rect) => new((int)rect.X, (int)rect.Y, (int)Math.Ceiling(rect.Width), (int)Math.Ceiling(rect.Height));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Drawing.Rectangle ToFormsRect(this in Int32Rect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point ToPoint(this in System.Drawing.Point point) => new(point.X, point.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Drawing.Point ToFormsPoint(this in Point point) => new((int)point.X, (int)point.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Round(this ref Point point)
        {
            point.X = Math.Round(point.X);
            point.Y = Math.Round(point.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Ceiling(this ref Point point)
        {
            point.X = Math.Ceiling(point.X);
            point.Y = Math.Ceiling(point.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Floor(this ref Point point)
        {
            point.X = Math.Floor(point.X);
            point.Y = Math.Floor(point.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Truncate(this ref Point point)
        {
            point.X = Math.Truncate(point.X);
            point.Y = Math.Truncate(point.Y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Width, int Height) ToInt(this in Point size) => ((int)size.X, (int)size.Y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Width, int Height) ToInt(this in Size size) => ((int)size.Width, (int)size.Height);

        public static void Deconstruct(this in Rect rect, out double x, out double y, out double width, out double height)
        {
            x = rect.X;
            y = rect.Y;
            width = rect.Width;
            height = rect.Height;
        }

        public static void Deconstruct(this in Int32Rect rect, out int x, out int y, out int width, out int height)
        {
            x = rect.X;
            y = rect.Y;
            width = rect.Width;
            height = rect.Height;
        }

        public static void Deconstruct(this in System.Drawing.Rectangle rect, out int x, out int y, out int width, out int height)
        {
            x = rect.X;
            y = rect.Y;
            width = rect.Width;
            height = rect.Height;
        }

        public static void Deconstruct(this in Point point, out double x, out double y)
        {
            x = point.X;
            y = point.Y;
        }

        public static void Deconstruct(this in System.Drawing.Point point, out int x, out int y)
        {
            x = point.X;
            y = point.Y;
        }

        public static void Deconstruct(this in Size size, out double width, out double height)
        {
            width = size.Width;
            height = size.Height;
        }
    }
}
