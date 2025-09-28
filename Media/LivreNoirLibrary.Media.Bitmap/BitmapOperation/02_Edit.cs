using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class BitmapOperation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetColorCore(uint* p, int length, uint clearMask, uint setMask)
        {
            SimdOperations.And(p, clearMask, length);
            SimdOperations.Or(p, setMask, length);
        }

        public static void SetColor(this LnBitmapData bitmap, ColorIndex index, byte value)
        {
            if (bitmap.IsValid)
            {
                var (clearMask, setMask) = ColorUtils.GetClearSetMask((int)index, value);
                SetColorCore(bitmap.Pointer, bitmap.PixelSize, setMask, clearMask);
            }
        }

        public static void SetColor(this LnBitmapData bitmap, Rectangle rect, ColorIndex index, byte value)
        {
            if (bitmap.IsValid && Structs.Adjust(ref rect, bitmap))
            {
                var (clearMask, setMask) = ColorUtils.GetClearSetMask((int)index, value);
                var w = rect.Width;
                foreach (var p in bitmap.EnumerateLines(rect))
                {
                    SetColorCore(p, w, clearMask, setMask);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (int, int, uint, uint) GetSetColorMask(ColorIndex from, ColorIndex to)
        {
            var fromBits = (int)from * 8;
            var toBits = (int)to * 8;
            var fromMask = 255u << fromBits;
            var toMask = 255u << toBits;
            return (fromBits, toBits, fromMask, toMask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetColorCore(uint* p, int length, int fromBits, int toBits, uint fromMask, uint toMask)
        {
            var count = Vector<uint>.Count;
            var vector = (Vector<uint>*)p;
            var fromMaskVector = new Vector<uint>(fromMask);
            var toMaskVector = new Vector<uint>(toMask);
            for (; length >= count; length -= count, vector++)
            {
                // 参照元の抽出
                var buffer = *vector & fromMaskVector;
                // 参照元の位置から参照先の位置へ変換
                buffer >>= fromBits;
                buffer <<= toBits;
                // 参照先のビットをクリア
                *vector &= toMaskVector;
                // 参照元からコピー
                *vector |= buffer;
            }
            p = (uint*)vector;
            for (; length is > 0; length--, p++)
            {
                var value = *p & fromMask;
                value >>= fromBits;
                value <<= toBits;
                *p &= toMask;
                *p |= value;
            }
        }

        public static void SetColor(this LnBitmapData bitmap, ColorIndex from, ColorIndex to)
        {
            if (from != to && bitmap.IsValid)
            {
                var (fromBits, toBits, fromMask, toMask) = GetSetColorMask(from, to);
                SetColorCore(bitmap.Pointer, bitmap.PixelSize, fromBits, toBits, fromMask, ~toMask);
            }
        }

        public static void SetColor(this LnBitmapData bitmap, Rectangle rect, ColorIndex from, ColorIndex to)
        {
            if (from != to && bitmap.IsValid && Structs.Adjust(ref rect, bitmap))
            {
                var (fromBits, toBits, fromMask, toMask) = GetSetColorMask(from, to);
                toMask = ~toMask;
                var w = rect.Width;
                foreach (var p in bitmap.EnumerateLines(rect))
                {
                    SetColorCore(p, w, fromBits, toBits, fromMask, toMask);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SwapColorCore(uint* p, int length, int fromBits, int toBits, uint fromMask, uint toMask)
        {
            var count = Vector<uint>.Count;
            var vector = (Vector<uint>*)p;
            var fromMaskVector = new Vector<uint>(fromMask);
            var toMaskVector = new Vector<uint>(toMask);
            var mask = ~(toMask | fromMask);
            var maskVector = ~(toMaskVector | fromMaskVector);
            for (; length >= count; length -= count, vector++)
            {
                // 参照元の抽出
                var buffer1 = *vector & fromMaskVector;
                var buffer2 = *vector & toMaskVector;
                // 参照元の位置から参照先の位置へ変換
                buffer1 >>= fromBits;
                buffer1 <<= toBits;
                buffer2 >>= toBits;
                buffer2 <<= fromBits;
                // 参照先のビットをクリア
                *vector &= maskVector;
                // 参照元からコピー
                *vector |= buffer1 | buffer2;
            }
            p = (uint*)vector;
            for (; length is > 0; length--, p++)
            {
                var value1 = *p & fromMask;
                var value2 = *p & toMask;
                value1 >>= fromBits;
                value1 <<= toBits;
                value2 >>= toBits;
                value2 <<= fromBits;
                *p &= mask;
                *p |= value1 | value2;
            }
        }

        public static void SwapColor(this LnBitmapData bitmap, ColorIndex from, ColorIndex to)
        {
            if (from != to && bitmap.IsValid)
            {
                var (fromBits, toBits, fromMask, toMask) = GetSetColorMask(from, to);
                SwapColorCore(bitmap.Pointer, bitmap.PixelSize, fromBits, toBits, fromMask, toMask);
            }
        }

        public static void SwapColor(this LnBitmapData bitmap, Rectangle rect, ColorIndex from, ColorIndex to)
        {
            if (from != to && bitmap.IsValid && Structs.Adjust(ref rect, bitmap))
            {
                var (fromBits, toBits, fromMask, toMask) = GetSetColorMask(from, to);
                var w = rect.Width;
                foreach (var p in bitmap.EnumerateLines(rect))
                {
                    SwapColorCore(p, w, fromBits, toBits, fromMask, toMask);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InvertColorCore(uint* p, int length)
        {
            var count = Vector<uint>.Count;
            var vector = (Vector<uint>*)p;
            var maskVector = new Vector<uint>(ColorUtils.Mask_A);
            for (; length >= count; length -= count, vector++)
            {
                var buffer = ~(*vector & maskVector);
                *vector = (*vector & maskVector) | buffer;
            }
            p = (uint*)vector;
            for (; length is > 0; length--, p++)
            {
                var value = ~(*p & ColorUtils.Mask_A);
                *p = (*p & ColorUtils.Mask_A) | value;
            }
        }

        public static void InvertColor(this LnBitmapData bitmap)
        {
            if (bitmap.IsValid)
            {
                InvertColorCore(bitmap.Pointer, bitmap.PixelSize);
            }
        }

        public static void InvertColor(this LnBitmapData bitmap, Rectangle rect)
        {
            if (bitmap.IsValid && Structs.Adjust(ref rect, bitmap))
            {
                var w = rect.Width;
                foreach (var p in bitmap.EnumerateLines(rect))
                {
                    InvertColorCore(p, w);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InvertColorCore(uint* p, int length, uint mask)
        {
            var count = Vector<uint>.Count;
            var vector = (Vector<uint>*)p;
            var maskVector = new Vector<uint>(mask);
            for (; length >= count; length -= count, vector++)
            {
                var buffer = ~*vector & maskVector;
                *vector = (*vector & ~maskVector) | buffer;
            }
            p = (uint*)vector;
            for (; length is > 0; length--, p++)
            {
                var value = ~*p & ~mask;
                *p = (*p & mask) | value;
            }
        }

        public static void InvertColor(this LnBitmapData bitmap, ColorIndex index)
        {
            if (bitmap.IsValid)
            {
                InvertColorCore(bitmap.Pointer, bitmap.PixelSize, ColorUtils.GetMask((int)index));
            }
        }

        public static void InvertColor(this LnBitmapData bitmap, Rectangle rect, ColorIndex index)
        {
            if (bitmap.IsValid && Structs.Adjust(ref rect, bitmap))
            {
                var mask = ColorUtils.GetMask((int)index);
                var w = rect.Width;
                foreach (var p in bitmap.EnumerateLines(rect))
                {
                    InvertColorCore(p, w, mask);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetTransparentCore(uint* p, int length, uint color)
        {
            var count = Vector<uint>.Count;
            var vector = (Vector<uint>*)p;
            var mask = ColorUtils.Mask_A;
            var maskVector = new Vector<uint>(mask);
            var colorVector = new Vector<uint>(color);
            for (; length >= count; length -= count, vector++)
            {
                var buffer = *vector & ~maskVector;
                var equals = Vector.Equals(buffer, colorVector);
                *vector = Vector.ConditionalSelect(equals, buffer, *vector);
            }
            p = (uint*)vector;
            for (; length is > 0; length--, p++)
            {
                var value = *p & ~mask;
                if (value == color)
                {
                    *p = value;
                }
            }
        }

        public static void SetTransparent(this LnBitmapData bitmap, LnColor color)
        {
            if (bitmap.IsValid)
            {
                SetTransparentCore(bitmap.Pointer, bitmap.PixelSize, color.RGB);
            }
        }

        public static void SetTransparent(this LnBitmapData bitmap, Rectangle rect, LnColor color)
        {
            if (bitmap.IsValid && Structs.Adjust(ref rect, bitmap))
            {
                var rgb = color.RGB;
                var w = rect.Width;
                foreach (var p in bitmap.EnumerateLines(rect))
                {
                    SetTransparentCore(p, w, rgb);
                }
            }
        }
    }
}
