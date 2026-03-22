using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Collections
{
    public static unsafe partial class SimdOperations
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, List<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, List<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, List<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, List<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, List<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, List<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, List<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, List<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, List<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, List<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, List<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, List<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, List<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, List<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, List<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, List<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, List<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, List<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, List<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, List<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, List<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, List<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, List<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, List<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, byte[] source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, byte[] source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, sbyte[] source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, sbyte[] source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, short[] source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, short[] source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, ushort[] source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, ushort[] source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, int[] source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, int[] source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, uint[] source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, uint[] source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, nint[] source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, nint[] source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, nuint[] source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, nuint[] source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, long[] source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, long[] source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, ulong[] source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, ulong[] source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, float[] source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, float[] source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, double[] source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, double[] source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, Memory<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, Memory<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, Memory<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, Memory<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, Memory<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, Memory<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, Memory<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, Memory<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, Memory<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, Memory<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, Memory<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, Memory<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, Memory<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, Memory<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, Memory<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, Memory<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, Memory<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, Memory<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, Memory<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, Memory<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, Memory<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, Memory<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, Memory<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, Memory<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, Span<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, Span<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, Span<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, Span<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, Span<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, Span<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, Span<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, Span<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, Span<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, Span<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, Span<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, Span<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, Span<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, Span<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, Span<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, Span<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, Span<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, Span<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, Span<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, Span<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, Span<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, Span<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, Span<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, Span<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, ReadOnlyMemory<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, ReadOnlyMemory<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, ReadOnlyMemory<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, ReadOnlyMemory<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, ReadOnlyMemory<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, ReadOnlyMemory<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, ReadOnlyMemory<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, ReadOnlyMemory<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, ReadOnlyMemory<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, ReadOnlyMemory<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, ReadOnlyMemory<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, ReadOnlyMemory<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, ReadOnlyMemory<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, ReadOnlyMemory<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, ReadOnlyMemory<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, ReadOnlyMemory<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, ReadOnlyMemory<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, ReadOnlyMemory<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, ReadOnlyMemory<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, ReadOnlyMemory<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, ReadOnlyMemory<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, ReadOnlyMemory<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, ReadOnlyMemory<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, ReadOnlyMemory<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, ReadOnlySpan<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, ReadOnlySpan<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, ReadOnlySpan<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, ReadOnlySpan<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, ReadOnlySpan<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, ReadOnlySpan<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, ReadOnlySpan<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, ReadOnlySpan<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, ReadOnlySpan<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, ReadOnlySpan<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, ReadOnlySpan<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, ReadOnlySpan<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, ReadOnlySpan<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, ReadOnlySpan<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, ReadOnlySpan<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, ReadOnlySpan<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, ReadOnlySpan<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, ReadOnlySpan<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, ReadOnlySpan<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, ReadOnlySpan<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, ReadOnlySpan<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, ReadOnlySpan<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, ReadOnlySpan<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, ReadOnlySpan<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, ObservableCollectionBase<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<byte> destination, ObservableCollectionBase<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, ObservableCollectionBase<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<sbyte> destination, ObservableCollectionBase<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, ObservableCollectionBase<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<short> destination, ObservableCollectionBase<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, ObservableCollectionBase<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ushort> destination, ObservableCollectionBase<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, ObservableCollectionBase<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<int> destination, ObservableCollectionBase<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, ObservableCollectionBase<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<uint> destination, ObservableCollectionBase<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, ObservableCollectionBase<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nint> destination, ObservableCollectionBase<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, ObservableCollectionBase<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<nuint> destination, ObservableCollectionBase<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, ObservableCollectionBase<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<long> destination, ObservableCollectionBase<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, ObservableCollectionBase<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<ulong> destination, ObservableCollectionBase<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, ObservableCollectionBase<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<float> destination, ObservableCollectionBase<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, ObservableCollectionBase<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this List<double> destination, ObservableCollectionBase<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, List<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, List<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, List<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, List<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, List<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, List<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, List<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, List<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, List<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, List<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, List<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, List<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, List<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, List<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, List<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, List<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, List<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, List<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, List<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, List<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, List<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, List<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, List<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, List<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, byte[] source, Vector<byte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, byte[] source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, sbyte[] source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, sbyte[] source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, short[] source, Vector<short> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, short[] source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, ushort[] source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, ushort[] source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, int[] source, Vector<int> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, int[] source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, uint[] source, Vector<uint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, uint[] source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, nint[] source, Vector<nint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, nint[] source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, nuint[] source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, nuint[] source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, long[] source, Vector<long> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, long[] source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, ulong[] source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, ulong[] source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, float[] source, Vector<float> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, float[] source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, double[] source, Vector<double> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, double[] source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, Memory<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, Memory<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, Memory<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, Memory<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, Memory<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, Memory<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, Memory<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, Memory<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, Memory<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, Memory<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, Memory<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, Memory<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, Memory<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, Memory<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, Memory<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, Memory<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, Memory<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, Memory<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, Memory<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, Memory<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, Memory<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, Memory<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, Memory<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, Memory<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, Span<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, Span<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, Span<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, Span<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, Span<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, Span<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, Span<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, Span<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, Span<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, Span<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, Span<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, Span<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, Span<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, Span<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, Span<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, Span<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, Span<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, Span<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, Span<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, Span<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, Span<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, Span<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, Span<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, Span<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, ReadOnlyMemory<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, ReadOnlyMemory<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, ReadOnlyMemory<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, ReadOnlyMemory<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, ReadOnlyMemory<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, ReadOnlyMemory<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, ReadOnlyMemory<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, ReadOnlyMemory<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, ReadOnlyMemory<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, ReadOnlyMemory<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, ReadOnlyMemory<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, ReadOnlyMemory<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, ReadOnlyMemory<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, ReadOnlyMemory<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, ReadOnlyMemory<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, ReadOnlyMemory<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, ReadOnlyMemory<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, ReadOnlyMemory<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, ReadOnlyMemory<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, ReadOnlyMemory<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, ReadOnlyMemory<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, ReadOnlyMemory<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, ReadOnlyMemory<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, ReadOnlyMemory<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, ReadOnlySpan<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, ReadOnlySpan<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, ReadOnlySpan<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, ReadOnlySpan<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, ReadOnlySpan<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, ReadOnlySpan<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, ReadOnlySpan<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, ReadOnlySpan<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, ReadOnlySpan<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, ReadOnlySpan<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, ReadOnlySpan<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, ReadOnlySpan<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, ReadOnlySpan<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, ReadOnlySpan<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, ReadOnlySpan<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, ReadOnlySpan<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, ReadOnlySpan<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, ReadOnlySpan<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, ReadOnlySpan<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, ReadOnlySpan<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, ReadOnlySpan<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, ReadOnlySpan<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, ReadOnlySpan<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, ReadOnlySpan<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, ObservableCollectionBase<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this byte[] destination, ObservableCollectionBase<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, ObservableCollectionBase<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this sbyte[] destination, ObservableCollectionBase<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, ObservableCollectionBase<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this short[] destination, ObservableCollectionBase<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, ObservableCollectionBase<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ushort[] destination, ObservableCollectionBase<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, ObservableCollectionBase<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this int[] destination, ObservableCollectionBase<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, ObservableCollectionBase<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this uint[] destination, ObservableCollectionBase<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, ObservableCollectionBase<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nint[] destination, ObservableCollectionBase<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, ObservableCollectionBase<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this nuint[] destination, ObservableCollectionBase<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, ObservableCollectionBase<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this long[] destination, ObservableCollectionBase<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, ObservableCollectionBase<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ulong[] destination, ObservableCollectionBase<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, ObservableCollectionBase<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this float[] destination, ObservableCollectionBase<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, ObservableCollectionBase<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this double[] destination, ObservableCollectionBase<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, List<byte> source, Vector<byte> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, List<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, List<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, List<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, List<short> source, Vector<short> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, List<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, List<ushort> source, Vector<ushort> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, List<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, List<int> source, Vector<int> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, List<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, List<uint> source, Vector<uint> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, List<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, List<nint> source, Vector<nint> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, List<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, List<nuint> source, Vector<nuint> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, List<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, List<long> source, Vector<long> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, List<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, List<ulong> source, Vector<ulong> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, List<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, List<float> source, Vector<float> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, List<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, List<double> source, Vector<double> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, List<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, byte[] source, Vector<byte> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, byte[] source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, sbyte[] source, Vector<sbyte> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, sbyte[] source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, short[] source, Vector<short> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, short[] source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, ushort[] source, Vector<ushort> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, ushort[] source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, int[] source, Vector<int> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, int[] source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, uint[] source, Vector<uint> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, uint[] source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, nint[] source, Vector<nint> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, nint[] source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, nuint[] source, Vector<nuint> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, nuint[] source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, long[] source, Vector<long> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, long[] source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, ulong[] source, Vector<ulong> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, ulong[] source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, float[] source, Vector<float> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, float[] source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, double[] source, Vector<double> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, double[] source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, Memory<byte> source, Vector<byte> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, Memory<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, Memory<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, Memory<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, Memory<short> source, Vector<short> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, Memory<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, Memory<ushort> source, Vector<ushort> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, Memory<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, Memory<int> source, Vector<int> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, Memory<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, Memory<uint> source, Vector<uint> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, Memory<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, Memory<nint> source, Vector<nint> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, Memory<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, Memory<nuint> source, Vector<nuint> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, Memory<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, Memory<long> source, Vector<long> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, Memory<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, Memory<ulong> source, Vector<ulong> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, Memory<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, Memory<float> source, Vector<float> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, Memory<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, Memory<double> source, Vector<double> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, Memory<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, Span<byte> source, Vector<byte> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, Span<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, Span<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, Span<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, Span<short> source, Vector<short> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, Span<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, Span<ushort> source, Vector<ushort> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, Span<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, Span<int> source, Vector<int> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, Span<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, Span<uint> source, Vector<uint> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, Span<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, Span<nint> source, Vector<nint> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, Span<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, Span<nuint> source, Vector<nuint> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, Span<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, Span<long> source, Vector<long> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, Span<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, Span<ulong> source, Vector<ulong> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, Span<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, Span<float> source, Vector<float> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, Span<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, Span<double> source, Vector<double> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, Span<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, ReadOnlyMemory<byte> source, Vector<byte> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, ReadOnlyMemory<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, ReadOnlyMemory<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, ReadOnlyMemory<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, ReadOnlyMemory<short> source, Vector<short> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, ReadOnlyMemory<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, ReadOnlyMemory<ushort> source, Vector<ushort> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, ReadOnlyMemory<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, ReadOnlyMemory<int> source, Vector<int> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, ReadOnlyMemory<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, ReadOnlyMemory<uint> source, Vector<uint> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, ReadOnlyMemory<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, ReadOnlyMemory<nint> source, Vector<nint> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, ReadOnlyMemory<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, ReadOnlyMemory<nuint> source, Vector<nuint> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, ReadOnlyMemory<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, ReadOnlyMemory<long> source, Vector<long> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, ReadOnlyMemory<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, ReadOnlyMemory<ulong> source, Vector<ulong> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, ReadOnlyMemory<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, ReadOnlyMemory<float> source, Vector<float> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, ReadOnlyMemory<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, ReadOnlyMemory<double> source, Vector<double> factor)
        {
            var dst = destination.Span;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, ReadOnlyMemory<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, ReadOnlySpan<byte> source, Vector<byte> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, ReadOnlySpan<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, ReadOnlySpan<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, ReadOnlySpan<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, ReadOnlySpan<short> source, Vector<short> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, ReadOnlySpan<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, ReadOnlySpan<ushort> source, Vector<ushort> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, ReadOnlySpan<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, ReadOnlySpan<int> source, Vector<int> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, ReadOnlySpan<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, ReadOnlySpan<uint> source, Vector<uint> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, ReadOnlySpan<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, ReadOnlySpan<nint> source, Vector<nint> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, ReadOnlySpan<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, ReadOnlySpan<nuint> source, Vector<nuint> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, ReadOnlySpan<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, ReadOnlySpan<long> source, Vector<long> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, ReadOnlySpan<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, ReadOnlySpan<ulong> source, Vector<ulong> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, ReadOnlySpan<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, ReadOnlySpan<float> source, Vector<float> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, ReadOnlySpan<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, ReadOnlySpan<double> source, Vector<double> factor)
        {
            var dst = destination.Span;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, ReadOnlySpan<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, ObservableCollectionBase<byte> source, Vector<byte> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<byte> destination, ObservableCollectionBase<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, ObservableCollectionBase<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<sbyte> destination, ObservableCollectionBase<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, ObservableCollectionBase<short> source, Vector<short> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<short> destination, ObservableCollectionBase<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, ObservableCollectionBase<ushort> source, Vector<ushort> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ushort> destination, ObservableCollectionBase<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, ObservableCollectionBase<int> source, Vector<int> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<int> destination, ObservableCollectionBase<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, ObservableCollectionBase<uint> source, Vector<uint> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<uint> destination, ObservableCollectionBase<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, ObservableCollectionBase<nint> source, Vector<nint> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nint> destination, ObservableCollectionBase<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, ObservableCollectionBase<nuint> source, Vector<nuint> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<nuint> destination, ObservableCollectionBase<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, ObservableCollectionBase<long> source, Vector<long> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<long> destination, ObservableCollectionBase<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, ObservableCollectionBase<ulong> source, Vector<ulong> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<ulong> destination, ObservableCollectionBase<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, ObservableCollectionBase<float> source, Vector<float> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<float> destination, ObservableCollectionBase<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, ObservableCollectionBase<double> source, Vector<double> factor)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Memory<double> destination, ObservableCollectionBase<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination.Span;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, List<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, List<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, List<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, List<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, List<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, List<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, List<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, List<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, List<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, List<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, List<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, List<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, List<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, List<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, List<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, List<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, List<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, List<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, List<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, List<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, List<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, List<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, List<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, List<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, byte[] source, Vector<byte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, byte[] source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, sbyte[] source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, sbyte[] source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, short[] source, Vector<short> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, short[] source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, ushort[] source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, ushort[] source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, int[] source, Vector<int> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, int[] source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, uint[] source, Vector<uint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, uint[] source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, nint[] source, Vector<nint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, nint[] source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, nuint[] source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, nuint[] source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, long[] source, Vector<long> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, long[] source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, ulong[] source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, ulong[] source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, float[] source, Vector<float> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, float[] source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, double[] source, Vector<double> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, double[] source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, Memory<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, Memory<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, Memory<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, Memory<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, Memory<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, Memory<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, Memory<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, Memory<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, Memory<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, Memory<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, Memory<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, Memory<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, Memory<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, Memory<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, Memory<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, Memory<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, Memory<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, Memory<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, Memory<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, Memory<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, Memory<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, Memory<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, Memory<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, Memory<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, Span<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, Span<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, Span<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, Span<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, Span<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, Span<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, Span<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, Span<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, Span<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, Span<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, Span<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, Span<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, Span<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, Span<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, Span<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, Span<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, Span<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, Span<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, Span<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, Span<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, Span<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, Span<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, Span<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, Span<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, ReadOnlyMemory<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, ReadOnlyMemory<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, ReadOnlyMemory<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, ReadOnlyMemory<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, ReadOnlyMemory<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, ReadOnlyMemory<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, ReadOnlyMemory<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, ReadOnlyMemory<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, ReadOnlyMemory<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, ReadOnlyMemory<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, ReadOnlyMemory<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, ReadOnlyMemory<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, ReadOnlyMemory<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, ReadOnlyMemory<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, ReadOnlyMemory<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, ReadOnlyMemory<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, ReadOnlyMemory<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, ReadOnlyMemory<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, ReadOnlyMemory<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, ReadOnlyMemory<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, ReadOnlyMemory<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, ReadOnlyMemory<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, ReadOnlyMemory<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, ReadOnlyMemory<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, ReadOnlySpan<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, ReadOnlySpan<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, ReadOnlySpan<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, ReadOnlySpan<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, ReadOnlySpan<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, ReadOnlySpan<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, ReadOnlySpan<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, ReadOnlySpan<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, ReadOnlySpan<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, ReadOnlySpan<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, ReadOnlySpan<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, ReadOnlySpan<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, ReadOnlySpan<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, ReadOnlySpan<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, ReadOnlySpan<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, ReadOnlySpan<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, ReadOnlySpan<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, ReadOnlySpan<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, ReadOnlySpan<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, ReadOnlySpan<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, ReadOnlySpan<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, ReadOnlySpan<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, ReadOnlySpan<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, ReadOnlySpan<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, ObservableCollectionBase<byte> source, Vector<byte> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<byte> destination, ObservableCollectionBase<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, ObservableCollectionBase<sbyte> source, Vector<sbyte> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<sbyte> destination, ObservableCollectionBase<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, ObservableCollectionBase<short> source, Vector<short> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<short> destination, ObservableCollectionBase<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, ObservableCollectionBase<ushort> source, Vector<ushort> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ushort> destination, ObservableCollectionBase<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, ObservableCollectionBase<int> source, Vector<int> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<int> destination, ObservableCollectionBase<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, ObservableCollectionBase<uint> source, Vector<uint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<uint> destination, ObservableCollectionBase<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, ObservableCollectionBase<nint> source, Vector<nint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nint> destination, ObservableCollectionBase<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, ObservableCollectionBase<nuint> source, Vector<nuint> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<nuint> destination, ObservableCollectionBase<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, ObservableCollectionBase<long> source, Vector<long> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<long> destination, ObservableCollectionBase<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, ObservableCollectionBase<ulong> source, Vector<ulong> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<ulong> destination, ObservableCollectionBase<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, ObservableCollectionBase<float> source, Vector<float> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<float> destination, ObservableCollectionBase<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, ObservableCollectionBase<double> source, Vector<double> factor)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this Span<double> destination, ObservableCollectionBase<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = destination;
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, List<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, List<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, List<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, List<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, List<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, List<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, List<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, List<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, List<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, List<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, List<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, List<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, List<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, List<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, List<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, List<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, List<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, List<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, List<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, List<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, List<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, List<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, List<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, List<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, byte[] source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, byte[] source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, sbyte[] source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, sbyte[] source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, short[] source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, short[] source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, ushort[] source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, ushort[] source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, int[] source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, int[] source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, uint[] source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, uint[] source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, nint[] source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, nint[] source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, nuint[] source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, nuint[] source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, long[] source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, long[] source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, ulong[] source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, ulong[] source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, float[] source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, float[] source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, double[] source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, double[] source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, Memory<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, Memory<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, Memory<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, Memory<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, Memory<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, Memory<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, Memory<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, Memory<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, Memory<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, Memory<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, Memory<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, Memory<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, Memory<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, Memory<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, Memory<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, Memory<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, Memory<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, Memory<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, Memory<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, Memory<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, Memory<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, Memory<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, Memory<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, Memory<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, Span<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, Span<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, Span<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, Span<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, Span<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, Span<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, Span<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, Span<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, Span<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, Span<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, Span<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, Span<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, Span<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, Span<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, Span<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, Span<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, Span<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, Span<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, Span<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, Span<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, Span<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, Span<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, Span<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, Span<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, ReadOnlyMemory<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, ReadOnlyMemory<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, ReadOnlyMemory<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, ReadOnlyMemory<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, ReadOnlyMemory<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, ReadOnlyMemory<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, ReadOnlyMemory<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, ReadOnlyMemory<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, ReadOnlyMemory<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, ReadOnlyMemory<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, ReadOnlyMemory<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, ReadOnlyMemory<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, ReadOnlyMemory<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, ReadOnlyMemory<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, ReadOnlyMemory<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, ReadOnlyMemory<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, ReadOnlyMemory<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, ReadOnlyMemory<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, ReadOnlyMemory<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, ReadOnlyMemory<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, ReadOnlyMemory<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, ReadOnlyMemory<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, ReadOnlyMemory<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, ReadOnlyMemory<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source.Span;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, ReadOnlySpan<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, ReadOnlySpan<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, ReadOnlySpan<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, ReadOnlySpan<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, ReadOnlySpan<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, ReadOnlySpan<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, ReadOnlySpan<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, ReadOnlySpan<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, ReadOnlySpan<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, ReadOnlySpan<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, ReadOnlySpan<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, ReadOnlySpan<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, ReadOnlySpan<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, ReadOnlySpan<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, ReadOnlySpan<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, ReadOnlySpan<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, ReadOnlySpan<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, ReadOnlySpan<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, ReadOnlySpan<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, ReadOnlySpan<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, ReadOnlySpan<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, ReadOnlySpan<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, ReadOnlySpan<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, ReadOnlySpan<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = source;
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, ObservableCollectionBase<byte> source, Vector<byte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<byte> destination, ObservableCollectionBase<byte> source, Vector<byte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (byte* dstPtr = dst)
            fixed (byte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, ObservableCollectionBase<sbyte> source, Vector<sbyte> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<sbyte> destination, ObservableCollectionBase<sbyte> source, Vector<sbyte> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (sbyte* dstPtr = dst)
            fixed (sbyte* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, ObservableCollectionBase<short> source, Vector<short> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<short> destination, ObservableCollectionBase<short> source, Vector<short> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (short* dstPtr = dst)
            fixed (short* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, ObservableCollectionBase<ushort> source, Vector<ushort> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ushort> destination, ObservableCollectionBase<ushort> source, Vector<ushort> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ushort* dstPtr = dst)
            fixed (ushort* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, ObservableCollectionBase<int> source, Vector<int> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<int> destination, ObservableCollectionBase<int> source, Vector<int> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (int* dstPtr = dst)
            fixed (int* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, ObservableCollectionBase<uint> source, Vector<uint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<uint> destination, ObservableCollectionBase<uint> source, Vector<uint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (uint* dstPtr = dst)
            fixed (uint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, ObservableCollectionBase<nint> source, Vector<nint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nint> destination, ObservableCollectionBase<nint> source, Vector<nint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nint* dstPtr = dst)
            fixed (nint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, ObservableCollectionBase<nuint> source, Vector<nuint> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<nuint> destination, ObservableCollectionBase<nuint> source, Vector<nuint> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (nuint* dstPtr = dst)
            fixed (nuint* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, ObservableCollectionBase<long> source, Vector<long> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<long> destination, ObservableCollectionBase<long> source, Vector<long> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (long* dstPtr = dst)
            fixed (long* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, ObservableCollectionBase<ulong> source, Vector<ulong> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<ulong> destination, ObservableCollectionBase<ulong> source, Vector<ulong> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (ulong* dstPtr = dst)
            fixed (ulong* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, ObservableCollectionBase<float> source, Vector<float> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<float> destination, ObservableCollectionBase<float> source, Vector<float> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (float* dstPtr = dst)
            fixed (float* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, ObservableCollectionBase<double> source, Vector<double> factor)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            var length = Math.Min(dst.Length, src.Length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr, srcPtr, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(this ObservableCollectionBase<double> destination, ObservableCollectionBase<double> source, Vector<double> factor, int dstOffset, int srcOffset, int length)
        {
            var dst = CollectionsMarshal.AsSpan(destination._list);
            var src = CollectionsMarshal.AsSpan(source._list);
            AdjustArgs(dst.Length, src.Length, ref dstOffset, ref srcOffset, ref length);
            fixed (double* dstPtr = dst)
            fixed (double* srcPtr = src)
            {
                CopyFromCore(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(byte* destination, byte* source, Vector<byte> factor, nuint length) => CopyFromCore(destination, source, factor, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(sbyte* destination, sbyte* source, Vector<sbyte> factor, nuint length) => CopyFromCore(destination, source, factor, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(short* destination, short* source, Vector<short> factor, nuint length) => CopyFromCore(destination, source, factor, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(ushort* destination, ushort* source, Vector<ushort> factor, nuint length) => CopyFromCore(destination, source, factor, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(int* destination, int* source, Vector<int> factor, nuint length) => CopyFromCore(destination, source, factor, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(uint* destination, uint* source, Vector<uint> factor, nuint length) => CopyFromCore(destination, source, factor, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(nint* destination, nint* source, Vector<nint> factor, nuint length) => CopyFromCore(destination, source, factor, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(nuint* destination, nuint* source, Vector<nuint> factor, nuint length) => CopyFromCore(destination, source, factor, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(long* destination, long* source, Vector<long> factor, nuint length) => CopyFromCore(destination, source, factor, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(ulong* destination, ulong* source, Vector<ulong> factor, nuint length) => CopyFromCore(destination, source, factor, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(float* destination, float* source, Vector<float> factor, nuint length) => CopyFromCore(destination, source, factor, length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyFrom(double* destination, double* source, Vector<double> factor, nuint length) => CopyFromCore(destination, source, factor, length);
    }
}
