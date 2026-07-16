using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Converters;

namespace LivreNoirLibrary.YuGiOh.Data
{
    [JsonConverter(typeof(CardPoolJsonConverter))]
    public class CardPool : IWriteJson
    {
        public const string DefaultResourceName = "Resources/CardPool.json";
        public static string ResourceFilePath { get; set; } = Utils.GetFullPath(DefaultResourceName);

        public CardDataCollection Cards { get; } = [];
        public CardPackCollection Packs { get; } = [];
        public DateTime LastUpdate { get; set; }

        public bool LoadFile(string path)
        {
            var t0 = Stopwatch.GetTimestamp();
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
            Cards.Load(source.Cards);
            Console.WriteLine($"CardPool: loaded Cards in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");

            t0 = Stopwatch.GetTimestamp();
            Packs.Load(source.Packs, Cards);
            Console.WriteLine($"CardPool: loaded Packs in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");

            t0 = Stopwatch.GetTimestamp();
            UpdateCardPackInfo();
            LastUpdate = source.LastUpdate;
            Console.WriteLine($"CardPool: updated pack info in {Stopwatch.GetElapsedTime(t0).TotalMilliseconds}ms");
        }

        public void UpdateCardPackInfo()
        {
            var cards = Cards;
            foreach (var card in cards)
            {
                card.PackInfo.Clear();
            }
            foreach (var pack in Packs.AsSpan())
            {
                var pid = pack.ProductId;
                var name = pack.Name;
                var date = pack.Date;
                foreach (var info in pack.AsSpan())
                {
                    info.Card.PackInfo.Add(new(pid, info.Number, name, date));
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
    }
}
