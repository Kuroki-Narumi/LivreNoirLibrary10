using System;
using System.Text;
using System.Text.Json;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public partial class ID3
    {
        public class Frame : IJsonWriter
        {
            public string FrameId { get; set; } = "";
            public ushort Flags { get; set; }
            public byte[] Data { get; set; } = [];

            public static Frame Load(BinaryReader reader, long endPos)
            {
                var stream = reader.BaseStream;
                var id = reader.ReadASCII(4);
                var length = reader.ReadUInt32();
                var flags = reader.ReadUInt16();
                Frame f;
                if (id.StartsWith('T'))
                {
                    f = new Text()
                    {
                        Encoding = reader.ReadByte(),
                    };
                    length -= 1;
                }
                else if (id == "COMM")
                {
                    f = new Comment()
                    {
                        Encoding = reader.ReadByte(),
                        Language = reader.ReadASCII(3),
                    };
                    length -= 4;
                }
                else
                {
                    f = new Frame();
                    if (length <= 0)
                    {
                        length = (uint)(endPos - stream.Position);
                    }
                }
                f.FrameId = id;
                f.Flags = flags;
                f.Data = reader.ReadBytes((int)length);
                return f;
            }

            public virtual void Dump(BinaryWriter writer)
            {
                writer.WriteASCII(FrameId, 4);
                writer.Write(GetFrameLength());
                writer.Write(Flags);
                DumpExt(writer);
                writer.Write(Data);
            }

            protected virtual uint GetFrameLength() => (uint)Data.Length;
            protected virtual void DumpExt(BinaryWriter s) { }

            public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
            {
                writer.WriteStartObject();
                writer.WriteString("id", FrameId);
                writer.WriteString("flags", $"{Flags:X4}");
                WriteJsonExt(writer, options);
                writer.WriteBase64String("data", Data);
                writer.WriteString("(data string)", Encoding.UTF8.GetString(Data));
                writer.WriteEndObject();
            }

            protected virtual void WriteJsonExt(Utf8JsonWriter writer, JsonSerializerOptions options) { }
        }
    }
}
