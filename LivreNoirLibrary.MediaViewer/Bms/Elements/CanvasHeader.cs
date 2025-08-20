using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class CanvasHeader : FrameworkElement
    {
        [DependencyProperty]
        private Color _textColor = Colors.HeaderText;

        private readonly Dictionary<string, WriteableBitmap> _source_cache = [];
        private readonly List<Child> _children = [];

        private class Child(string text, WriteableBitmap bitmap, Rect rect)
        {
            public string Text { get; } = text;
            public WriteableBitmap Bitmap { get; set; } = bitmap;
            public Rect Rect { get; } = rect;
        }

        internal void ClearChildren() => _children.Clear();
        internal void AddChild(double x, double width, string text)
        {
            var bitmap = GetBitmapSource(text);
            var w = bitmap.PixelWidth;
            _children.Add(new(text, bitmap, new(Math.Floor(0.5 + x + (width - w) / 2), 2, w, bitmap.PixelHeight)));
        }

        private WriteableBitmap GetBitmapSource(string text)
        {
            if (!_source_cache.TryGetValue(text, out var bitmap))
            {
                var source = NoteRectRenderer.GetTextSource(text);
                bitmap = Bitmap.Create(source);
                using (var ptr = new BitmapPointer(bitmap))
                {
                    ptr.AsUIntSpan().And(ColorOperation.ToUInt(_textColor));
                }
                _source_cache.Add(text, bitmap);
            }
            return bitmap;
        }

        private void OnTextColorChanged()
        {
            _source_cache.Clear();
            foreach (var item in CollectionsMarshal.AsSpan(_children))
            {
                var bitmap = GetBitmapSource(item.Text);
                item.Bitmap = bitmap;
            }
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            foreach (var item in CollectionsMarshal.AsSpan(_children))
            {
                drawingContext.DrawImage(item.Bitmap, item.Rect);
            }
        }
    }
}
