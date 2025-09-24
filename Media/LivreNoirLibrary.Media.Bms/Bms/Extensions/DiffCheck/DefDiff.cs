using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    using static IDifference;

    public class BarDefDiff : DiffBase<Rational>
    {
        public string GetChangeText(int number) => $"{GetSymbol(DiffType.Changed)} #{number:D3}: {OldValue} -> {NewValue}";
    }

    public class DefDiff : DiffBase<string?>
    {
        public string GetChangeText(string index) => $"{GetSymbol(DiffType)} {index}: {(
            OldValue is null ? $"{NewValue} (added)" :
            NewValue is null ? $"{OldValue} (removed)" :
            $"{OldValue} -> {NewValue}"
            )}";
    }
}
