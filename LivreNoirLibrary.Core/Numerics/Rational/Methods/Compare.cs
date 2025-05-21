using System;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : IComparable
    {
        public override int GetHashCode() => HashCode.Combine(_numerator, _denominatorMinusOne);

        public override bool Equals(object? obj)
        {
            return obj switch
            {
                Rational v => Equals(v),
                byte v => Equals(v),
                sbyte v => Equals(v),
                short v => Equals(v),
                ushort v => Equals(v),
                int v => Equals(v),
                uint v => Equals(v),
                long v => Equals(v),
                ulong v => Equals(v),
                float v => Equals(v),
                double v => Equals(v),
                decimal v => Equals(v),
                _ => false,
            };
        }

        public int CompareTo(object? obj)
        {
            return obj switch
            {
                Rational v => CompareTo(v),
                byte v => CompareTo(v),
                sbyte v => CompareTo(v),
                short v => CompareTo(v),
                ushort v => CompareTo(v),
                int v => CompareTo(v),
                uint v => CompareTo(v),
                long v => CompareTo(v),
                ulong v => CompareTo(v),
                float v => CompareTo(v),
                double v => CompareTo(v),
                decimal v => CompareTo(v),
                _ => ThrowIncomparableException(obj)
            };
        }
    }
}
