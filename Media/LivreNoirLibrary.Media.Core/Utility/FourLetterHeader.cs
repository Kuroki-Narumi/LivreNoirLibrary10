using System;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media
{
    public static class FourLetterHeader
    {
        public const int Length = 4;

        private static readonly ConcurrentDictionary<string, uint> _cache = [];
        private static readonly ConcurrentDictionary<uint, string> _inv_cache = [];

        public static uint GetValue(string str)
        {
            if (!_cache.TryGetValue(str, out var value))
            {
                value = str[0] | ((uint)str[1] << 8) | ((uint)str[2] << 16) | ((uint)str[3] << 24);
                _cache.TryAdd(str, value);
                _inv_cache.TryAdd(value, str);
            }
            return value;
        }

        public static unsafe string GetString(uint value)
        {
            if (!_inv_cache.TryGetValue(value, out var str))
            {
                var buffer = stackalloc sbyte[Length + 1];
                *(uint*)buffer = value;
                buffer[4] = 0;
                str = new(buffer);
                _cache.TryAdd(str, value);
                _inv_cache.TryAdd(value, str);
            }
            return str;
        }

        private static void ThrowArgumentException(int length) => throw new ArgumentException($"Argument must be 4-byte-string. (given: {length}bytes)");
        private static void ThrowEndOfStreamException() => throw new EndOfStreamException("Cannot read 4 bytes.");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(BinaryWriter writer, string chid)
        {
            if (chid.Length is not 4)
            {
                ThrowArgumentException(chid.Length);
            }
            writer.Write(GetValue(chid));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Read(BinaryReader reader)
        {
            var value = reader.ReadUInt32();
            return GetString(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Check(BinaryReader reader, string chid)
        {
            var value = reader.ReadUInt32();
            return value == GetValue(chid);
        }

        public static void CheckAndThrow(BinaryReader reader, string chid)
        {
            var value = reader.ReadUInt32();
            if (value != GetValue(chid))
            {
                throw new InvalidDataException($"Header pattern mismatched (\"{GetString(value)}\" expected \"{chid}\")");
            }
        }
    }
}
