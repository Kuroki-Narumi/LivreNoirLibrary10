using System.IO;
using System.Text.Json;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class TListData : ContainerDataWithChid, IContainerData<TListData>
    {
        public static uint ByteSize => 24;

        public uint Unknown1 { get; set; }
        public byte Unknown2 { get; set; }
        public byte UnityNote { get; set; }
        public byte Unknown3 { get; set; }
        public byte Unknown4 { get; set; }
        public uint Unknown5 { get; set; }
        public uint Unknown6 { get; set; }

        public static TListData Load(BinaryReader reader) => new()
        {
            Chid = reader.ReadASCII(4),
            Id = reader.ReadInt32(),
            Unknown1 = reader.ReadUInt32(),
            Unknown2 = reader.ReadByte(),
            UnityNote = reader.ReadByte(),
            Unknown3 = reader.ReadByte(),
            Unknown4 = reader.ReadByte(),
            Unknown5 = reader.ReadUInt32(),
            Unknown6 = reader.ReadUInt32(),
        };

        public override void Dump(BinaryWriter writer)
        {
            writer.WriteASCII(Chid, 4);
            base.Dump(writer);
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(UnityNote);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
        }

        protected override void WriteJsonContent(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteString("chid", Chid);
            base.WriteJsonContent(writer, options);
            writer.WriteNumber("unknown 1", Unknown1);
            writer.WriteNumber("unknown 2", Unknown2);
            writer.WriteNumber("unity note", UnityNote);
            writer.WriteNumber("unknown 3", Unknown3);
            writer.WriteNumber("unknown 4", Unknown4);
            writer.WriteNumber("unknown 5", Unknown5);
            writer.WriteNumber("unknown 6", Unknown6);
        }
    }
}
