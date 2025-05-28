using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public interface ICardList
    {
        public bool Contains(Card card);
        public void Add(Card card);
        public bool Remove(Card card);

        public IEnumerable<Card> EnumCards();
        public void Load(IEnumerable<Card> source);
    }

    public static class ICardListExtensions
    {
        public static CardList ToCardList<T>(this T list) where T : ICardList => [.. list.EnumCards()];
        public static List<Card> ToList<T>(this T list) where T : ICardList => [.. list.EnumCards()];
        public static List<int> ToIdList<T>(this T list) where T : ICardList => [.. list.EnumCards().Select(c => c._id)];
        public static List<string> ToNameList<T>(this T list) where T : ICardList => [.. list.EnumCards().Select(c => c._name)];

        public static void WriteJson<T>(this T list, Utf8JsonWriter writer, JsonSerializerOptions options)
            where T : ICardList
        {
            writer.WriteStartArray();
            foreach (var card in list.EnumCards())
            {
                writer.WriteStringValue(card.Name);
            }
            writer.WriteEndArray();
        }

        public static void SaveAsRawText<T>(this T list, string path)
            where T : ICardList
        {
            File.WriteAllLines(path, list.EnumCards().Select(c => c.Name), Encoding.UTF8);
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
                if (Json.TryParse<List<string>>(path, out var nameSource))
                {
                    list.Load(nameSource);
                }
                else
                {
                    list.Load(EnumFromCardNames(path));
                }
                return true;
            }
            return false;
        }

        public static void Load<T>(this T list, ICardList source) where T : ICardList => list.Load(source.EnumCards());
        public static void Load<T>(this T list, IEnumerable<int> source) where T : ICardList => list.Load(EnumFromCardIds(source));
        public static void Load<T>(this T list, IEnumerable<string> source) where T : ICardList => list.Load(EnumFromCardNames(source));

        public static IEnumerable<Card> EnumFromCardNames(string path)
        {
            foreach (var line in File.ReadLines(path))
            {
                if (CardPool.Instance.TryGet(line.Trim(), out var card))
                {
                    yield return card;
                }
            }
        }

        public static IEnumerable<Card> EnumFromCardIds(IEnumerable<int> source)
        {
            foreach (var id in source)
            {
                if (CardPool.Instance.TryGet(id, out var card))
                {
                    yield return card;
                }
            }
        }

        public static IEnumerable<Card> EnumFromCardNames(IEnumerable<string> source)
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
