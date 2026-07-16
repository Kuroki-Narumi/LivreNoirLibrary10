using System;
using System.Collections.Generic;
using System.Text.Json;

namespace LivreNoirLibrary.Text
{
    public static class JsonSerializeExtensions
    {
        public static void WriteNumberIfNotZero(this Utf8JsonWriter writer, string propertyName, long value)
        {
            if (value is not 0)
            {
                writer.WriteNumber(propertyName, value);
            }
        }

        public static void WriteNumberIfNotZero(this Utf8JsonWriter writer, string propertyName, ulong value)
        {
            if (value is not 0)
            {
                writer.WriteNumber(propertyName, value);
            }
        }

        public static void WriteNumberIfNotZero(this Utf8JsonWriter writer, string propertyName, float value)
        {
            if (value is not 0)
            {
                writer.WriteNumber(propertyName, value);
            }
        }

        public static void WriteNumberIfNotZero(this Utf8JsonWriter writer, string propertyName, double value)
        {
            if (value is not 0)
            {
                writer.WriteNumber(propertyName, value);
            }
        }

        public static void WriteNumberIfNotZero(this Utf8JsonWriter writer, string propertyName, decimal value)
        {
            if (value is not 0)
            {
                writer.WriteNumber(propertyName, value);
            }
        }

        public static void WriteStringIfNotNull(this Utf8JsonWriter writer, string propertyName, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                writer.WriteString(propertyName, value);
            }
        }

        public static void WriteBooleanIfTrue(this Utf8JsonWriter writer, string propertyName, bool value = true)
        {
            if (value)
            {
                writer.WriteBoolean(propertyName, true);
            }
        }

        public static void WriteIf<T>(this Utf8JsonWriter writer, string propertyName, T value, bool condition, JsonSerializerOptions? options)
        {
            if (condition)
            {
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, value, options);
            }
        }

        public static void WriteIfNot<T>(this Utf8JsonWriter writer, string propertyName, T? value, T defaultValue, JsonSerializerOptions? options)
        {
            if (!EqualityComparer<T>.Default.Equals(value, defaultValue))
            {
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, value, options);
            }
        }

        public static void WriteIfNotDefault<T>(this Utf8JsonWriter writer, string propertyName, T? value, JsonSerializerOptions? options)
        {
            if (!EqualityComparer<T>.Default.Equals(value, default))
            {
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, value, options);
            }
        }

        public static void WriteIfNotDefault<T>(this Utf8JsonWriter writer, string propertyName, T? value, Action<T> writeAction)
        {
            if (!EqualityComparer<T>.Default.Equals(value, default))
            {
                writer.WritePropertyName(propertyName);
                writeAction(value!);
            }
        }

        public static void WriteIfNotNull<T>(this Utf8JsonWriter writer, string propertyName, T? value, JsonSerializerOptions? options)
            where T : class
        {
            if (value is not null)
            {
                writer.WritePropertyName(propertyName);
                JsonSerializer.Serialize(writer, value, options);
            }
        }

        public static void WriteIfNotNull<T>(this Utf8JsonWriter writer, string propertyName, T? value, Action<Utf8JsonWriter, T> writeAction)
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
