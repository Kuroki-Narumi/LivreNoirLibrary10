using System;
using System.Collections.Generic;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface IIdEnumerable
    {
        public IEnumerable<int> EnumerateIds();
    }

    public static partial class Extensions
    {
        public static void WriteJson<T>(this T list, Utf8JsonWriter writer, JsonSerializerOptions options)
            where T : IIdEnumerable
        {
            writer.WriteStartArray();
            foreach (var id in list.EnumerateIds())
            {
                writer.WriteNumberValue(id);
            }
            writer.WriteEndArray();
        }
    }
}
