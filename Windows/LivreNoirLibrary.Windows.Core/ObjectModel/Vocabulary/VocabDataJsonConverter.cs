using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Windows
{
    public sealed class VocabDataJsonConverter : JsonConverter<VocabData>
    {
        public const string PropertyName_Header = "header";
        public const string PropertyName_Description = "desc";
        public const string PropertyName_KeyTip = "key";

        public override VocabData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                return new() { Header = reader.GetString()! };
            }
            else if (reader.TokenType is JsonTokenType.StartObject)
            {
                string? header = null, desc = null, key = null;
                using (var doc = JsonDocument.ParseValue(ref reader))
                {
                    foreach (var prop in doc.RootElement.EnumerateObject())
                    {
                        switch (prop.Name)
                        {
                            case PropertyName_Header:
                                header = prop.Value.GetString();
                                break;
                            case PropertyName_Description:
                                desc = prop.Value.GetString();
                                break;
                            case PropertyName_KeyTip:
                                key = prop.Value.GetString();
                                break;
                        }
                    }
                }
                VocabData data = new()
                {
                    Header = header ?? "",
                    Description = desc,
                    KeyTip = key,
                };
                return data;
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, VocabData value, JsonSerializerOptions options) => WriteStatic(value, writer, options);

        public static void WriteStatic(VocabData value, Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            var header = value.Header;
            var desc = value.Description;
            var keyTip = value.KeyTip;
            var empty_desc = string.IsNullOrEmpty(desc);
            var empty_key = string.IsNullOrEmpty(keyTip);
            if (empty_desc && empty_key)
            {
                writer.WriteStringValue(header);
            }
            else
            {
                writer.WriteStartObject();
                var empty_header = string.IsNullOrEmpty(header);
                if (!empty_header)
                {
                    writer.WriteString(PropertyName_Header, header);
                }
                if (!empty_desc)
                {
                    writer.WriteString(PropertyName_Description, desc);
                }
                if (!empty_key)
                {
                    writer.WriteString(PropertyName_KeyTip, keyTip);
                }
                writer.WriteEndObject();
            }
        }
    }
}
