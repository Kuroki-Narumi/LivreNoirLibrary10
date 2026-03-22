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
            public Line(double pos, int value)
            {
                var rPos = Rationalize(pos);
                _list[rPos] = (ushort)value;
                _den = rPos.Denominator;
            }

            private static Rational Rationalize(double position) => Rational.RationalizeUnsafe(position, BmsConstants.MaxInnerResolution);

            public bool TryAdd(double position, int value)
            {
                var rPos = Rationalize(position);
                if (_list.ContainsKey(rPos))
                {
                    return false;
                }
                _den = NumberExtensions.LCM(_den, rPos.Denominator);
                _list.Add(rPos, (ushort)value);
                return true;
            }

            public void ReductDenominator(long limit)
            {
                var list = _list;
                using var o = ObjectPool.Rent<List<(Rational, ushort)>>();
                var newList = o.Value;
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