using LivreNoirLibrary.Media;
using LivreNoirLibrary.Numerics;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public static partial class StructExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect ToRect(this in Rectangle rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect ToInt32Rect(this in Rectangle rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect ToRect(this in Int32Rect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect ToInt32Rect(this in Rect rect) => new(rect.X.RoundToInt(), rect.Y.RoundToInt(), rect.Width.RoundToInt(), rect.Height.RoundToInt());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect ToRect(this in DoubleRect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect ToInt32Rect(this in DoubleRect rect) => new(rect.X.RoundToInt(), rect.Y.RoundToInt(), rect.Width.RoundToInt(), rect.Height.RoundToInt());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleRect ToDoubleRect(this in Int32Rect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DoubleRect ToDoubleRect(this in Rect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect ToRect(this (int, int, int, int) rect) => new(rect.Item1, rect.Item2, rect.Item3, rect.Item4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect ToInt32Rect(this (int, int, int, int) rect) => new(rect.Item1, rect.Item2, rect.Item3, rect.Item4);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle ToDrawingRect(this in Rect rect) => new(rect.X.RoundToInt(), rect.Y.RoundToInt(), rect.Width.RoundToInt(), rect.Height.RoundToInt());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rectangle ToDrawingRect(this in Int32Rect rect) => new(rect.X, rect.Y, rect.Width, rect.Height);

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

        extension(Int32Rect rect)
        {
            public int Left => rect.X;
            public int Right => rect.X + rect.Width;
            public int Top => rect.Y;
            public int Bottom => rect.Y + rect.Height;
        }
    }
}
