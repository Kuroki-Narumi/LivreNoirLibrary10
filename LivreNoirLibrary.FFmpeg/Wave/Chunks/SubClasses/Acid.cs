using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class Acid : RiffChunk, IRiffChunk<Acid>
    {
        public override string Chid => ChunkIds.Acid;
        public override uint ByteSize => sizeof(uint) + sizeof(ushort) + sizeof(ushort) + sizeof(float) + sizeof(uint) + sizeof(ushort) + sizeof(ushort) + sizeof(float);

        public uint FileType { get; set; }
        public ushort RootNote { get; set; }
        public ushort Reserved1 { get; set; }
        public float Reserved2 { get; set; }
        public uint Beats { get; set; }
        public ushort SignatureDen { get; set; }
        public ushort SignatureNum { get; set; }
        public float Tempo { get; set; }

        public void SetSignature(int num, int den)
        {
            SignatureDen = (ushort)den;
            SignatureNum = (ushort)num;
        }

        public static Acid Create()
        {
            Acid data = new()
            {
                FileType = 4,
                RootNote = 60,
                Reserved1 = 128,
                Reserved2 = 0,
                SignatureDen = 4,
                SignatureNum = 4,
            };
            return data;
        }

        public void SetTempo(double tempo, double seconds)
        {
            var beats = (uint)Math.Ceiling(seconds * tempo / 60);
            Tempo = (float)tempo;
            Beats = beats;
        }

        public static Acid LoadContents(BinaryReader reader, uint length) => new()
        {
            FileType = reader.ReadUInt32(),
            RootNote = reader.ReadUInt16(),
            Reserved1 = reader.ReadUInt16(),
            Reserved2 = reader.ReadSingle(),
            Beats = reader.ReadUInt32(),
            SignatureDen = reader.ReadUInt16(),
            SignatureNum = reader.ReadUInt16(),
            Tempo = reader.ReadSingle(),
        };

        public override void DumpContents(BinaryWriter writer)
        {
            writer.Write(FileType);
            writer.Write(RootNote);
            writer.Write(Reserved1);
            writer.Write(Reserved2);
            writer.Write(Beats);
            writer.Write(SignatureDen);
            writer.Write(SignatureNum);
            writer.Write(Tempo);
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteNumber("file type", FileType);
            writer.WriteNumber("root note", RootNote);
            writer.WriteNumber("reserved 1", Reserved1);
            writer.WriteNumber("reserved 2", Reserved2);
            writer.WriteNumber("beats", Beats);
            writer.WriteString("time signature", $"{SignatureNum}/{SignatureDen}");
            writer.WriteNumber("tempo", Tempo);
        }
    }
}
