using System;
using System.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.IO
{
    public static class RationalIOExtensions
    {
        public static Rational ReadRational(this BinaryReader reader)
        {
            var negative = reader.ReadBoolean();
            var num = reader.Read7BitEncodedInt64();
            var den = reader.Read7BitEncodedInt64();
            return Rational.CreateFromBuffer(negative, num, den);
        }

        public static void Write(this BinaryWriter writer, Rational value)
        {
            var (negative, num, den) = value.GetBuffer();
            writer.Write(negative);
            writer.Write7BitEncodedInt64(num);
            writer.Write7BitEncodedInt64(den);
        }
    }
}

namespace LivreNoirLibrary.Numerics
{
    public partial struct Rational
    {
        internal static Rational CreateFromBuffer(bool negative, long numAbs, long den)
        {
            return new(true, negative ? -numAbs : numAbs, den);
        }

        internal (bool, long, long) GetBuffer()
        {
            var num = _numerator;
            var negative = false;
            if (num is < 0)
            {
                negative = true;
                num = -num;
            }
            return (negative, num, Denominator);
        }
    }
}