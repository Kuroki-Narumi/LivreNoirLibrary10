using System;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class BitmapOperation
    {
        public static void ProcessAsWriteableBitmap(this Span<byte> span, int width, Action<PointerBitmap> action) => ProcessAsWriteableBitmap(span, width, 0, false, action);
        public static void ProcessAsWriteableBitmap(this Span<byte> span, int width, int height, Action<PointerBitmap> action) => ProcessAsWriteableBitmap(span, width, height, false, action);
        public static void ProcessAsWriteableBitmap(this Span<float> span, int width, Action<PointerBitmap> action) => ProcessAsWriteableBitmap(span, width, 0, true, action);
        public static void ProcessAsWriteableBitmap(this Span<float> span, int width, int height, Action<PointerBitmap> action) => ProcessAsWriteableBitmap(span, width, height, true, action);
        public static void ProcessAsWriteableBitmap<T>(this Span<T> span, int width, bool isFloat, Action<PointerBitmap> action)
            where T : unmanaged => ProcessAsWriteableBitmap(span, width, 0, isFloat, action);

        public static void ProcessAsWriteableBitmap<T>(this Span<T> span, int width, int height, bool isFloat, Action<PointerBitmap> action)
            where T : unmanaged
        {
            var sourceByteSize = span.Length * sizeof(T);
            var bytesPerPixel = isFloat ? BytesPerFloatPixel : BytesPerUIntPixel;
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(width, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(width, sourceByteSize / bytesPerPixel);
            var spanStride = width * bytesPerPixel;
            if (height is <= 0)
            {
                height = sourceByteSize / spanStride;
            }
            else
            {
                ArgumentOutOfRangeException.ThrowIfGreaterThan(height, sourceByteSize / spanStride);
            }
            fixed (T* ptr = span)
            {
                PointerBitmap bitmap = new((nint)ptr, width, height, isFloat);
                action(bitmap);
            }
        }

        public static void ProcessAsReadOnlyBitmap(this ReadOnlySpan<byte> span, int width, Action<PointerBitmap> action) => ProcessAsReadOnlyBitmap(span, width, 0, false, action);
        public static void ProcessAsReadOnlyBitmap(this ReadOnlySpan<byte> span, int width, int height, Action<PointerBitmap> action) => ProcessAsReadOnlyBitmap(span, width, height, false, action);
        public static void ProcessAsReadOnlyBitmap(this ReadOnlySpan<float> span, int width, Action<PointerBitmap> action) => ProcessAsReadOnlyBitmap(span, width, 0, true, action);
        public static void ProcessAsReadOnlyBitmap(this ReadOnlySpan<float> span, int width, int height, Action<PointerBitmap> action) => ProcessAsReadOnlyBitmap(span, width, height, true, action);
        public static void ProcessAsReadOnlyBitmap<T>(this ReadOnlySpan<T> span, int width, bool isFloat, Action<PointerBitmap> action)
            where T : unmanaged => ProcessAsReadOnlyBitmap(span, width, 0, isFloat, action);

        public static void ProcessAsReadOnlyBitmap<T>(this ReadOnlySpan<T> span, int width, int height, bool isFloat, Action<PointerBitmap> action)
            where T : unmanaged
        {
            var sourceByteSize = span.Length * sizeof(T);
            var bytesPerPixel = isFloat ? BytesPerFloatPixel : BytesPerUIntPixel;
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(width, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(width, sourceByteSize / bytesPerPixel);
            var spanStride = width * bytesPerPixel;
            if (height is <= 0)
            {
                height = sourceByteSize / spanStride;
            }
            else
            {
                ArgumentOutOfRangeException.ThrowIfGreaterThan(height, sourceByteSize / spanStride);
            }
            fixed (T* ptr = span)
            {
                PointerBitmap bitmap = new((nint)ptr, width, height, isFloat);
                action(bitmap);
            }
        }
    }
}
