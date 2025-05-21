using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed partial class ID3 : RiffChunk, IRiffChunk<ID3>
    {
        public const string Identifier = "ID3";
        public override string Chid => ChunkIds.ID3;

        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public string Version => $"2.{MajorVersion}.{MinorVersion}";
        public byte Flags { get; set; }
        public List<Frame> List { get; } = [];

        public static ID3 LoadContents(BinaryReader reader, uint length)
        {
            ID3 data = new();
            data.ProcessLoad(reader);
            return data;
        }

        private void ProcessLoad(BinaryReader reader)
        {
            var id = reader.ReadASCII(3);
            if (id is Identifier)
            {
                MajorVersion = reader.ReadByte();
                MinorVersion = reader.ReadByte();
                Flags = reader.ReadByte();
                var t1 = reader.ReadByte();
                var t2 = reader.ReadByte();
                var t3 = reader.ReadByte();
                var t4 = reader.ReadByte();
                long length = (t1 << 21) | (t2 << 14) | (t3 << 7) | t4;
                var stream = reader.BaseStream;
                var endPos = stream.Position + length;
                var list = List;
                while (stream.Position <= endPos)
                {
                    var f = Frame.Load(reader, endPos);
                    if (f.Data.Length > 0)
                    {
                        list.Add(f);
                    }
                    else
                    {
                        break;
                    }
                }
                stream.Position = endPos;
            }
        }

        public override void DumpContents(BinaryWriter s)
        {
            s.WriteASCII(Identifier, 3);
            s.Write((byte)MajorVersion);
            s.Write((byte)MinorVersion);
            s.Write(Flags);
            var list = List;
            s.ProcessWrite(
                () =>
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Dump(s);
                    }
                },
                length =>
                {
                    long t4 = length & 0x7F;
                    length >>= 7;
                    long t3 = length & 0x7F;
                    length >>= 7;
                    long t2 = length & 0x7F;
                    length >>= 7;
                    long t1 = length & 0x7F;
                    s.Write((byte)t1);
                    s.Write((byte)t2);
                    s.Write((byte)t3);
                    s.Write((byte)t4);
                });
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteString("id", Identifier);
            writer.WriteString("version", Version);
            writer.WriteString("flags", $"{Flags:X2}");
            writer.WritePropertyName("frames");
            writer.WriteStartArray();
            foreach (var frame in CollectionsMarshal.AsSpan(List))
            {
                frame.WriteJson(writer, options);
            }
            writer.WriteEndArray();
        }
    }
}
