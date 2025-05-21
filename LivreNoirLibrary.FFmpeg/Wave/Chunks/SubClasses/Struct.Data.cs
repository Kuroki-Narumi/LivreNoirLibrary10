using System.IO;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public partial class Struct
    {
        public sealed record Data
        {
            public const int ByteSize = sizeof(uint) * 6;

            public uint Unknown1 { get; set; }
            public uint SampleOffset { get; set; }
            public uint Unknown2 { get; set; }
            public uint Unknown3 { get; set; }
            public uint Unknown4 { get; set; }
            public uint Unknown5 { get; set; }

            internal static Data Load(BinaryReader reader) => new()
            {
                Unknown1 = reader.ReadUInt32(),
                SampleOffset = reader.ReadUInt32(),
                Unknown2 = reader.ReadUInt32(),
                Unknown3 = reader.ReadUInt32(),
                Unknown4 = reader.ReadUInt32(),
                Unknown5 = reader.ReadUInt32(),
            };

            internal void Dump(BinaryWriter writer)
            {
                writer.Write(Unknown1);
                writer.Write(SampleOffset);
                writer.Write(Unknown2);
                writer.Write(Unknown3);
                writer.Write(Unknown4);
                writer.Write(Unknown5);
            }

            internal void WriteJsonContent(Utf8JsonWriter writer, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteNumber("unknown 1", Unknown1);
                writer.WriteNumber("sample offset", SampleOffset);
                writer.WriteNumber("unknown 2", Unknown2);
                writer.WriteNumber("unknown 3", Unknown3);
                writer.WriteNumber("unknown 4", Unknown4);
                writer.WriteNumber("unknown 5", Unknown5);
                writer.WriteEndObject();
            }
        }
    }
}
