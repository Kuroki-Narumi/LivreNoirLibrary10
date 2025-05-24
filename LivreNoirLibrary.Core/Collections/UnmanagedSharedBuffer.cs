using System;
using System.Numerics;

namespace LivreNoirLibrary.Collections
{
    public unsafe sealed class UnmanagedSharedBuffer<T>(int capacity = SharedBuffer<T>.DefaultCapacity) : SharedBuffer<T>(capacity)
        where T : unmanaged
    {
        public static readonly bool IsHardwareAccelerated = Vector<T>.IsSupported;

        protected override void ProcessCopy(ReadOnlySpan<T> source, Span<T> destination)
        {
            if (IsHardwareAccelerated)
            {
                fixed (T* src = source)
                fixed (T* dst = destination)
                {
                    SimdOperations.CopyFrom(dst, src, source.Length);
                }
            }
            else
            {
                base.ProcessCopy(source, destination);
            }
        }
    }
}
