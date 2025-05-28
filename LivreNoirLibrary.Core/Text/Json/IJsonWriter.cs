using System;
using System.IO;
using System.Text;
using System.Text.Json;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Text
{
    public interface IJsonWriter
    {
        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options);
    }

    public static class IJsonWriterExtension
    {
        public static void WriteJson<T>(this T obj, Stream utf8json, bool pretty = true)
            where T : IJsonWriter
        {
            using Utf8JsonWriter writer = new(utf8json, Json.GetWriterOption(pretty));
            obj.WriteJson(writer, Json.GetOptions(pretty));
        }

        public static void SaveJson<T>(this T obj, string path, bool pretty = true)
            where T : IJsonWriter
        {
            using var file = General.CreateSafe(path);
            WriteJson(obj, file, pretty);
        }

        public static MemoryStream GetJsonAsStream<T>(this T obj, bool pretty = false)
            where T : IJsonWriter
        {
            MemoryStream ms = new();
            WriteJson(obj, ms, pretty);
            ms.Position = 0;
            return ms;
        }

        public static byte[] GetJsonAsBuffer<T>(this T obj, bool pretty = false)
            where T : IJsonWriter
        {
            return GetJsonAsStream(obj, pretty).GetBuffer();
        }

        public static string GetJsonAsString<T>(this T obj, bool pretty = true)
            where T : IJsonWriter
        {
            var buffer = GetJsonAsBuffer(obj, pretty);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
