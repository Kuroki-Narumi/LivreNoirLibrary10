using System;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Linq;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public class Deck : ObservableObjectBase, IJsonWriter
    {
        public DeckCardList MainDeck { get; } = [];
        public DeckCardList ExtraDeck { get; } = [];
        public DeckCardList SideDeck { get; } = [];

        public bool IsLimitEnabled { get; set; }

        public Deck() { }
        public Deck(Deck deck) => Load(deck);

        public void Clear()
        {
            MainDeck.Clear();
            ExtraDeck.Clear();
            SideDeck.Clear();
        }

        public void Load(Deck source)
        {
            MainDeck.Load(source.MainDeck);
            ExtraDeck.Load(source.ExtraDeck);
            SideDeck.Load(source.SideDeck);
        }

        public void Load(Serializable.Deck source)
        {
            if (source.MainDeck is not null)
            {
                MainDeck.Load(source.MainDeck);
            }
            if (source.ExtraDeck is not null)
            {
                ExtraDeck.Load(source.ExtraDeck);
            }
            if (source.SideDeck is not null)
            {
                SideDeck.Load(source.SideDeck);
            }
        }

        public void Load(IEnumerable<string> source)
        {
            Clear();
            var main = MainDeck;
            var extra = ExtraDeck;
            foreach (var name in source)
            {
                if (CardPool.Instance.TryGet(name, out var card))
                {
                    if (card.IsExtraDeck())
                    {
                        extra.AddWithoutNotify(card);
                    }
                    else
                    {
                        main.AddWithoutNotify(card);
                    }
                }
            }
        }

        public static bool TryOpen(string path, out Deck deck)
        {
            deck = new();
            return deck.LoadFile(path);
        }

        public bool LoadFile(string path)
        {
            if (File.Exists(path))
            {
                if (Json.TryParse<Serializable.Deck>(path, out var deck))
                {
                    Load(deck);
                }
                else if (Json.TryOpen<List<string>>(path, out var data))
                {
                    Load(data);
                }
                else
                {
                    Load(File.ReadLines(path).Select(line => line.Trim()));
                }
                return true;
            }
            return false;
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            if (MainDeck.Count is > 0)
            {
                writer.WritePropertyName(JsonPropertyNames.MainDeck);
                MainDeck.WriteJson(writer, options);
            }
            if (ExtraDeck.Count is > 0)
            {
                writer.WritePropertyName(JsonPropertyNames.ExtraDeck);
                ExtraDeck.WriteJson(writer, options);
            }
            if (SideDeck.Count is > 0)
            {
                writer.WritePropertyName(JsonPropertyNames.SideDeck);
                SideDeck.WriteJson(writer, options);
            }
        }
    }
}
