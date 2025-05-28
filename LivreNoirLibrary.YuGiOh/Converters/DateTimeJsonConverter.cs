using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Converters
{
    public abstract class DateTimeJsonConverterBase : JsonConverter<DateTime>
    {
        public abstract string Format { get; }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType is JsonTokenType.String && reader.GetString() is string str)
            {
                return DateTime.Parse(str);
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) => Write(writer, value, Format);
        public static void Write(Utf8JsonWriter writer, DateTime value, string format) => writer.WriteStringValue(value.ToString(format));
    }

    public sealed class DateOnlyJsonConverter : DateTimeJsonConverterBase
    {
        const string _format = "yyyy-MM-dd";
        public override string Format => _format;
        public static void Write(Utf8JsonWriter writer, DateTime value) => Write(writer, value, _format);
    }

    public class DateTimeJsonConverter : DateTimeJsonConverterBase
    {
        const string _format = "yyyy-MM-dd HH:mm:ss";
        public override string Format => _format;
        public static void Write(Utf8JsonWriter writer, DateTime value) => Write(writer, value, _format);
    }

    public class NoSecondsDateJsonConverter : DateTimeJsonConverterBase
    {
        const string _format = "yyyy-MM-dd HH:mm";
        public override string Format => _format;
        public static void Write(Utf8JsonWriter writer, DateTime value) => Write(writer, value, _format);
    }
}
