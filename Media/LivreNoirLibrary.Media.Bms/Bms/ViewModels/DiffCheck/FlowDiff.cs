using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    using static IDifference;

    public class FlowDiff : DiffBase<string?>
    {
        public required FlowAddress Address { get; init; }
        public SortedDictionary<int, BranchDiff> Branches { get; } = [];

        public string GetChangeText() => $"{GetSymbol(DiffType)} {(
            OldValue is null ? $"{NewValue} (added)" :
            NewValue is null ? $"{OldValue} (removed)" :
            OldValue == NewValue ? $"{OldValue}" :
            $"{OldValue} -> {NewValue}"
            )}";
    }
}
