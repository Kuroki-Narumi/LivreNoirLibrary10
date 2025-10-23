using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Text
{
    public sealed class LnColorJsonConverter : JsonConverter<LnColor>
    {
        public override LnColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                return LnColorTypeConverter.ConvertFromStatic(reader.GetString()!);
            }
            else if (reader.TokenType is JsonTokenType.StartArray)
            {
                if (JsonSerializer.Deserialize<double[]>(ref reader, options) is { } array && array.Length is >= 3)
                {
                    if (array.Length is 3)
                    {
                        return new((byte)array[0], (byte)array[1], (byte)array[2]);
                    }
                    else
                    {
                        return new((byte)array[0], (byte)array[1], (byte)array[2], (byte)array[3]);
                    }
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, LnColor value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
