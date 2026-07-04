using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.YuGiOh.Search
{
    public abstract class RangeBase(bool isEnabled, bool exclusive) : ObservableObjectBase
    {
        public bool IsEnabled { get; set => SetValue(ref field, value); } = isEnabled;
        public bool Exclusive { get;set => SetValue(ref field, value);  } = exclusive;

        public void CopyFrom(RangeBase other)
        {
            IsEnabled = other.IsEnabled;
            Exclusive = other.Exclusive;
        }
    }
}
