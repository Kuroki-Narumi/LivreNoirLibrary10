using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public class AttributeJsonConverter : JsonConverter<Attribute>
    {
        public override Attribute Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    var str = reader.GetString();
                    if (Enum.TryParse<Attribute>(str, out var value))
                    {
                        return value;
                    }
                    return Vocab.GetAttribute(str);
                case JsonTokenType.Number:
                    return (Attribute)reader.GetInt32();
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Attribute value, JsonSerializerOptions options) => writer.WriteStringValue(Vocab.GetName(value));
    }
}
