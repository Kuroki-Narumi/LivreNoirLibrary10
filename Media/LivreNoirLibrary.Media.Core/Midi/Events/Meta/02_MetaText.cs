using System;
using System.Buffers;
using System.IO;
using System.Text;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Midi.Events
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
            using var o = ArrayPool.Rent<byte>(count);
            var buffer = o.Array;
            reader.Read(buffer, 0, count);
            var text = TextEncoding.GetString(buffer.AsSpan(0, count));
            return new(type, text);
        }

        protected override void DumpDataWithSize(BinaryWriter writer)
        {
            var count = TextEncoding.GetMaxByteCount(_text.Length);
            using var o = ArrayPool.Rent<byte>(count);
            var buffer = o.Array;
            count = TextEncoding.GetBytes(_text, buffer);
            writer.Write7BitEncodedIntBigEndian(count);
            writer.Write(buffer, 0, count);
        }

        public override string ToString() => $"{nameof(MetaText)}{{{nameof(Type)}={Type}, {nameof(Text)}={_text}}}";
    }
}
