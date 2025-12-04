using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class BitmapOperation
    {
        extension<T>(T bitmap) where T : IBitmap
        {
            public void CopyTo<TDest>(TDest destination)
                where TDest : IBitmap
            {
                if (bitmap.IsFloat)
                {
                    destination.CopyFloatFrom(bitmap.Pointer, bitmap.Width, bitmap.Height, bitmap.Stride);
                }
                else
                {
                    destination.CopyByteFrom(bitmap.Pointer, bitmap.Width, bitmap.Height, bitmap.Stride);
                }
            }

            public void CopyTo<TDest>(TDest destination, Rectangle sourceRect)
                where TDest : IBitmap
            {
                if (Adjust(bitmap, ref sourceRect))
                {
                    if (bitmap.IsFloat)
                    {
                        destination.CopyFloatFrom(bitmap.Offset(sourceRect.X, sourceRect.Y), sourceRect.Width, sourceRect.Height, bitmap.Stride);
                    }
                    else
                    {
                        destination.CopyByteFrom(bitmap.Offset(sourceRect.X, sourceRect.Y), sourceRect.Width, sourceRect.Height, bitmap.Stride);
                    }
                }
            }

            public void CopyByteFrom(ReadOnlySpan<byte> source, int sourceWidth)
            {
                ArgumentOutOfRangeException.ThrowIfGreaterThan(sourceWidth, source.Length);
                var stride = sourceWidth * 4;
                fixed (byte* ptr = source)
                {
                    CopyByteFrom(bitmap, (nint)ptr, sourceWidth, source.Length / stride, stride);
                }
            }

            public void CopyByteFrom(nint source, int sourceWidth, int sourceHeight, int sourceStride)
            {
                sourceWidth = Math.Min(sourceWidth, bitmap.Width);
                sourceHeight = Math.Min(sourceHeight, bitmap.Height);
                if (bitmap.IsFloat)
                {
                    Parallel.For(0, sourceHeight, y =>
                    {
                        var srcPtr = (byte*)(source + y * sourceStride);
                        var destPtr = (float*)bitmap.Offset(y);
                        ByteToBuffer(ref srcPtr, destPtr, sourceWidth * 4);
                    });
                }
                else
                {
                    Parallel.For(0, sourceHeight, y =>
                    {
                        var srcPtr = (uint*)(source + y * sourceStride);
                        var destPtr = (uint*)bitmap.Offset(y);
                        SimdOperations.CopyFrom(destPtr, srcPtr, sourceWidth);
                    });
                }
            }

            public void CopyFloatFrom(ReadOnlySpan<float> source, int sourceWidth)
            {
                ArgumentOutOfRangeException.ThrowIfGreaterThan(sourceWidth, source.Length);
                var stride = sourceWidth * 4;
                fixed (float* ptr = source)
                {
                    CopyFloatFrom(bitmap, (nint)ptr, sourceWidth, source.Length / stride, stride * 4);
                }
            }

            public void CopyFloatFrom(nint source, int sourceWidth, int sourceHeight, int sourceStride)
            {
                sourceWidth = Math.Min(sourceWidth, bitmap.Width);
                sourceHeight = Math.Min(sourceHeight, bitmap.Height);
                if (bitmap.IsFloat)
                {
                    Parallel.For(0, sourceHeight, y =>
                    {
                        var srcPtr = (float*)(source + y * sourceStride);
                        var destPtr = (float*)bitmap.Offset(y);
                        SimdOperations.CopyFrom(destPtr, srcPtr, sourceWidth * 4);
                    });
                }
                else
                {
                    Parallel.For(0, sourceHeight, y =>
                    {
                        var srcPtr = (float*)(source + y * sourceStride);
                        var destPtr = (byte*)bitmap.Offset(y);
                        BufferToByte(ref destPtr, srcPtr, sourceWidth * 4);
                    });
                }
            }

            public unsafe void WriteBytesTo(Span<byte> destination, int destWidth)
            {
                ArgumentOutOfRangeException.ThrowIfGreaterThan(destWidth, destination.Length);
                var stride = destWidth * 4;
                fixed (byte* ptr = destination)
                {
                    WriteBytesTo(bitmap, (nint)ptr, destWidth, destination.Length / stride, stride);
                }
            }

            public unsafe void WriteBytesTo(nint destination, int destWidth, int destHeight, int destStride)
            {
                destWidth = Math.Min(destStride, bitmap.Width);
                destHeight = Math.Min(destHeight, bitmap.Height);
                if (bitmap.IsFloat)
                {
                    Parallel.For(0, destHeight, y =>
                    {
                        var srcPtr = (float*)bitmap.Offset(y);
                        var destPtr = (byte*)(destination + y * destStride);
                        BufferToByte(ref destPtr, srcPtr, destWidth * 4);
                    });
                }
                else
                {
                    var source = (byte*)bitmap.Pointer;
                    Parallel.For(0, destHeight, y =>
                    {
                        var srcPtr = (uint*)bitmap.Offset(y);
                        var destPtr = (uint*)(source + y * destStride);
                        SimdOperations.CopyFrom(destPtr, srcPtr, destWidth);
                    });
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void ByteToBuffer(ref byte* source, float* buffer, int count, bool rewind = false)
        {
            for (var i = 0; i < count; i += 4)
            {
                *buffer++ = ColorUtils.RgbToScRgb(*source++);
                *buffer++ = ColorUtils.RgbToScRgb(*source++);
                *buffer++ = ColorUtils.RgbToScRgb(*source++);
                *buffer++ = ColorUtils.GetFloat(*source++);
            }
            if (rewind)
            {
                source -= count;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void BufferToByte(ref byte* destination, float* buffer, int count)
        {
            for (var i = 0; i < count; i += 4)
            {
                *destination++ = ColorUtils.ScRgbToRgb(*buffer++);
                *destination++ = ColorUtils.ScRgbToRgb(*buffer++);
                *destination++ = ColorUtils.ScRgbToRgb(*buffer++);
                *destination++ = ColorUtils.GetByte(*buffer++);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector<float> FillRemain(Vector<float>* source, int count) => VectorUtils.CreateFilling(new ReadOnlySpan<float>(source, count));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void VectorToFloat(Vector<float>* destination, Vector<float> buffer, int count)
        {
            var dest = (float*)destination;
            for (var i = 0; i < count; i++)
            {
                *dest++ = buffer[i];
            }
        }
    }
}