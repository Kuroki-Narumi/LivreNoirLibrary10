using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BmsFormatter
    {
        private class Line
        {
            private readonly SortedList<Rational, ushort> _list = [];
            internal long _den = 1;

            public Line() { }
            public Line(Rational pos, int value)
            {
                _list[pos] = (ushort)value;
                _den = pos.Denominator;
            }

            public bool TryAdd(Rational position, int value)
            {
                if (_list.ContainsKey(position))
                {
                    return false;
                }
                _den = NumberExtensions.LCM(_den, position.Denominator);
                _list.Add(position, (ushort)value);
                return true;
            }

            public void ReductDenominator(long limit)
            {
                var list = _list;
                var newList = ObjectPool.Rent<List<(Rational, ushort)>>();
                try
                {
                    foreach (var ((n, d), value) in list)
                    {
                        var newPos = new Rational(n * limit / d, limit);
                        newList.Add((newPos, value));
                    }
                    list.Clear();
                    foreach (var (pos, value) in newList.AsSpan())
                    {
                        list[pos] = value;
                    }
                    _den = limit;
                }
                finally
                {
                    ObjectPool.Return(newList);
                }
            }

            public void WriteText(BmsTextWriter writer, int radix)
            {
                var den = _den;
                var index = 0;
                foreach (var ((n, d), value) in _list)
                {
                    var num = n * den / d;
                    while (index < num)
                    {
                        writer.Write("00");
                        index++;
                    }
                    writer.Write(BmsUtils.ToBased(value, radix));
                    index++;
                }
                while (index < den)
                {
                    writer.Write("00");
                    index++;
                }
                writer.WriteLine();
            }
        }
    }
}