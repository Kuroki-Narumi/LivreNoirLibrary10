using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Windows
{
    public class VocabDataJsonConverter : JsonConverter<VocabData>
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
                string header = "", desc = "", key = "";
                ref string target = ref header;
                reader.Read();
                while (reader.TokenType is not JsonTokenType.EndObject)
                {
                    if (reader.TokenType is JsonTokenType.PropertyName)
                    {
                        switch (reader.GetString())
                        {
                            case PropertyName_Header:
                                target = ref header;
                                break;
                            case PropertyName_Description:
                                target = ref desc;
                                break;
                            case PropertyName_KeyTip:
                                target = ref key;
                                break;
                        }
                    }
                    else if (reader.TokenType is JsonTokenType.String)
                    {
                        if (reader.GetString() is string value)
                        {
                            target = value;
                        }
                    }
                    reader.Read();
                }
                VocabData data = new() { Header = header };
                if (!string.IsNullOrEmpty(desc))
                {
                    data.Description = desc;
                }
                if (!string.IsNullOrEmpty(key))
                {
                    data.KeyTip = key;
                }
                return data;
            }
            else
            {
                throw new JsonException();
            }
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
