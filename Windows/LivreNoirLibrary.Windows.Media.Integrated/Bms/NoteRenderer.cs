using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public static class NoteRenderer
    {
        private const string ResourcePath = "pack://application:,,,/LivreNoirLibrary.Windows.Media;component/Resources/IndexLetters.png";
        private const int HorizontalCount = 10;
        private const int CharWidth = 7;
        private const int CharHeight = 9;

        private static readonly LnColor _defaultColor = LnColor.FromRgb(255, 255, 255);
        private static readonly DrawingVisual _dv = new();
        private static readonly Dictionary<char, ImageSource> _resources;
        private static readonly Dictionary<string, RenderTargetBitmap> _whiteCache = [];
        private static readonly Dictionary<LnColor, Dictionary<string, WriteableBitmap>> _coloredCache = [];
        private static readonly Dictionary<(int, int), (StreamGeometry, StreamGeometry)> _outlineCache = [];
        private static readonly Dictionary<RectKey, RenderTargetBitmap> _rectCache = [];
        private static readonly Dictionary<RectKey, RenderTargetBitmap> _selectedRectCache = [];

        public static SolidColorBrush Hilight { get; } = MediaUtils.GetBrush(Colors.NoteHilight);
        public static SolidColorBrush Shadow { get; } = MediaUtils.GetBrush(Colors.NoteShadow);
        public static SolidColorBrush SelectedStroke { get; } = MediaUtils.GetBrush(Colors.SelectedStroke);

        static NoteRenderer()
        {
            BitmapImage source = new(new Uri(ResourcePath));
            Dictionary<char, ImageSource> resources = [];
            void Add(char c, int x, int y, int w = CharWidth)
            {
                resources.Add(c, new(source, x, y, w, CharHeight));
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
            _resources = resources;
        }

        private static RenderTargetBitmap CreateRenderTarget(int width, int height, Visual visual)
        {
            RenderTargetBitmap bitmap = new(width, height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(visual);
            bitmap.Freeze();
            return bitmap;
        }
        
        public static RenderTargetBitmap GetTextSource(string text)
        {
            if (!_whiteCache.TryGetValue(text, out var bitmap))
            {
                var x = 0;
                var dv = _dv;
                dv.Children.Clear();
                using (var ctx = dv.RenderOpen())
                {
                    foreach (var c in text)
                    {
                        var w = 1;
                        if (c is ' ')
                        {
                            w = CharWidth;
                        }
                        else if (_resources.TryGetValue(c, out var source))
                        {
                            w = source.Width;
                            ctx.DrawImage(source.Bitmap, new(x, 0, w, CharHeight));
                        }
                        x += w - 1;
                    }
                }
                bitmap = CreateRenderTarget(x + 1, CharHeight, dv);
                _whiteCache.Add(text, bitmap);
            }
            return bitmap;
        }

        public static BitmapSource GetTextSource(string text, Color color)
        {
            var lnColor = color.ToLnColorWithoutAlpha();
            if (lnColor == _defaultColor)
            {
                return GetTextSource(text);
            }
            var dic = _coloredCache.GetOrAdd(lnColor);
            if (!dic.TryGetValue(text, out var bitmap))
            {
                var source = GetTextSource(text);
                WriteableBitmap b = new(source);
                using (var p = b.BeginWrite())
                {
                    p.Blend(BlendMode.Multiply, lnColor);
                }
                bitmap = b;
                dic.Add(text, bitmap);
            }
            return bitmap;
        }

        public static void DrawNoteText(this DrawingContext ctx, int x, int y, string? text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var bitmap = GetTextSource(text);
                ctx.DrawImage(bitmap, new(x, y, bitmap.PixelWidth, bitmap.PixelHeight));
            }
        }

        public static void DrawNoteText(this DrawingContext ctx, int x, int y, string? text, Color color)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var bitmap = GetTextSource(text, color);
                ctx.DrawImage(bitmap, new(x, y, bitmap.PixelWidth, bitmap.PixelHeight));
            }
        }

        private static (StreamGeometry, StreamGeometry) GetOutlineGeometry(int width, int height)
        {
            var key = (width, height);
            if (!_outlineCache.TryGetValue(key, out var gg))
            {
                var h1 = height - 1;
                var g1 = MediaUtils.CreateGeometry($"M0,0 H{width} v1 H1 V{h1} H0 Z");
                var g2 = MediaUtils.CreateGeometry($"M0,{height} H{width} V1 h-1 V{h1} H0 Z");
                gg = (g1, g2);
                _outlineCache.Add(key, gg);
            }
            return gg;
        }

        public static RenderTargetBitmap GetRectSource(int width, int height, Color color)
        {
            RectKey key = new(width, height, color);
            if (!_rectCache.TryGetValue(key, out var bitmap))
            {
                var dv = _dv;
                dv.Children.Clear();
                using (var ctx = dv.RenderOpen())
                {
                    ctx.DrawRectangle(MediaUtils.GetBrush(color), null, new(0, 0, width, height));
                    var (g1, g2) = GetOutlineGeometry(width, height);
                    ctx.DrawGeometry(Hilight, null, g1);
                    ctx.DrawGeometry(Shadow, null, g2);
                }
                bitmap = CreateRenderTarget(width, height, dv);
                _rectCache.Add(key, bitmap);
            }
            return bitmap;
        }

        public static void DrawNoteRect(this DrawingContext ctx, int x, int y, int width, int height, Color color)
        {
            var bitmap = GetRectSource(width, height, color);
            ctx.DrawImage(bitmap, new(x, y, width, height));
        }

        public static RenderTargetBitmap GetSelectedRectSource(int width, int height, Color color)
        {
            RectKey key = new(width, height, color);
            if (!_selectedRectCache.TryGetValue(key, out var bitmap))
            {
                var dv = _dv;
                dv.Children.Clear();
                using (var ctx = dv.RenderOpen())
                {
                    ctx.DrawRectangle(SelectedStroke, null, new(0, 0, width, height));
                    ctx.DrawRectangle(MediaUtils.GetBrush(color), null, new(1, 1, width - 2, height - 2));
                }
                bitmap = CreateRenderTarget(width, height, dv);
                _selectedRectCache.Add(key, bitmap);
            }
            return bitmap;
        }

        public static void DrawSelectedNoteRect(this DrawingContext ctx, int x, int y, int width, int height, Color color)
        {
            var bitmap = GetSelectedRectSource(width, height, color);
            ctx.DrawImage(bitmap, new(x, y, width, height));
        }

        private class ImageSource(BitmapSource source, int x, int y, int width, int height)
        {
            public CroppedBitmap Bitmap { get; } = new(source, new(x, y, width, height));
            public int Width { get; } = width;
        }

        private readonly struct RectKey(int width, int height, Color color)
        {
            public int Width { get; } = width;
            public int Height { get; } = height;
            public byte A { get; } = color.A;
            public byte R { get; } = color.R;
            public byte G { get; } = color.G;
            public byte B { get; } = color.B;
        }
    }
}
