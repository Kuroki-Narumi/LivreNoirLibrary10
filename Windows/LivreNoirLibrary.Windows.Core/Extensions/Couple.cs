using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public static partial class StructExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Point ToPoint(this in System.Drawing.Point point) => new(point.X, point.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Drawing.Point ToDrawingPoint(this in Point point) => new((int)point.X, (int)point.Y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size ToSize(this in System.Drawing.Size size) => new(size.Width, size.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static System.Drawing.Size ToDrawingSize(this in Size size) => new((int)size.Width, (int)size.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Width, int Height) ToInt(this in Point size) => ((int)size.X, (int)size.Y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Width, int Height) ToInt(this in Size size) => ((int)size.Width, (int)size.Height);

        public static void Deconstruct(this in Point point, out double x, out double y)
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
