using System;

namespace LivreNoirLibrary.Media.Midi
{
    public readonly struct SortKey(double v1, double v2, double v3, int v4) : IComparable<SortKey>
    {
        private readonly double _v1 = v1;
        private readonly double _v2 = v2;
        private readonly double _v3 = v3;
        private readonly int _v4 = v4;

        public int CompareTo(SortKey other)
        {
            if (_v1 > other._v1)
            {
                return 1;
            }
            if (_v1 < other._v1)
            {
                return -1;
            }
            if (_v2 > other._v2)
            {
                return 1;
            }
            if (_v2 < other._v2)
            {
                return -1;
            }
            if (_v3 > other._v3)
            {
                return 1;
            }
            if (_v3 < other._v3)
            {
                return -1;
            }
            return _v4.CompareTo(other._v4);
        }
    }
}
