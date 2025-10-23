using System;
using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.Text
{
    public class DrawingPointFJsonConverter : JsonConverter<PointF>
    {
        public override PointF Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => Read(ref reader, options);
        public override void Write(Utf8JsonWriter writer, PointF value, JsonSerializerOptions options) => Write(writer, value);

        public static PointF Parse(string? text)
        {
            if (text is not null && TupleStringConverter.TryConvertFromString<float, float>(text, out var value))
            {
                return new(value.Item1, value.Item2);
            }
            throw new FormatException("The input string was not in a correct format.");
        }

        public static PointF Read(ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.StartArray:
                    if (JsonSerializer.Deserialize<float[]>(ref reader, options) is { } array && array.Length is >= 2)
                    {
                        return new(array[0], array[1]);
                    }
                    break;
                case JsonTokenType.String:
                    return Parse(reader.GetString());
                case JsonTokenType.StartObject:
                    using (var document = JsonDocument.ParseValue(ref reader))
                    {
                        float x = 0, y = 0;
                        foreach (var property in document.RootElement.EnumerateObject())
                        {
                            switch (property.Name.ToLower())
                            {
                                case "x":
                                    x = property.Value.GetSingle();
                                    break;
                                case "y":
                                    y = property.Value.GetSingle();
                                    break;
                            }
                        }
                        return new(x, y);
                    }
            }
            throw new JsonException();
        }

        public static void Write(Utf8JsonWriter writer, PointF value)
        {
            writer.WriteStartArray();
            writer.WriteNumberValue(value.X);
            writer.WriteNumberValue(value.Y);
            writer.WriteEndArray();
        }
    }
}