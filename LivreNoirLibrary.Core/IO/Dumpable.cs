using System;
using System.IO;

namespace LivreNoirLibrary.IO
{
    public static class Dumpable
    {
        public static void Dump<T>(BinaryWriter writer, T obj) where T : IDumpable<T> => obj.Dump(writer);

        public static void Dump(BinaryWriter writer, byte[] buffer) => IOExtensions.WriteWithSize(writer, buffer);
        public static void Dump(BinaryWriter writer, bool value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, byte value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, sbyte value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, short value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, ushort value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, int value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, uint value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, long value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, ulong value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, Half value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, float value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, double value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, decimal value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, char value) => writer.Write(value);
        public static void Dump(BinaryWriter writer, string value) => writer.Write(value);

        public static byte[] LoadBytes(BinaryReader reader) => IOExtensions.ReadWithSize(reader);
        public static byte LoadByte(BinaryReader reader) => reader.ReadByte();
        public static sbyte LoadSByte(BinaryReader reader) => reader.ReadSByte();
        public static short LoadInt16(BinaryReader reader) => reader.ReadInt16();
        public static ushort LoadUInt16(BinaryReader reader) => reader.ReadUInt16();
        public static int LoadInt32(BinaryReader reader) => reader.ReadInt32();
        public static uint LoadUInt32(BinaryReader reader) => reader.ReadUInt32();
        public static long LoadInt64(BinaryReader reader) => reader.ReadInt64();
        public static ulong LoadUInt64(BinaryReader reader) => reader.ReadUInt64();
        public static Half LoadHalf(BinaryReader reader) => reader.ReadHalf();
        public static float LoadSingle(BinaryReader reader) => reader.ReadSingle();
        public static double LoadDouble(BinaryReader reader) => reader.ReadDouble();
        public static decimal LoadDecimal(BinaryReader reader) => reader.ReadDecimal();
        public static char LoadChar(BinaryReader reader) => reader.ReadChar();
        public static string LoadString(BinaryReader reader) => reader.ReadString();
    }
}
