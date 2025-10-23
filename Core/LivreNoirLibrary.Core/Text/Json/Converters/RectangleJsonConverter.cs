using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Text
{
    public class RectangleJsonConverter : JsonConverter<Rectangle>
    {
        private static readonly RectangleConverter _converter = new();

        public override Rectangle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Read(ref reader, options);
        public override void Write(Utf8JsonWriter writer, Rectangle value, JsonSerializerOptions options) => Write(writer, value);

        public static Rectangle Parse(string? text)
        {
            if (text is not null && _converter.ConvertFromString(text) is Rectangle value)
            {
                return value;
            }
            throw new FormatException("The string is not in a correct format.");
        }

        public static Rectangle Read(ref Utf8JsonReader reader, JsonSerializerOptions options)
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
                    return Parse(reader.GetString());
                case JsonTokenType.StartObject:
                    using (var document = JsonDocument.ParseValue(ref reader))
                    {
                        int x = 0, y = 0, width = 0, height = 0;
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
                                case "w" or "width":
                                    width = property.Value.GetInt32();
                                    break;
                                case "h" or "height":
                                    height = property.Value.GetInt32();
                                    break;
                            }
                        }
                        return new(x, y, width, height);
                    }

            }
            throw new JsonException();
        }

        public static void Write(Utf8JsonWriter writer, Rectangle value)
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
