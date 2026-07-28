using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Search;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class TextSearchBar : SearchBarBase, ITextSearch, ISearchListBox
    {
        [RoutedEvent]
        public partial event RoutedEventHandler<string?> SearchExecuted;

        [DependencyProperty]
        private string? _defaultSearchText;
        [DependencyProperty]
        private TextSearchFlags _defaultTextFlags = TextSearchFlags.Default;

        ListBox? ISearchListBox.SearchListBox => ListBox;
        TextSearchConditions? ITextSearch.TextSearchConditions { get; } = new();

        public TextSearchBar()
        {
            this.RegisterTextSearchCommands();
        }

        void ITextSearch.SetSearchText(string? text) => SearchText = text;
        void ITextSearch.OnTextSearchExecuted() => RaiseEvent(new RoutedEventArgs<string?>(SearchText, SearchExecutedEvent, this));
    }
}
