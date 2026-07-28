using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Collections
{
    public class ByteArrayEqualityComparer : IEqualityComparer<byte[]>, IAlternateEqualityComparer<ReadOnlySpan<byte>, byte[]>
    {
        public static ByteArrayEqualityComparer Default { get; } = new();

        public bool Equals(byte[]? x, byte[]? y) => Equals(x.AsSpan(), y.AsSpan());

        public bool Equals(ReadOnlySpan<byte> alternate, byte[] other) => Equals(alternate, other.AsSpan());

        public static bool Equals(ReadOnlySpan<byte> x, ReadOnlySpan<byte> y) => x.EqualsAll(y);

        public int GetHashCode([DisallowNull] byte[] obj) => GetHashCode(obj.AsSpan());

        public int GetHashCode(ReadOnlySpan<byte> span)
        {
            HashCode hash = new();
            hash.AddBytes(span);
            return hash.ToHashCode();
        }

        public byte[] Create(ReadOnlySpan<byte> alternate) => alternate.ToArray();
    }
}
