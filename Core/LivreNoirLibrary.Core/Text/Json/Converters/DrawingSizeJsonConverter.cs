using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Text
{
    public class DrawingSizeJsonConverter : JsonConverter<Size>
    {
        private static readonly SizeConverter _converter = new();

        public override Size Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Read(ref reader, options);
        public override void Write(Utf8JsonWriter writer, Size value, JsonSerializerOptions options) => Write(writer, value);

        public static Size Parse(string? text)
        {
            if (text is not null && _converter.ConvertFromString(text) is Size value)
            {
                return value;
            }
            throw new FormatException("The string is not in a correct format.");
        }

        public static Size Read(ref Utf8JsonReader reader, JsonSerializerOptions options)
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
                        int width = 0, height = 0;
                        foreach (var property in document.RootElement.EnumerateObject())
                        {
                            switch (property.Name.ToLower())
                            {
                                case "w" or "width":
                                    width = property.Value.GetInt32();
                                    break;
                                case "h" or "height":
                                    height = property.Value.GetInt32();
                                    break;
                            }
                        }
                        return new(width, height);
                    }
            }
            throw new JsonException();
        }

        public static void Write(Utf8JsonWriter writer, Size value)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.Width);
            writer.WriteNumberValue(value.Height);
            writer.WriteEndArray();
        }
    }
}
