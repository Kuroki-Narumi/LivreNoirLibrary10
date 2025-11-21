using System;

namespace LivreNoirLibrary.Media.Bms
{
    using static IDifference;

    public sealed class BranchDiff : DiffBase<string?>
    {
        public required FlowAddress Address { get; init; }
        public DiffResultBase? DataDifference { get; set; }

        public string GetChangeText() => $"{GetSymbol(DiffType)} {(
            OldValue is null ? $"{NewValue} (added)" :
            NewValue is null ? $"{OldValue} (removed)" :
            OldValue == NewValue ? $"{OldValue}" :
            $"{OldValue} -> {NewValue}"
            )}";
    }
}
