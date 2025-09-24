using FFmpeg.AutoGen;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.FFmpeg;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media;

namespace LivreNoirLibrary.SandBox
{
    public class ImageData : DisposableBase
    {
        private readonly VideoDecoder _decoder;
        private readonly UnmanagedArray<byte> _buffer;
        private bool _needInitialize;
        private bool _canRead;
        private decimal _lastPosition;
        private decimal _nextPosition;

        public int Width => _decoder.OutputWidth;
        public int Height => _decoder.OutputHeight;

        public ImageData(string path, int width = 0, int height = 0)
        {
            _decoder = new(path, new(width, height));
            _buffer = new(_decoder.OutputWidth * _decoder.OutputHeight * 4);
            Rewind();
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
            _decoder.Dispose();
            _buffer.Dispose();
        }

        public void Rewind()
        {
            _decoder.SeekByTick(0);
            _buffer.Clear();
            _needInitialize = true;
            _canRead = true;
            _lastPosition = 0;
            _nextPosition = 0;
        }

        public Span<byte> GetBytes(decimal time)
        {
            if (time < _lastPosition)
            {
                Rewind();
            }
            if (_canRead)
            {
                while (_needInitialize || (_nextPosition < time))
                {
                    if (_decoder.ReadOneFrame(_buffer, out var pos, out var duration))
                    {
                        _needInitialize = false;
                        _lastPosition = (decimal)pos;
                        _nextPosition = _lastPosition + duration;
                    }
                    else
                    {
                        _canRead = false;
                        break;
                    }
                }
            }
            return _buffer;
        }

        public unsafe void CopyPixels(decimal time, Span<byte> target, int targetWidth, int targetHeight)
        {
            var source = GetBytes(time);
            fixed (byte* targetPtr = target)
            fixed (byte* sourcePtr = source)
            {
                var sourceWidth = Width;
                var width = Math.Min(sourceWidth, targetWidth);
                var height = Math.Min(Height, targetHeight);
                for (var y = 0; y < height; y++)
                {
                    var front = sourcePtr + (y * sourceWidth * 4);
                    var back = targetPtr + (y * targetWidth * 4);
                    for (var x = 0; x < width; x++, front += 4, back += 4)
                    {
                        // アルファ値(正規化)
                        var frontA = ColorUtils.GetFloat(front[(int)ColorIndex.A]);
                        // アルファ以外の色成分
                        var frontColor = *(uint*)front & (~ColorOperation.Mask_Alpha);
                        // アルファが0より大きく、アルファ以外の色成分が0(完全な黒)でない場合
                        if (frontA is > 0 && frontColor is > 0)
                        {
                            *(uint*)back = *(uint*)front;
                        }
                    }
                }
            }
        }
    }
}
