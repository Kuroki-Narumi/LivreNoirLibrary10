using System;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class CueData() : ContainerDataWithChid, IContainerData<CueData>
    {
        public static uint ByteSize => 24;

        public uint Position { get; set; }
        public uint ChunkStart { get; set; }
        public uint BlockStart { get; set; }
        public uint SampleOffset { get; set; }

        public CueData(long position) : this()
        {
            Position = SampleOffset = (uint)position;
        }

        public static CueData Load(BinaryReader reader) => new()
        {
            Id = reader.ReadInt32(),
            Position = reader.ReadUInt32(),
            Chid = reader.ReadASCII(4),
            ChunkStart = reader.ReadUInt32(),
            BlockStart = reader.ReadUInt32(),
            SampleOffset = reader.ReadUInt32(),
        };

        public override void Dump(BinaryWriter writer)
        {
            base.Dump(writer);
            writer.Write(Position);
            writer.WriteASCII(Chid, 4);
            writer.Write(ChunkStart);
            writer.Write(BlockStart);
            writer.Write(SampleOffset);
        }

        protected override void WriteJsonContent(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            base.WriteJsonContent(writer, options);
            writer.WriteNumber("position", Position);
            writer.WriteString("chid", Chid);
            writer.WriteNumber("chunk start", ChunkStart);
            writer.WriteNumber("block start", BlockStart);
            writer.WriteNumber("sample offset", SampleOffset);
        }

        public void Move(int offset, int limit, bool abs = false)
        {
            var m = Math.Clamp(abs ? 0 : SampleOffset + offset, 0, limit);
            SampleOffset = Position = (uint)m;
        }
    }
}
