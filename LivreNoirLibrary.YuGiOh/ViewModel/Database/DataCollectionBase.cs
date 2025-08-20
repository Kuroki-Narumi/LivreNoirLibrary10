using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;

namespace LivreNoirLibrary.YuGiOh.ViewModel
{
    public abstract class DataCollectionBase<TKey, TValue> : ObservableSortedList<TKey, TValue>, IJsonWriter
        where TKey : IComparable<TKey>
    {
        protected readonly Dictionary<string, int> _name2idx = [];
        private int _version = 0;
        private int _last_version = -1;

        protected override void ClearItems()
        {
            base.ClearItems();
            _version++;
        }

        protected override void AddItem(TValue item, out bool replaced, out int index, out TValue? oldItem)
        {
            base.AddItem(item, out replaced, out index, out oldItem);
            _version++;
        }

        protected override int RemoveItem(TValue item)
        {
            _version++;
            return base.RemoveItem(item);
        }

        protected Dictionary<string, int> CheckUpdate()
        {
            if (_version != _last_version)
            {
                Refresh();
                _last_version = _version;
            }
            return _name2idx;
        }

        public abstract void Refresh();

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var item in CollectionsMarshal.AsSpan(_list))
            {
                JsonSerializer.Serialize(writer, item, options);
            }
            writer.WriteEndArray();
        }
    }
}
