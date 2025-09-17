using System.IO;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class PlayListData : ContainerData, IContainerData<PlayListData>
    {
        public static uint ByteSize => 12;

        public uint Length { get; set; }
        public uint Repeat { get; set; }

        public static PlayListData Load(BinaryReader reader) => new()
        {
            Id = reader.ReadInt32(),
            Length = reader.ReadUInt32(),
            Repeat = reader.ReadUInt32(),
        };

        public override void Dump(BinaryWriter writer)
        {
            base.Dump(writer);
            writer.Write(Length);
            writer.Write(Repeat);
        }

        protected override void WriteJsonContent(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            base.WriteJsonContent(writer, options);
            writer.WriteNumber("length", Length);
            writer.WriteNumber("repeat", Repeat);
        }
    }
}
