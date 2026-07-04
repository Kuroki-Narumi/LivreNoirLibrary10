using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICardList : ICardEnumerable
    {
        bool Contains(Card card);

        void Add(Card card);
        bool Remove(Card card);

        void Load(IEnumerable<Card> source);
    }

    public static class ICardListExtensions
    {
        public static CardList ToCardList<T>(this T list) where T : ICardEnumerable => [.. list.EnumerateCards()];
        public static List<Card> ToList<T>(this T list) where T : ICardEnumerable => [.. list.EnumerateCards()];
        public static List<int> ToIdList<T>(this T list) where T : ICardEnumerable => [.. list.EnumerateCards().Select(c => c.Id)];
        public static List<string> ToNameList<T>(this T list) where T : ICardEnumerable => [.. list.EnumerateCards().Select(c => c.Name)];

        public static void WriteJson<T>(this T list, Utf8JsonWriter writer, JsonSerializerOptions options)
            where T : ICardEnumerable
        {
            writer.WriteStartArray();
            foreach (var card in list.EnumerateCards())
            {
                writer.WriteNumberValue(card.Id);
            }
            writer.WriteEndArray();
        }

        public static void SaveAsRawText<T>(this T list, string path)
            where T : ICardEnumerable
        {
            File.WriteAllLines(path, list.EnumerateCards().Select(c => c.Id.ToString()), Encoding.UTF8);
        }

        public static bool TryOpen<T>(string path, out T list)
            where T : ICardList, new()
        {
            list = new();
            return LoadFile(list, path);
        }

        public static bool LoadFile<T>(this T list, string path)
            where T : ICardList
        {
            if (File.Exists(path))
            {
                if (Json.TryParse<int[]>(path, out var idSource))
                {
                    list.Load(idSource);
                }
                else if (Json.TryParse<string[]>(path, out var nameSource))
                {
                    list.Load(nameSource);
                }
                else
                {
                    list.Load(EnumerateFromCardNames(path));
                }
                return true;
            }
            return false;
        }

        public static void Load<T>(this T list, ICardList source) where T : ICardList => list.Load(source.EnumerateCards());
        public static void Load<T>(this T list, IEnumerable<int> source) where T : ICardList => list.Load(EnumerateFromCardIds(source));
        public static void Load<T>(this T list, IEnumerable<string> source) where T : ICardList => list.Load(EnumerateFromCardNames(source));

        public static IEnumerable<Card> EnumerateFromCardNames(string path)
        {
            foreach (var line in File.ReadLines(path))
            {
                if (CardPool.Instance.TryGet(line.Trim(), out var card))
                {
                    yield return card;
                }
            }
        }

        public static IEnumerable<Card> EnumerateFromCardIds(IEnumerable<int> source)
        {
            foreach (var id in source)
            {
                if (CardPool.Instance.TryGet(id, out var card))
                {
                    yield return card;
                }
            }
        }

        public static IEnumerable<Card> EnumerateFromCardNames(IEnumerable<string> source)
        {
            foreach (var name in source)
            {
                if (CardPool.Instance.TryGet(name, out var card))
                {
                    yield return card;
                }
            }
        }
    }
}
