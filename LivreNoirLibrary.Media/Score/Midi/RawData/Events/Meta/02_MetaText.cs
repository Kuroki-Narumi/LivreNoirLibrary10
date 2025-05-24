using System;
using System.Buffers;
using System.IO;
using System.Text;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Midi.RawData
{
    public sealed class MetaText(MetaType type, string text) : MetaEvent(type)
    {
        public static Encoding DefaultTextEncoding => Encodings.Shift_JIS;
        public static Encoding TextEncoding { get; set; } = DefaultTextEncoding;

        private string _text = text;

        public string Text { get => _text; set => _text = value; }

        internal static MetaText Load(MetaType type, BinaryReader reader)
        {
            var count = CheckDataLength(reader, 0);
            var buffer = ArrayPool<byte>.Shared.Rent(count);
            try
            {
                reader.Read(buffer, 0, count);
                var text = TextEncoding.GetString(buffer.AsSpan(0, count));
                return new(type, text);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        protected override void DumpDataWithSize(BinaryWriter writer)
        {
            var count = TextEncoding.GetMaxByteCount(_text.Length);
            var buffer = ArrayPool<byte>.Shared.Rent(count);
            try
            {
                count = TextEncoding.GetBytes(_text, buffer);
                writer.Write7BitEncodedIntBigEndian(count);
                writer.Write(buffer, 0, count);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }

        public override string ToString() => $"{nameof(MetaText)}{{{nameof(Type)}={Type}, {nameof(Text)}={_text}}}";
    }
}
