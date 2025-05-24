using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bms
{
    public class KeyTypeJsonConverter : JsonConverter<KeyType>
    {
        public override KeyType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                var str = reader.GetString();
                return KeyType.Parse(str);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, KeyType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
