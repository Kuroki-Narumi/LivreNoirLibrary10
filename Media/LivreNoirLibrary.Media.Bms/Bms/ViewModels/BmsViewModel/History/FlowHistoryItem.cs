using System;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public readonly record struct FlowHistoryItem(bool IsExpanded, bool IsSelected, bool IsFocused)
    {
        public FlowHistoryItem Update(bool expanded, bool selected) => new(expanded, selected, IsFocused);
    }
}
