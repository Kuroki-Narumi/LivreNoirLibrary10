using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media
{
    public class DrawingSizeJsonConverter : JsonConverter<Size>
    {
        private static readonly SizeConverter _converter = new();

        public override Size Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartArray:
                    if (JsonSerializer.Deserialize<double[]>(ref reader, options) is { } array && array.Length is >= 2)
                    {
                        return new((int)array[0], (int)array[1]);
                    }
                    break;
                case JsonTokenType.String:
                    if (reader.GetString() is { } text && _converter.ConvertFromString(text) is Size value)
                    {
                        return value;
                    }
                    break;
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Size value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Width);
            writer.WriteNumberValue(value.Height);
            writer.WriteEndArray();
        }
    }
}
