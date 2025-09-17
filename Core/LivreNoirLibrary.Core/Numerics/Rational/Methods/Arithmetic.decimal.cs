using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational : 
        IAdditionOperators<Rational, decimal, decimal>,
        ISubtractionOperators<Rational, decimal, decimal>,
        IMultiplyOperators<Rational, decimal, decimal>,
        IDivisionOperators<Rational, decimal, decimal>,
        IModulusOperators<Rational, decimal, decimal>
    {
        public static decimal operator +(Rational left, decimal right) => (decimal)left + right;
        public static decimal operator -(Rational left, decimal right) => (decimal)left - right;
        public static decimal operator *(Rational left, decimal right) => (decimal)left * right;
        public static decimal operator /(Rational left, decimal right) => (decimal)left / right;
        public static decimal operator %(Rational left, decimal right) => (decimal)left % right;

        /// <inheritdoc cref="operator+(Rational, Rational)"/>
        public static decimal operator +(decimal left, Rational right) => left + (decimal)right;
        /// <inheritdoc cref="operator-(Rational, Rational)"/>
        public static decimal operator -(decimal left, Rational right) => left - (decimal)right;
        /// <inheritdoc cref="operator*(Rational, Rational)"/>
        public static decimal operator *(decimal left, Rational right) => left * (decimal)right;
        /// <inheritdoc cref="operator/(Rational, Rational)"/>
        public static decimal operator /(decimal left, Rational right) => left / (decimal)right;
        /// <inheritdoc cref="operator%(Rational, Rational)"/>
        public static decimal operator %(decimal left, Rational right) => left % (decimal)right;
    }
}
