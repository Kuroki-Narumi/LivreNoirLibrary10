using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Inspect
{
    public class HandConditions : ObservableObjectBase, IWriteJson
    {
        public string? Name { get; set => SetValue(ref field, value); }
        public int GroupId { get; set => SetValue(ref field, value); }
        public double Value1 { get; set => SetValue(ref field, value); }
        public double Value2 { get; set => SetValue(ref field, value); }
        public ObservableList<HandConditionItem> Items { get; } = [];

        public int Index { get; set; }
        public int Count { get; set; }

        public void Load<T>(Serializable.HandInspectConditions<T> source, ICardProvider? provider)
        {
            Name = source.Name;
            GroupId = source.GroupId ?? source.Group ?? 0;
            Value1 = source.Value1;
            Value2 = source.Value2;
            var dst = Items;
            var count = dst.Count;
            var i = 0;
            foreach (var item in source.Items.AsSpan())
            {
                HandConditionItem current;
                if (i < count)
                {
                    current = dst[i];
                }
                else
                {
                    current = new();
                    dst.AddWithoutNotify(current);
                }
                current.IsFirst = i is 0;
                if (typeof(T) == typeof(int))
                {
                    current.Load((item as List<int>)!, provider);
                }
                else if (typeof(T) == typeof(string))
                {
                    current.Load((item as List<string>)!, provider);
                }
                i++;
            }
            if (i < count)
            {
                dst.RemoveRange(i, count - i);
            }
            else
            {
                dst.NotifyCollectionReset();
            }
        }

        public void CopyFrom(HandConditions source)
        {
            Name = source.Name;
            GroupId = source.GroupId;
            Value1 = source.Value1;
            Value2 = source.Value2;
            var items = Items;
            items.ClearWithoutNotify();
            foreach (var item in source.Items.AsSpan())
            {
                items.AddWithoutNotify(item.Clone());
            }
            items.NotifyCollectionReset();
        }

        public HandConditions Clone()
        {
            HandConditions result = new();
            result.CopyFrom(this);
            return result;
        }

        public HandConditionItem AddNewItem(IEnumerable<Card> cards)
        {
            HandConditionItem item = new()
            {
                IsFirst = Items.Count is 0
            };
            item.Cards.AddRange(cards);
            Items.Add(item);
            return item;
        }

        public bool RemoveCardsFrom(HandConditionItem item, IEnumerable<Card> cards)
        {
            var target = item.Cards;
            target.RemoveRange(cards);
            if (target.Count is 0 && Items.Remove(item))
            {
                if (item.IsFirst && Items.Count > 0)
                {
                    Items[0].IsFirst = true;
                }
                return true;
            }
            return false;
        }

        public void Prepare()
        {
            Count = 0;
            foreach (var item in Items.AsSpan())
            {
                item.Prepare();
            }
        }

        public bool IsMatch(ReadOnlySpan<int> source, List<int> buffer)
        {
            try
            {
                if (source.Length is 0)
                {
                    return false;
                }
                buffer.AddRange(source);
                foreach (var item in Items.AsSpan())
                {
                    if (!item.IsMatch(buffer))
                    {
                        return false;
                    }
                }
                return true;
            }
            finally
            {
                buffer.Clear();
            }
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteStringIfNotNull(JsonPropertyNames.Name, Name);
            writer.WriteNumber(JsonPropertyNames.GroupId, GroupId);
            writer.WriteNumber(JsonPropertyNames.Value1, Value1);
            writer.WriteNumber(JsonPropertyNames.Value2, Value2);
            var items = Items;
            if (items.Any(HandConditionItem.IsEffective))
            {
                writer.WritePropertyName(JsonPropertyNames.Items);
                writer.WriteStartArray();
                foreach (var item in items.AsSpan())
                {
                    JsonSerializer.Serialize(writer, item.Cards.Select(IId.GetId), options);
                }
                writer.WriteEndArray();
            }
            writer.WriteEndObject();
        }
    }
}
