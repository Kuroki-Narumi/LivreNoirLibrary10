using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public static class NoteRenderer
    {
        private static readonly Dictionary<RectKey, WriteableBitmap> _rectCache = [];
        private static readonly Dictionary<RectKey, WriteableBitmap> _selectedRectCache = [];

        public static uint HilightColor { get; } = Colors.NoteHilight.ToUInt();
        public static uint ShadowColor { get; } = Colors.NoteShadow.ToUInt();
        public static uint SelectedStrokeColor { get; } = Colors.SelectedStroke.ToUInt();

        static WriteableBitmap CreateCore(in RectKey key, uint leftTop, uint rightBottom)
        {
            var width = key.Width;
            var height = key.Height;
            var fill = key.Color;
            var bitmap = Bitmap.Create(width, height);
            using (var p = bitmap.BeginWrite())
            {
                // 上
                p.Fill(new(0, 0, width, 1), leftTop);
                // 左
                p.Fill(new(0, 1, 1, height - 2), leftTop);
                // 下
                p.Fill(new(0, height - 1, width, 1), rightBottom);
                // 右
                p.Fill(new(width - 1, 1, 1, height - 2), rightBottom);
                // 中
                p.Fill(new(1, 1, width - 2, height - 2), fill);
            }
            bitmap.Freeze();
            return bitmap;
        }

        public static WriteableBitmap GetRect(int width, int height, Color color)
        {
            return _rectCache.GetOrAdd(new(width, height, color), k => CreateCore(k, HilightColor, ShadowColor));
        }

        public static WriteableBitmap GetSelectedRect(int width, int height, Color color)
        {
            return _selectedRectCache.GetOrAdd(new(width, height, color), k => CreateCore(k, SelectedStrokeColor, SelectedStrokeColor));
        }

        public static void DrawNoteRect(this DrawingContext ctx, int x, int y, int width, int height, Color color)
        {
            var bitmap = GetRect(width, height, color);
            ctx.DrawImage(bitmap, new(x, y, width, height));
        }

        public static void DrawSelectedNoteRect(this DrawingContext ctx, int x, int y, int width, int height, Color color)
        {
            var bitmap = GetSelectedRect(width, height, color);
            ctx.DrawImage(bitmap, new(x, y, width, height));
        }

        private readonly struct RectKey(int width, int height, Color color)
        {
            public int Width { get; } = width;
            public int Height { get; } = height;
            public uint Color { get; } = color.ToUInt();

            public override int GetHashCode() => HashCode.Combine(Width, Height, Color);
            public override bool Equals([NotNullWhen(true)] object? obj) => obj is RectKey other
                && Width == other.Width
                && Height == other.Height
                && Color == other.Color;
        }
    }
}
