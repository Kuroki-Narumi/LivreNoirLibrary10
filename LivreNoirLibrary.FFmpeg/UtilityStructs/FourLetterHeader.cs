using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media
{
    public static class FourLetterHeader
    {
        public const int Length = 4;

        private static readonly Lock _lock = new();
        private static readonly Dictionary<string, uint> _cache = [];
        private static readonly Dictionary<uint, string> _inv_cache = [];

        public static uint GetValue(string str)
        {
            if (!_cache.TryGetValue(str, out var value))
            {
                value = (uint)((byte)str[0] | ((byte)str[1] << 8) | ((byte)str[2] << 16) | ((byte)str[3] << 24));
                _cache.Add(str, value);
                _inv_cache.Add(value, str);
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
                _cache.Add(str, value);
                _inv_cache.Add(value, str);
            }
            return str;
        }

        private static void ThrowArgumentException(int length) => throw new ArgumentException($"Argument must be 4-byte-string. (given: {length}bytes)");
        private static void ThrowEndOfStreamException() => throw new EndOfStreamException("Cannot read 4 bytes.");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(BinaryWriter writer, string chid)
        {
            lock (_lock)
            {
                if (chid.Length is not 4)
                {
                    ThrowArgumentException(chid.Length);
                }
                writer.Write(GetValue(chid));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Read(BinaryReader reader)
        {
            var value = reader.ReadUInt32();
            lock (_lock)
            {
                return GetString(value);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Check(BinaryReader reader, string chid)
        {
            var value = reader.ReadUInt32();
            lock (_lock)
            {
                return value == GetValue(chid);
            }
        }

        public static void CheckAndThrow(BinaryReader reader, string chid)
        {
            var value = reader.ReadUInt32();
            lock (_lock)
            {
                if (value != GetValue(chid))
                {
                    throw new InvalidDataException($"Header pattern mismatched (\"{GetString(value)}\" expected \"{chid}\")");
                }
            }
        }
    }
}
