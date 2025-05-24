using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    partial class BaseData : IBarPositionProvider
    {
        BarPosition IBarPositionProvider.MaxBarPosition => Constants.MaxBarPosition;
        public Rational GetBarLength(int number) => Bars.Get(number);

        public Rational GetHead(int number) => Root.GetHead(number, Bars.Get);
        public Rational GetHead(BarPosition position) => GetHead(position.Bar);
        public Rational GetTail(BarPosition position) => GetHead(position.Bar + 1);
        public Rational GetBeat(BarPosition position) => GetHead(position.Bar) + position.Beat;

        public BarPosition GetPosition(Rational beat) => Root.GetPosition(beat, Bars.Get);
        public int GetNumber(Rational beat) => GetPosition(beat).Bar;

        public IEnumerable<BarInfo> EachBar(int first, int last = 0)
        {
            if (last is <= 0)
            {
                last = Bars.LastNumber;
            }
            foreach (var item in Root.EnumBars(first, last, Bars.Get))
            {
                yield return item;
            }
        }

        public IEnumerable<BarInfo> EachBar(Rational first, Rational last) => EachBar(GetNumber(first), GetNumber(last));
    }
}
