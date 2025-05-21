using System;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational
    {
        private const string DenominatorMustBeNonZero = "Denominator must be non-zero.";
        private static void ThrowDivideByZeroException(string? message = null) => throw new DivideByZeroException(message);

        private const string TooLargeForRational = "Value was either too large or too small for a Rational.";
        private static void ThrowOverflowException(string message = TooLargeForRational) => throw new OverflowException(message);

        private static void ThrowFormatException(string message = "") => throw new FormatException(message);

        private static int ThrowIncomparableException(object? obj) => throw new ArgumentException($"cannot compare to {(obj is null ? "null" : obj.GetType())}");

    }
}
