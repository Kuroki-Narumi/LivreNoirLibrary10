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
        public static void WriteJson<T>(this T j, Stream utf8json, bool pretty = true)
            where T : IJsonWriter
        {
            using Utf8JsonWriter writer = new(utf8json, Json.GetWriterOption(pretty));
            j.WriteJson(writer, Json.GetOptions(pretty));
        }

        public static void SaveJson(this IJsonWriter j, string path, bool pretty = true)
        {
            using var file = General.CreateSafe(path);
            WriteJson(j, file, pretty);
        }

        public static MemoryStream GetJsonAsStream(this IJsonWriter j, bool pretty = false)
        {
            MemoryStream ms = new();
            WriteJson(j, ms, pretty);
            ms.Position = 0;
            return ms;
        }

        public static byte[] GetJsonAsBuffer(this IJsonWriter j, bool pretty = false)
        {
            return GetJsonAsStream(j, pretty).GetBuffer();
        }

        public static string GetJsonAsString(this IJsonWriter j, bool pretty = true)
        {
            var buffer = GetJsonAsBuffer(j, pretty);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
