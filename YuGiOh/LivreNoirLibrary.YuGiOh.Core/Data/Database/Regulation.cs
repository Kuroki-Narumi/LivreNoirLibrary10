using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Collections;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Data
{
    public class Regulation : IJsonWriter
    {
        public static Regulation Instance { get; } = new();

        private readonly SortedDictionary<int, int> _list = [];
        private readonly Dictionary<int, RegulationCardList> _list_map;

        public RegulationCardList Forbidden { get; } = [];
        public RegulationCardList Limit1 { get; } = [];
        public RegulationCardList Limit2 { get; } = [];
        public RegulationCardList Specified { get; } = [];

        public int Count => _list.Count;

        public int this[Card key]
        {
            get => Get(key);
            set => Set(key, value);
        }

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

        public Regulation(Regulation source) : this() { Load(source); }
        public Regulation(Serializable.Regulation source) : this() { Load(source); }

        public void Clear()
        {
            _list.Clear();
            Forbidden.Clear();
            Limit1.Clear();
            Limit2.Clear();
            Specified.Clear();
        }

        public void Clear(int value)
        {
            if (_list_map.TryGetValue(value, out var list))
            {
                foreach (var card in list.AsSpan())
                {
                    var id = card.Id;
                    _list.Remove(id);
                }
                list.Clear();
            }
        }

        public bool LoadFile(string path)
        {
            if (Json.TryOpen<Serializable.Regulation>(path, out var data))
            {
                Load(data);
                return true;
            }
            else if (Json.TryOpen<Serializable.StringRegulation>(path, out var sData))
            {
                Load(sData);
                return true;
            }
            return false;
        }

        public void Load(Regulation source)
        {
            Clear();
            Set(source.Forbidden.EnumerateKeys(), LimitCount.Forbidden);
            Set(source.Limit1.EnumerateKeys(), LimitCount.Limit1);
            Set(source.Limit2.EnumerateKeys(), LimitCount.Limit2);
            Set(source.Specified.EnumerateKeys(), LimitCount.Specified);
        }

        public void Load(Serializable.Regulation source)
        {
            Clear();
            void SetInternal(List<int>? list, int num)
            {
                if (list is not null)
                {
                    Set(list, num);
                }
            }
            SetInternal(source.Forbidden, LimitCount.Forbidden);
            SetInternal(source.Limit1, LimitCount.Limit1);
            SetInternal(source.Limit2, LimitCount.Limit2);
            SetInternal(source.Specified, LimitCount.Specified);
        }

        public void Load(Serializable.StringRegulation source)
        {
            var database = CardPool.Instance.Cards;
            Clear();
            void SetInternal(List<string>? list, int num)
            {
                if (list is not null)
                {
                    Set(list.Select(database.Get), num);
                }
            }
            SetInternal(source.Forbidden, LimitCount.Forbidden);
            SetInternal(source.Limit1, LimitCount.Limit1);
            SetInternal(source.Limit2, LimitCount.Limit2);
            SetInternal(source.Specified, LimitCount.Specified);
        }

        public void Load(IDictionary<Card, int> source)
        {
            Clear();
            Set(source);
        }

        public int Get(ICard card)
        {
            if (card.Unusable)
            {
                return LimitCount.Unusable;
            }
            else if (_list.TryGetValue(card.Id, out var value))
            {
                return value;
            }
            else
            {
                return LimitCount.Unlimited;
            }
        }

        public int GetActualCount(Card card) => Math.Clamp(Get(card), LimitCount.Forbidden, LimitCount.Unlimited);

        public bool Set(int id, int value)
        {
            RegulationCardList? list;
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
                list.Add(id);
            }
            return true;
        }

        public bool Set(ICard card, int value) => Set(card.Id, value);

        public void Set(ReadOnlySpan<int> ids, int value)
        {
            RegulationCardList? list;
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
                list.AddRange(ids);
            }
        }

        public void Set(IEnumerable<int> ids, int value)
        {
            RegulationCardList? list;
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
                list.AddRange(ids);
            }
        }

        public void Set(IEnumerable<Card> cards, int value) => Set(cards.Select(card => card.Id), value);

        public void Set(IDictionary<Card, int> items)
        {
            foreach (var group in items.GroupBy(kv => kv.Value))
            {
                Set(group.Select(kv => kv.Key.Id), group.Key);
            }
        }

        public void Remove(List<int> ids)
        {
            Forbidden.RemoveKeys(ids);
            Limit1.RemoveKeys(ids);
            Limit2.RemoveKeys(ids);
            Specified.RemoveKeys(ids);
            foreach (var id in ids.AsSpan())
            {
                _list.Remove(id);
            }
        }

        public void SetForbidden(List<int> ids) => Set(ids, LimitCount.Forbidden);
        public void SetLimit1(List<int> ids) => Set(ids, LimitCount.Limit1);
        public void SetLimit2(List<int> ids) => Set(ids, LimitCount.Limit2);
        public void SetSpecified(List<int> ids) => Set(ids, LimitCount.Specified);

        public SortedDictionary<int, int>.Enumerator GetEnumerator() => _list.GetEnumerator();

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            static void WriteInternal(Utf8JsonWriter writer, RegulationCardList list, string propertyName)
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
