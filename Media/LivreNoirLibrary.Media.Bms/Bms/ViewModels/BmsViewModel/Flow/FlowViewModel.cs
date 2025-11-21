using System;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public abstract class FlowViewModel(FlowAddress address) : ObservableObjectBase
    {
        public abstract string? Name { get; }
        public bool IsFocused { get; set => SetValue(ref field, value); }
        public bool IsExpanded { get; set => SetValue(ref field, value); }
        public bool IsSelected { get; set => SetValue(ref field, value); }
        public FlowAddress Address { get; private set; } = address;

        internal void UpdateAddress(FlowAddress address)
        {
            Address = address;
            UpdateChildrenAddress();
        }
        internal abstract void UpdateChildrenAddress();
        internal abstract void OnDelete(IBmsData root);
    }
}