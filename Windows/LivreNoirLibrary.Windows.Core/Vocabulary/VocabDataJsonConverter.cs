using LivreNoirLibrary.Text;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Windows
{
    public sealed class VocabDataJsonConverter : JsonConverter<VocabData>
    {
        public const string PropertyName_Value = "v";
        public const string PropertyName_KeyTip = "k";

        public override VocabData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    return new() { Value = reader.GetString()! };
                case JsonTokenType.StartObject:
                    var currentProp = "";
                    var depth = 0;
                    string? value = null, key = null;
                    while (reader.Read())
                    {
                        if (depth is 0)
                        {
                            switch (reader.TokenType)
                            {
                                case JsonTokenType.EndObject:
                                    goto LoopEnd;
                                case JsonTokenType.StartArray:
                                case JsonTokenType.StartObject:
                                    depth++;
                                    continue;
                                case JsonTokenType.PropertyName:
                                    currentProp = reader.GetString();
                                    continue;
                                case JsonTokenType.String:
                                    switch (currentProp)
                                    {
                                        case PropertyName_Value:
                                            value = reader.GetString();
                                            break;
                                        case PropertyName_KeyTip:
                                            key = reader.GetString();
                                            break;
                                    }
                                    break;
                            }
                            currentProp = "";
                        }
                        else if (reader.TokenType is JsonTokenType.EndArray or JsonTokenType.EndObject)
                        {
                            depth--;
                        }
                    }
                LoopEnd:
                    VocabData data = new()
                    {
                        Value = value,
                        KeyTip = key,
                    };
                    return data;
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, VocabData value, JsonSerializerOptions options) => WriteStatic(value, writer, options);

        public static void WriteStatic(VocabData data, Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            var value = data.Value;
            var keyTip = data.KeyTip;
            writer.WriteStartObject();
            writer.WriteStringIfNotNull(PropertyName_Value, value);
            writer.WriteStringIfNotNull(PropertyName_KeyTip, keyTip);
            writer.WriteEndObject();
        }
    }
}
