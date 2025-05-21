using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed partial class Struct : RiffChunk, IRiffChunk<Struct>
    {
        public const int BaseSize = 28;
        public override string Chid => ChunkIds.Struct;
        public override uint ByteSize => (uint)(BaseSize + DataList.Count * Data.ByteSize);

        public uint Unknown1 { get; set; } = 28;
        public uint Unknown2 { get; set; }
        public uint Unknown3 { get; set; }
        public uint Unknown4 { get; set; } = 1;
        public uint Unknown5 { get; set; } = 10;
        public uint Unknown6 { get; set; }
        public List<Data> DataList { get; } = [];

        public static Struct LoadContents(BinaryReader reader, uint length)
        {
            var uk1 = reader.ReadUInt32();
            var dataCount = (int)reader.ReadUInt32();
            Struct data = new()
            {
                Unknown1 = uk1,
                Unknown2 = reader.ReadUInt32(),
                Unknown3 = reader.ReadUInt32(),
                Unknown4 = reader.ReadUInt32(),
                Unknown5 = reader.ReadUInt32(),
                Unknown6 = reader.ReadUInt32(),
            };
            var list = data.DataList;
            for (var i = 0; i < dataCount; i++)
            {
                list.Add(Data.Load(reader));
            }
            return data;
        }

        public override void DumpContents(BinaryWriter writer)
        {
            writer.Write(Unknown1);
            writer.Write((uint)DataList.Count);
            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);
            foreach (var data in CollectionsMarshal.AsSpan(DataList))
            {
                data.Dump(writer);
            }
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteNumber("unknown 1", Unknown1);
            writer.WriteNumber("unknown 2", Unknown2);
            writer.WriteNumber("unknown 3", Unknown3);
            writer.WriteNumber("unknown 4", Unknown4);
            writer.WriteNumber("unknown 5", Unknown5);
            writer.WriteNumber("unknown 6", Unknown6);
            writer.WritePropertyName("data");
            writer.WriteStartArray();
            foreach (var data in CollectionsMarshal.AsSpan(DataList))
            {
                data.WriteJsonContent(writer, options);
            }
            writer.WriteEndArray();
        }
    }
}
