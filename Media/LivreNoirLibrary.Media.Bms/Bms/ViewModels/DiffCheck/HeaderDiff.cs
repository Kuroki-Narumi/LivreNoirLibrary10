using System;

namespace LivreNoirLibrary.Media.Bms
{
    using static IDifference;

    public class HeaderDiff : DiffBase<string?>
    {
        public required string Key { get; init; }
        public string GetChangeText() => $"{GetSymbol(DiffType)} #{Key}: {(
            OldValue is null ? $"{NewValue} (added)" :
            NewValue is null ? $"{OldValue} (removed)" :
            $"{OldValue} -> {NewValue}"
            )}";
    }
}
