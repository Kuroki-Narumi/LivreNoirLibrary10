using LivreNoirLibrary.ObjectModel;
using System;
using System.Runtime.InteropServices;

namespace LivreNoirLibrary.YuGiOh.Search
{
    [StructLayout(LayoutKind.Explicit)]
    public class NumbersKey(int v1, int v2, int v3, int v4) : IComparable<NumbersKey>, IEquatable<NumbersKey>
    {
        [FieldOffset(0)]
        public readonly long _m_value;
        [FieldOffset(6)]
        public readonly short _v1 = (short)v1;
        [FieldOffset(4)]
        public readonly short _v2 = (short)v2;
        [FieldOffset(2)]
        public readonly short _v3 = (short)v3;
        [FieldOffset(0)]
        public readonly short _v4 = (short)v4;

        public int Value1 => _v1;
        public int Value2 => _v2;
        public int Value3 => _v3;
        public int Value4 => _v4;

        public int CompareTo(NumbersKey? other) => other is null ? 1 : _m_value.CompareTo(other._m_value);

        public bool Equals(NumbersKey? other) => other is not null && _m_value == other._m_value;
        public override bool Equals(object? obj) => Equals(obj as NumbersKey);
        public override int GetHashCode() => _m_value.GetHashCode();
    }
}
