using System;

namespace LivreNoirLibrary.ObjectModel
{
    public interface ISelectableObject
    {
        bool IsSelected { get; set; }
    }

    public partial class SelectableObject : ObservableObjectBase, ISelectableObject
    {
        public event EventHandler<bool>? IsSelectedChanged;

        public bool IsSelected
        {
            get; 
            set
            {
                if (SetValue(ref field, value))
                {
                    IsSelectedChanged?.Invoke(this, value);
                }
            }
        }
    }
}
