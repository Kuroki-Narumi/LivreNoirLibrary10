using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class LTxt : DataChunk, IIdChunk, IRiffChunk<LTxt>
    {
        public const uint BaseSize = 20;
        public override string Chid => ChunkIds.LTxt;
        public override uint ByteSize => BaseSize + (uint)Data.Length;

        public int Id { get; set; }
        public uint SampleLength { get; set; }
        public string Purpose { get; set; } = "";
        public short Country { get; set; }
        public short Language { get; set; }
        public short Dialect { get; set; }
        public short CodePage { get; set; }

        public override string GetText() => this.GetText(Encodings.Get(CodePage));
        public override void SetText(string text) => this.SetText(text, Encodings.Get(CodePage));

        public void SetCodePage(int page) => CodePage = (short)page;
        public void SetCodePage(string name) => CodePage = (short)Encodings.Get(name).CodePage;

        public override void CopyTo<T>(T target)
        {
            base.CopyTo(target);
            if (target is LTxt t)
            {
                t.SampleLength = SampleLength;
                t.Purpose = Purpose;
                t.Country = Country;
                t.Language = Language;
                t.Dialect = Dialect;
                t.CodePage = CodePage;
            }
        }

        public static LTxt LoadContents(BinaryReader reader, uint length)
        {
            LTxt data = new();
            data.ProcessLoad(reader, length);
            return data;
        }

        private void ProcessLoad(BinaryReader reader, uint length)
        {
            Id = reader.ReadInt32();
            SampleLength = reader.ReadUInt32();
            Purpose = FourLetterHeader.Read(reader);
            Country = reader.ReadInt16();
            Language = reader.ReadInt16();
            Dialect = reader.ReadInt16();
            CodePage = reader.ReadInt16();
            if (length > BaseSize)
            {
                Data = reader.ReadBytes((int)(length - BaseSize));
            }
        }

        public override void DumpContents(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(SampleLength);
            FourLetterHeader.Write(writer, Purpose);
            writer.Write(Country);
            writer.Write(Language);
            writer.Write(Dialect);
            writer.Write(CodePage);
            if (Data.Length is > 0)
            {
                writer.Write(Data);
            }
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteNumber("id", Id);
            writer.WriteNumber("sample length", SampleLength);
            writer.WriteString("purpose", Purpose);
            writer.WriteNumber("country", Country);
            writer.WriteNumber("language", Language);
            writer.WriteNumber("dialect", Dialect);
            writer.WriteNumber("code page", CodePage);
            writer.WriteBase64String("data", Data);
            writer.WriteString("(data string)", GetText());
        }
    }
}
