using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace LivreNoirLibrary.YuGiOh.Inspect
{
    public class HandConditionsCollection : ObservableList<HandConditions>, IWriteJson
    {
        public bool LoadFile(string path, ICardProvider? provider)
        {
            if (Json.TryOpen<List<Serializable.HandInspectConditions<int>>>(path, out var source) && source.Any(cond => cond.Items is not null))
            {
                Load(source, provider);
                return true;
            }
            if (Json.TryOpen<List<Serializable.HandInspectConditions<string>>>(path, out var strSource, true))
            {
                Load(strSource, provider);
                return true;
            }
            return false;
        }

        public void Load<T>(List<Serializable.HandInspectConditions<T>> source, ICardProvider? provider)
        {
            var i = 0;
            var count = Count;
            foreach (var item in source.AsSpan())
            {
                HandConditions current;
                if (i < count)
                {
                    current = _list[i];
                }
                else
                {
                    current = new();
                    AddWithoutNotify(current);
                }
                current.Load(item, provider);
                i++;
            }
            if (i < count)
            {
                RemoveRange(i, count - i);
            }
            else
            {
                this.NotifyCollectionReset();
            }
        }

        public void WriteJson(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var item in AsSpan())
            {
                item.WriteJson(writer, options);
            }
            writer.WriteEndArray();
        }

        public new void Sort() => Sort(HandConditionsComparer.Default);

        public void Prepare(Dictionary<int, List<HandConditions>> dic)
        {
            var i = 0;
            foreach (var item in this.AsSpan())
            {
                item.Index = i;
                item.Prepare();
                dic.Add(item.GroupId, item);
                i++;
            }
        }
    }
}
