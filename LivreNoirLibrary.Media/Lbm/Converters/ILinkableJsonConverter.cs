using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.Media.Lbm
{
    public sealed class ILinkableJsonConverter<TSelf> : JsonConverter<TSelf>
        where TSelf : ILinkable<TSelf>
    {
        public override TSelf Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    if (reader.GetString() is string uri)
                    {
                        return TSelf.Open(uri);
                    }
                    break;
                case JsonTokenType.StartObject:
                    if (TSelf.TryParse(ref reader, options, out var value))
                    {
                        return value;
                    }
                    break;
            }
            throw new JsonException($"Expected a string token for {typeToConvert.Name}, but got {reader.TokenType}.");
        }

        public override void Write(Utf8JsonWriter writer, TSelf value, JsonSerializerOptions options)
        {
            if (value is not null)
            {
                value.WriteJson(writer, options);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }

    public sealed class ILinkableJsonConverterFactory : JsonConverterFactory
    {
        private bool IsILinkable(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(ILinkable<>);

        public override bool CanConvert(Type typeToConvert) => typeToConvert.GetInterfaces().Any(IsILinkable);

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            // IJsonString<TSelf>のTSelfを解決
            var selfType = typeToConvert.GetInterfaces().FirstOrDefault(IsILinkable)?.GetGenericArguments()[0];
            // 型が一致しない場合はnullを返す
            if (selfType != typeToConvert)
            {
                return null;
            }
            // ジェネリック型IJsonStringJsonConverter<TSelf>を構築
            var converterType = typeof(ILinkableJsonConverter<>).MakeGenericType(typeToConvert);
            return Activator.CreateInstance(converterType) as JsonConverter;
        }
    }
}
