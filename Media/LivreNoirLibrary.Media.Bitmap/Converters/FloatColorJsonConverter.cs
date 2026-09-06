using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Text
{
    public sealed class FloatColorJsonConverter : JsonConverter<FloatColor>
    {
        public override FloatColor Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                return FloatColorTypeConverter.ConvertFromStatic(reader.GetString()!);
            }
            else if (reader.TokenType is JsonTokenType.StartArray)
            {
                if (JsonSerializer.Deserialize<float[]>(ref reader, options) is { } array && array.Length is >= 3)
                {
                    if (array.Length is 3)
                    {
                        return new(1, array[0], array[1], array[2]);
                    }
                    else
                    {
                        return new(array[0], array[1], array[2], array[3]);
                    }
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, FloatColor value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
