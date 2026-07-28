using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class CardSearchBar : SearchBarBase, ICardSearch, ICardSort, IGridViewSort
    {
        [RoutedEvent]
        public partial event RoutedEventHandler? SearchExecuted;
        [RoutedEvent]
        public partial event RoutedEventHandler<SortDescriptionCollection>? SortExecuted;

        [DependencyProperty]
        private ICardProvider? _cardProvider;
        [DependencyProperty]
        private bool _canOpenSortDialog = true;
        [DependencyProperty]
        private CardSortOptionCollection? _defaultSortOptions;
        [DependencyProperty]
        private bool _canOpenSearchDialog = true;
        [DependencyProperty]
        private CardSearchConditions? _defaultSearchConditions;

        public CardSortOptionCollection SortOptions { get; } = [];
        public CardSearchConditions SearchConditions { get; } = new();

        CardSortOptionCollection? ICardSort.CardSortOptions => SortOptions;
        CardSortOptionCollection? ICardSort.DefaultCardSortOptions => DefaultSortOptions;
        CardSearchConditions? ICardSearch.CardSearchConditions => SearchConditions;
        CardSearchConditions? ICardSearch.DefaultCardSearchConditions => DefaultSearchConditions;

        private readonly RoutedEventHandler _clickHandler;
        private string? _currentSort;
        private bool _currentAscending;

        public CardSearchBar()
        {
            this.RegisterCardSearchCommands();
            this.RegisterCardSortCommands();
            _clickHandler = new(OnClick_GridViewColumn);
        }

        private void OnDefaultSearchConditionsChanged()
        {
            this.ClearCardFilter();
        }

        private void OnDefaultSortOptionsChanged()
        {
            this.ClearCardSort();
        }

        void ICardSearch.OnCardSearchExecuted() => RaiseSearchExecuted();
        void ICardSort.OnCardSortExecuted(SortDescriptionCollection descriptions) => RaiseSortExecuted(descriptions);

        private void RaiseSearchExecuted() => RaiseEvent(new RoutedEventArgs(SearchExecutedEvent, this));
        private void RaiseSortExecuted(SortDescriptionCollection descriptions) => RaiseEvent(new RoutedEventArgs<SortDescriptionCollection>(descriptions, SortExecutedEvent, this));

        void ICardSearch.SetCardSearchText(string? text) => SearchText = text;
        void IGridViewSort.SortBy(ListBox control, string key)
        {
            control.UpdateSort(key, ref _currentSort, ref _currentAscending);
            RaiseSortExecuted(control.Items.SortDescriptions);
        }

        protected override void InsulateListBox(ListBox listBox)
        {
            base.InsulateListBox(listBox);
            listBox.RemoveHandler(GridViewColumnHeader.ClickEvent, _clickHandler);

        }

        protected override void BindListBox(ListBox listBox)
        {
            base.BindListBox(listBox);
            listBox.AddHandler(GridViewColumnHeader.ClickEvent, _clickHandler);
            this.UpdateCardFilter();
            this.UpdateCardSort();
        }

        private void OnClick_GridViewColumn(object sender, RoutedEventArgs e) => this.OnClick_ColumnHeader(sender, e);
    }
}
