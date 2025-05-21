using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.Media.Wave.Chunks
{
    public abstract class Container<T> : RiffChunk, ICollection<T>
        where T : IContainerData<T>
    {
        public override uint ByteSize => (uint)DataList.Count * T.ByteSize + sizeof(uint);
        protected List<T> DataList { get; set; } = [];

        public int Count => DataList.Count;
        public bool IsReadOnly => false;

        protected void LoadData(BinaryReader reader)
        {
            var count = (int)reader.ReadUInt32();
            var list = DataList;
            for (var i = 0; i < count; i++)
            {
                list.Add(T.Load(reader));
            }
        }

        public override void DumpContents(BinaryWriter writer)
        {
            writer.Write((uint)DataList.Count);
            foreach (var data in CollectionsMarshal.AsSpan(DataList))
            {
                data.Dump(writer);
            }
        }

        public override void WriteJsonContents(Utf8JsonWriter writer, JsonSerializerOptions options)
        {
            writer.WritePropertyName("data");
            writer.WriteStartArray();
            foreach (var data in CollectionsMarshal.AsSpan(DataList))
            {
                data.WriteJson(writer, options);
            }
            writer.WriteEndArray();
        }

        public void Clear() => DataList.Clear();

        public T? Get(int id) => DataList.Find(d => d.Id == id);

        public void Set(int id, T obj)
        {
            int index = DataList.FindIndex(d => d.Id == id);
            if (index >= 0)
            {
                DataList[index] = obj;
            }
            else
            {
                DataList.Add(obj);
            }
        }

        public void Add(T obj) => DataList.Add(obj);

        public int FindId(int id = 65536)
        {
            while (Get(id) is not null)
            {
                id++;
            }
            return id;
        }

        public bool Remove(T item) => DataList.Remove(item);
        public void Remove(int id) => DataList.RemoveAll(d => d.Id == id);

        public void Sort(Comparison<T> comp) => DataList.Sort(comp);
        public bool Contains(T item) => DataList.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => DataList.CopyTo(array, arrayIndex);

        public List<T>.Enumerator GetEnumerator() => DataList.GetEnumerator();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
