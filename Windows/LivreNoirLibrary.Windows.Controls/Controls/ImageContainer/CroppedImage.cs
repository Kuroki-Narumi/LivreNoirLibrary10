using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class CroppedImage : ImageContainerBase
    {
        [DependencyProperty(AffectsMeasure = true, AffectsRender = true)]
        private BitmapSource? _source;
        [DependencyProperty(AffectsMeasure = true, AffectsRender = true)]
        private Int32Rect _sourceRect;

        public CroppedBitmap? CreateCroppedBitmap()
        {
            if (ValidateIntRect(Source, SourceRect, out var rect))
            {
                return new(Source, rect);
            }
            return null;
        }

        private void OnSourceChanged(BitmapSource? oldValue, BitmapSource? newValue)
        {
            DetachSourceEvents(oldValue);
            AttachSourceEvents(newValue);
        }

        public override Size GetNaturalSize() => ValidateRect(Source, SourceRect, out var rect) ? rect.Size : new(0, 0);

        protected override void OnRender(DrawingContext drawingContext)
        {
            var source = Source;
            if (!ValidateRect(source, SourceRect, out var finalRect))
            {
                return;
            }
            ImageBrush brush = new(source)
            {
                Viewbox = finalRect,
                ViewboxUnits = BrushMappingMode.Absolute,
                Stretch = Stretch.Fill,
            };
            drawingContext.DrawRectangle(brush, null, new(new(0, 0), RenderSize));
        }

        private static bool ValidateIntRect([NotNullWhen(true)] BitmapSource? source, Int32Rect rect, out Int32Rect finalRect)
        {
            finalRect = default;
            if (source is null)
            {
                return false;
            }
            var (cx, cy, cw, ch) = rect;
            var left = Math.Max(cx, 0);
            var right = Math.Min(cx + cw, source.PixelWidth);
            var top = Math.Max(cy, 0);
            var bottom = Math.Min(cy + ch, source.PixelHeight);
            cx = left;
            cy = top;
            cw = right - left;
            ch = bottom - top;
            if (cw is <= 0 || ch is <= 0)
            {
                return false;
            }
            finalRect = new(cx, cy, cw, ch);
            return true;
        }

        private static bool ValidateRect([NotNullWhen(true)] BitmapSource? source, Int32Rect rect, out Rect finalRect)
        {
            if (ValidateIntRect(source, rect, out var iRect))
            {
                var scaleX = 96.0 / source.DpiX;
                var scaleY = 96.0 / source.DpiY;
                finalRect = new(iRect.X * scaleX, iRect.Y * scaleY, iRect.Width * scaleX, iRect.Height * scaleY);
                return true;
            }
            finalRect = default;
            return false;
        }
    }
}