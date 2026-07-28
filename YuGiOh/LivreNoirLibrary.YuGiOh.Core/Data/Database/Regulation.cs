using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LivreNoirLibrary.YuGiOh.Data
{
    [JsonConverter(typeof(IWriteJsonJsonConverter<Regulation>))]
    public class Regulation : IWriteJson, ILimitProvider
    {
        private readonly SortedDictionary<int, int> _list = [];
        private readonly Dictionary<int, SortedCardList> _list_map;

        public SortedCardList Forbidden { get; } = [];
        public SortedCardList Limit1 { get; } = [];
        public SortedCardList Limit2 { get; } = [];
        public SortedCardList Specified { get; } = [];

        public int Count => _list.Count;

        public Regulation()
        {
            _list_map = new()
            {
                { LimitCount.Forbidden, Forbidden },
                { LimitCount.Limit1, Limit1 },
                { LimitCount.Limit2, Limit2 },
                { LimitCount.Specified, Specified },
            };
        }

        public bool LoadFile(string path, ICardProvider provider)
        {
            if (Json.TryOpen<Serializable.Regulation>(path, out var data) && !data.IsEmpty())
            {
                Load(data, provider);
                return true;
            }
            if (Json.TryOpen<Serializable.StringRegulation>(path, out var sData, true))
            {
                Load(sData, provider);
                return true;
            }
            return false;
        }

        public void Load(Regulation source)
        {
            Clear();
            Set(source.Forbidden, LimitCount.Forbidden);
            Set(source.Limit1, LimitCount.Limit1);
            Set(source.Limit2, LimitCount.Limit2);
            Set(source.Specified, LimitCount.Specified);
        }

        public void Load(Serializable.Regulation source, ICardProvider provider)
        {
            Clear();
            void SetInternal(List<int>? list, int num)
            {
                if (list is not null)
                {
                    Set(list.AsSpan(), num, provider);
                }
            }
            SetInternal(source.Forbidden, LimitCount.Forbidden);
            SetInternal(source.Limit1, LimitCount.Limit1);
            SetInternal(source.Limit2, LimitCount.Limit2);
            SetInternal(source.Specified, LimitCount.Specified);
        }

        public void Load(Serializable.StringRegulation source, ICardProvider provider)
        {
            Clear();
            void SetInternal(List<string>? list, int num)
            {
                if (list is not null)
                {
                    foreach (var name in list.AsSpan())
                    {
                        if (provider.TryGetByName(name, out var card))
                        {
                            Set(card, num);
                        }
                    }
                }
            }
            SetInternal(source.Forbidden, LimitCount.Forbidden);
            SetInternal(source.Limit1, LimitCount.Limit1);
            SetInternal(source.Limit2, LimitCount.Limit2);
            SetInternal(source.Specified, LimitCount.Specified);
        }

        public void Clear()
        {
            _list.Clear();
            Clear(Forbidden);
            Clear(Limit1);
            Clear(Limit2);
            Clear(Specified);
        }

        private static void Clear(SortedCardList list)
        {
            foreach (var card in list.AsSpan())
            {
                card.LimitCount = LimitCount.Unlimited;
            }
            list.Clear();
        }

        public void ClearLimit(int value)
        {
            if (_list_map.TryGetValue(value, out var list))
            {
                foreach (var card in list.AsSpan())
                {
                    var id = card.Id;
                    _list.Remove(id);
                    card.LimitCount = LimitCount.Unlimited;
                }
                list.Clear();
            }
        }

        public void Clear(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
            {
                var id = card.Id;
                if (_list.Remove(id, out var current) && _list_map.TryGetValue(current, out var list))
                {
                    list.RemoveKey(id);
                }
                card.LimitCount = LimitCount.Unlimited;
            }
        }

        public bool TryGet(int id, out int count) => _list.TryGetValue(id, out count);

        public bool Set(Card card, int value)
        {
            var id = card.Id;
            SortedCardList? list;
            if (_list.TryGetValue(id, out var current))
            {
                if (current == value)
                {
                    return false;
                }
                _list.Remove(id);
                if (_list_map.TryGetValue(current, out list))
                {
                    list.RemoveKey(id);
                }
            }
            if (_list_map.TryGetValue(value, out list))
            {
                _list[id] = value;
                list.Add(card);
                card.LimitCount = value;
            }
            else
            {
                card.LimitCount = LimitCount.Unlimited;
            }
            return true;
        }

        public bool Set(int id, int value, ICardProvider provider)
        {
            if (provider.TryGet(id, out var card))
            {
                return Set(card, value);
            }
            return false;
        }

        public void Set(IEnumerable<Card> cards, int value)
        {
            SortedCardList? list;
            foreach (var card in cards)
            {
                var id = card.Id;
                if (_list.TryGetValue(id, out var current))
                {
                    if (current == value)
                    {
                        continue;
                    }
                    _list[id] = value;
                    if (_list_map.TryGetValue(current, out list))
                    {
                        list.RemoveKey(id);
                    }
                }
                else
                {
                    _list.Add(id, value);
                }
                card.LimitCount = value;
            }
            if (_list_map.TryGetValue(value, out list))
            {
                list.AddRange(cards);
            }
        }

        public void Set(ReadOnlySpan<int> ids, int value, ICardProvider provider)
        {
            SortedCardList? list;
            foreach (var id in ids)
            {
                if (_list.TryGetValue(id, out var current))
                {
                    if (current == value)
                    {
                        continue;
                    }
                    _list[id] = value;
                    if (_list_map.TryGetValue(current, out list))
                    {
                        list.RemoveKey(id);
                    }
                }
                else
                {
                    _list.Add(id, value);
                }
            }
            if (_list_map.TryGetValue(value, out list))
            {
                foreach (var id in ids)
                {
                    if (provider.TryGet(id, out var card))
                    {
                        list.AddWithoutNotify(card);
                        card.LimitCount = value;
                    }
                }
                list.NotifyCollectionReset();
            }
        }

        public Serializable.Regulation ToSerializable(bool containsSpecified = true)
        {
            Serializable.Regulation result = new()
            {
                Forbidden = [.. Forbidden.GetKeySpan()],
                Limit1 = [.. Limit1.GetKeySpan()],
                Limit2 = [.. Limit2.GetKeySpan()],
                Specified = containsSpecified ? [.. Specified.GetKeySpan()] : null,
            };
            return result;
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            static void WriteInternal(Utf8JsonWriter writer, SortedCardList list, string propertyName)
            {
                if (list.Count is 0)
                {
                    return;
                }
                writer.WritePropertyName(propertyName);
                writer.WriteStartArray();
                foreach (var card in list.AsSpan())
                {
                    writer.WriteNumberValue(card.Id);
                }
                writer.WriteEndArray();
            }
            WriteInternal(writer, Forbidden, JsonPropertyNames.Forbidden);
            WriteInternal(writer, Limit1, JsonPropertyNames.Limit1);
            WriteInternal(writer, Limit2, JsonPropertyNames.Limit2);
            WriteInternal(writer, Specified, JsonPropertyNames.Specified);
            writer.WriteEndObject();
        }
    }
}
