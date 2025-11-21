using LivreNoirLibrary.Collections;
using System;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public abstract class FlowViewModel<TModel, TChildType> : FlowViewModel
        where TModel : INoteObject
        where TChildType : FlowViewModel
    {
        public TChildType? Parent { get; private set; }
        public TModel Model { get; private set; }
        public string? Note { get; set => SetValue(ref field, value, OnNoteChanged); }
        public ObservableList<TChildType> Children { get; } = [];

        public FlowViewModel(IBmsData root, TChildType? parent, FlowAddress address, TModel model) : base(address)
        {
            Parent = parent;
            Model = model;
            RefreshChildren(root);
        }

        protected abstract void RefreshChildren(IBmsData root);

        private void OnNoteChanged(string? oldValue, string? newValue)
        {
            Model.Note = newValue;
        }

        internal override void OnDelete(IBmsData root)
        {
            Parent = null!;
            Model = default!;
            foreach (var child in Children.AsSpan())
            {
                child.OnDelete(root);
            }
        }
    }
}
