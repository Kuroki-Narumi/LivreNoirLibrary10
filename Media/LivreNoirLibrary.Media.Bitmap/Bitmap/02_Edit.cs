using LivreNoirLibrary.Collections;
using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media
{
    public static unsafe partial class BitmapOperation
    {
        extension<T>(T bitmap) where T : IBitmap
        {
            public void Clear()
            {
                SimdOperations.Clear((byte*)bitmap.Pointer, (nuint)(bitmap.Height * bitmap.Stride));
            }

            public void Clear(Rectangle rect)
            {
                foreach (var (p, len) in bitmap.EnumerateLines(rect))
                {
                    SimdOperations.Clear((byte*)p, (nuint)len);
                }
            }

            public void InvertColor(ColorFlags flags = ColorFlags.RGB)
            {
                if (bitmap.IsValid)
                {
                    if (bitmap.IsFloat)
                    {
                        var mask = FloatColor.GetMask(flags).AsVector(); ;
                        InvertColorCore((float*)bitmap.Pointer, bitmap.Width * bitmap.Height * 4, mask);
                    }
                    else
                    {
                        var mask = ColorUtils.GetMask(flags);
                        InvertColorCore((uint*)bitmap.Pointer, bitmap.Width * bitmap.Height, mask);
                    }
                }
            }

            public void InvertColor(Rectangle rect, ColorFlags flags = ColorFlags.RGB)
            {
                if (bitmap.Adjust(ref rect))
                {
                    if (bitmap.IsFloat)
                    {
                        var mask = FloatColor.GetMask(flags).AsVector();
                        var stride = rect.Width * 4;
                        foreach (var (p, _) in bitmap.EnumerateLines(rect))
                        {
                            InvertColorCore((float*)p, stride, mask);
                        }
                    }
                    else
                    {
                        var mask = ColorUtils.GetMask(flags);
                        var stride = rect.Width;
                        foreach (var (p, _) in bitmap.EnumerateLines(rect))
                        {
                            InvertColorCore((uint*)p, stride, mask);
                        }
                    }
                }
            }

            public void SetColor(ColorFlags flags, byte value)
            {
                if (bitmap.IsValid)
                {
                    AssertType(bitmap, false);
                    var (clear, set) = GetClearSetMask(flags, value);
                    SetColorCore((uint*)bitmap.Pointer, bitmap.Width * bitmap.Height, clear, set);
                }
            }

            public void SetColor(Rectangle rect, ColorFlags flags, byte value)
            {
                if (bitmap.Adjust(ref rect))
                {
                    AssertType(bitmap, false);
                    var (clear, set) = GetClearSetMask(flags, value);
                    var stride = rect.Width;
                    foreach (var (p, _) in bitmap.EnumerateLines(rect))
                    {
                        SetColorCore((uint*)p, stride, clear, set);
                    }
                }
            }

            public void SetColor(ColorIndex from, ColorIndex to)
            {
                if (from != to && bitmap.IsValid)
                {
                    AssertType(bitmap, false);
                    var (fromBits, toBits, fromExtractMask, toMask) = GetSetColorMask(from, to);
                    SetColorCore((uint*)bitmap.Pointer, bitmap.Width * bitmap.Height, fromBits, toBits, fromExtractMask, ~toMask);
                }
            }

            public void SetColor(Rectangle rect, ColorIndex from, ColorIndex to)
            {
                if (from != to && bitmap.Adjust(ref rect))
                {
                    AssertType(bitmap, false);
                    var (fromBits, toBits, fromExtractMask, toMask) = GetSetColorMask(from, to);
                    var toClearMask = ~toMask;
                    var stride = rect.Width;
                    foreach (var (p, _) in bitmap.EnumerateLines(rect))
                    {
                        SetColorCore((uint*)p, stride, fromBits, toBits, fromExtractMask, toClearMask);
                    }
                }
            }

            public void SwapColor(ColorIndex from, ColorIndex to)
            {
                if (from != to && bitmap.IsValid)
                {
                    AssertType(bitmap, false);
                    var (fromBits, toBits, fromMask, toMask) = GetSetColorMask(from, to);
                    SwapColorCore((uint*)bitmap.Pointer, bitmap.Width * bitmap.Height, fromBits, toBits, fromMask, toMask);
                }
            }

            public void SwapColor(Rectangle rect, ColorIndex from, ColorIndex to)
            {
                if (from != to && bitmap.Adjust(ref rect))
                {
                    AssertType(bitmap, false);
                    var (fromBits, toBits, fromMask, toMask) = GetSetColorMask(from, to);
                    var stride = rect.Width;
                    foreach (var (p, _) in bitmap.EnumerateLines(rect))
                    {
                        SwapColorCore((uint*)p, stride, fromBits, toBits, fromMask, toMask);
                    }
                }
            }

            public void SetTransparent(LnColor color, ColorFlags compareFlags = ColorFlags.RGB)
            {
                if (bitmap.IsValid)
                {
                    AssertType(bitmap, false);
                    var uintColor = (uint)color;
                    var compareMask = ColorUtils.GetMask(compareFlags);
                    SetTransparentCore((uint*)bitmap.Pointer, bitmap.Width * bitmap.Height, uintColor, compareMask);
                }
            }

            public void SetTransparent(Rectangle rect, LnColor color, ColorFlags compareFlags = ColorFlags.RGB)
            {
                AssertType(bitmap, false);
                if (bitmap.Adjust(ref rect))
                {
                    var uintColor = (uint)color;
                    var compareMask = ColorUtils.GetMask(compareFlags);
                    var stride = rect.Width;
                    foreach (var (p, _) in bitmap.EnumerateLines(rect))
                    {
                        SetTransparentCore((uint*)p, stride, uintColor, compareMask);
                    }
                }
            }

            public void SetColor(ColorFlags flags, float value)
            {
                if (bitmap.IsValid)
                {
                    AssertType(bitmap, true);
                    var (clear, set) = GetClearSetMask(flags, value);
                    SetColorCore((float*)bitmap.Pointer, bitmap.Width * bitmap.Height * 4, clear, set);
                }
            }

            public void SetColor(Rectangle rect, ColorFlags flags, float value)
            {
                AssertType(bitmap, true);
                var (clear, set) = GetClearSetMask(flags, value);
                var stride = rect.Width * 4;
                foreach (var (p, _) in bitmap.EnumerateLines(rect))
                {
                    SetColorCore((float*)p, stride, clear, set);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (uint, uint) GetClearSetMask(ColorFlags flags, byte value)
        {
            var clearMask = ColorUtils.GetMask(~flags);
            var setMask = ColorUtils.GetMask(flags, value);
            return (clearMask, setMask);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetColorCore(uint* pointer, int elementCount, uint clearMask, uint setMask)
        {
            var count = Vector<uint>.Count;
            var vector = (Vector<uint>*)pointer;
            var clear = Vector.Create(clearMask);
            var set = Vector.Create(setMask);
            for (; elementCount >= count; elementCount -= count, vector++)
            {
                *vector = (*vector & clear) | set;
            }
            pointer = (uint*)vector;
            for (; elementCount is > 0 ; elementCount--, pointer++)
            {
                *pointer = (*pointer & clearMask) | setMask;
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
        private static void SetColorCore(uint* pointer, int elementCount, int fromBits, int toBits, uint fromExtractMask, uint toClearMask)
        {
            var count = Vector<uint>.Count;
            var vector = (Vector<uint>*)pointer;
            var fromExtract = Vector.Create(fromExtractMask);
            var toClear = Vector.Create(toClearMask);
            for (; elementCount >= count; elementCount -= count, vector++)
            {
                // 参照元の抽出
                var buffer = *vector & fromExtract;
                // 参照元の位置から参照先の位置へ変換
                buffer >>= fromBits;
                buffer <<= toBits;
                // 参照先のビットをクリア
                *vector &= toClear;
                // 参照元からコピー
                *vector |= buffer;
            }
            pointer = (uint*)vector;
            for (; elementCount is > 0; elementCount--, pointer++)
            {
                var value = *pointer & fromExtractMask;
                value >>= fromBits;
                value <<= toBits;
                *pointer &= toClearMask;
                *pointer |= value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SwapColorCore(uint* pointer, int elementCount, int fromBits, int toBits, uint fromMask, uint toMask)
        {
            var count = Vector<uint>.Count;
            var vector = (Vector<uint>*)pointer;
            var fromMaskVector = new Vector<uint>(fromMask);
            var toMaskVector = new Vector<uint>(toMask);
            var mask = ~(toMask | fromMask);
            var maskVector = ~(toMaskVector | fromMaskVector);
            for (; elementCount >= count; elementCount -= count, vector++)
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
            pointer = (uint*)vector;
            for (; elementCount is > 0; elementCount--, pointer++)
            {
                var value1 = *pointer & fromMask;
                var value2 = *pointer & toMask;
                value1 >>= fromBits;
                value1 <<= toBits;
                value2 >>= toBits;
                value2 <<= fromBits;
                *pointer &= mask;
                *pointer |= value1 | value2;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InvertColorCore(uint* pointer, int elementCount, uint mask)
        {
            var count = Vector<uint>.Count;
            var vector = (Vector<uint>*)pointer;
            // 反転する色成分のマスク
            var maskVector = Vector.Create(mask);
            // 反転しない色成分のマスク
            var invertMaskVector = ~maskVector;
            for (; elementCount >= count; elementCount -= count, vector++)
            {
                // 「元ベクトルの反転」と「反転するマスク」の論理積 -> 対象の色成分のみ反転した状態
                var buffer = ~*vector & maskVector;
                // ↑と『「元ベクトル」と「反転しないマスク」の論理積』の論理和
                *vector = buffer | (*vector & invertMaskVector);
            }
            pointer = (uint*)vector;
            var invertMask = ~mask;
            for (; elementCount is > 0; elementCount--, pointer++)
            {
                var value = ~*pointer & invertMask;
                *pointer = value | (*pointer & mask);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetTransparentCore(uint* pointer, int elementCount, uint color, uint compareMask)
        {
            var count = Vector<uint>.Count;
            var vector = (Vector<uint>*)pointer;
            // 比べる色成分のマスク
            var compareVector = Vector.Create(compareMask);
            // 比べる色(先にマスクしておく)
            var colorVector = Vector.Create(color) & compareVector;
            for (; elementCount >= count; elementCount -= count, vector++)
            {
                // 「元ベクトル」と「比べるマスク」の論理積 -> 比べたい色成分のみ抽出
                var buffer = *vector & compareVector;
                // マスク済みの参照ベクトルと比較
                var equals = Vector.Equals(buffer, colorVector);
                // 参照ベクトルに等しい要素は、比べた成分以外の成分がゼロになる
                *vector = Vector.ConditionalSelect(equals, buffer, *vector);
            }
            pointer = (uint*)vector;
            for (; elementCount is > 0; elementCount--, pointer++)
            {
                var value = *pointer & compareMask;
                if (value == color)
                {
                    *pointer = value;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (Vector<float>, Vector<float>) GetClearSetMask(ColorFlags flags, float value)
        {
            var clearMask = FloatColor.GetMask(~flags).AsVector();
            var setMask = FloatColor.GetMask(flags).AsVector();
            return (clearMask, setMask * value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetColorCore(float* pointer, int elementCount, Vector<float> clearMask, Vector<float> setMask)
        {
            var count = Vector<float>.Count;
            var vector = (Vector<float>*)pointer;
            for (; elementCount >= count; elementCount -= count, vector++)
            {
                *vector = (*vector * clearMask) + setMask;
            }
            pointer = (float*)vector;
            for (var i = 0; i < elementCount; i++, pointer++)
            {
                *pointer = (*pointer * clearMask[i]) + setMask[i];
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void InvertColorCore(float* pointer, int elementCount, Vector<float> mask)
        {
            var count = Vector<float>.Count;
            var vector = (Vector<float>*)pointer;
            // 反転しない色成分のマスク
            var invertMaskVector = Vector<float>.One - mask;
            for (; elementCount >= count; elementCount -= count, vector++)
            {
                // 「元ベクトルの反転」と「反転するマスク」の論理積 -> 対象の色成分のみ反転した状態
                var buffer = (Vector<float>.One - *vector) * mask;
                // ↑と『「元ベクトル」と「反転しないマスク」の論理積』の論理和
                *vector = buffer + (*vector * invertMaskVector);
            }
            pointer = (float*)vector;
            for (var i = 0; i < elementCount; i++, pointer++)
            {
                var value = (1 - *pointer) * mask[i];
                *pointer = value + (*pointer * invertMaskVector[i]);
            }
        }
    }
}
