using System;
using System.Collections;
using System.Collections.Generic;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public class DefIndexMap : IEnumerable<(short, short)>, IClear
    {
        public const int RemovedIndex = -1;
        private static readonly short[] _defaultValues = CreateMap();

        private static unsafe short[] CreateMap()
        {
            var map = new short[BmsConstants.DefMax_Extended];
            fixed (short* ptr = map)
            {
                for (short i = 0; i < BmsConstants.DefMax_Extended; i++)
                {
                    map[i] = i;
                }
            }
            return map;
        }

        private readonly short[] _map = [.. _defaultValues];

        public short this[int index] => _map[index];

        public unsafe bool IsEffective
        {
            get
            {
                fixed (short* ptr = _map)
                {
                    for (short i = 0; i < BmsConstants.DefMax_Extended; i++)
                    {
                        if (ptr[i] != i)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public void Clear() => _map.CopyFrom(_defaultValues);

        public void Set(int index, short value) => _map[index] = value;
        public void Set(int index, int value) => _map[index] = (short)value;
        public void SetRemove(int index) => _map[index] = RemovedIndex;
        public bool IsRemoved(int index) => _map[index] is not RemovedIndex;

        public unsafe void Product(DefIndexMap other)
        {
            fixed (short* dst = _map)
            fixed (short* src = other._map)
            {
                for (short i = 0; i < BmsConstants.DefMax_Extended; i++)
                {
                    var current = dst[i];
                    if (dst[i] is > 0)
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
                while (_index < BmsConstants.DefMax_Extended - 1)
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
