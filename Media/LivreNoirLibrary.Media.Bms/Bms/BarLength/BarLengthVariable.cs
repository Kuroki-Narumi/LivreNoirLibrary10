using LivreNoirLibrary.Numerics;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class BarLengthVariable
    {
        public const string BarLengthSymbol = "l";
        public const string BarLengthSymbol2 = "Length";
        public const string FirstLengthSymbol = "f";
        public const string FirstLengthSymbol2 = "First";
        public const string MaxCountSymbol = "m";
        public const string MaxCountSymbol2 = "Count";
        public const string IndexSymbol = "i";
        public const string PreviousSymbol = "p";
        public const string PrePreviousSymbol = "q";

        public Rational BarLength { get; private set; } = Rational.One;
        public Rational FirstLength { get; private set; } = new(1, 2);
        public int MaxCount { get; private set; } = 8;
        public int Index { get; set; } = 1;
        public Rational Previous { get; private set; } = Rational.One;
        public Rational PrePrevious { get; private set; } = Rational.One;

        public void Setup(Rational barLength, Rational firstLength, int maxCount)
        {
            BarLength  = barLength;
            FirstLength = firstLength;
            MaxCount = maxCount;
            Previous = firstLength;
            PrePrevious = Rational.Zero;
        }

        public void UpdatePrevious(Rational value)
        {
            PrePrevious = Previous;
            Previous = value;
        }

        public bool TryGetValue(string symbol, out Rational value)
        {
            value = symbol switch
            {
                BarLengthSymbol or BarLengthSymbol2 => BarLength,
                FirstLengthSymbol or FirstLengthSymbol2 => FirstLength,
                MaxCountSymbol or MaxCountSymbol2 => MaxCount,
                IndexSymbol => Index,
                PreviousSymbol => Previous,
                PrePreviousSymbol => PrePrevious,
                _ => Rational.MinusOne,
            };
            return value.IsPositive();
        }
    }
}
