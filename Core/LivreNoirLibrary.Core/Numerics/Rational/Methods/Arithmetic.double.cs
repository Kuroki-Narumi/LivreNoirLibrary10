using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational :
        IAdditionOperators<Rational, double, double>,
        ISubtractionOperators<Rational, double, double>,
        IMultiplyOperators<Rational, double, double>,
        IDivisionOperators<Rational, double, double>,
        IModulusOperators<Rational, double, double>
    {
        public static double operator +(Rational left, double right) => (double)left + right;
        public static double operator -(Rational left, double right) => (double)left - right;
        public static double operator *(Rational left, double right) => (double)left * right;
        public static double operator /(Rational left, double right) => (double)left / right;
        public static double operator %(Rational left, double right) => (double)left % right;

        /// <inheritdoc cref="operator+(Rational, Rational)"/>
        public static double operator +(double left, Rational right) => left + (double)right;
        /// <inheritdoc cref="operator-(Rational, Rational)"/>
        public static double operator -(double left, Rational right) => left - (double)right;
        /// <inheritdoc cref="operator*(Rational, Rational)"/>
        public static double operator *(double left, Rational right) => left * (double)right;
        /// <inheritdoc cref="operator/(Rational, Rational)"/>
        public static double operator /(double left, Rational right) => left / (double)right;
        /// <inheritdoc cref="operator%(Rational, Rational)"/>
        public static double operator %(double left, Rational right) => left % (double)right;
    }
}
