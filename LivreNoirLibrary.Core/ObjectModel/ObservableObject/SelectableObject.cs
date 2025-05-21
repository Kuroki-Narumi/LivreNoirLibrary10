using System;

namespace LivreNoirLibrary.ObjectModel
{
    public interface ISelectableObject
    {
        bool IsSelected { get; set; }
    }

    public partial class SelectableObject : ObservableObjectBase, ISelectableObject
    {
        public event BooleanPropertyChangedEventHandler? IsSelectedChanged;

        [ObservableProperty]
        protected bool _isSelected;

        protected virtual void OnIsSelectedChanged(bool value)
        {
            IsSelectedChanged?.Invoke(this, value);
        }
    }
}
