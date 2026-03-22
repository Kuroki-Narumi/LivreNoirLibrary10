using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class BarPositionJsonConverter : JsonConverter<BarPosition>
    {
        public override BarPosition Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                var text = reader.GetString()!;
                if (BarPosition.TryParse(text, out var result))
                {
                    return result;
                }
            }
            else if (reader.TokenType is JsonTokenType.Number)
            {
                return new(reader.GetDouble());
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, BarPosition value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
