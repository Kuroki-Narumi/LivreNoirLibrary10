using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed partial class Sampler : RiffChunk, IRiffChunk<Sampler>
    {
        public const int BaseSize = 36;
        public override string Chid => ChunkIds.Sampler;
        public override uint ByteSize => (uint)(BaseSize + DataList.Count * Data.ByteSize + SamplerData.Length);

        public uint Manufacturer { get; set; }
        public uint ProductCode { get; set; }
        /// <summary>
        /// Time per sample by unit of nano second.
        /// </summary>
        public uint SamplePeriod { get; set; }
        public uint UnityNote { get; set; } = 60;
        public uint PitchFraction { get; set; }
        public uint SMPTEFormat { get; set; }
        public byte[] SMPTEOffset { get; set; } = new byte[4];
        public List<Data> DataList { get; } = [];
        public byte[] SamplerData { get; set; } = [];

        public int SampleRate
        {
            get => (int)Math.Round(1.0e9 / SamplePeriod);
            set => SamplePeriod = (uint)Math.Round(1.0e9 / value);
        }

        public static Sampler Create(int sampleRate)
        {
            var period = (uint)Math.Round(1.0e9 / sampleRate);
            return new()
            {
                Manufacturer = 0,
                ProductCode = 0,
                SamplePeriod = period,
                UnityNote = 60,
                PitchFraction = 0,
                SMPTEFormat = 0,
                SMPTEOffset = [0, 0, 0, 0],
            };
        }

        public static Sampler LoadContents(BinaryReader reader, uint length)
        {
            Sampler data = new();
            data.ProcessLoad(reader);
            return data;
        }

        private void ProcessLoad(BinaryReader reader)
        {
            Manufacturer = reader.ReadUInt32();
            ProductCode = reader.ReadUInt32();
            SamplePeriod = reader.ReadUInt32();
            UnityNote = reader.ReadUInt32();
            PitchFraction = reader.ReadUInt32();
            SMPTEFormat = reader.ReadUInt32();
            reader.Read(SMPTEOffset, 0, 4);
            var dataCount = (int)reader.ReadUInt32();
            var samplerLength = (int)reader.ReadUInt32();
            var list = DataList;
            for (var i = 0; i < dataCount; i++)
            {
                list.Add(Data.Load(reader));
            }
            if (samplerLength > 0)
            {
                SamplerData = reader.ReadBytes(samplerLength);
            }
        }

        public override void DumpContents(BinaryWriter writer)
        {
            writer.Write(Manufacturer);
            writer.Write(ProductCode);
            writer.Write(SamplePeriod);
            writer.Write(UnityNote);
            writer.Write(PitchFraction);
            writer.Write(SMPTEFormat);
            writer.Write(SMPTEOffset);
            writer.Write((uint)DataList.Count);
            writer.Write((uint)SamplerData.Length);
            foreach (var data in CollectionsMarshal.AsSpan(DataList))
            {
                data.Dump(writer);
            }
            if (SamplerData.Length > 0)
            {
                writer.Write(SamplerData);
            }
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteNumber("manufacturer", Manufacturer);
            writer.WriteNumber("product code", ProductCode);
            writer.WriteNumber("sample period", SamplePeriod);
            writer.WriteNumber("(sample rate)", SampleRate);
            writer.WriteNumber("unity note", UnityNote);
            writer.WriteNumber("pitch fraction", PitchFraction);
            writer.WriteNumber("smpte format", SMPTEFormat);
            writer.WritePropertyName("smpte offset");
            writer.WriteStartArray();
            for (int i = 0; i < SMPTEOffset.Length; i++)
            {
                writer.WriteNumberValue(SMPTEOffset[i]);
            }
            writer.WriteEndArray();
            writer.WritePropertyName("data");
            writer.WriteStartArray();
            for (int i = 0; i < DataList.Count; i++)
            {
                DataList[i].WriteJsonContent(writer, options);
            }
            writer.WriteEndArray();
            if (SamplerData.Length > 0)
            {
                writer.WriteString("sampler data", BitConverter.ToString(SamplerData));
                writer.WriteString("(sampler data string)", Encoding.UTF8.GetString(SamplerData));
            }
        }
    }
}
