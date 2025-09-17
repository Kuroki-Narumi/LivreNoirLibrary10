using System;
using System.IO;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public readonly struct TimeSignature : IDumpable, ILoadable<TimeSignature>
    {
        public static TimeSignature Default { get; } = new(4, 4);

        /// <summary>
        /// The numerator is held at its actual value minus 4, so that default(<see cref="TimeSignature"/>) represents 4.
        /// </summary>
        private readonly int _numeratorMinus4;
        /// <summary>
        /// The denominator is held at its actual value minus 4, so that default(<see cref="TimeSignature"/>) represents 4.
        /// </summary>
        private readonly int _denominatorMinus4;

        public int Numerator => _numeratorMinus4 + 4;
        public int Denominator => _denominatorMinus4 + 4;

        public TimeSignature(int numerator, int denominator)
        {
            if (numerator < 1) throw new ArgumentOutOfRangeException(nameof(numerator), "Numerator must be at least 1.");
            if (denominator < 1) throw new ArgumentOutOfRangeException(nameof(denominator), "Denominator must be at least 1.");
            _numeratorMinus4 = numerator - 4;
            _denominatorMinus4 = denominator - 4;
        }

        public TimeSignature(Rational value) : this((int)value.Numerator, (int)value.Denominator) { }

        public void Dump(BinaryWriter writer)
        {
            writer.Write((ushort)Numerator);
            writer.Write((ushort)Denominator);
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
        public static implicit operator (ushort, ushort)(TimeSignature obj) => ((ushort)obj.Numerator, (ushort)obj.Denominator);
        public static implicit operator TimeSignature((int, int) obj) => new(obj.Item1, obj.Item2);
        public static implicit operator TimeSignature((ushort, ushort) obj) => new(obj.Item1, obj.Item2);
        public static implicit operator Rational(TimeSignature obj) => obj.ToRational();
        public static explicit operator TimeSignature(Rational obj) => new(obj);
    }
}
