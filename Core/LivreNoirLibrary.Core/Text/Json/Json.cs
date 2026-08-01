using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Threading;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Text
{
    public static partial class Json
    {
        private static readonly JsonSerializerOptions _readOptions;
        private static readonly JsonSerializerOptions _readOptions_ignoreCase;
        private static readonly JsonSerializerOptions _writeOptions;
        private static readonly JsonSerializerOptions _prettyWriteOptions;

        static Json()
        {
            _readOptions = new()
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            };
            var converters = _readOptions.Converters;
            converters.Add(new DrawingPointFJsonConverter());
            converters.Add(new DrawingPointJsonConverter());
            converters.Add(new DrawingSizeJsonConverter());
            converters.Add(new RectangleJsonConverter());

            _readOptions_ignoreCase = new(_readOptions)
            {
                PropertyNameCaseInsensitive = true,
            };

            _writeOptions = new(_readOptions)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            _prettyWriteOptions = new(_writeOptions)
            {
                WriteIndented = true
            };
        }

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

        private static JsonSerializerOptions GetReadOptions(bool ignoreCase) => ignoreCase ? _readOptions_ignoreCase : _readOptions;

        public static T Parse<T>(string json, bool ignorePropertyCase = false)
            where T : class
        {
            var obj = JsonSerializer.Deserialize<T>(json, GetReadOptions(ignorePropertyCase));
            return GetValueOrThrow(obj);
        }

        public static T Parse<T>(ReadOnlySpan<byte> utf8json, bool ignorePropertyCase = false)
            where T : class
        {
            var obj = JsonSerializer.Deserialize<T>(utf8json, GetReadOptions(ignorePropertyCase));
            return GetValueOrThrow(obj);
        }

        public static T Open<T>(string path) where T : class => Open<T>(path, false);

        public static T Open<T>(string path, bool ignorePropertyCase)
            where T : class
        {
            using var file = File.OpenRead(path);
            return Load<T>(file, ignorePropertyCase);
        }

        public static T Load<T>(Stream utf8Json) where T : class => Load<T>(utf8Json, false);

        public static T Load<T>(Stream utf8json, bool ignorePropertyCase)
            where T : class
        {
            var obj = JsonSerializer.Deserialize<T>(utf8json, GetReadOptions(ignorePropertyCase));
            return GetValueOrThrow(obj);
        }

        public static bool TryParse<T>(string? json, [MaybeNullWhen(false)] out T obj, bool ignorePropertyCase = false)
            where T : class
        {
            if (string.IsNullOrEmpty(json))
            {
                obj = null;
                return false;
            }
            try
            {
                obj = Parse<T>(json, ignorePropertyCase);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                obj = null;
                return false;
            }
        }

        public static bool TryParse<T>(ReadOnlySpan<byte> utf8json, [MaybeNullWhen(false)] out T obj, bool ignorePropertyCase = false)
            where T : class
        {
            try
            {
                obj = Parse<T>(utf8json, ignorePropertyCase);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                obj = null;
                return false;
            }
        }

        public static bool TryOpen<T>(string path, [MaybeNullWhen(false)]out T result, bool ignorePropertyCase = false)
            where T : class
        {
            try
            {
                if (File.Exists(path))
                {
                    result = Open<T>(path, ignorePropertyCase);
                    return true;
                }
            }
            catch(Exception e)
            {
                ExConsole.Write(e);
            }
            result = null;
            return false;
        }

        public static bool TryLoad<T>(Stream utf8json, [MaybeNullWhen(false)] out T result, bool ignorePropertyCase = false)
            where T : class
        {
            try
            {
                result = Load<T>(utf8json, ignorePropertyCase);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }

        public static void Save(string path, object? obj, bool pretty = true)
        {
            if (obj is null)
            {
                return;
            }
            using var file = General.CreateSafe(path);
            Dump(file, obj, pretty);
        }

        public static void Save(string path, IWriteJson? w, bool pretty = true)
        {
            if (w is null)
            {
                return;
            }
            using var file = General.CreateSafe(path);
            Dump(file, w, pretty);
        }

        public static void Dump(Stream utf8json, object obj, bool pretty = true)
        {
            using Utf8JsonWriter writer = new(utf8json, GetStructWriteOptions(pretty));
            JsonSerializer.Serialize(writer, obj, GetWriteOptions(pretty));
        }

        public static void Dump(Stream utf8json, IWriteJson obj, bool pretty = true)
        {
            using Utf8JsonWriter writer = new(utf8json, GetStructWriteOptions(pretty));
            obj.WriteJson(writer, GetWriteOptions(pretty));
        }

        public static string GetJsonText<T>(this T obj, bool pretty = false)
        {
            if (obj is IWriteJson w)
            {
                return Encoding.UTF8.GetString(GetJsonBytes(w, pretty));
            }
            else
            {
                return JsonSerializer.Serialize(obj, GetWriteOptions(pretty));
            }
        }

        public static byte[] GetJsonBytes<T>(this T obj, bool pretty = false)
        {
            if (obj is IWriteJson w)
            {
                var ms = _msBuffer.Value!;
                try
                {
                    Dump(ms, w, pretty);
                    return ms.ToArray();
                }
                finally
                {
                    ms.SetLength(0);
                }
            }
            else
            {
                return JsonSerializer.SerializeToUtf8Bytes(obj, pretty ? _prettyWriteOptions : _writeOptions);
            }
        }

        private static readonly ThreadLocal<MemoryStream> _msBuffer = new(() => new());

        public static JsonSerializerOptions GetWriteOptions(bool pretty) => pretty ? _prettyWriteOptions : _writeOptions;
        public static JsonWriterOptions GetStructWriteOptions(bool pretty)
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

        private static readonly Lock _equals_lock = new();
        private static readonly MemoryStream _equals_left_ms = new(32768);
        private static readonly MemoryStream _equals_right_ms = new(32768);

        public static bool Equals<T>(T left, T right)
        {
            var op = GetWriteOptions(false);
            lock (_equals_lock)
            {
                var l = _equals_left_ms;
                var r = _equals_right_ms;
                l.SetLength(0);
                r.SetLength(0);
                JsonSerializer.Serialize(l, left, op);
                JsonSerializer.Serialize(r, right, op);
                return l.GetBuffer().AsSpan(0, (int)l.Length).EqualsAll(r.GetBuffer().AsSpan(0, (int)r.Length));
            }
        }
    }
}
