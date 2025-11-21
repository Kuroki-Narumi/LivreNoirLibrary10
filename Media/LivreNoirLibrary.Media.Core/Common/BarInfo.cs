using System;

namespace LivreNoirLibrary.Media
{
    public readonly record struct BarInfo<T>(int Number, TimeSignature Signature, T Head, T Length);
    public readonly record struct BarLineInfo<T>(int Number, T Position, bool IsHead);
}
