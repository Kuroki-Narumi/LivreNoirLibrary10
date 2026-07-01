using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Collections
{
    public class ByteArrayEqualityComparer : IEqualityComparer<byte[]>, IAlternateEqualityComparer<ReadOnlySpan<byte>, byte[]>
    {
        public static ByteArrayEqualityComparer Default { get; } = new();

        public bool Equals(byte[]? x, byte[]? y) => x is not null ? y is not null && x.AsSpan().SequenceEqual(y) : y is null;

        public int GetHashCode([DisallowNull] byte[] obj) => GetHashCode(obj.AsSpan());

        private const uint Prime1 = 17;
        private const uint Prime2 = 31;

        public unsafe int GetHashCode(ReadOnlySpan<byte> span)
        {
            unchecked
            {
                var hash = Prime1;
                var length = span.Length;
                fixed (byte* ptr = span)
                {
                    var uintPtr = (uint*)ptr;
                    for (; length is >= sizeof(uint); uintPtr++, length -= sizeof(uint))
                    {
                        hash = (hash * Prime2) + *uintPtr;
                    }
                    var bytePtr = (byte*)uintPtr;
                    for (; length is > 0; bytePtr++, length--)
                    {
                        hash = (hash * Prime2) + *bytePtr;
                    }
                }
                return (int)hash;
            }
        }

        public bool Equals(ReadOnlySpan<byte> alternate, byte[] other) => alternate.SequenceEqual(other);

        public byte[] Create(ReadOnlySpan<byte> alternate) => alternate.ToArray();
    }
}
