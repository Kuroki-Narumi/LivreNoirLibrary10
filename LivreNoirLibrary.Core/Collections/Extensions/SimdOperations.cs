using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Collections
{
    public static unsafe partial class SimdOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AdjustArgs(int spanLength, ref int offset, ref int length)
        {
            if (offset is < 0)
            {
                offset += spanLength;
            }
            if (offset is < 0)
            {
                spanLength = 0;
                offset = 0;
            }
            offset = Math.Min(offset, spanLength);
            if (length is <= 0)
            {
                length += spanLength;
            }
            length = Math.Min(length, spanLength - offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AdjustArgs(int dstLength, int srcLength, ref int dstOffset, ref int srcOffset, ref int length)
        {
            if (dstOffset is < 0)
            {
                dstOffset += dstLength;
            }
            if (dstOffset is < 0)
            {
                dstLength = dstOffset = 0;
            }
            if (srcOffset is < 0)
            {
                srcOffset += srcLength;
            }
            if (srcOffset is < 0)
            {
                srcLength = srcOffset = 0;
            }
            dstOffset = Math.Min(dstOffset, dstLength);
            srcOffset = Math.Min(srcOffset, srcLength);
            if (length is <= 0)
            {
                length += srcLength;
            }
            length = Math.Min(length, Math.Min(dstLength - dstOffset, srcLength - srcOffset));
        }

        private static bool EqualsCore<T>(T* left, T* right, int length)
            where T : unmanaged, IComparisonOperators<T, T, bool>
        {
            var count = Vector<T>.Count;
            var leftVec = (Vector<T>*)left;
            var rightVec = (Vector<T>*)right;
            for (; length >= count; length -= count, leftVec++, rightVec++)
            {
                if (!Vector.EqualsAll(*leftVec, *rightVec))
                {
                    return false;
                }
            }
            left = (T*)leftVec;
            right = (T*)rightVec;
            for (; length is > 0; length--, left++, right++)
            {
                if (*left != *right)
                {
                    return false;
                }
            }
            return true;
        }

        internal static void Clear<T>(T* destination, int length) where T : unmanaged => ClearCore(destination, length);
        internal static void CopyFrom<T>(T* destination, T value, int length) where T : unmanaged => CopyFromCore(destination, value, length);
        internal static void CopyFrom<T>(T* destination, T* source, int length) where T : unmanaged => CopyFromCore(destination, source, length);

        private static void ClearCore<T>(T* destination, int length)
            where T : unmanaged
        {
            var count = Vector<T>.Count;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec = default;
            }
            if (length is > 0)
            {
                NativeMemory.Clear(dstVec, (nuint)(length * sizeof(T)));
            }
        }

        private static void CopyFromCore<T>(T* destination, T value, int length)
            where T : unmanaged
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec = source;
            }
            if (length is > 0)
            {
                new Span<T>(dstVec, length).Fill(value);
            }
        }

        private static void AddCore<T>(T* destination, T value, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec += source;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination += value;
            }
        }

        private static unsafe void SubtractCore<T>(T* destination, T value, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec -= source;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination -= value;
            }
        }

        private static unsafe void MultiplyCore<T>(T* destination, T value, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec *= source;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination *= value;
            }
        }

        private static unsafe void DivideCore<T>(T* destination, T value, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec /= source;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination /= value;
            }
        }

        private static void CopyFromCore<T>(T* destination, T* source, int length)
            where T : unmanaged
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec = *srcVec;
            }
            if (length is > 0)
            {
                NativeMemory.Copy(srcVec, dstVec, (nuint)(length * sizeof(T)));
            }
        }

        private static void AddCore<T>(T* destination, T* source, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec += *srcVec;
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                *destination += *source;
            }
        }

        private static void SubtractCore<T>(T* destination, T* source, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec -= *srcVec;
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                *destination -= *source;
            }
        }

        private static void MultiplyCore<T>(T* destination, T* source, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec *= *srcVec;
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                *destination *= *source;
            }
        }

        private static void DivideCore<T>(T* destination, T* source, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec /= *srcVec;
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                *destination /= *source;
            }
        }

        private static unsafe void MinCore<T>(T* destination, T value, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec = Vector.Min(*dstVec, source);
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                if (value < *destination)
                {
                    *destination = value;
                }
            }
        }

        private static void MaxCore<T>(T* destination, T value, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec = Vector.Max(*dstVec, source);
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                if (value > *destination)
                {
                    *destination = value;
                }
            }
        }

        private static unsafe void ClampCore<T>(T* destination, T min, T max, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var minVec = Vector.Create(min);
            var maxVec = Vector.Create(max);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec = Vector.Clamp(*dstVec, minVec, maxVec);
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination = T.Clamp(*destination, min, max);
            }
        }

        private static unsafe void MinCore<T>(T* destination, T* source, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec = Vector.Min(*srcVec, *dstVec);
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                if (*source < *destination)
                {
                    *destination = *source;
                }
            }
        }

        private static void MaxCore<T>(T* destination, T* source, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec = Vector.Max(*srcVec, *dstVec);
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                if (*source > *destination)
                {
                    *destination = *source;
                }
            }
        }

        private static unsafe void ClampCore<T>(T* destination, T* min, T* max, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var minVec = (Vector<T>*)min;
            var maxVec = (Vector<T>*)max;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, minVec++, maxVec++, dstVec++)
            {
                *dstVec = Vector.Clamp(*dstVec, *minVec, *maxVec);
            }
            min = (T*)minVec;
            max = (T*)maxVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, min++, max++, destination++)
            {
                *destination = T.Clamp(*destination, *min, *max);
            }
        }

        private static T MinCore<T>(T* source, int length)
            where T : unmanaged, INumber<T>
        {
            if (length is <= 0)
            {
                return default;
            }
            var count = Vector<T>.Count;
            var result = source[0];
            if (length >= count)
            {
                var srcVec = (Vector<T>*)source;
                var resultVec = *srcVec;
                for (; length >= count; length -= count, srcVec++)
                {
                    resultVec = Vector.Min(*srcVec, resultVec);
                }
                for (var i = 0; i < count; i++)
                {
                    result = T.Min(result, resultVec[i]);
                }
                source = (T*)srcVec;
            }
            for (; length is > 0; length--, source++)
            {
                if (*source < result)
                {
                    result = *source;
                }
            }
            return result;
        }

        private static unsafe T MaxCore<T>(T* source, int length)
            where T : unmanaged, INumber<T>
        {
            if (length is <= 0)
            {
                return default;
            }
            var count = Vector<T>.Count;
            var result = source[0];
            if (length >= count)
            {
                var srcVec = (Vector<T>*)source;
                var resultVec = *srcVec;
                for (; length >= count; length -= count, srcVec++)
                {
                    resultVec = Vector.Max(*srcVec, resultVec);
                }
                for (var i = 0; i < count; i++)
                {
                    result = T.Max(result, resultVec[i]);
                }
                source = (T*)srcVec;
            }
            for (; length is > 0; length--, source++)
            {
                if (*source > result)
                {
                    result = *source;
                }
            }
            return result;
        }

        private static unsafe (T Min, T Max) MinMaxCore<T>(T* source, int length)
            where T : unmanaged, INumber<T>
        {
            if (length is <= 0)
            {
                return default;
            }
            var count = Vector<T>.Count;
            var min = source[0];
            var max = min;
            if (length >= count)
            {
                var srcVec = (Vector<T>*)source;
                var minVec = *srcVec;
                var maxVec = minVec;
                for (; length >= count; length -= count, srcVec++)
                {
                    minVec = Vector.Min(*srcVec, minVec);
                    maxVec = Vector.Max(*srcVec, maxVec);
                }
                for (var i = 0; i < count; i++)
                {
                    min = T.Min(min, minVec[i]);
                    max = T.Max(max, maxVec[i]);
                }
                source = (T*)srcVec;
            }
            for (; length is > 0; length--, source++)
            {
                if (*source > max)
                {
                    max = *source;
                }
                if (*source < min)
                {
                    min = *source;
                }
            }
            return (min, max);
        }

        private static T SumCore<T>(T* source, int length)
            where T : unmanaged, INumber<T>
        {
            if (length is <= 0)
            {
                return default;
            }
            var count = Vector<T>.Count;
            var result = T.Zero;
            if (length >= count)
            {
                var srcVec = (Vector<T>*)source;
                var resultVec = new Vector<T>();
                for (; length >= count; length -= count, srcVec++)
                {
                    resultVec += *srcVec;
                }
                result = Vector.Sum(resultVec);
                source = (T*)srcVec;
            }
            for (; length is > 0; length--, source++)
            {
                result += *source;
            }
            return result;
        }

        private static unsafe T SquareCore<T>(T* source, int length)
            where T : unmanaged, INumber<T>
        {
            if (length is <= 0)
            {
                return default;
            }
            var count = Vector<T>.Count;
            var result = T.Zero;
            if (length >= count)
            {
                var srcVec = (Vector<T>*)source;
                var resultVec = new Vector<T>();
                for (; length >= count; length -= count, srcVec++)
                {
                    resultVec += *srcVec * *srcVec;
                }
                result = Vector.Sum(resultVec);
                source = (T*)srcVec;
            }
            for (; length is > 0; length--, source++)
            {
                result += *source * *source;
            }
            return result;
        }

        private static int AverageCore(int* source, int length) => length is <= 0 ? 0 : SumCore(source, length) / length;
        private static uint AverageCore(uint* source, int length) => length is <= 0 ? 0 : SumCore(source, length) / (uint)length;
        private static long AverageCore(long* source, int length) => length is <= 0 ? 0 : SumCore(source, length) / length;
        private static ulong AverageCore(ulong* source, int length) => length is <= 0 ? 0 : SumCore(source, length) / (ulong)length;
        private static float AverageCore(float* source, int length) => length is <= 0 ? 0 : SumCore(source, length) / length;
        private static double AverageCore(double* source, int length) => length is <= 0 ? 0 : SumCore(source, length) / length;

        private static int MeanSquareCore(int* source, int length) => length is <= 0 ? 0 : SquareCore(source, length) / length;
        private static uint MeanSquareCore(uint* source, int length) => length is <= 0 ? 0 : SquareCore(source, length) / (uint)length;
        private static long MeanSquareCore(long* source, int length) => length is <= 0 ? 0 : SquareCore(source, length) / length;
        private static ulong MeanSquareCore(ulong* source, int length) => length is <= 0 ? 0 : SquareCore(source, length) / (ulong)length;
        private static float MeanSquareCore(float* source, int length) => length is <= 0 ? 0 : SquareCore(source, length) / length;
        private static double MeanSquareCore(double* source, int length) => length is <= 0 ? 0 : SquareCore(source, length) / length;

        private static void AbsCore<T>(T* destination, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec = Vector.Abs(*dstVec);
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination = T.Abs(*destination);
            }
        }

        private static unsafe void NegateCore<T>(T* destination, int length)
            where T : unmanaged, INumber<T>
        {
            var count = Vector<T>.Count;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec = -*dstVec;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination = -*destination;
            }
        }

        private static unsafe void NotCore<T>(T* destination, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec = ~*dstVec;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination = ~*destination;
            }
        }

        private static unsafe void AndCore<T>(T* destination, T value, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec &= source;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination &= value;
            }
        }

        private static unsafe void OrCore<T>(T* destination, T value, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec |= source;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination |= value;
            }
        }

        private static unsafe void XorCore<T>(T* destination, T value, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec ^= source;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination ^= value;
            }
        }

        private static unsafe void NandCore<T>(T* destination, T value, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec &= ~source;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination &= ~value;
            }
        }

        private static unsafe void NorCore<T>(T* destination, T value, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec |= ~source;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination |= ~value;
            }
        }

        private static unsafe void XnorCore<T>(T* destination, T value, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec ^= ~source;
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                *destination ^= ~value;
            }
        }

        private static unsafe void AndCore<T>(T* destination, T* source, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec &= *srcVec;
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                *destination &= *source;
            }
        }

        private static unsafe void OrCore<T>(T* destination, T* source, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec |= *srcVec;
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                *destination |= *source;
            }
        }

        private static unsafe void XorCore<T>(T* destination, T* source, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec ^= *srcVec;
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                *destination ^= *source;
            }
        }

        private static unsafe void NandCore<T>(T* destination, T* source, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec &= ~*srcVec;
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                *destination &= ~*source;
            }
        }

        private static unsafe void NorCore<T>(T* destination, T* source, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec |= ~*srcVec;
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                *destination |= ~*source;
            }
        }

        private static unsafe void XnorCore<T>(T* destination, T* source, int length)
            where T : unmanaged, IBinaryInteger<T>
        {
            var count = Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec ^= ~*srcVec;
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                *destination ^= ~*source;
            }
        }
    }
}
