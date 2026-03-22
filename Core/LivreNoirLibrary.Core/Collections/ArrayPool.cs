using System;
using System.Buffers;

namespace LivreNoirLibrary.Collections
{
    public static class ArrayPool
    {
        public static ArrayPoolDisposable<T> Rent<T>(int length) => new(length);
    }

    public readonly struct ArrayPoolDisposable<T>(int length) : IDisposable
    {
        public readonly int RequiredLength = length;
        public readonly T[] Array = ArrayPool<T>.Shared.Rent(length);

        public Span<T> Span => Array.AsSpan(0, RequiredLength);

        public Span<T> AsSpan(int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(length);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(length, Array.Length);
            return Array.AsSpan(0, length);
        }

        public Memory<T> Memory => Array.AsMemory(0, RequiredLength);

        public Memory<T> AsMemory(int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(length);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(length, Array.Length);
            return Array.AsMemory(0, length);
        }

        public void Dispose() => ArrayPool<T>.Shared.Return(Array);
    }
}
