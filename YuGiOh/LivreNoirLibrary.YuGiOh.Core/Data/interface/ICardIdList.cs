using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public interface ICardIdList : IIdEnumerable
    {
        bool Contains(int id);
        void Add(int id);
        bool Remove(int id);
        void Load(IEnumerable<int> ids);
    }

    public static partial class Extensions
    {
        extension (ICardIdList list)
        {
            public bool Contains(ICard card) => list.Contains(card.ThisCard.Id);
            public void Add(ICard card) => list.Add(card.ThisCard.Id);
            public bool Remove(ICard card) => list.Remove(card.ThisCard.Id);
        }

        public static bool TryOpen<T>(string path, out T list, ICardProvider? provider = null)
            where T : ICardIdList, new()
        {
            list = new();
            return LoadFile(list, path, provider);
        }

        public static bool LoadFile<T>(this T list, string path, ICardProvider? provider = null)
            where T : ICardIdList
        {
            if (File.Exists(path))
            {
                if (Json.TryParse<int[]>(path, out var idSource))
                {
                    list.Load(idSource);
                }
                else if (provider is not null)
                {
                    if (Json.TryParse<string[]>(path, out var nameSource))
                    {
                        list.Load(nameSource, provider);
                    }
                    else
                    {
                        list.Load(EnumerateIdFromCardNames(path, provider));
                    }
                }
                return true;
            }
            return false;
        }

        public static void Load<T>(this T list, ICardIdList source) where T : ICardIdList => list.Load(source.EnumerateIds());
        public static void Load<T>(this T list, IEnumerable<string> source, ICardProvider provider) where T : ICardIdList => list.Load(EnumerateIdFromCardNames(source, provider));

        public static IEnumerable<int> EnumerateIdFromCardNames(string path, ICardProvider provider)
        {
            foreach (var line in File.ReadLines(path))
            {
                if (provider.TryGetByName(line.Trim(), out var card))
                {
                    yield return card.Id;
                }
            }
        }

        public static IEnumerable<int> EnumerateIdFromCardNames(IEnumerable<string> source, ICardProvider provider)
        {
            foreach (var name in source)
            {
                if (provider.TryGetByName(name, out var card))
                {
                    yield return card.Id;
                }
            }
        }
    }
}
