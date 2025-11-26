using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract partial class WriteableBitmapView : FrameworkElement
    {
        public const int DefaultBitmapWidth = 640;
        public const int DefaultBitmapHeight = 640;

        private double _requiredWidth;
        private double _requiredHeight;

        private int _bitmapWidth = DefaultBitmapWidth;
        private int _bitmapHeight = DefaultBitmapHeight;
        private WriteableBitmap? _bitmap;

        private bool _needRefresh;

        public WriteableBitmap? Bitmap => _bitmap;
        public double RequiredWidth => _requiredWidth;
        public double RequiredHeight => _requiredHeight;

        public WriteableBitmapView()
        {
            ClipToBounds = true;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateRenderSize(sizeInfo.NewSize);
        }

        protected void UpdateRenderSize(Size size) => UpdateRenderSize(size.Width, size.Height);
        protected virtual void UpdateRenderSize(double width, double height)
        {
            bool modified = false;
            if (_requiredWidth != width)
            {
                _requiredWidth = width;
                while (_requiredWidth > _bitmapWidth)
                {
                    _bitmapWidth *= 2;
                    modified = true;
                }
                OnRequiredWidthChanged(width);
            }
            if (_requiredHeight != height)
            {
                _requiredHeight = height;
                while (_requiredHeight > _bitmapHeight)
                {
                    _bitmapHeight *= 2;
                    modified = true;
                }
                OnRequiredHeightChanged(height);
            }
            if (modified)
            {
                _bitmap = null;
                ReserveRefresh();
            }
        }

        protected virtual void OnRequiredWidthChanged(double value) { }
        protected virtual void OnRequiredHeightChanged(double value) { }

        public virtual void ReserveRefresh()
        {
            _needRefresh = true;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            _bitmap ??= Media.Bitmap.Create(_bitmapWidth, _bitmapHeight);
            if (_needRefresh)
            {
                _needRefresh = false;
                Refresh();
            }
            base.OnRender(dc);
            dc.DrawImage(_bitmap, new(GetBitmapOffsetX(), GetBitmapOffsetY(), _bitmapWidth, _bitmapHeight));
        }

        protected virtual void Refresh() { }
        protected virtual double GetBitmapOffsetX() => 0;
        protected virtual double GetBitmapOffsetY() => 0;
    }
}
