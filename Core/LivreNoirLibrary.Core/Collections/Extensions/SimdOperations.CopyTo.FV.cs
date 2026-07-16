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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
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
                CopyFrom(dstPtr, srcPtr, factor, (nuint)length);
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
                CopyFrom(dstPtr + dstOffset, srcPtr + srcOffset, factor, (nuint)length);
            }
        }
    }
}
