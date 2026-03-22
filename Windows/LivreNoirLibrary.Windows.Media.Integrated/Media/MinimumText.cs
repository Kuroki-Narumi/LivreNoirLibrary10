using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

namespace LivreNoirLibrary.Windows.Media
{
    public static class MinimumText
    {
        private const string ResourcePath = "pack://application:,,,/LivreNoirLibrary.Windows.Media;component/Resources/IndexLetters.png";
        private const string ResourcePath2 = "pack://application:,,,/LivreNoirLibrary.Windows.Media;component/Resources/IndexLetters2.png";
        private const int HorizontalCount = 10;
        private const int CharWidth = 7;
        private const int CharHeight = 9;
        private const int CharWidth2 = 12;
        private const int CharHeight2 = 18;

        private static readonly WriteableBitmap _letterBitmap;
        private static readonly Dictionary<char, Rectangle> _rects = [];
        private static readonly WriteableBitmap _letterBitmap2;
        private static readonly Dictionary<char, Rectangle> _rects2 = [];

        private static readonly LnColor _defaultColor = LnColor.FromRgb(255, 255, 255);
        private static readonly Dictionary<string, WriteableBitmap> _whiteCache = [];
        private static readonly Dictionary<LnColor, Dictionary<string, WriteableBitmap>> _coloredCache = [];

        static MinimumText()
        {
            (_letterBitmap, _rects) = CreateSource1();
            (_letterBitmap2, _rects2) = CreateSource2();
        }

        private static (WriteableBitmap, Dictionary<char, Rectangle>) CreateSource1()
        {
            var bitmap = Bitmap.FromResource(ResourcePath);
            bitmap.Freeze();
            Dictionary<char, Rectangle> rects = [];
            void Add(char c, int x, int y, int w = CharWidth)
            {
                rects.Add(c, new(x, y, w, CharHeight));
            }
            for (int i = 0; i < 62; i++)
            {
                var x = (i % HorizontalCount) * CharWidth;
                var y = (i / HorizontalCount) * CharHeight;
                var c = i.ToBased(62, 1)[0];
                Add(c, x, y);
            }
            var xx = CharWidth * 2;
            var yy = CharHeight * 6;
            void Add2(char c, int w = CharWidth)
            {
                Add(c, xx, yy, w);
                xx += w;
            }
            Add2('.', 4);
            Add2(':', 4);
            Add2('/', 5);
            Add2('(', 4);
            Add2(')', 4);
            Add2('+');
            Add2('-', 6);
            xx += 1;
            Add2('*');
            Add2('^');
            Add2('%');
            xx = 0;
            yy += CharHeight;
            Add2('!');
            Add2('?');
            Add2('#');
            return (bitmap, rects);
        }

        private static (WriteableBitmap, Dictionary<char, Rectangle>) CreateSource2()
        {
            var bitmap = Bitmap.FromResource(ResourcePath2);
            bitmap.Freeze();
            Dictionary<char, Rectangle> rects = [];
            void Add(char c, int x, int y, int w = CharWidth2)
            {
                rects.Add(c, new(x, y, w, CharHeight2));
            }
            for (int i = 0; i < 62; i++)
            {
                var x = (i % HorizontalCount) * CharWidth2;
                var y = (i / HorizontalCount) * CharHeight2;
                var c = i.ToBased(62, 1)[0];
                Add(c, x, y);
            }
            var xx = CharWidth2 * 2;
            var yy = CharHeight2 * 6;
            void Add2(char c, int w = CharWidth2)
            {
                Add(c, xx, yy, w);
                xx += w;
            }
            Add2('.', 4);
            Add2(':', 4);
            xx += 4;
            Add2('/', 11);
            xx += 1;
            Add2('(', 6);
            Add2(')', 6);
            Add2('+');
            Add2('-');
            Add2('*');
            Add2('^');
            Add2('%');
            xx = 0;
            yy += CharHeight2;
            Add2('!');
            Add2('?');
            Add2('#');
            return (bitmap, rects);
        }

        public static WriteableBitmap GetBitmap(string text) => _whiteCache.GetOrAdd(text, CreateBitmap);

        private static WriteableBitmap CreateBitmap(string text)
        {
            using var o = ObjectPool.Rent<List<(Rectangle, Point)>>();
            var buffer = o.Value;
            var x = 0;
            foreach (var c in text)
            {
                var w = 1;
                if (c is ' ')
                {
                    w = CharWidth;
                }
                else if (_rects.TryGetValue(c, out var rect))
                {
                    w = rect.Width;
                    buffer.Add((rect, new(x, 0)));
                }
                x += w - 1;
            }
            x += 1;
            var bitmap = Bitmap.Create(x, CharHeight);
            using (var src = _letterBitmap.BeginRead())
            using (var dest = bitmap.BeginWrite())
            {
                foreach (var (rect, point) in buffer.AsSpan())
                {
                    src.CopyTo(dest, rect, point);
                }
            }
            bitmap.Freeze();
            return bitmap;
        }

        public static WriteableBitmap GetBitmap(string text, Color color)
        {
            var lnColor = color.ToLnColorWithoutAlpha();
            if (lnColor == _defaultColor)
            {
                return GetBitmap(text);
            }
            var dic = _coloredCache.GetOrAdd(lnColor);
            if (!dic.TryGetValue(text, out var bitmap))
            {
                var source = GetBitmap(text);
                bitmap = Bitmap.Create(source.PixelWidth, source.PixelHeight);
                using (var src = source.BeginRead())
                using (var dest = bitmap.BeginWrite())
                {
                    src.CopyTo(dest, lnColor.ToFloatColor());
                }
                bitmap.Freeze();
                dic.Add(text, bitmap);
            }
            return bitmap;
        }

        public static void DrawMinimumText(this DrawingContext ctx, int x, int y, string? text, double scale = 1)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var bitmap = GetBitmap(text);
                var w = bitmap.PixelWidth * scale;
                var h = bitmap.PixelHeight * scale;
                ctx.DrawImage(bitmap, new(x, y - h, w, h));
            }
        }

        public static void DrawMinimumText(this DrawingContext ctx, int x, int y, string? text, Color color, double scale = 1)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var bitmap = GetBitmap(text, color);
                var w = bitmap.PixelWidth * scale;
                var h = bitmap.PixelHeight * scale;
                ctx.DrawImage(bitmap, new(x, y - h, w, h));
            }
        }
    }
}
