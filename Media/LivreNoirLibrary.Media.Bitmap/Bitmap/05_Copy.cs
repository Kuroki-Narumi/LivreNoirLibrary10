using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class BitmapOperation
    {
        extension<T>(T bitmap) where T : IBitmap
        {
            public void CopyTo<TDest>(TDest destination) where TDest : IBitmap => bitmap.CopyTo(destination, bitmap.Rect);
            public void CopyTo<TDest>(TDest destination, Rectangle sourceRect, Point destLocation = default)
                where TDest : IBitmap
            {
                if (!Adjust(bitmap, ref sourceRect, destination, ref destLocation))
                {
                    return;
                }
                CopyToCore(
                    bitmap.Offset(sourceRect), bitmap.Stride, bitmap.IsFloat,
                    destination.Offset(destLocation), destination.Stride, destination.IsFloat,
                    sourceRect.Width, sourceRect.Height
                    );
            }

            public void CopyTo<TSpan>(Span<TSpan> destination, int destWidth, int destHeight = 0, bool destIsFloat = false)
                where TSpan : unmanaged => bitmap.CopyTo(destination, bitmap.Rect, destWidth, destHeight, destIsFloat);

            public void CopyTo<TSpan>(Span<TSpan> destination, Rectangle sourceRect, int destWidth, int destHeight = 0, bool destIsFloat = false)
                where TSpan : unmanaged => destination.ProcessAsWriteableBitmap(destWidth, destHeight, destIsFloat, dest => bitmap.CopyTo(dest, sourceRect));

            public void CopyFrom<TSpan>(ReadOnlySpan<TSpan> source, int sourceWidth, int sourceHeight = 0, Point destLocation = default, bool sourceIsFloat = false)
                where TSpan : unmanaged => source.ProcessAsReadOnlyBitmap(sourceWidth, sourceHeight, sourceIsFloat, source => source.CopyTo(bitmap, source.Rect, destLocation));

            public void CopyTo<TDest>(TDest destination, FloatColor colorCorrection)
                where TDest : IBitmap => bitmap.CopyTo(destination, bitmap.Rect, new Point(0, 0), colorCorrection);

            public void CopyTo<TDest>(TDest destination, Rectangle sourceRect, FloatColor colorCorrection)
                where TDest : IBitmap => bitmap.CopyTo(destination, sourceRect, new Point(0, 0), colorCorrection);

            public void CopyTo<TDest>(TDest destination, Rectangle sourceRect, Point destLocation, FloatColor colorCorrection)
                where TDest : IBitmap
            {
                if (colorCorrection == Vector<float>.One)
                {
                    CopyTo(bitmap, destination, sourceRect, destLocation);
                    return;
                }
                if (!Adjust(bitmap, ref sourceRect, destination, ref destLocation))
                {
                    return;
                }
                CopyToCore(
                    bitmap.Offset(sourceRect), bitmap.Stride, bitmap.IsFloat,
                    destination.Offset(destLocation), destination.Stride, destination.IsFloat,
                    sourceRect.Width, sourceRect.Height, colorCorrection
                    );
            }

            public void CopyTo<TDest>(TDest destination, ColorFlags flags)
                where TDest : IBitmap => bitmap.CopyTo(destination, bitmap.Rect, new Point(0, 0), flags);

            public void CopyTo<TDest>(TDest destination, Rectangle sourceRect, ColorFlags flags)
                where TDest : IBitmap => bitmap.CopyTo(destination, sourceRect, new Point(0, 0), flags);

            public void CopyTo<TDest>(TDest destination, Rectangle sourceRect, Point destLocation, ColorFlags flags)
                where TDest : IBitmap
            {
                if (flags is ColorFlags.All)
                {
                    CopyTo(bitmap, destination, sourceRect, destLocation);
                    return;
                }
                if (!Adjust(bitmap, ref sourceRect, destination, ref destLocation))
                {
                    return;
                }
                CopyToCore(
                    bitmap.Offset(sourceRect), bitmap.Stride, bitmap.IsFloat,
                    destination.Offset(destLocation), destination.Stride, destination.IsFloat,
                    sourceRect.Width, sourceRect.Height, flags
                    );
            }
        }

        static void CopyToCore(nint source, int sourceStride, bool sourceIsFloat, nint destination, int destStride, bool destIsFloat, int width, int height)
        {
            if (sourceIsFloat)
            {
                width *= 4;
                if (destIsFloat)
                {
                    Parallel.For(0, height, y =>
                    {
                        var srcPtr = (float*)(source + y * sourceStride);
                        var destPtr = (float*)(destination + y * destStride);
                        SimdOperations.CopyFrom(destPtr, srcPtr, width);
                    });
                }
                else
                {
                    Parallel.For(0, height, y =>
                    {
                        var srcPtr = (float*)(source + y * sourceStride);
                        var destPtr = (byte*)(destination + y * destStride);
                        BufferToByte(ref destPtr, srcPtr, width);
                    });
                }
            }
            else if (destIsFloat)
            {
                width *= 4;
                Parallel.For(0, height, y =>
                {
                    var srcPtr = (byte*)(source + y * sourceStride);
                    var destPtr = (float*)(destination + y * destStride);
                    ByteToBuffer(ref srcPtr, destPtr, width);
                });
            }
            else
            {
                Parallel.For(0, height, y =>
                {
                    var srcPtr = (uint*)(source + y * sourceStride);
                    var destPtr = (uint*)(destination + y * destStride);
                    SimdOperations.CopyFrom(destPtr, srcPtr, width);
                });
            }
        }

        static void CopyToCore(nint source, int sourceStride, bool sourceIsFloat, nint destination, int destStride, bool destIsFloat, int width, int height, Vector<float> colorCorrection)
        {
            if (sourceIsFloat)
            {
                if (destIsFloat)
                {
                    width *= 4;
                    Parallel.For(0, height, y =>
                    {
                        var srcPtr = (float*)(source + y * sourceStride);
                        var destPtr = (float*)(destination + y * destStride);
                        SimdOperations.CopyFrom(destPtr, srcPtr, colorCorrection, width);
                    });
                }
                else
                {
                    var count = Vector<float>.Count;
                    Parallel.For(0, height, y =>
                    {
                        var w = width * 4;
                        var srcPtr = (Vector<float>*)(source + y * sourceStride);
                        var destPtr = (byte*)(destination + y * destStride);
                        var buffer = stackalloc float[count];
                        for (; w >= count; w -= count)
                        {
                            *(Vector<float>*)buffer = *srcPtr++ * colorCorrection;
                            BufferToByte(ref destPtr, buffer, count);
                        }
                        if (w is > 0)
                        {
                            *(Vector<float>*)buffer = FillRemain(srcPtr, w) * colorCorrection;
                            BufferToByte(ref destPtr, buffer, w);
                        }
                    });
                }
            }
            else if (destIsFloat)
            {
                var count = Vector<float>.Count;
                Parallel.For(0, height, y =>
                {
                    var w = width * 4;
                    var srcPtr = (byte*)(source + y * sourceStride);
                    var destPtr = (Vector<float>*)(destination + y * destStride);
                    var buffer = stackalloc float[count];
                    for (; w >= count; w -= count)
                    {
                        ByteToBuffer(ref srcPtr, buffer, count);
                        *destPtr++ = *(Vector<float>*)buffer * colorCorrection;
                    }
                    if (w is > 0)
                    {
                        ByteToBuffer(ref srcPtr, buffer, count);
                        VectorToFloat(destPtr, *(Vector<float>*)buffer * colorCorrection, w);
                    }
                });
            }
            else
            {
                var count = Vector<float>.Count;
                Parallel.For(0, height, y =>
                {
                    var w = width * 4;
                    var srcPtr = (byte*)(source + y * sourceStride);
                    var destPtr = (byte*)(destination + y * destStride);
                    var buffer = stackalloc float[count];
                    for (; w >= count; w -= count)
                    {
                        ByteToBuffer(ref srcPtr, buffer, count);
                        *(Vector<float>*)buffer *= colorCorrection;
                        BufferToByte(ref destPtr, buffer, count);
                    }
                    if (w is > 0)
                    {
                        ByteToBuffer(ref srcPtr, buffer, w);
                        *(Vector<float>*)buffer *= colorCorrection;
                        BufferToByte(ref destPtr, buffer, w);
                    }
                });
            }
        }

        static void CopyToCore(nint source, int sourceStride, bool sourceIsFloat, nint destination, int destStride, bool destIsFloat, int width, int height, ColorFlags flags)
        {
            if (sourceIsFloat)
            {
                var (toClear, fromExtract) = GetClearSetMask(flags, 1f);
                var count = Vector<float>.Count;
                width *= 4;
                if (destIsFloat)
                {
                    Parallel.For(0, height, y =>
                    {
                        var w = width;
                        var srcVector = (Vector<float>*)(source + y * sourceStride);
                        var destVector = (Vector<float>*)(destination + y * destStride);
                        for (; w >= count; w -= count, srcVector++, destVector++)
                        {
                            var buffer = *srcVector * fromExtract;
                            *destVector = (*destVector * toClear) + buffer;
                        }
                        var srcPtr = (float*)srcVector;
                        var destPtr = (float*)destVector;
                        for (var i = 0; i < w; srcPtr++, destPtr++)
                        {
                            var buffer = *srcPtr * fromExtract[i];
                            *destPtr = (*destPtr * toClear[i]) + buffer;
                        }
                    });
                }
                else
                {
                    Parallel.For(0, height, y =>
                    {
                        var w = width;
                        var srcVector = (Vector<float>*)(source + y * sourceStride);
                        var destPtr = (byte*)(destination + y * destStride);
                        var destBuffer = stackalloc float[count];
                        var destVector = (Vector<float>*)destBuffer;
                        for (; w >= count; w -= count, srcVector++)
                        {
                            var buffer = *srcVector * fromExtract;
                            ByteToBuffer(ref destPtr, destBuffer, count, true);
                            *destVector = (*destVector * toClear) + buffer;
                            BufferToByte(ref destPtr, destBuffer, count);
                        }
                        if (w is > 0)
                        {
                            var buffer = FillRemain(srcVector, w) * fromExtract;
                            ByteToBuffer(ref destPtr, destBuffer, w, true);
                            *destVector = (*destVector * toClear) + buffer;
                            BufferToByte(ref destPtr, destBuffer, w);
                        }
                    });
                }
            }
            else if (destIsFloat)
            {
                var (toClear, fromExtract) = GetClearSetMask(flags, 1f);
                var count = Vector<float>.Count;
                Parallel.For(0, height, y =>
                {
                    var w = width;
                    var srcPtr = (byte*)(source + y * sourceStride);
                    var srcBuffer = stackalloc float[count];
                    var srcVector = (Vector<float>*)srcBuffer;
                    var destVector = (Vector<float>*)(destination + y * destStride);
                    for (; w >= count; w -= count, destVector++)
                    {
                        ByteToBuffer(ref srcPtr, srcBuffer, count);
                        var buffer = *srcVector * fromExtract;
                        *destVector = (*destVector * toClear) + buffer;
                    }
                    if (w is > 0)
                    {
                        ByteToBuffer(ref srcPtr, srcBuffer, w);
                        var buffer = *srcVector * fromExtract;
                        var dest = (FillRemain(destVector, w) * toClear) + buffer;
                        VectorToFloat(destVector, dest, w);
                    }
                });
            }
            else
            {
                var (toClear, fromExtract) = GetClearSetMask(flags, 255);
                var count = Vector<uint>.Count;
                var fromExtractVector = Vector.Create(fromExtract);
                var toClearVector = Vector.Create(toClear);
                Parallel.For(0, height, y =>
                {
                    var w = width;
                    var srcVector = (Vector<uint>*)(source + y * sourceStride);
                    var destVector = (Vector<uint>*)(destination + y * destStride);
                    for (; w >= count; w -= count, srcVector++, destVector++)
                    {
                        var buffer = *srcVector & fromExtractVector;
                        *destVector = (*destVector & toClearVector) | buffer;
                    }
                    var srcPtr = (uint*)srcVector;
                    var destPtr = (uint*)destVector;
                    for (; w is > 0; w--, srcPtr++, destPtr++)
                    {
                        var buffer = *srcPtr & fromExtract;
                        *destPtr = (*destPtr & toClear) | buffer;
                    }
                });
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