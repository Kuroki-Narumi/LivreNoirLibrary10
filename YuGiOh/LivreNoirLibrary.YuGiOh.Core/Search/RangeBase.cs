using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public abstract class RangeBase : ObservableObjectBase
    {
        public bool IsEnabled { get; set => SetValue(ref field, value); }
        public bool Exclusive { get;set => SetValue(ref field, value);  }
    }
}
