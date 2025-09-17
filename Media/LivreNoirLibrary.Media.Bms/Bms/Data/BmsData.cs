using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class BmsData : BaseData, IRootData
    {
        public ChartType ChartType { get; set; } = ChartType.Beat;
        public BarLengthCache BarLengthCache { get; } = new();

        public static BmsData Create()
        {
            BmsData data = new();
            data.Headers.SetDefault();
            return data;
        }

        internal override void ClearBarLengthCache(int number) => BarLengthCache.Clear(number);
        internal override Rational GetHead(int number, IBarPositionProvider provider) => BarLengthCache.GetHead(number, provider);
        internal override Rational GetAbsolutePosition(BarPosition position, IBarPositionProvider provider) => BarLengthCache.GetAbsolutePosition(position, provider);
        internal override BarPosition GetBarPosition(Rational absolutePosition, IBarPositionProvider provider) => BarLengthCache.GetBarPosition(absolutePosition, provider);
        internal override IEnumerable<BarInfo> EnumerateBars(int first, int last, IBarPositionProvider provider) => BarLengthCache.EnumerateBars(first, last, provider);
    }
}
