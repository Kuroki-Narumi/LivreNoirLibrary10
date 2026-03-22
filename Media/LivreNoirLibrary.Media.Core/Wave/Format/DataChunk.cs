using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Wave
{
    public readonly record struct DataChunk(long Position, uint Length) : IRiffChunk<DataChunk>, IJsonWriter
    {
        public string Chid => ChunkIds.Data;
        public uint ByteSize => Length;

        public static DataChunk LoadContents(BinaryReader reader, ref uint length)
        {
            var stream = reader.BaseStream;
            var pos = stream.Position;
            var actualLength = (uint)(stream.Length - pos);
            if (actualLength < length)
            {
                IRiffChunk.Assert(ChunkIds.Data, actualLength, length);
                length = actualLength;
            }
            try
            {
                return new(pos, length);
            }
            finally
            {
                stream.Position = pos + length;
            }
        }

        public void DumpContents(BinaryWriter writer)
        {
            writer.BaseStream.Position += Length;
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options) => this.WriteJsonBasic(writer, options);
        public void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteNumber("bytes", Length);
        }

        public void Deconstruct(out long beginning, out uint length)
        {
            beginning = Position;
            length = Length;
        }
    }
}
