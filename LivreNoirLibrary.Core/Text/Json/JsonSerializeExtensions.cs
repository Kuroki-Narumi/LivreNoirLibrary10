using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LivreNoirLibrary.Text
{
    public static class JsonSerializeExtensions
    {
        public static void WriteIfNotNull(this Utf8JsonWriter writer, string propertyName, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                writer.WriteString(propertyName, value);
            }
        }

        public static void WriteIfTrue(this Utf8JsonWriter writer, string propertyName, bool value = true)
        {
            if (value)
            {
                writer.WriteBoolean(propertyName, true);
            }
        }

        public static void WriteObjectIfNotNull<T>(this Utf8JsonWriter writer, string propertyName, T? value, JsonSerializerOptions? options)
            where T : class
        {
            if (value is not null)
            {
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, value, options);
            }
        }

        public static void WriteObjectIfNotNull<T>(this Utf8JsonWriter writer, string propertyName, T? value, Action<Utf8JsonWriter, T> writeAction)
            where T : class
        {
            if (value is not null)
            {
                writer.WritePropertyName(propertyName);
                writeAction(writer, value);
            }
        }

        public static void WriteArrayIfNotNull<T>(this Utf8JsonWriter writer, string propertyName, ICollection<T>? value, JsonSerializerOptions? options)
        {
            if (value is not null && value.Count is > 0)
            {
                writer.WriteStartArray(propertyName);
                foreach (var item in value)
                {
                    JsonSerializer.Serialize(writer, item, options);
                }
                writer.WriteEndArray();
            }
        }

        public static void WriteArrayIfNotNull<T>(this Utf8JsonWriter writer, string propertyName, ICollection<T>? value, Action<Utf8JsonWriter, T> writeAction)
        {
            if (value is not null && value.Count is > 0)
            {
                writer.WriteStartArray(propertyName);
                foreach (var item in value)
                {
                    writeAction(writer, item);
                }
                writer.WriteEndArray();
            }
        }
    }
}
