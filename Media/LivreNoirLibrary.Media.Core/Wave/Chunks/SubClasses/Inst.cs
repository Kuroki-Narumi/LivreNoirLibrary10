using System.IO;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class Inst : RiffChunk, IRiffChunk<Inst>
    {
        public override string Chid => ChunkIds.Inst;
        public override uint ByteSize => 7;

        public sbyte UnshitedNote { get; set; } = 60;
        public sbyte FineTune { get; set; }
        public sbyte Gain { get; set; }
        public sbyte LowNote { get; set; }
        public sbyte HighNote { get; set; } = 127;
        public sbyte LowVelocity { get; set; }
        public sbyte HighVelocity { get; set; } = 127;

        public static Inst LoadContents(BinaryReader reader, uint length)=> new()
        {
            UnshitedNote = reader.ReadSByte(),
            FineTune = reader.ReadSByte(),
            Gain = reader.ReadSByte(),
            LowNote = reader.ReadSByte(),
            HighNote = reader.ReadSByte(),
            LowVelocity = reader.ReadSByte(),
            HighVelocity = reader.ReadSByte(),
        };

        public override void DumpContents(BinaryWriter writer)
        {
            writer.Write(UnshitedNote);
            writer.Write(FineTune);
            writer.Write(Gain);
            writer.Write(LowNote);
            writer.Write(HighNote);
            writer.Write(LowVelocity);
            writer.Write(HighVelocity);
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteNumber("unshifted note", UnshitedNote);
            writer.WriteNumber("fine tune", FineTune);
            writer.WriteNumber("gain", Gain);
            writer.WriteNumber("low note", LowNote);
            writer.WriteNumber("high note", HighNote);
            writer.WriteNumber("low velocity", LowVelocity);
            writer.WriteNumber("high velocity", HighVelocity);
        }
    }
}
