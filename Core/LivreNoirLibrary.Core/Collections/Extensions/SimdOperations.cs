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
                if (offset is < 0)
                {
                    spanLength = offset = 0;
                }
            }
            else if (offset > spanLength)
            {
                offset = spanLength;
            }
            if (length is <= 0)
            {
                length = Math.Max(length + spanLength, 0);
            }
            else
            {
                length = Math.Min(length, spanLength - offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AdjustArgs(int dstLength, int srcLength, ref int dstOffset, ref int srcOffset, ref int length)
        {
            if (dstOffset is < 0)
            {
                dstOffset += dstLength;
                if (dstOffset is < 0)
                {
                    dstLength = dstOffset = 0;
                }
            }
            else if (dstOffset > dstLength)
            {
                dstOffset = dstLength;
            }
            if (srcOffset is < 0)
            {
                srcOffset += srcLength;
                if (srcOffset is < 0)
                {
                    srcLength = srcOffset = 0;
                }
            }
            else if (srcOffset > srcLength)
            {
                srcOffset = srcLength;
            }
            if (length is <= 0)
            {
                length = Math.Max(length + srcLength, 0);
            }
            else
            {
                length = Math.Min(length, Math.Min(dstLength - dstOffset, srcLength - srcOffset));
            }
        }

        public static bool EqualsAll<T>(T* left, T* right, nuint length)
            where T : unmanaged, IComparisonOperators<T, T, bool>
        {
            var count = (nuint)Vector<T>.Count;
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

        public static void Clear<T>(T* destination, nuint length)
            where T : unmanaged
        {
            var count = (nuint)Vector<T>.Count;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec = default;
            }
            if (length is > 0)
            {
                NativeMemory.Clear(dstVec, length * (nuint)sizeof(T));
            }
        }

        public static void CopyFrom<T>(T* destination, T value, nuint length)
            where T : unmanaged
        {
            var count = (nuint)Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                *dstVec = source;
            }
            if (length is > 0)
            {
                new Span<T>(dstVec, (int)length).Fill(value);
            }
        }

        public static void CopyFrom<T>(T* destination, T* source, nuint length)
            where T : unmanaged
        {
            var count = (nuint)Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                *dstVec = *srcVec;
            }
            if (length is > 0)
            {
                NativeMemory.Copy(srcVec, dstVec, length * (nuint)sizeof(T));
            }
        }

        delegate void VectorOperation<T>(Vector<T> source, Vector<T>* destination) where T : unmanaged;
        delegate void ScalarOperation<T>(T source, T* destination) where T : unmanaged;

        private static void OperateCore<T>(T* destination, T value, nuint length, VectorOperation<T> vop, ScalarOperation<T> sop)
            where T : unmanaged
        {
            var count = (nuint)Vector<T>.Count;
            var source = Vector.Create(value);
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                vop(source, dstVec);
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                sop(value, destination);
            }
        }

        private static void OperateCore<T>(T* destination, T* source, nuint length, VectorOperation<T> vop, ScalarOperation<T> sop)
            where T : unmanaged
        {
            var count = (nuint)Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                vop(*srcVec, dstVec);
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                sop(*source, destination);
            }
        }

        private static void OperateCore<T>(T* destination, T* source, T factor, nuint length, VectorOperation<T> vop, ScalarOperation<T> sop)
            where T : unmanaged, INumber<T>
        {
            var count = (nuint)Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                vop(*srcVec * factor, dstVec);
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (; length is > 0; length--, source++, destination++)
            {
                sop(*source * factor, destination);
            }
        }

        private static void OperateCore<T>(T* destination, T* source, Vector<T> factor, nuint length, VectorOperation<T> vop, ScalarOperation<T> sop)
            where T : unmanaged, INumber<T>
        {
            var count = (nuint)Vector<T>.Count;
            var srcVec = (Vector<T>*)source;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, srcVec++, dstVec++)
            {
                vop(*srcVec * factor, dstVec);
            }
            source = (T*)srcVec;
            destination = (T*)dstVec;
            for (var i = 0; length is > 0; length--, source++, destination++, i++)
            {
                sop(*source * factor[i], destination);
            }
        }

        private static void V_Copy<T>(Vector<T> source, Vector<T>* dest) => *dest = source;
        private static void S_Copy<T>(T source, T* dest) where T : unmanaged, INumber<T> => *dest = source;
        public static void CopyFrom<T>(T* destination, T* source, T factor, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, factor, length, V_Copy, S_Copy);
        public static void CopyFrom<T>(T* destination, T* source, Vector<T> factor, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, factor, length, V_Copy, S_Copy);

        private static void V_Add<T>(Vector<T> source, Vector<T>* dest) => *dest += source;
        private static void S_Add<T>(T source, T* dest) where T : unmanaged, INumber<T> => *dest += source;
        public static void Add<T>(T* destination, T value, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, value, length, V_Add, S_Add);
        public static void Add<T>(T* destination, T* source, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, length, V_Add, S_Add);
        public static void Add<T>(T* destination, T* source, T factor, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, factor, length, V_Add, S_Add);
        public static void Add<T>(T* destination, T* source, Vector<T> factor, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, factor, length, V_Add, S_Add);

        private static void V_Sub<T>(Vector<T> source, Vector<T>* dest) => *dest -= source;
        private static void S_Sub<T>(T source, T* dest) where T : unmanaged, INumber<T> => *dest -= source;
        public static void Subtract<T>(T* destination, T value, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, value, length, V_Sub, S_Sub);
        public static void Subtract<T>(T* destination, T* source, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, length, V_Sub, S_Sub);
        public static void Subtract<T>(T* destination, T* source, T factor, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, factor, length, V_Sub, S_Sub);
        public static void Subtract<T>(T* destination, T* source, Vector<T> factor, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, factor, length, V_Sub, S_Sub);

        private static void V_Mul<T>(Vector<T> source, Vector<T>* dest) => *dest *= source;
        private static void S_Mul<T>(T source, T* dest) where T : unmanaged, INumber<T> => *dest *= source;
        public static void Multiply<T>(T* destination, T value, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, value, length, V_Mul, S_Mul);
        public static void Multiply<T>(T* destination, T* source, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, length, V_Mul, S_Mul);
        public static void Multiply<T>(T* destination, T* source, T factor, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, factor, length, V_Mul, S_Mul);
        public static void Multiply<T>(T* destination, T* source, Vector<T> factor, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, factor, length, V_Mul, S_Mul);

        private static void V_Div<T>(Vector<T> source, Vector<T>* dest) => *dest /= source;
        private static void S_Div<T>(T source, T* dest) where T : unmanaged, INumber<T> => *dest /= source;
        public static void Divide<T>(T* destination, T value, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, value, length, V_Div, S_Div);
        public static void Divide<T>(T* destination, T* source, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, length, V_Div, S_Div);
        public static void Divide<T>(T* destination, T* source, T factor, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, factor, length, V_Div, S_Div);
        public static void Divide<T>(T* destination, T* source, Vector<T> factor, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, factor, length, V_Div, S_Div);

        private static void V_Min<T>(Vector<T> source, Vector<T>* dest) => *dest = Vector.Min(*dest, source);
        private static void S_Min<T>(T source, T* dest) where T : unmanaged, INumber<T>
        {
            if (source < *dest)
            {
                *dest = source;
            }
        }
        public static void Min<T>(T* destination, T value, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, value, length, V_Min, S_Min);
        public static void Min<T>(T* destination, T* source, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, length, V_Min, S_Min);

        private static void V_Max<T>(Vector<T> source, Vector<T>* dest) => *dest = Vector.Max(*dest, source);
        private static void S_Max<T>(T source, T* dest) where T : unmanaged, INumber<T>
        {
            if (source > *dest)
            {
                *dest = source;
            }
        }
        public static void Max<T>(T* destination, T value, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, value, length, V_Max, S_Max);
        public static void Max<T>(T* destination, T* source, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, source, length, V_Max, S_Max);

        public static void Clamp<T>(T* destination, T min, T max, nuint length)
            where T : unmanaged, INumber<T>
        {
            var count = (nuint)Vector<T>.Count;
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

        public static void Clamp<T>(T* destination, T* min, T* max, nuint length)
            where T : unmanaged, INumber<T>
        {
            var count = (nuint)Vector<T>.Count;
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

        public static T Min<T>(T* source, nuint length)
            where T : unmanaged, INumber<T>
        {
            if (length is <= 0)
            {
                return default;
            }
            var count = (nuint)Vector<T>.Count;
            var result = source[0];
            if (length >= count)
            {
                var srcVec = (Vector<T>*)source;
                var resultVec = *srcVec;
                for (; length >= count; length -= count, srcVec++)
                {
                    resultVec = Vector.Min(*srcVec, resultVec);
                }
                var intCount = Vector<T>.Count;
                for (var i = 0; i < intCount; i++)
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

        public static T Max<T>(T* source, nuint length)
            where T : unmanaged, INumber<T>
        {
            if (length is <= 0)
            {
                return default;
            }
            var count = (nuint)Vector<T>.Count;
            var result = source[0];
            if (length >= count)
            {
                var srcVec = (Vector<T>*)source;
                var resultVec = *srcVec;
                for (; length >= count; length -= count, srcVec++)
                {
                    resultVec = Vector.Max(*srcVec, resultVec);
                }
                var intCount = Vector<T>.Count;
                for (var i = 0; i < intCount; i++)
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

        public static (T Min, T Max) MinMax<T>(T* source, nuint length)
            where T : unmanaged, INumber<T>
        {
            if (length is <= 0)
            {
                return default;
            }
            var count = (nuint)Vector<T>.Count;
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
                var intCount = Vector<T>.Count;
                for (var i = 0; i < intCount; i++)
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

        public static T Sum<T>(T* source, nuint length)
            where T : unmanaged, INumber<T>
        {
            if (length is <= 0)
            {
                return default;
            }
            var count = (nuint)Vector<T>.Count;
            var result = T.Zero;
            if (length >= count)
            {
                var srcVec = (Vector<T>*)source;
                var resultVec = Vector<T>.Zero;
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

        public static T Square<T>(T* source, nuint length)
            where T : unmanaged, INumber<T>
        {
            if (length is <= 0)
            {
                return default;
            }
            var count = (nuint)Vector<T>.Count;
            var result = T.Zero;
            if (length >= count)
            {
                var srcVec = (Vector<T>*)source;
                var resultVec = Vector<T>.Zero;
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

        public static int Average(int* source, nuint length) => length is 0 ? 0 : Sum(source, length) / (int)length;
        public static uint Average(uint* source, nuint length) => length is 0 ? 0 : Sum(source, length) / (uint)length;
        public static long Average(long* source, nuint length) => length is 0 ? 0 : Sum(source, length) / (long)length;
        public static ulong Average(ulong* source, nuint length) => length is 0 ? 0 : Sum(source, length) / (ulong)length;
        public static float Average(float* source, nuint length) => length is 0 ? 0 : Sum(source, length) / length;
        public static double Average(double* source, nuint length) => length is 0 ? 0 : Sum(source, length) / length;

        public static int MeanSquare(int* source, nuint length) => length is 0 ? 0 : Square(source, length) / (int)length;
        public static uint MeanSquare(uint* source, nuint length) => length is 0 ? 0 : Square(source, length) / (uint)length;
        public static long MeanSquare(long* source, nuint length) => length is 0 ? 0 : Square(source, length) / (long)length;
        public static ulong MeanSquare(ulong* source, nuint length) => length is 0 ? 0 : Square(source, length) / (ulong)length;
        public static float MeanSquare(float* source, nuint length) => length is 0 ? 0 : Square(source, length) / length;
        public static double MeanSquare(double* source, nuint length) => length is 0 ? 0 : Square(source, length) / length;

        delegate void UnaryVectorOperation<T>(Vector<T>* vector);
        delegate void UnaryScalarOperation<T>(T* vector) where T : unmanaged;

        private static void OperateCore<T>(T* destination, nuint length, UnaryVectorOperation<T> vop, UnaryScalarOperation<T> sop)
            where T : unmanaged
        {
            var count = (nuint)Vector<T>.Count;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                vop(dstVec);
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                sop(destination);
            }
        }

        static void V_Abs<T>(Vector<T>* dest) where T : unmanaged, INumber<T> => *dest = Vector.Abs(*dest);
        static void S_Abs<T>(T* dest) where T : unmanaged, INumber<T> => *dest = T.Abs(*dest);
        public static void Abs<T>(T* destination, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, length, V_Abs, S_Abs);

        static void V_Negate<T>(Vector<T>* dest) where T : unmanaged, INumber<T> => *dest = -*dest;
        static void S_Negate<T>(T* dest) where T : unmanaged, INumber<T> => *dest = -*dest;
        public static void Negate<T>(T* destination, nuint length) where T : unmanaged, INumber<T> => OperateCore(destination, length, V_Negate, S_Negate);

        delegate void VectorShiftOperation<T>(Vector<T>* vector, int count);
        delegate void ScalarShitfOperation<T>(T* vector, int count) where T : unmanaged;
        private static void ShiftCore<T>(T* destination, int shiftCount, nuint length, VectorShiftOperation<T> vop, ScalarShitfOperation<T> sop)
            where T : unmanaged
        {
            var count = (nuint)Vector<T>.Count;
            var dstVec = (Vector<T>*)destination;
            for (; length >= count; length -= count, dstVec++)
            {
                vop(dstVec, shiftCount);
            }
            destination = (T*)dstVec;
            for (; length is > 0; length--, destination++)
            {
                sop(destination, shiftCount);
            }
        }

        static void V_ShiftLeft<T>(Vector<T>* dest, int count) where T : unmanaged, IBinaryInteger<T> => *dest <<= count;
        static void S_ShiftLeft<T>(T* dest, int count) where T : unmanaged, IBinaryInteger<T> => *dest <<= count;
        public static void ShiftLeft<T>(T* destination, int shiftCount, nuint length) where T : unmanaged, IBinaryInteger<T> => ShiftCore(destination, shiftCount, length, V_ShiftLeft, S_ShiftLeft);

        static void V_ShiftRA<T>(Vector<T>* dest, int count) where T : unmanaged, IBinaryInteger<T> => *dest >>= count;
        static void S_ShiftRA<T>(T* dest, int count) where T : unmanaged, IBinaryInteger<T> => *dest >>= count;
        public static void ShiftRightArithmetic<T>(T* destination, int shiftCount, nuint length) where T : unmanaged, IBinaryInteger<T> => ShiftCore(destination, shiftCount, length, V_ShiftRA, S_ShiftRA);

        static void V_ShiftRL<T>(Vector<T>* dest, int count) where T : unmanaged, IBinaryInteger<T> => *dest >>>= count;
        static void S_ShiftRL<T>(T* dest, int count) where T : unmanaged, IBinaryInteger<T> => *dest >>>= count;
        public static void ShiftRightLogical<T>(T* destination, int shiftCount, nuint length) where T : unmanaged, IBinaryInteger<T> => ShiftCore(destination, shiftCount, length, V_ShiftRL, S_ShiftRL);

        static void V_Not<T>(Vector<T>* dest) where T : unmanaged, IBinaryInteger<T> => *dest = ~*dest;
        static void S_Not<T>(T* dest) where T : unmanaged, IBinaryInteger<T> => *dest = ~*dest;
        public static void Not<T>(T* destination, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, length, V_Not, S_Not);

        static void V_And<T>(Vector<T> source, Vector<T>* dest) where T : unmanaged, IBinaryInteger<T> => *dest &= source;
        static void S_And<T>(T source, T* dest) where T : unmanaged, IBinaryInteger<T> => *dest &= source;
        public static void And<T>(T* destination, T value, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, value, length, V_And, S_And);
        public static void And<T>(T* destination, T* source, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, source, length, V_And, S_And);

        static void V_Or<T>(Vector<T> source, Vector<T>* dest) where T : unmanaged, IBinaryInteger<T> => *dest |= source;
        static void S_Or<T>(T source, T* dest) where T : unmanaged, IBinaryInteger<T> => *dest |= source;
        public static void Or<T>(T* destination, T value, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, value, length, V_Or, S_Or);
        public static void Or<T>(T* destination, T* source, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, source, length, V_Or, S_Or);

        static void V_Xor<T>(Vector<T> source, Vector<T>* dest) where T : unmanaged, IBinaryInteger<T> => *dest ^= source;
        static void S_Xor<T>(T source, T* dest) where T : unmanaged, IBinaryInteger<T> => *dest ^= source;
        public static void Xor<T>(T* destination, T value, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, value, length, V_Xor, S_Xor);
        public static void Xor<T>(T* destination, T* source, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, source, length, V_Xor, S_Xor);

        static void V_Nand<T>(Vector<T> source, Vector<T>* dest) where T : unmanaged, IBinaryInteger<T> => *dest = ~(*dest & source);
        static void S_Nand<T>(T source, T* dest) where T : unmanaged, IBinaryInteger<T> => *dest = ~(*dest & source);
        public static void Nand<T>(T* destination, T value, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, value, length, V_Nand, S_Nand);
        public static void Nand<T>(T* destination, T* source, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, source, length, V_Nand, S_Nand);

        static void V_Nor<T>(Vector<T> source, Vector<T>* dest) where T : unmanaged, IBinaryInteger<T> => *dest = ~(*dest | source);
        static void S_Nor<T>(T source, T* dest) where T : unmanaged, IBinaryInteger<T> => *dest = ~(*dest | source);
        public static void Nor<T>(T* destination, T value, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, value, length, V_Nor, S_Nor);
        public static void Nor<T>(T* destination, T* source, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, source, length, V_Nor, S_Nor);

        static void V_Xnor<T>(Vector<T> source, Vector<T>* dest) where T : unmanaged, IBinaryInteger<T> => *dest = ~(*dest ^ source);
        static void S_Xnor<T>(T source, T* dest) where T : unmanaged, IBinaryInteger<T> => *dest = ~(*dest ^ source);
        public static void Xnor<T>(T* destination, T value, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, value, length, V_Xnor, S_Xnor);
        public static void Xnor<T>(T* destination, T* source, nuint length) where T : unmanaged, IBinaryInteger<T> => OperateCore(destination, source, length, V_Xnor, S_Xnor);
    }
}
