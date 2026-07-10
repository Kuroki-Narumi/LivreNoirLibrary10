using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.YuGiOh.Search;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public class SortOptionViewModel(string header) : ObservableObjectBase
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
            IsDescending = option.Direction is SortDirection.Descending;
            IsAscending = !IsDescending;
        }

        public CardSortOption GetOption() => new(SourceItem.Key, IsDescending ? SortDirection.Descending : SortDirection.Ascending);

        public override string ToString() => $"({SourceItem}, {IsAscending})";
    }
}
