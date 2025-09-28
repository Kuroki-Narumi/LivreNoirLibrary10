using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Media
{
    public class DrawingPointJsonConverter : JsonConverter<Point>
    {
        private static readonly PointConverter _converter = new();

        public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
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
                    if (reader.GetString() is { } text && _converter.ConvertFromString(text) is Point value)
                    {
                        return value;
                    }
                    break;
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteEndArray();
        }
    }
}
