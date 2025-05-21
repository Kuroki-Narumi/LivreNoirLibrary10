using System.IO;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public partial class Sampler
    {
        public sealed record Data
        {
            public const int ByteSize = sizeof(uint) * 6;

            public uint Id { get; set; }
            public uint LoopType { get; set; }
            public uint LoopStart { get; set; }
            public uint LoopEnd { get; set; }
            public uint Fraction { get; set; }
            public uint PlayCount { get; set; }

            internal static Data Load(BinaryReader s) => new()
            {
                Id = s.ReadUInt32(),
                LoopType = s.ReadUInt32(),
                LoopStart = s.ReadUInt32(),
                LoopEnd = s.ReadUInt32(),
                Fraction = s.ReadUInt32(),
                PlayCount = s.ReadUInt32(),
            };

            internal void Dump(BinaryWriter s)
            {
                s.Write(Id);
                s.Write(LoopType);
                s.Write(LoopStart);
                s.Write(LoopEnd);
                s.Write(Fraction);
                s.Write(PlayCount);
            }

            internal void WriteJsonContent(Utf8JsonWriter writer, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteNumber("id", Id);
                writer.WriteNumber("loop type", LoopType);
                writer.WriteNumber("loop start", LoopStart);
                writer.WriteNumber("loop end", LoopEnd);
                writer.WriteNumber("fraction", Fraction);
                writer.WriteNumber("play count", PlayCount);
                writer.WriteEndObject();
            }
        }
    }
}
