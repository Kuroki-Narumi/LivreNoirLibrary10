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

        public static readonly RoutedEvent RequestSearchEvent = Events.Register<SearchBar, RoutedEventHandler<string>>();

        static SearchBar()
        {
            PropertyUtils.OverrideDefaultStyleKey<SearchBar>();
        }

        public event RoutedEventHandler<string>? RequestSearch { add => AddHandler(RequestSearchEvent, value); remove => RemoveHandler(RequestSearchEvent, value); }

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
                RaiseEvent(new RoutedEventArgs<string>(text, RequestSearchEvent, this));
                e.Handled = true;
            }
        }
    }
}
