using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media
{
    public class ClipRectJsonConverter : JsonConverter<ClipRect>
    {
        public override ClipRect? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                return ClipRectTypeConverter.ConvertFromStatic(reader.GetString()!);
            }
            else if (reader.TokenType is JsonTokenType.StartArray)
            {
                if (JsonSerializer.Deserialize<double[]>(ref reader, options) is { } array && array.Length is >= 4)
                {
                    return new(array[0], array[1], array[2], array[3]);
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, ClipRect value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
