using System;
using System.IO;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class IdChunk : DataChunk, IDataChunk, IIdChunk
    {
        public override string Chid { get; }
        public override uint ByteSize => sizeof(int) + (uint)Data.Length;

        public int Id { get; set; }

        public IdChunk(string chid, int id, byte[] data)
        {
            Chid = chid;
            Id = id;
            Data = data;
        }

        internal static IdChunk Load(string chid, BinaryReader reader)
        {
            var size = (int)reader.ReadUInt32();
            var id = reader.ReadInt32();
            var data = reader.ReadBytes(size - sizeof(int));
            if (size % 2 is 1)
            {
                reader.ReadByte();
            }
            return new(chid, id, data);
        }

        public override void DumpContents(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(Data);
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteNumber("id", Id);
            writer.WriteBase64String("data", Data);
            writer.WriteString("(data string)", GetText());
        }
    }
}
