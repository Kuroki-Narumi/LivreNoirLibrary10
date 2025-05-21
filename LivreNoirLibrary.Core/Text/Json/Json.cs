using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Text
{
    public static partial class Json
    {
        private static T GetValueOrThrow<T>(T? value)
        {
            if (value is not null)
            {
                return value;
            }
            else
            {
                throw new InvalidDataException();
            }
        }

        public static T Parse<T>(string json)
            where T : class
        {
            var obj = JsonSerializer.Deserialize<T>(json, GetOptions(true));
            return GetValueOrThrow(obj);
        }

        public static T Parse<T>(ReadOnlySpan<byte> utf8json)
            where T : class
        {
            var obj = JsonSerializer.Deserialize<T>(utf8json, GetOptions(true));
            return GetValueOrThrow(obj);
        }

        public static T Open<T>(string path)
            where T : class
        {
            using var file = File.OpenRead(path);
            return Load<T>(file);
        }

        public static T Load<T>(Stream utf8json)
            where T : class
        {
            var obj = JsonSerializer.Deserialize<T>(utf8json, GetOptions(true));
            return GetValueOrThrow(obj);
        }

        public static bool TryParse<T>(string? json, [MaybeNullWhen(false)] out T obj)
            where T : class
        {
            if (string.IsNullOrEmpty(json))
            {
                obj = null;
                return false;
            }
            try
            {
                obj = Parse<T>(json);
                return true;
            }
            catch
            {
                obj = null;
                return false;
            }
        }

        public static bool TryParse<T>(ReadOnlySpan<byte> utf8json, [MaybeNullWhen(false)] out T obj)
            where T : class
        {
            try
            {
                obj = Parse<T>(utf8json);
                return true;
            }
            catch
            {
                obj = null;
                return false;
            }
        }

        public static bool TryOpen<T>(string path, [MaybeNullWhen(false)]out T result)
            where T : class
        {
            try
            {
                if (File.Exists(path))
                {
                    result = Open<T>(path);
                    return true;
                }
            }
            catch { }
            result = null;
            return false;
        }

        public static bool TryLoad<T>(Stream utf8json, [MaybeNullWhen(false)] out T result)
            where T : class
        {
            try
            {
                result = Load<T>(utf8json);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static void Save(string path, object obj, bool pretty = true)
        {
            using var file = General.CreateSafe(path);
            Dump(file, obj, pretty);
        }

        public static void Dump(Stream utf8json, object obj, bool pretty = true)
        {
            var options = GetOptions(pretty);
            using Utf8JsonWriter writer = new(utf8json, GetWriterOption(pretty));
            JsonSerializer.Serialize(writer, obj, options);
        }

        public static string GetJsonText<T>(this T obj, bool pretty = false)
        {
            return JsonSerializer.Serialize(obj, GetOptions(pretty));
        }

        public static JsonSerializerOptions GetOptions(bool pretty) => new()
        {
            WriteIndented = pretty,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            ReadCommentHandling = JsonCommentHandling.Skip,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        };

        public static JsonWriterOptions GetWriterOption(bool pretty)
        {
            if (pretty)
            {
                return new()
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    Indented = true,
                };
            }
            else
            {
                return default;
            }
        }

        public static T Clone<T>(T source)
            where T : class
        {
            using MemoryStream ms = new();
            Dump(ms, source, false);
            ms.Position = 0;
            return Load<T>(ms);
        }

        public static bool Equals<T>(T left, T right)
        {
            var op = GetOptions(false);
            var l = JsonSerializer.SerializeToUtf8Bytes(left, op);
            var r = JsonSerializer.SerializeToUtf8Bytes(right, op);
            return new ReadOnlySpan<byte>(l).SequenceEqual(r);
        }
    }
}
