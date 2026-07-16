using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class Deck : ObservableObjectBase, IWriteJson, IClear, IIdEnumerable, ICardEnumerable
    {
        public const int DefaultMaxCount = 3;

        public int MaxCount
        {
            get;
            set
            {
                if (SetValue(ref field, value))
                {
                    MainDeck.MaxCount = value;
                    ExtraDeck.MaxCount = value;
                    SideDeck.MaxCount = value;
                }
            }
        } = DefaultMaxCount;

        public DeckCardList MainDeck { get; } = new() { MaxCount = DefaultMaxCount };
        public DeckCardList ExtraDeck { get; } = new() { MaxCount = DefaultMaxCount };
        public DeckCardList SideDeck { get; } = new() { MaxCount = DefaultMaxCount };

        public void Clear()
        {
            MainDeck.Clear();
            ExtraDeck.Clear();
            SideDeck.Clear();
        }

        public void Add(Card card, bool max = false, bool toSideDeck = false)
        {
            DeckCardList target = toSideDeck ? SideDeck : (card.IsMainDeck() ? MainDeck : ExtraDeck);
            if (max)
            {
                target.Set(card, MaxCount);
            }
            else
            {
                target.Add(card);
            }
        }

        public void Remove(Card card, bool max = false, bool toSideDeck = false)
        {
            DeckCardList target = toSideDeck ? SideDeck : (card.IsMainDeck() ? MainDeck : ExtraDeck);
            if (max)
            {
                target.Set(card, 0);
            }
            else
            {
                target.Remove(card);
            }
        }

        public void AddWithoutNotify(Card card, bool max = false, bool toSideDeck = false)
        {
            DeckCardList target = toSideDeck ? SideDeck : (card.IsMainDeck() ? MainDeck : ExtraDeck);
            if (max)
            {
                target.SetWithoutNotify(card, MaxCount, out _, out _);
            }
            else
            {
                target.AddWithoutNotify(card, out _, out _);
            }
        }

        public void RemoveWithoutNotify(Card card, bool max = false, bool toSideDeck = false)
        {
            DeckCardList target = toSideDeck ? SideDeck : (card.IsMainDeck() ? MainDeck : ExtraDeck);
            if (max)
            {
                target.SetWithoutNotify(card, 0, out _, out _);
            }
            else
            {
                target.RemoveWithoutNotify(card, out _, out _);
            }
        }

        public void NotifyCollectionReset()
        {
            MainDeck.NotifyCollectionReset();
            ExtraDeck.NotifyCollectionReset();
            SideDeck.NotifyCollectionReset();
        }

        public void Load(Serializable.Deck source, ICardProvider provider)
        {
            ProcessLoad(MainDeck, source.MainDeck.AsSpan(), provider);
            ProcessLoad(ExtraDeck, source.ExtraDeck.AsSpan(), provider);
            ProcessLoad(SideDeck, source.SideDeck.AsSpan(), provider);
        }

        private static void ProcessLoad(DeckCardList list, ReadOnlySpan<int> ids, ICardProvider provider)
        {
            list.ClearWithoutNotify();
            foreach (var id in ids)
            {
                if (provider.TryGet(id, out var card))
                {
                    list.AddWithoutNotify(card, out _, out _);
                }
            }
            list.NotifyCollectionReset();
        }

        public void Load(DeckForHistoryData source, ICardProvider provider)
        {
            ProcessLoad(MainDeck, source.MainDeck.AsSpan(), provider);
            ProcessLoad(ExtraDeck, source.ExtraDeck.AsSpan(), provider);
            ProcessLoad(SideDeck, source.SideDeck.AsSpan(), provider);
        }

        private static void ProcessLoad(DeckCardList list, ReadOnlySpan<(int, int)> values, ICardProvider provider)
        {
            list.ClearWithoutNotify();
            foreach (var (id, count) in values)
            {
                if (provider.TryGet(id, out var card))
                {
                    list.SetWithoutNotify(card, count, out _, out _);
                }
            }
            list.NotifyCollectionReset();
        }

        public void Load(IEnumerable<string> source, ICardProvider provider)
        {
            Clear();
            var main = MainDeck;
            var extra = ExtraDeck;
            foreach (var name in source)
            {
                if (provider.TryGetByName(name, out var card))
                {
                    var target = card.IsMainDeck() ? main : extra;
                    target.AddWithoutNotify(card, out _, out _);
                }
            }
            main.NotifyCollectionReset();
            extra.NotifyCollectionReset();
        }

        public bool LoadFile(string path, ICardProvider provider)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            if (Json.TryParse<Serializable.Deck>(path, out var deck) && !deck.IsEmpty())
            {
                Load(deck, provider);
            }
            else if (Json.TryOpen<string[]>(path, out var data))
            {
                Load(data, provider);
            }
            else
            {
                Load(File.ReadLines(path).Select(line => line.Trim()), provider);
            }
            return true;
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            MainDeck.WriteJson(JsonPropertyNames.MainDeck, writer);
            ExtraDeck.WriteJson(JsonPropertyNames.ExtraDeck, writer);
            SideDeck.WriteJson(JsonPropertyNames.SideDeck, writer);
            writer.WriteEndObject();
        }

        public IEnumerable<int> EnumerateIds() => ConcutnateEnumerator.Create(MainDeck.EnumerateIds(), ExtraDeck.EnumerateIds(), SideDeck.EnumerateIds());

        public IEnumerable<ICard> EnumerateCards() => ConcutnateEnumerator.Create(MainDeck.EnumerateCards(), ExtraDeck.EnumerateCards(), SideDeck.EnumerateCards());
    }
}
