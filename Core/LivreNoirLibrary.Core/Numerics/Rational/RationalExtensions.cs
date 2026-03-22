using System;
using System.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public static class RationalExtensions
    {
        public static Rational ToRational(this Half value, long denominatorLimit = Rational.HalfDenominatorLimit)
        {
            var (num, den) = Rational.Rationalize((float)value, denominatorLimit);
            return new(false, num, den);
        }

        public static Rational ToRational(this float value, long denominatorLimit = Rational.FloatDenominatorLimit)
        {
            var (num, den) = Rational.Rationalize(value, denominatorLimit);
            return new(false, num, den);
        }

        public static Rational ToRational(this double value, long denominatorLimit = Rational.DoubleDenominatorLimit)
        {
            var (num, den) = Rational.Rationalize(value, denominatorLimit);
            return new(false, num, den);
        }

        public static Rational ToRational(this decimal value, long denominatorLimit = Rational.DoubleDenominatorLimit)
        {
            var (num, den) = Rational.Rationalize(value, denominatorLimit);
            return new(false, num, den);
        }
    }
}

namespace LivreNoirLibrary.IO
{
    public static class RationalIOExtensions
    {
        public static Rational ReadRational(this BinaryReader reader)
        {
            var negative = reader.ReadBoolean();
            var num = reader.Read7BitEncodedInt64();
            var den = reader.Read7BitEncodedInt64();
            return new(true, negative ? -num : num, den);
        }

        public static void Write(this BinaryWriter writer, Rational value)
        {
            var (num, den) = value;
            bool negative;
            if (negative = (num is < 0))
            {
                num = -num;
            }
            writer.Write(negative);
            writer.Write7BitEncodedInt64(num);
            writer.Write7BitEncodedInt64(den);
        }
    }
}