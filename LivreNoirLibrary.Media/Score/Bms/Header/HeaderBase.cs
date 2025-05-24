using System;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public abstract class HeaderBase : IComparable<HeaderBase>, IJsonWriter
    {
        public HeaderType Type { get; }
        public string Key { get; protected set; }
        public string Value { get; set; }

        protected HeaderBase(HeaderType type, string value)
        {
            Type = type;
            Key = type.ToString().ToUpper();
            Value = value;
        }

        protected HeaderBase(string type, string value)
        {
            Type = HeaderType.Other;
            Key = type;
            Value = value;
        }

        public void AddValue(string value)
        {
            if (string.IsNullOrEmpty(Value))
            {
                Value = value;
            }
            else
            {
                Value = $"{Value}, {value}";
            }
        }

        internal void Dump(RawData.BmsTextWriter writer)
        {
            if (!string.IsNullOrEmpty(Value))
            {
                writer.Dump($"#{Key} {Value}");
            }
        }

        public void Dump(BinaryWriter stream)
        {
            stream.Write(Key);
            stream.Write(Value);
        }

        public static HeaderBase Create(string key, string value)
        {
            if (TryGetType(key, out var type))
            {
                return new TypedHeader(type, value);
            }
            else
            {
                return new Header(key, value);
            }
        }

        public static (string Key, string Value) LoadKV(BinaryReader reader)
        {
            var key = reader.ReadString();
            var value = reader.ReadString();
            return (key, value);
        }

        public int CompareTo(HeaderBase? other) => other is not null ? Type.CompareTo(other.Type) : -1;
        public int CompareTo(object? other) => other is HeaderBase h ? Type.CompareTo(h.Type) : -1;

        public static bool TryGetType(string typeText, out HeaderType type)
        {
            if (Enum.TryParse(typeText, true, out type))
            {
                return true;
            }
            type = HeaderType.Other;
            return false;
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("key", Key);
            writer.WriteString("value", Value);
            writer.WriteEndObject();
        }
    }
}
