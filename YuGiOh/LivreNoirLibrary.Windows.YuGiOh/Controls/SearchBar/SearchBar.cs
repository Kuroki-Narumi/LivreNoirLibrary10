using LivreNoirLibrary.Windows.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class SearchBar : Control
    {
        public const string PART_TextBox = nameof(PART_TextBox);

        static SearchBar()
        {
            PropertyUtils.OverrideDefaultStyleKey<SearchBar>();
        }

        [DependencyProperty]
        private string? _searchText;
        [DependencyProperty]
        private int _itemsCount;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _canSort;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _canSearch;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _canClear;

        private TextBox? _textBox;

        public SearchBar()
        {
            this.RegisterCommand(SearchCommands.Sort, Executed_Sort);
            this.RegisterCommand(SearchCommands.Search, Executed_Search);
            this.RegisterCommand(SearchCommands.Clear, Executed_Clear);
        }

        public override void OnApplyTemplate()
        {
            _textBox?.KeyDown -= TextBox_KeyDown;

            base.OnApplyTemplate();

            _textBox = GetTemplateChild(PART_TextBox) as TextBox;
            _textBox?.KeyDown += TextBox_KeyDown;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox { Text: { } text } && e.Key is Key.Enter)
            {
                RaiseEvent(new RoutedEventArgs<string>(text, RequestSearchEvent, this));
                e.Handled = true;
            }
        }

        private void Executed_Sort(object sender, ExecutedRoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(RequestOpenSortEvent, this));
            e.Handled = true;
        }

        private void Executed_Search(object sender, ExecutedRoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(RequestOpenSearchEvent, this));
            e.Handled = true;
        }

        private void Executed_Clear(object sender, ExecutedRoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(RequestClearEvent, this));
            e.Handled = true;
        }
    }
}
