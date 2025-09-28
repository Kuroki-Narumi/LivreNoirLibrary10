using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.FFmpeg;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using DrRect = System.Drawing.Rectangle;

namespace LivreNoirLibrary.Windows.Media
{
    public sealed class VideoBuffer : MediaBuffer
    {
        private VideoDecoder _decoder;
        private WriteableBitmap? _bitmap;
        private Rect _rect;
        private bool _needInitialize;
        private bool _canRead;
        private long _lastPosition;
        private long _nextPosition;

        public VideoBuffer(string path, in DrRect requiredRect)
        {
            var decoder = new VideoDecoder(path);
            (_decoder, _bitmap) = RefreshDecoder(path, decoder, requiredRect, false);
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            _decoder.Dispose();
        }

        private (VideoDecoder, WriteableBitmap) RefreshDecoder(string path, VideoDecoder decoder, in DrRect requiredRect, bool dispose)
        {
            var (x, y, width, height) = requiredRect;
            var sw = decoder.InputWidth;
            var sh = decoder.InputHeight;
            var scale = Math.Min((double)width / sw, (double)height / sh);
            // 縮小が必要な場合、デコード時点で縮小処理を行うようにする
            if (scale < 1)
            {
                sw = (int)(sw * scale);
                sh = (int)(sh * scale);
                dispose = true;
            }
            if (dispose)
            {
                decoder.Dispose();
                decoder = new(path, new(sw, sh));
                _decoder = decoder;
                _bitmap = null;
            }
            _bitmap ??= Bitmap.Create(sw, sh);
            RefreshRectInternal(x, y, width, height, sw, sh);
            Rewind();
            return (_decoder, _bitmap);
        }

        public override void RefreshRect(string path, in DrRect requiredRect)
        {
            var d = _decoder;
            var sw = d.OutputWidth;
            var sh = d.OutputHeight;
                // 現在のデコーダーが縮小デコード状態か
            if (d.InputWidth != sw || d.InputHeight != sh ||
                // 要求サイズがソースサイズより小さい場合
                requiredRect.Width < sw || requiredRect.Height < sh)
            {
                RefreshDecoder(path, d, requiredRect, true);
            }
            else
            {
                var (x, y, width, height) = requiredRect;
                RefreshRectInternal(x, y, width, height, sw, sh);
            }
        }

        private void RefreshRectInternal(int x, int y, int w, int h, int sw, int sh)
        {
            var scale = Math.Min((double)w / sw, (double)h / sh);
            var width = sw * scale;
            var height = sh * scale;
            var ox = x + (w - width) / 2;
            var oy = y + (h - height) / 2;
            _rect = new(ox, oy, width, height);
        }

        private void Rewind()
        {
            _decoder.SeekByTick(0);
            _bitmap?.Clear();
            _needInitialize = true;
            _canRead = true;
            _lastPosition = 0;
            _nextPosition = 0;
        }

        public override (WriteableBitmap?, Rect) GetBitmap(long ticks)
        {
            if (ticks < _lastPosition)
            {
                Rewind();
            }
            if (_bitmap is { } bitmap && _canRead)
            {
                using BitmapPointer pointer = new(bitmap);
                while (_needInitialize || (_nextPosition < ticks))
                {
                    if (_decoder.ReadOneFrame(pointer.AsSpan(), out var pos, out var duration))
                    {
                        _needInitialize = false;
                        _lastPosition = pos.ToTicks();
                        _nextPosition = _lastPosition + duration.ToTicks();
                    }
                    else
                    {
                        _canRead = false;
                        break;
                    }
                }
                BitmapOperation.SetTransparent(pointer, new(0, 0, 0, 0));
            }
            return (_bitmap, _rect);
        }
    }
}
