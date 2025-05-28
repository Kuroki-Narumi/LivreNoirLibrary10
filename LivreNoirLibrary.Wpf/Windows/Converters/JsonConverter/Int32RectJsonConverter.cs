using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public class Int32RectJsonConverter : JsonConverter<Int32Rect>
    {
        public override Int32Rect Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonSerializer.Deserialize<int[]>(ref reader, options) is int[] ary && ary.Length is >= 4)
            {
                return new(ary[0], ary[1], ary[2], ary[3]);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Int32Rect value, JsonSerializerOptions options)
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
