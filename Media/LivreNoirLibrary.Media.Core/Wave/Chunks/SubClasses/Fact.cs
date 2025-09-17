using System.IO;
using System.Threading;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class Fact(uint sampleLength) : RiffChunk, IRiffChunk<Fact>
    {
        private static readonly Lock _lock = new();
        private static long _fact_position = -1;

        public override string Chid => ChunkIds.Fact;
        public override uint ByteSize => sizeof(uint);

        public uint SampleLength { get; set; } = sampleLength;

        public static Fact LoadContents(BinaryReader reader, uint length) => new(reader.ReadUInt32());

        public long Dump(BinaryWriter writer)
        {
            lock (_lock)
            {
                writer.WriteRiffChunk(this);
                return _fact_position;
            }
        }

        public override void DumpContents(BinaryWriter writer)
        {
            _fact_position = writer.BaseStream.Position;
            writer.Write(SampleLength);
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteNumber("sample length", SampleLength);
        }

        public Fact Clone() => new(SampleLength);
    }
}
