using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using S = LivreNoirLibrary.YuGiOh.Scraping;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// DeckEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class DeckEditor : DeckEditor_Base, IDragDrop, IProgressReporter
    {
        public CardSearchConditions DefaultCardSearchConditions => CardSearchConditions.Usable;

        WeakReference<object> IDragDrop.DragSource { get; } = new(null!);
        Point IDragDrop.DragStartPoint { get; set; }

        UIElement IProgressReporter.MainElement => MainGrid;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get; set; }

        protected override ListBox[] ListViews { get; }

        [DependencyProperty]
        private bool _isSideDeckVisible;

        private readonly List<Card> _selected = [];
        private bool _selectionChanging;

        public DeckEditor()
        {
            MainGrid.DataContext = this;
            ListViews = [ListView_CardList, ListView_MainDeck, ListView_ExtraDeck, ListView_SideDeck];
            CardClipboard.RegisterCopy(ListView_CardList);
            CardClipboard.RegisterCopy(ListView_MainDeck);
            CardClipboard.RegisterCopy(ListView_ExtraDeck);
            CardClipboard.RegisterCopy(ListView_SideDeck);
        }

        protected override void Initialize()
        {
            InitializeComponent();
        }

        protected override void OnCardProviderChanged(ICardProvider? value) => ListView_CardList.SetCloningSource(value);

        protected override void ApplyHistory(DeckHistoryData historyData)
        {
            _selectionChanging = true;
            historyData.ConvertBack(Deck, ListViews, CardProvider);
            _selectionChanging = false;
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.CardListView_PreviewMouseLeftButtonDown_Alt(sender, e);
            this.IDragDrop_PreviewMouseLeftButtonDown(sender, e);
        }

        private void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateSelection(sender as ListBox);
        private void ListView_OnGotFocus(object sender, RoutedEventArgs e)
        {
            var lv = sender as ListBox;
            UpdateSelection(lv);
        }

        private List<Card> BuildSelectedItem(ListBox lv)
        {
            var list = _selected;
            list.Clear();
            var provider = CardProvider;
            foreach (var item in lv.SelectedItems)
            {
                if (Card.TryGetCard(item, provider, out var card))
                {
                    list.Add(card);
                }
            }
            return list;
        }

        private void UpdateSelection(ListBox? lv)
        {
            if (lv is null || _selectionChanging)
            {
                return;
            }
            _selectionChanging = true;
            var list = BuildSelectedItem(lv);
            if (lv == ListView_CardList)
            {
                if (Deck is { } deck)
                {
                    SetSelectedItems(ListView_MainDeck, EnumerateItem(list, deck.MainDeck));
                    SetSelectedItems(ListView_ExtraDeck, EnumerateItem(list, deck.ExtraDeck));
                    SetSelectedItems(ListView_SideDeck, EnumerateItem(list, deck.SideDeck));
                }
            }
            else
            {
                SetSelectedItems(ListView_CardList, list);
            }
            _selectionChanging = false;
        }

        private static void SetSelectedItems(ListView lv, System.Collections.IEnumerable enumer)
        {
            lv.SetSelectedItems(enumer);
            if (lv.SelectedItems.Count > 0)
            {
                lv.ScrollIntoView(lv.SelectedItems[0]!);
            }
        }

        private static IEnumerable<CountedCard> EnumerateItem(List<Card> list, DeckCardList deck)
        {
            foreach (var card in list)
            {
                if (deck.TryGetItem(card, out var item))
                {
                    yield return item;
                }
            }
        }

        private void OnClick_Import1(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            try
            {
                if (Clipboard.GetText() is { } url && url.StartsWith(S.Deck.DeckUrl))
                {
                    Console.WriteLine(url);
                    TextBox_DeckUrl.Text = url;
                }
            }
            catch { }
            Area_Import1.Open();
        }

        private void ImportText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
            {
                e.Handled = true;
                Area_Import1.Close();
                ExecuteImport();
            }
        }

        private void ImportText_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (TextBox_DeckUrl.SelectedText != TextBox_DeckUrl.Text)
            {
                TextBox_DeckUrl.SelectAll();
            }
        }

        private void OnClick_Import1Button(object sender, RoutedEventArgs e)
        {
            ExecuteImport();
        }

        private void ExecuteImport()
        {
            Area_Import1.Close();
            var url = TextBox_DeckUrl.Text;
            if (!url.StartsWith(S.Deck.DeckUrl))
            {
                return;
            }
            this.StartTask(asyncProcess: ProcessImport, isAbortable: false);
        }

        private async Task ProcessImport(ProgressReporter p, CancellationToken c)
        {
            if (Deck is { } target)
            {
                LivreNoirLibrary.YuGiOh.Serializable.Deck deck = new();
                await S.Deck.Get(TextBox_DeckUrl.Text, deck, p, c);
                BeforeEdit();
                target.Load(deck, CardProvider);
                this.OnEdit();
            }
        }

        private void OnClick_Import2(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            try
            {
                Clipboard.SetText(S.Deck.JS_CopyDeck);
                Area_Import2.Open();
            }
            catch { }
        }

        private void OnClick_Import2Button(object sender, RoutedEventArgs e)
        {
            Area_Import2.Close();
            if (Deck is { } deck)
            {
                try
                {
                    var text = Clipboard.GetText();
                    if (Json.TryParse<LivreNoirLibrary.YuGiOh.Serializable.Deck>(text, out var source))
                    {
                        BeforeEdit();
                        deck.Load(source, CardProvider);
                        this.OnEdit();
                    }
                }
                catch { }
            }
        }

        private void OnClick_Export(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (Deck is { } deck)
            {
                var text = S.Deck.CreateDeckBuildText(deck);
                try
                {
                    Clipboard.SetText(text);
                    Area_Export.Open();
                }
                catch { }
            }
        }

        private void OnClick_ExportButton(object sender, RoutedEventArgs e)
        {
            Area_Export.Close();
        }

        private void OnClick_Add(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ProcessAdd(ListView_CardList);
        }

        private void OnClick_Remove(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ProcessRemove(ListView_CardList);
        }

        private object? _rightClickObject;
        private void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _rightClickObject = sender;
            if (sender is ListViewItem { DataContext: ICard card, IsSelected: false} f && f.TryGetAncestor<ListBox>(out var lv))
            {
                lv.SelectedItems.Clear();
                lv.SelectedItems.Add(card);
            }
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_rightClickObject == sender && (sender as DependencyObject).TryGetAncestor<ListBox>(out var lv))
            {
                if (lv == ListView_CardList)
                {
                    ProcessAdd(lv);
                }
                else
                {
                    ProcessRemove(lv);
                }
            }
            _rightClickObject = null;
        }

        private void ProcessImpl(ListBox lv, Deck deck, Action<Card, bool, bool> action)
        {
            _selectionChanging = true;
            var list = BuildSelectedItem(lv);
            BeforeEdit();
            var provider = CardProvider;
            var max = KeyInput.IsShiftDown();
            var toSide = IsSideDeckVisible;
            foreach (var item in _selected)
            {
                if (Card.TryGetCard(item, provider, out var card))
                {
                    action(card, max, toSide);
                }
            }
            list.Clear();
            deck.NotifyCollectionReset();
            this.OnEdit();
            _selectionChanging = false;
            UpdateSelection(ListView_CardList);
        }

        private void ProcessAdd(ListBox lv)
        {
            if (Deck is { } deck)
            {
                ProcessImpl(lv, deck, deck.AddWithoutNotify);
            }
        }

        private void ProcessRemove(ListBox lv)
        {
            if (Deck is { } deck)
            {
                ProcessImpl(lv, deck, deck.RemoveWithoutNotify);
            }
        }

        bool IDragDrop.HandleMouseButtonEvent(object sender, MouseButtonEventArgs e) => IDragDropExtensions.HandleMouseButton_ListViewItem(sender, e);

        void IDragDrop.BuildDataObject(DataObject obj, object sender) => IDragDropExtensions.BuildDataObject_ListView(DataObjectTypes.CardDragDrop, obj, sender);

        bool IDragDrop.CanDrop(IDataObject obj) => obj.GetDataPresent(DataObjectTypes.CardDragDrop);

        bool IDragDrop.HandleDrop(IDataObject obj, object sender)
        {
            if (sender is ListBox to && obj.GetData(DataObjectTypes.CardDragDrop) is ListBox from)
            {
                if (from == ListView_CardList)
                {
                    if (to != ListView_CardList)
                    {
                        ProcessAdd(from);
                    }
                }
                else if (to == ListView_CardList)
                {
                    ProcessRemove(from);
                }
                return true;
            }
            return false;
        }

        private void OnClick_ClearButton(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (sender is FrameworkElement { Tag: DeckCardList list })
            {
                BeforeEdit();
                list.Clear();
                this.OnEdit();
            }
        }

        public void AddCard(Card card, bool max = false, bool toSideDeck = false)
        {
            BeforeEdit();
            Deck?.Add(card, max, toSideDeck);
            this.OnEdit();
        }

        public void RemoveCard(Card card, bool max = false, bool fromSideDeck = false)
        {
            BeforeEdit();
            Deck?.Remove(card, max, fromSideDeck);
            this.OnEdit();
        }
    }
}
