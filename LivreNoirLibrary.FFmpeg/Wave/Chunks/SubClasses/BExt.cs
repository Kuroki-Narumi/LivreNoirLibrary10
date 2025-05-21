using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public sealed class BExt : RiffChunk, IRiffChunk<BExt>
    {
        public const uint BaseSize = 602;
        public override string Chid => ChunkIds.BExt;
        public override uint ByteSize => BaseSize + (uint)_history.Length;

        private readonly byte[] _description = new byte[256];
        private readonly byte[] _originator = new byte[32];
        private readonly byte[] _reference = new byte[32];
        private readonly byte[] _date = new byte[10];
        private readonly byte[] _time = new byte[8];
        private byte[] _history = [];

        public string Description { get => GetString(_description); set => SetString(_description, value); }
        public string Originator { get => GetString(_originator); set => SetString(_originator, value); }
        public string Reference { get => GetString(_reference); set => SetString(_reference, value); }
        public string Date { get => GetString(_date); set => SetString(_date, value); }
        public string Time { get => GetString(_time); set => SetString(_time, value); }

        public uint TimeReference_Low { get; set; }
        public uint TimeReference_High { get; set; }
        public ushort Version { get; set; }

        public byte[] UMID { get; } = new byte[64];
        public byte[] Reserved { get; } = new byte[190];
        public string CodingHistory { get => GetString(_history); set => _history = Encodings.Shift_JIS.GetBytes(value); }

        private static string GetString(byte[] bytes) => Encodings.Shift_JIS.GetString(bytes).TrimEnd('\u0000');

        private static void SetString(byte[] target, string text)
        {
            var encoding = Encodings.Shift_JIS;
            var len = target.Length;
            var byteCount = encoding.GetByteCount(text);
            if (byteCount > len)
            {
                var charCount = encoding.GetCharCount(target);
                encoding.GetBytes(text.ToCharArray(), 0, charCount, target, 0);
            }
            else
            {
                Array.Clear(target, byteCount, len - byteCount);
                encoding.GetBytes(text.ToCharArray(), 0, text.Length, target, 0);
            }
        }

        public static BExt LoadContents(BinaryReader reader, uint length)
        {
            BExt data = new();
            data.ProcessLoad(reader, length);
            return data;
        }

        private void ProcessLoad(BinaryReader reader, uint length)
        {
            var stream = reader.BaseStream;
            int Read(byte[] target) => stream.Read(target, 0, target.Length);

            Read(_description);
            Read(_originator);
            Read(_reference);
            Read(_date);
            Read(_time);
            TimeReference_Low = reader.ReadUInt32();
            TimeReference_High = reader.ReadUInt32();
            Version = reader.ReadUInt16();
            Read(UMID);
            Read(Reserved);
            if (length > BaseSize)
            {
                _history = reader.ReadBytes((int)(length - BaseSize));
            }
        }

        public override void DumpContents(BinaryWriter writer)
        {
            writer.Write(_description);
            writer.Write(_originator);
            writer.Write(_reference);
            writer.Write(_date);
            writer.Write(_time);
            writer.Write(TimeReference_Low);
            writer.Write(TimeReference_High);
            writer.Write(Version);
            writer.Write(UMID);
            writer.Write(Reserved);
            if (_history.Length is > 0)
            {
                writer.Write(_history);
            }
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteString("description", Description);
            writer.WriteString("originator", Originator);
            writer.WriteString("reference", Reference);
            writer.WriteString("date", Date);
            writer.WriteString("time", Time);
            writer.WriteNumber("time reference low", TimeReference_Low);
            writer.WriteNumber("time reference high", TimeReference_High);
            writer.WriteNumber("version", Version);
            if (_history.Length > 0)
            {
                writer.WriteString("coding history", CodingHistory);
            }
            writer.WriteString("umid", BitConverter.ToString(UMID));
            writer.WriteString("reserved", BitConverter.ToString(Reserved));
        }
    }
}
