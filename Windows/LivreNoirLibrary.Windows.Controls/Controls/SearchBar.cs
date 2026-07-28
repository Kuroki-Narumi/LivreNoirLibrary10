using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class SearchBar : Control
    {
        public const string PART_TextBox = nameof(PART_TextBox);

        static SearchBar()
        {
            PropertyUtils.OverrideDefaultStyleKey<SearchBar>();
        }

        [DependencyProperty]
        private string? _openSortText = "Sort";
        [DependencyProperty]
        private string? _openSearchText = "Search";
        [DependencyProperty]
        private string? _clearText = "Clear";

        [DependencyProperty]
        private string? _searchText;
        [DependencyProperty]
        private int _itemsCount;

        private TextBox? _textBox;

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
                Commands.TextSearch.Execute(text, this);
                e.Handled = true;
            }
        }
    }
}
