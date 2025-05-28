using System;
using System.Numerics;

namespace LivreNoirLibrary.ObjectModel
{
    public interface ICheckableObject
    {
        bool IsChecked { get; set; }
    }

    public partial class CheckableObject : ObservableObjectBase, ICheckableObject
    {
        public event ValueChangedEventHandler<bool>? IsCheckedChanged;

        [ObservableProperty]
        private bool _isChecked = false;

        protected virtual void OnIsCheckedChanged(bool value)
        {
            IsCheckedChanged?.Invoke(this, value);
        }
    }
}
