using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.MasterDuel;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// DuelLogEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class DuelLogEditor : DuelLogEditor_Base, IGridViewSort, IDragDrop
    {
        public static CardSearchConditions DefaultCardSearchConditions { get; } = new(CardSearchConditions.Usable)
        {
            CardTypes = [
                CardType.Main_Monster, CardType.Ritual_Monster,
                CardType.Normal_Spell, CardType.Field_Spell, CardType.Equip_Spell, CardType.Continuous_Spell, CardType.Quick_Spell, CardType.Ritual_Spell,
                CardType.Normal_Trap, CardType.Continuous_Trap, CardType.Counter_Trap,
            ],
        };

        protected override ListBox[] ListViews { get; }

        public DuelLog EditingLog { get; } = new();
        public CardList InitialHandList { get; } = [];
        public CardList AdditionalHandList { get; } = [];

        WeakReference<object> IDragDrop.DragSource { get; } = new(null!);
        Point IDragDrop.DragStartPoint { get; set; }

        [DependencyProperty]
        private ICardProvider? _cardProvider;
        [DependencyProperty]
        private DeckCardList? _deckCards;
        [DependencyProperty]
        private DeckTagCollection? _deckTagSource;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _canAddCard;

        private string? _currentProp = nameof(DuelLog.DateTime);
        private bool _currentAscending = true;
        private DuelLog? _selectedItem;
        private ListBox? _activeCardSource;

        public DuelLogEditor()
        {
            ListViews = [ListView_Main];
            MainGrid.DataContext = this;
            (this as IGridViewSort).SortBy(ListView_Main, nameof(DuelLog.DateTime));
            ListView_Main.RegisterCommand(ApplicationCommands.Delete, OnExecuted_Delete, ListView_Main.CanExecute_Item);
            InfoView.RegisterCommand(ApplicationCommands.New, OnExecuted_LogClear);
        }

        protected override void Initialize()
        {
            InitializeComponent();
        }

        private void OnCardProviderChanged(ICardProvider? value) => ListView_AllCards.SetCloningSource(value);

        protected override void ApplyHistory(DuelLogHistoryData historyData)
        {
            historyData.ConvertBack(ItemsSource);
            historyData.RestoreSelection(ListViews);
        }

        private void OnClick_ColumnHeader(object sender, RoutedEventArgs e) => ControlExtensions.OnClick_ColumnHeader(this, sender, e);

        void IGridViewSort.SortBy(ListBox control, string key) => SortDuelLogGridView(control, key, ref _currentProp, ref _currentAscending);

        public static void SortDuelLogGridView(ListBox control, string key, ref string? currentProp, ref bool currentAscending)
        {
            var desc = control.Items.SortDescriptions;
            desc.Clear();
            var isDescending = key == currentProp && currentAscending;
            var dir = isDescending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            currentProp = key;
            currentAscending = !isDescending;
            desc.Add(new(key, dir));
            control.ScrollSelectedItemIntoView();
        }

        private void OnExecuted_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            if (ItemsSource is { } items && ListView_Main.SelectedItem is DuelLog item)
            {
                e.Handled = true;
                BeforeEdit();
                items.Remove(item);
                this.OnEdit();
            }
        }

        private void OnClick_Add(object sender, RoutedEventArgs e)
        {
            if (ItemsSource is { } items)
            {
                e.Handled = true;
                DuelLog item = new();
                SaveEditingLog(item);
                item.DateTime = DateTime.Now;
                BeforeEdit();
                items.Add(item);
                ListView_Main.SelectedItem = item;
                ListView_Main.ScrollSelectedItemIntoView();
                this.OnEdit();
            }
        }

        private void OnClick_Save(object sender, RoutedEventArgs e)
        {
            if (ListView_Main.SelectedItem is DuelLog item)
            {
                e.Handled = true;
                BeforeEdit();
                SaveEditingLog(item);
                this.OnEdit();
            }
            else
            {
                OnClick_Add(sender, e);
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItem = ListView_Main.SelectedItem as DuelLog;
            UpdateEditingLog(_selectedItem);
        }

        private void OnClick_Load(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var selected = _selectedItem;
            ListView_Main.SelectedItem = null;
            ListView_Main.SelectedItem = selected;
        }

        public void UpdateEditingLog(DuelLog? value)
        {
            if (value is not null)
            {
                EditingLog.CopyFrom(value);
                UserSelector.SetFlags(value.UserTags);
                OpponentSelector.SetFlags(value.OpponentTags);
                InitialHandList.Load(value.InitialHand, CardProvider);
                AdditionalHandList.Load(value.AdditionalHand, CardProvider);
            }
        }

        private void SaveEditingLog(DuelLog target)
        {
            var log = EditingLog;
            log.InitialHand = (InitialHandList as IIdEnumerable).IdEnumerable;
            log.AdditionalHand = (AdditionalHandList as IIdEnumerable).IdEnumerable;
            target.CopyFrom(log);
        }

        private void OnExecuted_LogClear(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            OpponentSelector.ClearFlags();
            var log = EditingLog;
            log.DateTime = DateTime.Now;
            log.Order = Order.First;
            log.Result = Result.Lose;
            log.Turn = 0;
            InitialHandList.Clear();
            AdditionalHandList.Clear();
            log.Note = "";
        }

        private void OnMouseWheel_ComboBox(object sender, MouseWheelEventArgs e) => (sender as ComboBox)?.ChangeByWheel(e);

        private void User_TagChanged(object sender, RoutedEventArgs<IEnumerable<string>> e)
        {
            EditingLog.UserTags = e.Value;
        }

        private void Opponent_TagChanged(object sender, RoutedEventArgs<IEnumerable<string>> e)
        {
            EditingLog.OpponentTags = e.Value;
        }

        private void CardSource_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateActiveCardSource();
        private void CardSource_GotFocus(object sender, RoutedEventArgs e) => UpdateActiveCardSource();
        private void UpdateActiveCardSource()
        {
            _activeCardSource = TabItem_Deck.IsSelected ? ListView_DeckCards : ListView_AllCards;
            CanAddCard = _activeCardSource.SelectedItems.Count > 0;
        }

        private void OnClick_Hand_Clear(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            InitialHandList.Clear();
            AdditionalHandList.Clear();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.CardListView_PreviewMouseLeftButtonDown_Alt(sender, e);
            this.IDragDrop_PreviewMouseLeftButtonDown(sender, e);
        }

        bool IDragDrop.HandleMouseButtonEvent(object sender, MouseButtonEventArgs e) => IDragDropExtensions.HandleMouseButton_ListViewItem(sender, e);

        void IDragDrop.BuildDataObject(DataObject obj, object sender) => IDragDropExtensions.BuildDataObject_ListView(DataObjectTypes.CardDragDrop, obj, sender);

        bool IDragDrop.CanDrop(IDataObject obj) => obj.GetDataPresent(DataObjectTypes.CardDragDrop);

        bool IDragDrop.HandleDrop(IDataObject obj, object sender)
        {
            if (sender is ListBox to && obj.GetData(DataObjectTypes.CardDragDrop) is ListBox from && to != from)
            {
                if (to == ListView_InitialHand || to == ListView_AdditionalHand)
                {
                    AddCards(to, from);
                }
                if (from == ListView_InitialHand || from == ListView_AdditionalHand)
                {
                    RemoveCards(from);
                }
                return false;
            }
            return false;
        }

        private object? _rightClickObject;
        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _rightClickObject = sender;
            if (sender is ListViewItem { DataContext: ICard card, IsSelected: false } f && f.TryGetAncestor<ListBox>(out var lv))
            {
                lv.SelectedItems.Clear();
                lv.SelectedItems.Add(card);
            }
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_rightClickObject == sender && (sender as DependencyObject).TryGetAncestor<ListBox>(out var lv))
            {
                if (lv == ListView_InitialHand || lv == ListView_AdditionalHand)
                {
                    RemoveCards(lv);
                }
                else
                {
                    AddCards(InitialHandList.Count is >= 5 ? ListView_AdditionalHand : ListView_InitialHand, lv);
                }
            }
            _rightClickObject = null;
        }

        private void OnClick_Initial_Add(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            AddCards(ListView_InitialHand, _activeCardSource);
        }

        private void OnClick_Initial_Remove(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            RemoveCards(ListView_InitialHand);
        }

        private void OnClick_Additional_Add(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            AddCards(ListView_AdditionalHand, _activeCardSource);
        }

        private void OnClick_Additional_Remove(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            RemoveCards(ListView_AdditionalHand);
        }

        private readonly List<Card> _cardBuffer = [];

        private void AddCards(ListBox target, ListBox? source)
        {
            if (target.ItemsSource is not CardList list || source is null)
            {
                return;
            }
            var provider = CardProvider;
            var buffer = _cardBuffer;
            foreach (var item in source.SelectedItems)
            {
                if (Card.TryGetCard(item, provider, out var card))
                {
                    list.Add(card);
                    buffer.Add(card);
                }
            }
            target.SetSelectedItems(buffer);
            target.Focus();
            buffer.Clear();
        }

        private void RemoveCards(ListBox lv)
        {
            if (lv.ItemsSource is not CardList list)
            {
                return;
            }
            var provider = CardProvider;
            var buffer = _cardBuffer;
            foreach (var item in lv.SelectedItems)
            {
                if (Card.TryGetCard(item, provider, out var card))
                {
                    buffer.Add(card);
                }
            }
            list.RemoveRange(buffer);
            buffer.Clear();
        }
    }
}
