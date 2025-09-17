using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Collections
{
    public class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
    {
        private const uint Prime1 = 17;
        private const uint Prime2 = 31;

        public bool Equals(byte[]? x, byte[]? y) => x is not null ? y is not null && x.EqualsAll(y) : y is null;

        public unsafe int GetHashCode([DisallowNull] byte[] obj)
        {
            unchecked
            {
                var hash = Prime1;
                var length = obj.Length;
                fixed (byte* ptr = obj)
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
    }
}
