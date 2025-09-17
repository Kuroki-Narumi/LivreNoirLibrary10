using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Ogg.Vorbis
{
    public readonly struct VorbisComment(string key, string value) : IComparable<VorbisComment>
    {
        public const char Separator = '=';
        public const byte SeparatorByte = (byte)Separator;

        public readonly string Key = key;
        public readonly string Value = value;

        public static implicit operator VorbisComment((string, string) tuple) => new(tuple.Item1, tuple.Item2);
        public static implicit operator VorbisComment(KeyValuePair<string, string> kv) => new(kv.Key, kv.Value);

        public void Deconstruct(out string key, out string value)
        {
            key = Key;
            value = Value;
        }

        public int CompareTo(VorbisComment other)
        {
            var c = Key.CompareTo(other.Key);
            if (c is not 0)
            {
                return c;
            }
            return Value.CompareTo(other.Value);
        }

        public override string ToString() => $"{Key}{Separator}{Value}";

        internal static VorbisComment FromSpan(ReadOnlySpan<byte> span, Encoding encoding)
        {
            var separator = span.IndexOf(SeparatorByte);
            var key = encoding.GetString(span[..separator]);
            var value = encoding.GetString(span[(separator + 1)..]);
            return new(key, value);
        }

        internal int GetMaxByteCount(Encoding encoding) => encoding.GetMaxByteCount(Key.Length) + 1 + encoding.GetMaxByteCount(Value.Length);

        internal int CopyTo(byte[] buffer, int index, Encoding encoding)
        {
            var key = Key;
            var val = Value;
            // key
            var count = encoding.GetBytes(key, 0, key.Length, buffer, index);
            index += count;
            // separator
            buffer[index] = SeparatorByte;
            count++;
            index++;
            // value
            count += encoding.GetBytes(val, 0, val.Length, buffer, index);
            return count;
        }
    }
}
