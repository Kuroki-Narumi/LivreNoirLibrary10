using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public class CardPool : IJsonWriter
    {
        public const string DefaultResourceName = "Resources/CardPool.json";

        public static CardPool Instance { get; } = new();

        public CardDataCollection Cards { get; } = [];
        public CardPackCollection Packs { get; } = [];
        public DateTime LastUpdate { get; set; }

        public bool LoadFile(string path = "")
        {
            if (!File.Exists(path))
            {
                path = Utils.GetFullPath(path);
            }
            if (Json.TryOpen<Serializable.CardPool>(path, out var data))
            {
                Load(data);
                return true;
            }
            return false;
        }

        public void Load(Serializable.CardPool source)
        {
            Packs.Load(source.Packs);
            Cards.Load(source.Cards, Packs);
            LastUpdate = source.LastUpdate;
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(JsonPropertyNames.Cards);
            Cards.WriteJson(writer, options);
            writer.WritePropertyName(JsonPropertyNames.Packs);
            Packs.WriteJson(writer, options);
            writer.WritePropertyName(JsonPropertyNames.LastUpdate);
            DateTimeJsonConverter.Write(writer, LastUpdate);
            writer.WriteEndObject();
        }

        public Card Get(int id) => Cards.Get(id);
        public Card Get(string name) => Cards.Get(name);
        public bool TryGet(int id, [MaybeNullWhen(false)] out Card card) => Cards.TryGet(id, out card);
        public bool TryGet(string name, [MaybeNullWhen(false)] out Card card) => Cards.TryGet(name, out card);

        public CardPack GetPack(string pid) => Packs.Get(pid);
    }
}
