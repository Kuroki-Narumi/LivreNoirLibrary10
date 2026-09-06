using System;
using System.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly struct Operator_BarPosition : IPositionOperator<BarPosition>, IComparer<BarPosition, BarPosition>
    {
        public static BarPosition Zero { get; } = new(0, 0);
        public static int Compare(BarPosition x, BarPosition y) => x.CompareTo(y);
        public static bool IsXCloserThanY(BarPosition x, BarPosition y, BarPosition z)
        {
            var zVal = z.Bar + z.Offset;
            return (x.Bar + x.Offset) + (y.Bar + y.Offset) - zVal - zVal is > 0;
        }

        public static BarPosition Add(BarPosition x, BarPosition y) => new(x.Bar + y.Bar, 0);
        public static BarPosition Subtract(BarPosition x, BarPosition y) => new(x.Bar - y.Bar, 0);

        public static void Write(BinaryWriter writer, BarPosition value) => value.Dump(writer);
        public static BarPosition Read(BinaryReader reader) => BarPosition.Load(reader);
    }
}
