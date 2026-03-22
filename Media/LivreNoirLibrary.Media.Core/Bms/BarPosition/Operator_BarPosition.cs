using System;
using System.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly struct Operator_BarPosition : IPositionOperator<BarPosition>, IComparer<BarPosition, BarPosition>
    {
        public static BarPosition Zero { get; } = new(0, true);
        public static int Compare(BarPosition x, BarPosition y) => x.CompareTo(y);
        public static bool IsXCloserThanY(BarPosition x, BarPosition y, BarPosition z)
        {
            var zVal = z._value;
            return x._value + y._value - zVal - zVal is > 0;
        }

        public static BarPosition Add(BarPosition x, BarPosition y) => new(x._value + y._value);
        public static BarPosition Subtract(BarPosition x, BarPosition y) => new(x._value - y._value);

        public static void Write(BinaryWriter writer, BarPosition value) => value.Dump(writer);
        public static BarPosition Read(BinaryReader reader) => BarPosition.Load(reader);
    }
}
