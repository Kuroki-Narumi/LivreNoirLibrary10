using System;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public readonly struct FlowHistoryData
    {
        public readonly bool IsExpanded;
        public readonly bool IsSelected;
        public readonly bool IsFocused;

        internal FlowHistoryData(bool expanded, bool selected, bool focused)
        {
            IsExpanded = expanded;
            IsSelected = selected;
            IsFocused = focused;
        }

        public FlowHistoryData(FlowViewModel vm) : this(vm.IsExpanded, vm.IsSelected, vm.IsFocused) { }

        public FlowHistoryData Update(bool expanded, bool selected) => new(expanded, selected, IsFocused);
    }
}
