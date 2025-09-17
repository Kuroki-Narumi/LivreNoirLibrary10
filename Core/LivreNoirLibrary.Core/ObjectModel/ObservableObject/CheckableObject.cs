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
        public event EventHandler<bool>? IsCheckedChanged;

        public bool IsChecked
        {
            get;
            set
            {
                if (SetValue(ref field, value))
                {
                    IsCheckedChanged?.Invoke(this, value);
                }
            }
        }
    }
}
