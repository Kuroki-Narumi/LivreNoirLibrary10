using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public readonly struct TimeSignature(ushort numerator, ushort denominator) : IDumpable<TimeSignature>
    {
        public static TimeSignature Default { get; } = new(4, 4);

        public readonly ushort Numerator = numerator;
        public readonly ushort Denominator = denominator;

        public TimeSignature(int numerator, int denominator) : this((ushort)numerator, (ushort)denominator) { }
        public TimeSignature(Rational value) : this((ushort)value.Numerator, (ushort)value.Denominator) { }

        public void Dump(BinaryWriter writer)
        {
            writer.Write(Numerator);
            writer.Write(Denominator);
        }

        public static TimeSignature Load(BinaryReader reader)
        {
            var num = reader.ReadUInt16();
            var den = reader.ReadUInt16();
            return new(num, den);
        }

        public override string ToString() => $"{Numerator}/{Denominator}";

        public Rational ToRational() => new(Numerator, Denominator);
        public Rational ToRational(int barCount) => new(Numerator * barCount, Denominator);

        public static implicit operator (int, int)(TimeSignature obj) => (obj.Numerator, obj.Denominator);
        public static implicit operator (ushort, ushort)(TimeSignature obj) => (obj.Numerator, obj.Denominator);
        public static implicit operator TimeSignature((int, int) obj) => new(obj.Item1, obj.Item2);
        public static implicit operator TimeSignature((ushort, ushort) obj) => new(obj.Item1, obj.Item2);
        public static implicit operator Rational(TimeSignature obj) => obj.ToRational();
        public static explicit operator TimeSignature(Rational obj) => new(obj);
    }
}
