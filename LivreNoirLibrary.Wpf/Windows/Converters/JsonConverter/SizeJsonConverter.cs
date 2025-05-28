using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public class SizeJsonConverter : JsonConverter<Size>
    {
        public override Size Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (JsonSerializer.Deserialize<double[]>(ref reader, options) is double[] ary && ary.Length is >= 2)
            {
                return new(ary[0], ary[1]);
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
