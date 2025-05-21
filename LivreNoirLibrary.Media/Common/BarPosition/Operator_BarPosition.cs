using System;
using System.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public readonly struct Operator_BarPosition : IPositionOperator<BarPosition>, IComparer<BarPosition, BarPosition>
    {
        public static BarPosition Zero { get; } = BarPosition.Zero;
        public bool Equals(BarPosition x, BarPosition y) => x == y;
        public bool LessThan(BarPosition x, BarPosition y) => x < y;
        public bool IsCloser(BarPosition x, BarPosition y, BarPosition z) => y - z + x - z > BarPosition.Zero;
        public BarPosition Add(BarPosition x, BarPosition y) => x + y;
        public BarPosition Subtract(BarPosition x, BarPosition y) => x - y;

        public void Write(BinaryWriter writer, BarPosition value) => value.Dump(writer);
        public BarPosition Read(BinaryReader reader) => BarPosition.Load(reader);
    }
}
