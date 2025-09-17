using System;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.UnsafeOperations
{
    public static class UnsafeMethods
    {
        public static T Get<T>(this nint ptr) where T : struct => Marshal.PtrToStructure<T>(ptr);

        public static unsafe T Get<T>(void* ptr) where T : struct => Marshal.PtrToStructure<T>((nint)ptr);

        public static unsafe T Get<T>(this ReadOnlySpan<byte> span)
            where T : struct
        {
            fixed (byte* ptr = span)
            {
                return Get<T>(ptr);
            }
        }

        public static unsafe T Get<T>(this byte[] array, int index)
            where T : struct
        {
            fixed (byte* ptr = array)
            {
                return Get<T>(ptr + index);
            }
        }

        public static void Set<T>(this nint ptr, T value) where T : struct => Marshal.StructureToPtr(value, ptr, false);

        public static unsafe void Set<T>(void* ptr, T value) where T : struct => Marshal.StructureToPtr(value, (nint)ptr, false);

        public static unsafe void Set<T>(this Span<byte> span, T value)
            where T : struct
        {
            fixed (byte* ptr = span)
            {
                Set(ptr, value);
            }
        }

        public static unsafe void Set<T>(this byte[] array, int index, T value) 
            where T : struct
        {
            fixed(byte* ptr = array)
            {
                Set(ptr + index, value);
            }
        }
    }
}
