using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh.Data
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
            var t0 = Stopwatch.GetTimestamp();
            if (!File.Exists(path))
            {
                path = Utils.GetFullPath(path);
            }

            if (Json.TryOpen<Serializable.CardPool>(path, out var data))
            {
                Console.WriteLine($"CardPool: got json object in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");
                Load(data);
                Console.WriteLine($"CardPool: total time in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");
                return true;
            }
            return false;
        }

        public void Load(Serializable.CardPool source)
        {
            var t0 = Stopwatch.GetTimestamp();
            Packs.Load(source.Packs);
            Console.WriteLine($"CardPool: loaded Packs in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");

            t0 = Stopwatch.GetTimestamp();
            Cards.Load(source.Cards);
            Console.WriteLine($"CardPool: loaded Cards in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");

            t0 = Stopwatch.GetTimestamp();
            UpdateCardPackInfo();
            LastUpdate = source.LastUpdate;
            Console.WriteLine($"CardPool: updated pack info in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");
        }

        public void UpdateCardPackInfo()
        {
            foreach (var card in Cards.AsSpan())
            {
                card.PackInfo.Clear();
            }
            foreach (var pack in Packs.AsSpan())
            {
                var pid = pack.ProductId;
                foreach (var info in pack.AsSpan())
                {
                    if (TryGet(info.CardId, out var card))
                    {
                        card.PackInfo.Add(new(pid, info.Number));
                    }
                }
            }
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
