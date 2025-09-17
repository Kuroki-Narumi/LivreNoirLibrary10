using System;
using System.Numerics;

namespace LivreNoirLibrary.Numerics
{
    public readonly partial struct Rational :
        IAdditionOperators<Rational, float, float>,
        ISubtractionOperators<Rational, float, float>,
        IMultiplyOperators<Rational, float, float>,
        IDivisionOperators<Rational, float, float>,
        IModulusOperators<Rational, float, float>
    {
        public static float operator +(Rational left, float right) => (float)left + right;
        public static float operator -(Rational left, float right) => (float)left - right;
        public static float operator *(Rational left, float right) => (float)left * right;
        public static float operator /(Rational left, float right) => (float)left / right;
        public static float operator %(Rational left, float right) => (float)left % right;

        /// <inheritdoc cref="operator+(Rational, Rational)"/>
        public static float operator +(float left, Rational right) => left + (float)right;
        /// <inheritdoc cref="operator-(Rational, Rational)"/>
        public static float operator -(float left, Rational right) => left - (float)right;
        /// <inheritdoc cref="operator*(Rational, Rational)"/>
        public static float operator *(float left, Rational right) => left * (float)right;
        /// <inheritdoc cref="operator/(Rational, Rational)"/>
        public static float operator /(float left, Rational right) => left / (float)right;
        /// <inheritdoc cref="operator%(Rational, Rational)"/>
        public static float operator %(float left, Rational right) => left % (float)right;
    }
}
