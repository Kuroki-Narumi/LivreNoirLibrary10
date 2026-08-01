using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IRiffChunk
    {
        /// <summary>
        /// Riff chunk type represented by four letters.
        /// </summary>
        string Chid { get; }
        /// <summary>
        /// If the chunk byte size is fixed: return it. If it is indefinite: return 0.
        /// </summary>
        uint ByteSize { get; }
        void DumpContents(BinaryWriter writer);
        void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options);

        static void Assert(string chid, uint expectedSize, uint givenSize)
        {
            if (expectedSize != givenSize)
            {
                Console.WriteLine($"warning: wrong chunk size: chid=\"{chid}\", expected={expectedSize}, given={givenSize})");
            }
        }
    }

    public interface IDataChunk : IRiffChunk
    {
        byte[] Data { get; set; }
    }

    public interface IIdChunk : IRiffChunk, IId
    {
        void SetId(int id);
    }

    public interface IRiffChunk<TSelf> : IRiffChunk
        where TSelf : IRiffChunk<TSelf>
    {
        abstract static TSelf LoadContents(BinaryReader reader, ref uint length);
    }

    public static class IRiffChunkExtensions
    {
        public static T ReadRiffChunk<T>(this BinaryReader reader, string chid)
            where T : IRiffChunk<T>
        {
            FourLetterHeader.CheckAndThrow(reader, chid);
            return ReadRiffChunk<T>(reader);
        }

        public static T ReadRiffChunk<T>(this BinaryReader reader)
            where T : IRiffChunk<T>
        {
            var size = reader.ReadUInt32();
            var chunk = T.LoadContents(reader, ref size);
            if (size % 2 is 1)
            {
                _ = reader.BaseStream.ReadByte();
            }
            return chunk;
        }

        public static void WriteRiffChunk<T>(this BinaryWriter writer, T obj)
            where T : IRiffChunk
        {
            FourLetterHeader.Write(writer, obj.Chid);
            var bytes = obj.ByteSize;
            if (bytes is 0)
            {
                // 可変長
                var pos = writer.BaseStream.Position;
                writer.Write(0u);
                obj.DumpContents(writer);
                var endPos = writer.BaseStream.Position;
                writer.BaseStream.Position = pos;
                bytes = (uint)(endPos - pos - sizeof(uint));
                writer.Write(bytes);
                writer.BaseStream.Position = endPos;
            }
            else
            {
                // 固定長
                writer.Write(bytes);
                obj.DumpContents(writer);
            }
            if (bytes % 2 is 1)
            {
                writer.Write((byte)0);
            }
        }

        public static void WriteJsonBasic<T>(this T obj, Utf8JsonWriter writer, JsonSerializerOptions options)
            where T : IRiffChunk
        {
            writer.WriteStartObject();
            writer.WriteString("chid", obj.Chid);
            obj.WriteJsonContents(writer, options);
            writer.WriteEndObject();
        }

        public static void CopyTo<T1, T2>(this T1 source, T2 destination)
            where T1 : IDataChunk
            where T2 : IDataChunk
        {
            var srcData = source.Data;
            var newData = new byte[srcData.Length];
            Array.Copy(srcData, newData, srcData.Length);
            destination.Data = newData;
            if (source is IIdChunk i1 && destination is IIdChunk i2)
            {
                i2.SetId(i1.Id);
            }
        }

        public static string GetText<T>(this T obj, Encoding? encoding = null)
            where T : IDataChunk
        {
            return (encoding ?? Encoding.UTF8).GetString(obj.Data).TrimEnd('\u0000');
        }

        public static void SetText<T>(this T obj, string text, Encoding? encoding = null)
            where T : IDataChunk
        {
            obj.Data = (encoding ?? Encoding.UTF8).GetBytes(text);
        }
    }
}
