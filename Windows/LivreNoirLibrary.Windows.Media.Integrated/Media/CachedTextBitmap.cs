using LivreNoirLibrary.Media;
using LivreNoirLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Media
{
    public static class CachedTextBitmap
    {
        public const double BaseFontSize = 11;

        private static readonly SolidColorBrush _shadowBrush = MediaUtils.GetBrush(96, 0, 0, 0);
        private static readonly Dictionary<TextKey, RenderTargetBitmap> _cache1 = [];
        private static readonly Dictionary<TextKeyWithColor, WriteableBitmap> _cache2 = [];

        public static void Clear()
        {
            _cache1.Clear();
            _cache2.Clear();
        }

        private static RenderTargetBitmap CreateRenderTarget(int width, int height, Visual visual)
        {
            RenderTargetBitmap bitmap = new(width, height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            bitmap.Freeze();
            return bitmap;
        }

        private static readonly FormattedTextOptions _ft_options = new()
        {
            FontFamily = new("Consolas, MS Gothic"),
            Foreground = Brushes.White,
            FontWeight = FontWeights.Bold,
        };

        private static RenderTargetBitmap CreateTextBitmap(TextKey key)
        {
            var scale = key.Scale;
            var text = key.Text;
            var options = _ft_options;
            options.FontSize = BaseFontSize * scale;
            options.Foreground = Brushes.White;
            var foreground = text.CreateFormattedText(options);
            options.Foreground = _shadowBrush;
            var background = text.CreateFormattedText(options);

            DrawingVisual dv = new();
            using (var ctx = dv.RenderOpen())
            {
                // 上下左右
                ctx.DrawText(background, new(0, 1));
                ctx.DrawText(background, new(1, 0));
                ctx.DrawText(background, new(1, 2));
                ctx.DrawText(background, new(2, 1));
                // 斜め
                ctx.DrawText(background, new(0, 0));
                ctx.DrawText(background, new(0, 2));
                ctx.DrawText(background, new(2, 0));
                ctx.DrawText(background, new(2, 2));

                ctx.DrawText(foreground, new(1, 1));
            }
            var bitmap = CreateRenderTarget((int)Math.Ceiling(foreground.Width + 2), (int)Math.Ceiling(foreground.Height + 2), dv);
            return bitmap;
        }

        private static WriteableBitmap CreateTextBitmap(TextKeyWithColor key)
        {
            var source = GetTextBitmap(key.Text, key.Scale);
            var bitmap = Bitmap.Create(source);
            using (var p = bitmap.BeginWrite())
            {
                p.Blend(BlendMode.Multiply, key.Color);
            }
            bitmap.Freeze();
            return bitmap;
        }

        public static RenderTargetBitmap GetTextBitmap(string text, double scale = 1) => _cache1.GetOrAdd(new(scale, text), CreateTextBitmap);

        public static BitmapSource GetTextBitmap(string text, Color color, double scale = 1) => _cache2.GetOrAdd(new(scale, color, text), CreateTextBitmap);

        private static void DrawImageCore(DrawingContext ctx, BitmapSource bitmap, double x, double y, double originX, double originY)
        {
            var w = bitmap.Width;
            var h = bitmap.Height;
            ctx.DrawImage(bitmap, new Rect(x - w * originX, y - h * originY, w, h));
        }

        public static void DrawCachedText(this DrawingContext ctx, double x, double y, string? text, double scale = 1, double originX = 0, double originY = 0)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var bitmap = GetTextBitmap(text, scale);
                DrawImageCore(ctx, bitmap, x, y, originX, originY);
            }
        }

        public static void DrawCachedText(this DrawingContext ctx, double x, double y, string? text, Color color, double scale = 1, double originX = 0, double originY = 0)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var bitmap = GetTextBitmap(text, color, scale);
                DrawImageCore(ctx, bitmap, x, y, originX, originY);
            }
        }

        private readonly struct TextKey(double scale, string text)
        {
            public double Scale { get; } = scale;
            public string Text { get; } = text;

            public override int GetHashCode() => HashCode.Combine(Scale, Text);
            public override bool Equals([NotNullWhen(true)] object? obj) => obj is TextKey other 
                && Scale == other.Scale 
                && string.Equals(Text, other.Text, StringComparison.Ordinal);
        }

        private readonly struct TextKeyWithColor(double scale, Color color, string text)
        {
            public double Scale { get; } = scale;
            public uint Color { get; } = color.ToUInt();
            public string Text { get; } = text;

            public override int GetHashCode() => HashCode.Combine(Scale, Color, Text);
            public override bool Equals([NotNullWhen(true)] object? obj) => obj is TextKeyWithColor other 
                && Scale == other.Scale 
                && Color == other.Color
                && string.Equals(Text, other.Text, StringComparison.Ordinal);
        }
    }
}
