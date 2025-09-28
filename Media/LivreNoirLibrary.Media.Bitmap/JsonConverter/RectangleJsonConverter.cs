using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media
{
    public class RectangleJsonConverter : JsonConverter<Rectangle>
    {
        private static readonly RectangleConverter _converter = new();

        public override Rectangle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartArray:
                    if (JsonSerializer.Deserialize<double[]>(ref reader, options) is { } array && array.Length is >= 4)
                    {
                        return new((int)array[0], (int)array[1], (int)array[2], (int)array[3]);
                    }
                    break;
                case JsonTokenType.String:
                    if (reader.GetString() is { } text && _converter.ConvertFromString(text) is Rectangle value)
                    {
                        return value;
                    }
                    break;
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Rectangle value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteNumberValue(value.Width);
            writer.WriteNumberValue(value.Height);
            writer.WriteEndArray();
        }
    }
}
