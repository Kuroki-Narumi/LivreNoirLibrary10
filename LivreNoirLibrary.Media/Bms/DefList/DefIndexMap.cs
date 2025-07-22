using System;
using System.Collections;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;

namespace LivreNoirLibrary.Media.Bms
{
    public class DefIndexMapCollection : SortedDictionary<DefType, DefIndexMap>
    {
        public void Merge(DefIndexMapCollection other)
        {
            foreach (var (type, map) in other)
            {
                var current = this.GetOrAdd(type);
                current.Merge(map);
            }
        }

        public void Product(DefIndexMapCollection other)
        {
            foreach (var (type, map) in other)
            {
                var current = this.GetOrAdd(type);
                current.Product(map);
            }
        }
    }

    public class DefIndexMap : IEnumerable<(short, short)>
    {
        private readonly short[] _map = CreateMap();

        private static unsafe short[] CreateMap()
        {
            var map = new short[Constants.DefMax_Extended];
            Clear(map);
            return map;
        }

        private static unsafe void Clear(short[] map)
        {
            fixed (short* ptr = map)
            {
                for (short i = 0; i < Constants.DefMax_Extended; i++)
                {
                    map[i] = i;
                }
            }
        }

        public short this[int index]
        {
            get => _map[index];
            set => _map[index] = value;
        }

        public void Clear() => Clear(_map);

        public int Get(int index) => _map[index];
        public void Set(int index, short value) => _map[index] = value;
        public void Set(int index, int value) => _map[index] = (short)value;

        public void SetRemove(int index) => _map[index] = DefSortResult.RemovedIndex;

        public unsafe bool IsEffective()
        {
            fixed (short* ptr = _map)
            {
                for (short i = 0; i < Constants.DefMax_Extended; i++)
                {
                    if (ptr[i] != i)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public unsafe void Merge(DefIndexMap other)
        {
            fixed (short* dst = _map)
            fixed (short* src = other._map)
            {
                for (short i = 0; i < Constants.DefMax_Extended; i++)
                {
                    dst[i] = src[i];
                }
            }
        }

        public unsafe void Product(DefIndexMap other)
        {
            fixed (short* dst = _map)
            fixed (short* src = other._map)
            {
                for (short i = 0; i < Constants.DefMax_Extended; i++)
                {
                    var current = dst[i];
                    if (current is > 0)
                    {
                        dst[i] = src[current];
                    }
                }
            }
        }

        public Enumerator GetEnumerator() => new(this);
        IEnumerator<(short, short)> IEnumerable<(short, short)>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct Enumerator(DefIndexMap map) : IEnumerator<(short, short)>
        {
            private readonly short[] _map = map._map;
            private short _index = -1;

            public void Reset() => _index = -1;
            public readonly (short, short) Current => (_index, _map[_index]);

            public bool MoveNext()
            {
                while (_index < Constants.DefMax_Extended - 1)
                {
                    _index++;
                    if (_index != _map[_index])
                    {
                        return true;
                    }
                }
                return false;
            }

            readonly object IEnumerator.Current => Current;
            readonly void IDisposable.Dispose() { }
        }
    }
}
