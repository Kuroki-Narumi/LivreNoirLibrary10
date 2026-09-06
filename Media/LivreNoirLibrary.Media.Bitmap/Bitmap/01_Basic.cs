using System;
using System.Drawing;
using System.Numerics;

namespace LivreNoirLibrary.Media
{
    public static partial class BitmapOperation
    {
        public const int BytesPerUIntPixel = sizeof(uint);
        public const int BytesPerFloatPixel = sizeof(float) * 4;

        extension<T>(T bitmap) where T : IBitmap
        {
            public bool IsValid => bitmap.Pointer is not 0 && bitmap.Width is > 0 && bitmap.Height is > 0;

            public int BytesPerPixel => bitmap.IsFloat ? BytesPerFloatPixel : BytesPerUIntPixel;
            public int Stride => bitmap.Width * bitmap.BytesPerPixel;
            public int ByteSize => bitmap.Height * bitmap.Stride;
            public Rectangle Rect => new(0, 0, bitmap.Width, bitmap.Height);
            public DoubleRect DoubleRect => new(0, 0, bitmap.Width, bitmap.Height);

            public nint Offset(int x, int y) => bitmap.Pointer + (x + y * bitmap.Width) * bitmap.BytesPerPixel;
            public nint Offset(int y) => bitmap.Pointer + y * bitmap.Stride;
            public nint Offset(in Rectangle rect) => bitmap.Offset(rect.X, rect.Y);
            public nint Offset(in Point location) => bitmap.Offset(location.X, location.Y);

            public LineEnumerator<T> EnumerateLines() => new(bitmap, bitmap.Rect);
            public LineEnumerator<T> EnumerateLines(in Rectangle rect) => new(bitmap, rect);

            public unsafe Rectangle GetOpaqueRect(int margin = 0, byte transparentAlpha = 0)
            {
                if (!bitmap.IsValid || transparentAlpha is 255)
                {
                    return default;
                }
                AssertType(bitmap, false);

                // Alphaの位置にビットシフト
                var th = (transparentAlpha + 1u) << 24;
                var w = bitmap.Width;

                return GetOpaqueCore(CheckLine, (uint*)bitmap.Pointer, w, bitmap.Height, w, margin);

                bool CheckLine(uint* p, out int left, out int right)
                {
                    left = -1;
                    right = -1;
                    // 左端の検出
                    var ptr = p;
                    for (var x = 0; x < w; x++, ptr++)
                    {
                        if (*ptr >= th)
                        {
                            left = right = x;
                            break;
                        }
                    }
                    // 右端の検出
                    ptr = p + (w - 1);
                    for (var x = w - 1; x > left; x--, ptr--)
                    {
                        if (*ptr >= th)
                        {
                            right = x;
                            break;
                        }
                    }
                    return left is not -1;
                }
            }

            public unsafe Rectangle GetOpaqueRect(int margin = 0, float transparentAlpha = 0)
            {
                if (!bitmap.IsValid || transparentAlpha is >= 1)
                {
                    return default;
                }
                AssertType(bitmap, true);

                var w = bitmap.Width;
                return GetOpaqueCore(CheckLine, (float*)bitmap.Pointer, w, bitmap.Height, w * 4, margin);

                bool CheckLine(float* p, out int left, out int right)
                {
                    left = -1;
                    right = -1;
                    // 左端の検出
                    var ptr = p + 3;
                    for (var x = 0; x < w; x++, ptr += 4)
                    {
                        if (*ptr > transparentAlpha)
                        {
                            left = right = x;
                            break;
                        }
                    }
                    // 右端の検出
                    ptr = p + (w - 1) + 3;
                    for (var x = w - 1; x > left; x--, ptr-=4)
                    {
                        if (*ptr > transparentAlpha)
                        {
                            right = x;
                            break;
                        }
                    }
                    return left is not -1;
                }
            }

            public void AssertType(bool isFloat)
            {
                System.Diagnostics.Debug.Assert(bitmap.IsFloat == isFloat, $"This method is only valid for instances where IBitmap.IsFloat is {isFloat}.");
            }

            public bool Adjust(ref Rectangle rect) => bitmap.Pointer is not 0 && Structs.Adjust(ref rect, bitmap.Width, bitmap.Height);
        }

        public static bool Adjust<T1, T2>(T1 source, ref Rectangle sourceRect, T2 destination, ref Point destLocation)
            where T1 : IBitmap
            where T2 : IBitmap
            => source.Pointer is not 0 && destination.Pointer is not 0 && 
            Structs.Adjust(ref sourceRect, ref destLocation, source.Width, source.Height, destination.Width, destination.Height);

        public static bool Adjust<T1, T2>(
            T1 source, in DoubleRect sourceRect,
            T2 destination, in DoubleRect destValidRect, in DoubleRect destRect,
            out Rectangle actualSourceRect, out Rectangle actualDestRect)
            where T1 : IBitmap
            where T2 : IBitmap
        {
            actualSourceRect = default;
            actualDestRect = default;
            return source.Pointer is not 0 && destination.Pointer is not 0 &&
                Structs.AdjustStretch(source.DoubleRect, sourceRect, destValidRect, destRect, out actualSourceRect, out actualDestRect);
        }

        private unsafe delegate bool CheckLine<T>(T* p, out int left, out int right) where T : unmanaged;

        private static unsafe Rectangle GetOpaqueCore<T>(CheckLine<T> check, T* pointer, int w, int h, int stride, int margin)
            where T : unmanaged, INumber<T>
        {
            var left = w;
            var right = 0;
            var top = -1;
            var bottom = -1;

            for (var y = 0; y < h; y++, pointer += stride)
            {
                if (check(pointer, out var currentLeft, out var currentRight))
                {
                    // 左右端の更新
                    left = Math.Min(left, currentLeft);
                    right = Math.Max(right, currentRight);
                    if (top is -1)
                    {
                        top = y;
                    }
                    bottom = y;
                }
            }

            // 全て透明
            if (top is -1)
            {
                return default;
            }
            else
            {
                return new(Math.Max(left - margin, 0), Math.Max(top - margin, 0), Math.Min(right - left + 1 + margin, w), Math.Min(bottom - top + 1 + margin, h));
            }
        }

        public struct LineEnumerator<T>
            where T : IBitmap
        {
            private readonly int _stride;
            private readonly int _bytes;
            private readonly int _height;
            private nint _pointer;
            private int _y = 0;

            public LineEnumerator(T bitmap, Rectangle rect)
            {
                if (bitmap.Adjust(ref rect))
                {
                    _stride = bitmap.Stride;
                    _bytes = rect.Width * bitmap.BytesPerPixel;
                    _height = rect.Height;
                    _pointer = bitmap.Offset(rect) - _stride;
                }
                else
                {
                    _stride = _bytes = _height = 0;
                    _pointer = 0;
                    _y = 1;
                }
            }

            public readonly (nint Pointer, int Bytes) Current => (_pointer, _bytes);

            public bool MoveNext()
            {
                if (_y < _height)
                {
                    _pointer += _stride;
                    _y++;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public readonly LineEnumerator<T> GetEnumerator() => this;
        }
    }
}