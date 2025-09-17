using System;
using System.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public readonly struct Operator_BarPosition : IPositionOperator<BarPosition>, IComparer<BarPosition, BarPosition>
    {
        public static BarPosition Zero { get; } = BarPosition.Zero;
        public int Compare(BarPosition x, BarPosition y) => x.CompareTo(y);
        public bool IsXCloserThanY(BarPosition x, BarPosition y, BarPosition z)
        {
            if (y.Bar + x.Bar - z.Bar * 2 is > 0)
            {
                return true;
            }
            return (y.Offset + x.Offset - z.Offset * 2) > Rational.Zero;
        }
        public BarPosition Add(BarPosition x, BarPosition y) => new(x.Bar + y.Bar, x.Offset);
        public BarPosition Subtract(BarPosition x, BarPosition y) => new(x.Bar - y.Bar, x.Offset);

        public void Write(BinaryWriter writer, BarPosition value) => value.Dump(writer);
        public BarPosition Read(BinaryReader reader) => BarPosition.Load(reader);
    }
}
