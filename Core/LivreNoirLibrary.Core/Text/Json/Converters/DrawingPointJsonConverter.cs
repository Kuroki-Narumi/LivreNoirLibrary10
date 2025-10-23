using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Text
{
    public class DrawingPointJsonConverter : JsonConverter<Point>
    {
        private static readonly PointConverter _converter = new();

        public override Point Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Read(ref reader, options);
        public override void Write(Utf8JsonWriter writer, Point value, JsonSerializerOptions options) => Write(writer, value);

        public static Point Parse(string? text)
        {
            if (text is not null && _converter.ConvertFromString(text) is Point value)
            {
                return value;
            }
            throw new FormatException("The string is not in a correct format.");
        }

        public static Point Read(ref Utf8JsonReader reader, JsonSerializerOptions options)
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
                    return Parse(reader.GetString());
                case JsonTokenType.StartObject:
                    using (var document = JsonDocument.ParseValue(ref reader))
                    {
                        int x = 0, y = 0;
                        foreach (var property in document.RootElement.EnumerateObject())
                        {
                            switch (property.Name.ToLower())
                            {
                                case "x":
                                    x = property.Value.GetInt32();
                                    break;
                                case "y":
                                    y = property.Value.GetInt32();
                                    break;
                            }
                        }
                        return new(x, y);
                    }
            }
            throw new JsonException();
        }

        public static void Write(Utf8JsonWriter writer, Point value)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteEndArray();
        }
    }
}
