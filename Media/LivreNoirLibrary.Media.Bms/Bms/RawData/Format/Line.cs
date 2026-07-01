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
            internal long _resol = 1;

            public Line() { }
            public Line(Rational pos, int value)
            {
                _list[pos] = (ushort)value;
                _resol = pos.Denominator;
            }

            public bool TryAdd(Rational position, int value, long resolutionLimit)
            {
                if (_list.ContainsKey(position))
                {
                    return false;
                }
                var resol = _resol;
                var den = position.Denominator;
                if (den != resol)
                {
                    var newResol = NumberExtensions.LCM(resol, den);
                    if (newResol > resolutionLimit)
                    {
                        return false;
                    }
                    _resol = newResol;
                }
                _list.Add(position, (ushort)value);
                return true;
            }

            public void WriteText(BmsTextWriter writer, int radix)
            {
                var den = _resol;
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