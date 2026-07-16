using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Search;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class SortOptionViewModel(string header) : ObservableObjectBase, IClear
    {
        public string? Header { get; set => SetValue(ref field, value); } = header;
        public SortSelectionItem SourceItem { get; set => SetValue(ref field, value); } = SortSelectionItem.None;
        public bool IsAscending { get; set => SetValue(ref field, value);  }
        public bool IsDescending { get; set => SetValue(ref field, value); }

        public void Clear()
        {
            SourceItem = SortSelectionItem.None;
            IsAscending = true;
            IsDescending = false;
        }

        public void CopyFrom(CardSortOption option)
        {
            SourceItem = SortSelectionItem.GetSelectionItem(option.Key) ?? SortSelectionItem.None;
            IsDescending = option.IsDescending;
            IsAscending = !IsDescending;
        }

        public CardSortOption GetOption() => new(SourceItem.Key, IsDescending);

        public override string ToString() => $"({SourceItem}, {IsAscending})";
    }
}
