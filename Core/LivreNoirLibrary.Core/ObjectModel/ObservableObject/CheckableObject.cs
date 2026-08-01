using System;
using System.Numerics;

namespace LivreNoirLibrary.ObjectModel
{
    public interface ICheckableObject
    {
        bool IsChecked { get; set; }
    }

    public interface INotifyIsCheckedChanged : ICheckableObject
    {
        event EventHandler<bool>? IsCheckedChanged;
    }

    public partial class CheckableObject : ObservableObjectBase, INotifyIsCheckedChanged
    {
        public event EventHandler<bool>? IsCheckedChanged;

        public bool IsChecked
        {
            get;
            set
            {
                if (SetValue(ref field, value, OnIsCheckedChanged))
                {
                    IsCheckedChanged?.Invoke(this, value);
                }
            }
        }

        protected virtual void OnIsCheckedChanged(bool oldValue, bool newValue) { }
    }
}
