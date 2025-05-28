using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Media;
using LivreNoirLibrary.Media;

namespace LivreNoirLibrary.Windows.Converters
{
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String)
            {
                if (reader.GetString() is string code && ColorExtension.TryParseToColor(code, out var value))
                {
                    return value;
                }
            }
            else if (reader.TokenType is JsonTokenType.StartArray)
            {
                if (JsonSerializer.Deserialize<int[]>(ref reader, options) is int[] array && array.Length is >= 3)
                {
                    if (array.Length is 3)
                    {
                        return Color.FromRgb((byte)array[0], (byte)array[1], (byte)array[2]);
                    }
                    else
                    {
                        return Color.FromArgb((byte)array[0], (byte)array[1], (byte)array[2], (byte)array[3]);
                    }
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(ColorExtension.GetColorCode(value));
        }
    }
}
