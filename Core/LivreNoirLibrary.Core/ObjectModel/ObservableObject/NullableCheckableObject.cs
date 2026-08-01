using System;
using System.Numerics;

namespace LivreNoirLibrary.ObjectModel
{
    public interface INullableCheckableObject
    {
        bool? IsChecked { get; set; }
    }

    public interface INullableNotifyIsCheckedChanged : INullableCheckableObject
    {
        event EventHandler<bool?>? IsCheckedChanged;
    }

    public partial class NullableCheckableObject : ObservableObjectBase, INullableNotifyIsCheckedChanged
    {
        public event EventHandler<bool?>? IsCheckedChanged;

        public bool? IsChecked
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

        protected virtual void OnIsCheckedChanged(bool? oldValue, bool? newValue) { }
    }
}
