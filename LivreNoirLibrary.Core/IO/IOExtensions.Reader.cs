using System;
using System.IO;
using System.Buffers;
using System.Buffers.Binary;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace LivreNoirLibrary.IO
{
    public static partial class IOExtensions
    {
        public const int ChidLength = 6;

        public static string ReadASCII(this Stream stream, int length)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(length);
            try
            {
                var len = stream.Read(buffer, 0, length);
                return Encoding.ASCII.GetString(buffer, 0, len);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static string ReadASCII(this BinaryReader reader, int length) => ReadASCII(reader.BaseStream, length);

        public static string ReadChid(this Stream stream) => ReadASCII(stream, ChidLength);
        public static string ReadChid(this BinaryReader reader) => ReadASCII(reader.BaseStream, ChidLength);

        public static string CheckChid(this Stream stream, string? expected)
        {
            if (string.IsNullOrEmpty(expected))
            {
                return string.Empty;
            }
            var chid = ReadChid(stream);
            if (chid != expected)
            {
                ThrowInvalidDataException(expected, chid);
            }
            return chid;
        }

        public static byte[] ReadWithSize(this BinaryReader reader)
        {
            var length = reader.Read7BitEncodedInt();
            return reader.ReadBytes(length);
        }

        public static string CheckChid(this BinaryReader reader, string? expected)
        {
            if (string.IsNullOrEmpty(expected))
            {
                return string.Empty;
            }

            var chid = ReadChid(reader);
            if (chid != expected)
            {
                ThrowInvalidDataException(expected, chid);
            }
            return chid;
        }

        public static string CheckChid(this BinaryReader reader, params ReadOnlySpan<string?> expected)
        {
            var chid = ReadChid(reader);
            foreach (var exp in expected)
            {
                if (chid == exp)
                {
                    return chid;
                }
            }
            ThrowInvalidDataException(expected, chid);
            return chid;
        }

        public static bool TryCheckChid(this BinaryReader reader, string expected, out string given)
        {
            var chid = ReadChid(reader);
            given = chid;
            return chid == expected;
        }

        public static void ThrowInvalidDataException(string expected, string given)
        {
            throw new InvalidDataException($"Header pattern mismatched (expected \"{expected}\" given \"{given}\")");
        }

        public static void ThrowInvalidDataException(ReadOnlySpan<string?> expected, string given)
        {
            throw new InvalidDataException($"Header pattern mismatched (expected \"{string.Join(", ", expected)}\" given \"{given}\")");
        }

        public static short ReadInt16BigEndian(this BinaryReader reader)
        {
            var size = sizeof(short);
            var buffer = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                reader.Read(buffer, 0, size);
                return BinaryPrimitives.ReadInt16BigEndian(buffer.AsSpan(0, size));
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static ushort ReadUInt16BigEndian(this BinaryReader reader)
        {
            var size = sizeof(ushort);
            var buffer = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                reader.Read(buffer, 0, size);
                return BinaryPrimitives.ReadUInt16BigEndian(buffer.AsSpan(0, size));
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static int ReadInt32BigEndian(this BinaryReader reader)
        {
            var size = sizeof(int);
            var buffer = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                reader.Read(buffer, 0, size);
                return BinaryPrimitives.ReadInt32BigEndian(buffer.AsSpan(0, size));
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static uint ReadUInt32BigEndian(this BinaryReader reader)
        {
            var size = sizeof(uint);
            var buffer = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                reader.Read(buffer, 0, size);
                return BinaryPrimitives.ReadUInt32BigEndian(buffer.AsSpan(0, size));
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static long ReadInt64BigEndian(this BinaryReader reader)
        {
            var size = sizeof(long);
            var buffer = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                reader.Read(buffer, 0, size);
                return BinaryPrimitives.ReadInt64BigEndian(buffer.AsSpan(0, size));
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static ulong ReadUInt64BigEndian(this BinaryReader reader)
        {
            var size = sizeof(ulong);
            var buffer = ArrayPool<byte>.Shared.Rent(size);
            try
            {
                reader.Read(buffer, 0, size);
                return BinaryPrimitives.ReadUInt64BigEndian(buffer.AsSpan(0, size));
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static int Read7BitEncodedIntBigEndian(this BinaryReader reader)
        {
            var result = 0u;
            var next = true;
            while (next)
            {
                var b = reader.ReadByte();
                result = (result << 7) | (b & 0x7Fu);
                next = (b & 0x80u) is not 0;
            }
            return (int)result;
        }

        public static long Read7BitEncodedInt64BigEndian(this BinaryReader reader)
        {
            var result = 0UL;
            var next = true;
            while (next)
            {
                var b = reader.ReadByte();
                result = (result << 7) | (b & 0x7Fu);
                next = (b & 0x80u) is not 0;
            }
            return (long)result;
        }

        public static Complex ReadComplex(this BinaryReader reader) => new(reader.ReadDouble(), reader.ReadDouble());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Read<T>(this BinaryReader reader) where T : IDumpable<T> => T.Load(reader);
    }
}
