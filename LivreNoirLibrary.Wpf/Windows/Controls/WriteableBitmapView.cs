using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Controls
{
    public abstract partial class WriteableBitmapView : FrameworkElement
    {
        public const int DefaultBitmapWidth = 640;
        public const int DefaultBitmapHeight = 640;

        protected double _required_width;
        protected double _required_height;

        protected int _bitmap_width = DefaultBitmapWidth;
        protected int _bitmap_height = DefaultBitmapHeight;
        protected WriteableBitmap? _bitmap;

        private bool _need_refresh;

        public BitmapSource? BufferBitmap => _bitmap;

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            UpdateRenderSize(sizeInfo.NewSize);
        }

        protected void UpdateRenderSize(Size size) => UpdateRenderSize(size.Width, size.Height);
        protected virtual void UpdateRenderSize(double width, double height)
        {
            bool flag = false;
            if (_required_width != width)
            {
                _required_width = width;
                while (_required_width > _bitmap_width)
                {
                    _bitmap_width *= 2;
                    flag = true;
                }
                OnRequiredWidthChanged();
            }
            if (_required_height != height)
            {
                _required_height = height;
                while (_required_height > _bitmap_height)
                {
                    _bitmap_height *= 2;
                    flag = true;
                }
                OnRequiredHeightChanged();
            }
            if (flag)
            {
                _bitmap = null;
                ReserveRefresh();
            }
        }

        protected virtual void OnRequiredWidthChanged() { }
        protected virtual void OnRequiredHeightChanged() { }

        public virtual void ReserveRefresh()
        {
            _need_refresh = true;
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (_bitmap is null)
            {
                CreateBitmap();
            }
            if (_need_refresh)
            {
                _need_refresh = false;
                Refresh();
            }
            base.OnRender(dc);
            dc.DrawImage(_bitmap, new(GetBitmapOffsetX(), GetBitmapOffsetY(), _bitmap_width, _bitmap_height));
        }

        protected void CreateBitmap()
        {
            _bitmap = Bitmap.Create(_bitmap_width, _bitmap_height);
        }

        protected virtual void Refresh() { }
        protected virtual double GetBitmapOffsetX() => 0;
        protected virtual double GetBitmapOffsetY() => 0;
    }
}
