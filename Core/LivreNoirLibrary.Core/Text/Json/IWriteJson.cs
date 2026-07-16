using System;
using System.IO;
using System.Text;
using System.Text.Json;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Text
{
    public interface IWriteJson
    {
        void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options);
    }

    public static class IWriteJsonExtension
    {
        public static void WriteJson<T>(this T obj, Stream utf8json, bool pretty = true)
            where T : IWriteJson
        {
            using Utf8JsonWriter writer = new(utf8json, Json.GetStructWriteOptions(pretty));
            obj.WriteJson(writer, Json.GetWriteOptions(pretty));
        }

        public static void SaveJson<T>(this T obj, string path, bool pretty = true)
            where T : IWriteJson
        {
            using var file = General.CreateSafe(path);
            WriteJson(obj, file, pretty);
        }

        public static MemoryStream GetJsonAsStream<T>(this T obj, bool pretty = false)
            where T : IWriteJson
        {
            MemoryStream ms = new();
            WriteJson(obj, ms, pretty);
            ms.Position = 0;
            return ms;
        }

        public static byte[] GetJsonAsBuffer<T>(this T obj, bool pretty = false)
            where T : IWriteJson
        {
            return GetJsonAsStream(obj, pretty).GetBuffer();
        }

        public static string GetJsonAsString<T>(this T obj, bool pretty = true)
            where T : IWriteJson
        {
            var buffer = GetJsonAsBuffer(obj, pretty);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}
