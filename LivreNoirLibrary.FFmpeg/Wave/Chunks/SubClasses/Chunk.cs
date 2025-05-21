using System.IO;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class Chunk : DataChunk, IDataChunk
    {
        public override string Chid { get; }
        public override uint ByteSize => (uint)Data.Length;

        public Chunk(string chid, byte[] data)
        {
            Chid = chid;
            Data = data;
        }

        internal static Chunk Load(string chid, BinaryReader reader)
        {
            var size = reader.ReadUInt32();
            var data = reader.ReadBytes((int)size);
            if (size % 2 is 1)
            {
                reader.ReadByte();
            }
            return new(chid, data);
        }

        public override void DumpContents(BinaryWriter writer)
        {
            writer.Write(Data);
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteBase64String("data", Data);
            writer.WriteString("(data string)", GetText());
        }
    }
}
