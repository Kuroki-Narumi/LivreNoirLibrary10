using LivreNoirLibrary.Windows.YuGiOh;
using LivreNoirLibrary.Windows.YuGiOh.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// CardListWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class CardListWindow : Window, ICardSearch, ICardSort
    {
        private static int _globalCount;

        private CardListWindowViewModel ViewModel { get; }

        private readonly Window? _owner;
        private bool _closing;

        public CardListWindow(Window? owner, CardDataCollection cards)
        {
            ViewModel = new(cards);
            _owner = owner;
            Icon = owner?.Icon;
            DataContext = ViewModel;
            InitializeComponent();
            owner?.Closing += OnClosing_Owner;

            _globalCount++;
            SetBinding(TitleProperty, new Binding(nameof(ViewModel.CardSearchText))
            {
                Mode = BindingMode.OneWay,
                Converter = new TitleConverter(_globalCount),
            });
            this.RegisterCardSearchCommands();
            this.RegisterCardSortCommands();
        }

        ICardProvider? ICardSearch.CardProvider => ViewModel.CardProvider;
        CardSearchConditions ICardSearch.CardSearchConditions => ViewModel.CardSearchConditions;
        CardSearchConditions ICardSearch.DefaultCardSearchConditions => CardSearchConditions.Default;
        ListBox ICardListView.CardListBox => ListView_CardList;
        CardSortOptionCollection ICardSort.CardSortOptions => ViewModel.CardSortOptions;

        void ICardSearch.SetCardSearchText(string text) => ViewModel.CardSearchText = text;

        private void CardInfoView_CardLinkClicked(object sender, CardLinkClickedEventArgs e)
        {
            e.Handled = true;
            this.OpenUrl_Card(e.Id, e.IsTcg);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _closing = true;
            _owner?.Closing -= OnClosing_Owner;
            base.OnClosing(e);
        }

        private void OnClosing_Owner(object? sender, CancelEventArgs e)
        {
            if (!_closing)
            {
                Close();
            }
        }

        private class TitleConverter(int index) : IValueConverter
        {
            private readonly int _index = index;

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value is string text && !string.IsNullOrEmpty(text) ? $"YuGiOh CardList({text})" : $"YuGiOh CardList({_index})";
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => value;
        }
    }
}
