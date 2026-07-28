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
}
