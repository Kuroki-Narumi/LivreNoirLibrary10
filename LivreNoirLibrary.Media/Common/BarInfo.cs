using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media
{
    public readonly record struct BarInfo(int Number, TimeSignature Signature, Rational Head, Rational Length);
    public readonly record struct BarLineInfo(int Number, Rational Position, bool IsHead);
}
