using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Wave
{
    public interface IMarkerCollection : ICollection<Marker>
    {
        bool Contains(long position);
        void Set(long position, string? name);
        bool Remove(long position);
        bool RemoveRange(long start, long length);

        IEnumerable<MarkerInfo> EnumerateWithLength(long limit, bool skipIgnoreName);

        void ICollection<Marker>.Add(Marker item) => Set(item.Position, item.Name);
        bool ICollection<Marker>.Remove(Marker item) => Remove(item.Position);
        bool ICollection<Marker>.Contains(Marker item) => Contains(item.Position);
        bool ICollection<Marker>.IsReadOnly => false;
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class IMarkerCollectionExtensions
    {
        extension (IMarkerCollection collection)
        {
            public void Load(ReadOnlySpan<Marker> source)
            {
                collection.Clear();
                foreach (var (pos, name) in source)
                {
                    collection.Set(pos, name);
                }
            }

            public void Load(IMarkerCollection source)
            {
                collection.Clear();
                foreach (var (pos, name) in source)
                {
                    collection.Set(pos, name);
                }
            }
        }
    }
}
