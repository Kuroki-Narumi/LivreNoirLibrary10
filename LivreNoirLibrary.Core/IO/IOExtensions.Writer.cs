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
        public static void WriteASCII(this Stream stream, string value, int length = 0)
        {
            if (length is <= 0)
            {
                length = value.Length;
            }
            var buffer = ArrayPool<byte>.Shared.Rent(length);
            try
            {
                var len = value.Length;
                if (len >= length)
                {
                    Encoding.ASCII.GetBytes(value, 0, length, buffer, 0);
                }
                else
                {
                    Encoding.ASCII.GetBytes(value, 0, len, buffer, 0);
                    Array.Clear(buffer, len, length - len);
                }
                stream.Write(buffer, 0, length);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public static void WriteASCII(this BinaryWriter writer, string value, int length = 0) => WriteASCII(writer.BaseStream, value, length);

        public static void WriteChid(this Stream stream, string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                WriteASCII(stream, value, ChidLength);
            }
        }

        public static void WriteChid(this BinaryWriter writer, string? value) => WriteChid(writer.BaseStream, value);

        public static void WriteWithSize(this BinaryWriter writer, byte[] buffer)
        {
            writer.Write7BitEncodedInt(buffer.Length);
            writer.Write(buffer);
        }

        public static void WriteBigEndian(this BinaryWriter writer, short value)
        {
            var span = (stackalloc byte[sizeof(short)]);
            BinaryPrimitives.WriteInt16BigEndian(span, value);
            writer.Write(span);
        }

        public static void WriteBigEndian(this BinaryWriter writer, ushort value)
        {
            var span = (stackalloc byte[sizeof(ushort)]);
            BinaryPrimitives.WriteUInt16BigEndian(span, value);
            writer.Write(span);
        }

        public static void WriteBigEndian(this BinaryWriter writer, int value)
        {
            var span = (stackalloc byte[sizeof(int)]);
            BinaryPrimitives.WriteInt32BigEndian(span, value);
            writer.Write(span);
        }

        public static void WriteBigEndian(this BinaryWriter writer, uint value)
        {
            var span = (stackalloc byte[sizeof(uint)]);
            BinaryPrimitives.WriteUInt32BigEndian(span, value);
            writer.Write(span);
        }

        public static void WriteBigEndian(this BinaryWriter writer, long value)
        {
            var span = (stackalloc byte[sizeof(long)]);
            BinaryPrimitives.WriteInt64BigEndian(span, value);
            writer.Write(span);
        }

        public static void WriteBigEndian(this BinaryWriter writer, ulong value)
        {
            var span = (stackalloc byte[sizeof(ulong)]);
            BinaryPrimitives.WriteUInt64BigEndian(span, value);
            writer.Write(span);
        }

        public static void Write7BitEncodedIntBigEndian(this BinaryWriter writer, int value)
        {
            if (value is <= 0)
            {
                writer.Write((byte)0);
            }
            else
            {
                var count = int.Log2(value) / 7 + 1;
                var uValue = (uint)value;
                var buffer = ArrayPool<byte>.Shared.Rent(count);
                try
                {
                    for (var c = count - 1; c >= 0; c--)
                    {
                        buffer[c] = (byte)(uValue | ~0x7Fu);
                        uValue >>= 7;
                    }
                    buffer[count - 1] &= 0x7F;
                    writer.Write(buffer, 0, count);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
        }

        public static void Write7BitEncodedInt64BigEndian(this BinaryWriter writer, long value)
        {
            if (value is <= 0)
            {
                writer.Write((byte)0);
            }
            else
            {
                var count = (int)long.Log2(value) / 7 + 1;
                var uValue = (ulong)value;
                var buffer = ArrayPool<byte>.Shared.Rent(count);
                try
                {
                    for (var c = count - 1; c >= 0; c--)
                    {
                        buffer[c] = (byte)(uValue | ~0x7Ful);
                        uValue >>= 7;
                    }
                    buffer[count - 1] &= 0x7F;
                    writer.Write(buffer, 0, count);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buffer);
                }
            }
        }

        public static void Write(this BinaryWriter writer, Complex value)
        {
            writer.Write(value.Real);
            writer.Write(value.Imaginary);
        }

        public static void ProcessWrite(this BinaryWriter writer, Action writeAction, Action<long> lengthWriteAction)
        {
            var s = writer.BaseStream;
            var sizePos = s.Position;
            lengthWriteAction(0L);
            var beginPos = s.Position;
            writeAction();
            var endPos = s.Position;
            s.Position = sizePos;
            lengthWriteAction(endPos - beginPos);
            s.Position = endPos;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this BinaryWriter writer, T value) where T : IDumpable<T> => value.Dump(writer);
    }
}
