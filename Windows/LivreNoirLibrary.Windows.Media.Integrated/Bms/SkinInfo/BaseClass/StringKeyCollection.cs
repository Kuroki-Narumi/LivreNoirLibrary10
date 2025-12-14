using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    public abstract class StringKeyCollection<T>() : ICollection<T>
    {
        private readonly OrderedDictionary<string, T> _dic = [];

        public int Count => _dic.Count;

        protected abstract string GetKey(T item);

        public void Clear() => _dic.Clear();

        public bool Contains(T item) => _dic.TryGetValue(GetKey(item), out var current) && EqualityComparer<T>.Default.Equals(current, item);
        public void Add(T item) => _dic[GetKey(item)] = item;
        public bool Remove(T item) => _dic.Remove(GetKey(item));
        public bool TryGetValue(string key, [MaybeNullWhen(false)] out T item) => _dic.TryGetValue(key, out item);

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public T? Find(Predicate<T> match)
        {
            foreach (var (_, item) in _dic)
            {
                if (match(item))
                {
                    return item;
                }
            }
            return default;
        }

        bool ICollection<T>.IsReadOnly => false;
        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => _dic.Values.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => _dic.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
