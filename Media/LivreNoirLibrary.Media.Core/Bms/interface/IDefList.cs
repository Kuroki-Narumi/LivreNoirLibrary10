using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IDefList : IEnumerable<(short, string)>
    {
        public int Count { get; }
        public int MaxIndex { get; }

        public void Clear();
        public bool ContainsKey(short key);
        public bool TryGetValue(short key, [MaybeNullWhen(false)] out string value);
        public bool TryGetKey(string value, out short key);
        public void Set(short key, string? value);
        public bool Remove(short key);

        public void Swap(short key1, short key2);
        public void Map(DefIndexMap map);
        public void RemoveWithBasename(string basename, ICollection<short> removedKeys);
    }
}
