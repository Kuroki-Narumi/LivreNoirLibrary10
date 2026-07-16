using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.YuGiOh;
using LivreNoirLibrary.Windows.YuGiOh.Controls;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// RegulationEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class RegulationEditor : RegulationEditor_Base, ICardSort, ICardSearch, IDragDrop
    {
        static MainViewModel ViewModel => MainViewModel.Instance;

        private readonly List<Card> _mainSelectedItems = [];
        private readonly Dictionary<int, (IListView, List<Card>)> _listMap;
        private IListView? _currentListView;
        private bool _selectionChanging;

        protected override IListView[] ListViews { get; }

        ListBox ICardListView.CardListBox => ListView_CardList;
        CardSearchConditions ICardSearch.CardSearchConditions => ViewModel.Database.CardSearchConditions;
        CardSearchConditions ICardSearch.DefaultCardSearchConditions => CardSearchConditions.Default;
        CardSortOptionCollection ICardSort.CardSortOptions => ViewModel.Database.CardSortOptions;

        WeakReference<object> IDragDrop.DragSource { get; } = new(null!);
        Point IDragDrop.DragStartPoint { get; set; }

        public RegulationEditor()
        {
            _listMap = new()
            {
                { LimitCount.Forbidden, (ListView_Forbidden, []) },
                { LimitCount.Limit1, (ListView_Limit1, []) },
                { LimitCount.Limit2, (ListView_Limit2, []) },
                { LimitCount.Specified, (ListView_Specified, []) },
            };
            ListViews = [ListView_CardList, ListView_Forbidden, ListView_Limit1, ListView_Limit2, ListView_Specified];
            CardProvider = ViewModel.CardPool.Cards;
            Regulation = ViewModel.Regulation;
            this.RegisterCardSearchCommands();
            this.RegisterCardSortCommands();
            CardClipboard.RegisterCopy(ListView_CardList);
            CardClipboard.RegisterCopy(ListView_Forbidden);
            CardClipboard.RegisterCopy(ListView_Limit1);
            CardClipboard.RegisterCopy(ListView_Limit2);
            CardClipboard.RegisterCopy(ListView_Specified);
        }

        protected override void Initialize()
        {
            InitializeComponent();
        }

        void ICardSearch.SetCardSearchText(string text) => ViewModel.Database.CardSearchText = text;

        public void OnCardSortExecuted(SortDescriptionCollection descriptions)
        {
            Update(ListView_Forbidden, descriptions);
            Update(ListView_Limit1, descriptions);
            Update(ListView_Limit2, descriptions);
            Update(ListView_Specified, descriptions);

            static void Update(ListBox obj, SortDescriptionCollection desc)
            {
                var target = obj.Items.SortDescriptions;
                target.Clear();
                foreach (var item in desc)
                {
                    target.Add(item);
                }
            }
        }

        protected override void ApplyHistory(RegulationHistoryData historyData)
        {
            _selectionChanging = true;
            historyData.ConvertBack(Regulation, ListViews, CardProvider);
            _selectionChanging = false;
        }

        private void OnClick_ClearButton(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (sender is FrameworkElement { Tag : int value })
            {
                BeforeEdit();
                Regulation?.ClearLimit(value);
                this.OnEdit();
            }
        }

        private void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateSelection(sender as IListView);
        private void ListView_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var lv = sender as IListView;
            _currentListView = lv;
            UpdateSelection(lv);
        }

        private static void ApplySelection(IListView lv, System.Collections.IList list)
        {
            lv.SetSelectedItems(list);
            if (list.Count > 0)
            {
                lv.ScrollIntoView(list[0]!);
            }
            list.Clear();
        }

        private void UpdateSelection(IListView? control)
        {
            if (_selectionChanging || control is null)
            {
                return;
            }
            _selectionChanging = true;
            if (control == ListView_CardList)
            {
                var listMap = _listMap;
                foreach (var item in control.SelectedItems)
                {
                    if (item is Card card && listMap.TryGetValue(card.LimitCount, out var state))
                    {
                        state.Item2.Add(card);
                    }
                }
                foreach (var (_, (lv, list)) in listMap)
                {
                    ApplySelection(lv, list);
                }
            }
            else
            {
                var list = _mainSelectedItems;
                var cards = CardProvider;
                foreach (var item in control.SelectedItems)
                {
                    if (Card.TryGetCard(item, cards, out var card))
                    {
                        list.Add(card);
                    }
                }
                ApplySelection(ListView_CardList, list);
            }
            _selectionChanging = false;
        }

        bool IDragDrop.HandleMouseButtonEvent(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers is 0 && sender is ListViewItem { IsSelected: true } item && item.TryGetAncestor<IListView>(out var lv))
            {
                lv.Focus();
                return true;
            }
            return false;
        }

        void IDragDrop.BuildDataObject(DataObject obj, object sender)
        {
            obj.SetData(DataObjectTypes.RegulationDragDrop, _currentListView);
        }

        bool IDragDrop.CanDrop(IDataObject obj) => obj.GetDataPresent(DataObjectTypes.RegulationDragDrop);

        bool IDragDrop.HandleDrop(IDataObject obj, object sender)
        {
            if (sender is IListView to && obj.GetData(DataObjectTypes.RegulationDragDrop) is IListView from)
            {
                MoveItems(from, to);
                return true;
            }
            return false;
        }

        private void ContextMenu_OnClick_Forbidden(object sender, RoutedEventArgs e) => MoveItems(_currentListView, ListView_Forbidden, e);
        private void ContextMenu_OnClick_Limit1(object sender, RoutedEventArgs e) => MoveItems(_currentListView, ListView_Limit1, e);
        private void ContextMenu_OnClick_Limit2(object sender, RoutedEventArgs e) => MoveItems(_currentListView, ListView_Limit2, e);
        private void ContextMenu_OnClick_Unlimited(object sender, RoutedEventArgs e) => MoveItems(_currentListView, null, e);
        private void ContextMenu_OnClick_Specified(object sender, RoutedEventArgs e) => MoveItems(_currentListView, ListView_Specified, e);

        private void MoveItems(IListView? from, IListView? to, RoutedEventArgs? e = null)
        {
            e?.Handled = true;
            if (from is null || from == to)
            {
                return;
            }
            BeforeEdit();
            _selectionChanging = true;
            var items = _mainSelectedItems;
            var cards = CardProvider;
            foreach (var item in from.SelectedItems)
            {
                if (Card.TryGetCard(item, cards, out var card))
                {
                    items.Add(card);
                }
            }

            if (to?.Tag is int count)
            {
                Regulation?.Set(items, count);
                to.SetSelectedItems(items);
            }
            else
            {
                Regulation?.Clear(items);
                to = ListView_CardList;
                to.SetSelectedItems(items);
            }
            if (to.SelectedItems.Count > 0)
            {
                to.ScrollIntoView(to.SelectedItems[0]!);
            }
            to.Focus();
            items.Clear();
            this.OnEdit();
            _selectionChanging = false;
        }
    }
}
