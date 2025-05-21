using System.IO;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class FILE : DataChunk, IIdChunk, IRiffChunk<FILE>
    {
        public const int BaseSize = sizeof(int) + sizeof(uint);
        public override string Chid => ChunkIds.FILE;
        public override uint ByteSize => BaseSize + (uint)Data.Length;

        public int Id { get; set; }
        public uint MediaType { get; set; }

        public override void CopyTo<T>(T target)
        {
            base.CopyTo(target);
            if (target is FILE f)
            {
                f.MediaType = MediaType;
            }
        }

        public static FILE LoadContents(BinaryReader reader, uint length)
        {
            FILE data = new();
            data.ProcessLoad(reader, length);
            return data;
        }

        private void ProcessLoad(BinaryReader reader, uint length)
        {
            Id = reader.ReadInt32();
            MediaType = reader.ReadUInt32();
            if (length > BaseSize)
            {
                Data = reader.ReadBytes((int)(length - BaseSize));
            }
        }

        public override void DumpContents(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(MediaType);
            if (Data.Length is > 0)
            {
                writer.Write(Data);
            }
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteNumber("id", Id);
            writer.WriteNumber("media type", MediaType);
            writer.WriteBase64String("data", Data);
            writer.WriteString("(data string)", this.GetText());
        }
    }
}
