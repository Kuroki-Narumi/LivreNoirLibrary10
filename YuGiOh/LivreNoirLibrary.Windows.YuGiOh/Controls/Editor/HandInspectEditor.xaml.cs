using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Inspect;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// HandInspectEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class HandInspectEditor : HandInspectEditor_Base, IDragDrop
    {
        protected override IListView[] ListViews { get; }
        WeakReference<object> IDragDrop.DragSource { get; } = new(null!);
        Point IDragDrop.DragStartPoint { get; set; }

        [DependencyProperty]
        private ICardEnumerable? _cardSource;
        [DependencyProperty]
        private HandConditions? _selectedItem;
        [DependencyProperty(SetterScope = Scope.Private)]
        private HandConditions? _editingItem;
        [DependencyProperty(SetterScope = Scope.Private)]
        private IListView? _lastFocusedListView;

        private readonly List<Card> _selection = [];

        public HandInspectEditor()
        {
            EditingItem = new();
            MainGrid.DataContext = this;
            ListViews = [ListView_Main];

            this.RegisterCommand(Commands.OpenSortDialog, OnExecuted_OpenSortDialog);

            var lv = ListView_Main;
            this.RegisterCommand(Commands.Insert, OnExecuted_ListView_Insert);
            lv.RegisterCommand(Commands.Duplicate, OnExecuted_ListView_Duplicate, lv.CanExecute_Item);
            lv.RegisterCommand(Commands.Delete, OnExecuted_ListView_Delete, lv.CanExecute_Item);
            lv.RegisterCommand(Commands.MoveUp, OnExecuted_ListView_MoveUp, lv.CanExecute_MoveUp);
            lv.RegisterCommand(Commands.MoveDown, OnExecuted_ListView_MoveDown, lv.CanExecute_MoveDown);
            lv.RegisterCommand(Commands.Cut, OnExecuted_ListView_Cut, lv.CanExecute_Item);
            lv.RegisterCommand(Commands.Copy, OnExecuted_ListView_Copy, lv.CanExecute_Item);
            this.RegisterCommand(Commands.Paste, OnExecuted_ListView_Paste, CanExecute_Paste);
        }

        protected override void Initialize()
        {
            InitializeComponent();
        }

        protected override void OnConditionsChanged(HandConditionsCollection? value)
        {
            base.OnConditionsChanged(value);
            SelectedItem = null;
            LastFocusedListView = null;
        }

        protected override void ApplyHistory(HandInspectHistoryData historyData)
        {
            historyData.ConvertBack(Conditions, CardProvider);
            if (ListView_Main.SelectedIndex == historyData.SelectedIndex)
            {
                ListView_Main.SelectedIndex = -1;
            }
            ListView_Main.SelectedIndex = historyData.SelectedIndex;
        }

        private void OnSelectedItemChanged(HandConditions? value)
        {
            if (value is not null)
            {
                EditingItem!.CopyFrom(value);
            }
        }

        private void OnClick_Load(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var selected = SelectedItem;
            ListView_Main.SelectedItem = null;
            ListView_Main.SelectedItem = selected;
        }

        private void OnClick_Apply(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            BeforeEdit();
            SelectedItem?.CopyFrom(EditingItem!);
            this.OnEdit();
        }

        private void InsertItem(HandConditions item, int index = -1)
        {
            if (Conditions is not { } items)
            {
                return;
            }
            if (index < 0)
            {
                index = ListView_Main.SelectedIndex;
            }
            if (index < 0)
            {
                index = items.Count;
            }
            BeforeEdit();
            items.Insert(index, item);
            ListView_Main.ProcessSelect(index);
            this.OnEdit();
        }

        private void RemoveItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (Conditions is { } items)
            {
                BeforeEdit();
                ListView_Main.OnExecuted_Delete(items, e);
                this.OnEdit();
            }
        }

        private void OnExecuted_ListView_Insert(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            HandConditions item = new()
            {
                Name = $"Conditions#{Random.Shared.Next():X8}",
            };
            InsertItem(item);
        }

        private void OnExecuted_ListView_Copy(object sender, ExecutedRoutedEventArgs e)
        {
            if (ListView_Main.SelectedItem is HandConditions item)
            {
                e.Handled = true;
                try
                {
                    DataObject obj = new();
                    obj.SetData(DataObjectTypes.HandInspectClipboard, Json.GetJsonBytes(item));
                    Clipboard.SetDataObject(obj);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void CanExecute_Paste(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                e.CanExecute = Clipboard.ContainsData(DataObjectTypes.HandInspectClipboard);
            }
            catch { }
        }

        private void OnExecuted_ListView_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            if (Clipboard.ContainsData(DataObjectTypes.HandInspectClipboard) &&
                Clipboard.GetData(DataObjectTypes.HandInspectClipboard) is byte[] bytes &&
                Json.TryParse<LivreNoirLibrary.YuGiOh.Serializable.HandInspectConditions<int>>(bytes, out var serializable))
            {
                e.Handled = true;
                HandConditions item = new();
                item.Load(serializable, CardProvider);
                InsertItem(item);
            }
        }

        private void OnExecuted_ListView_Duplicate(object sender, ExecutedRoutedEventArgs e)
        {
            if (ListView_Main.SelectedItem is HandConditions item)
            {
                e.Handled = true;
                item = item.Clone();
                InsertItem(item, ListView_Main.SelectedIndex + 1);
            }
        }

        private void OnExecuted_ListView_Cut(object sender, ExecutedRoutedEventArgs e)
        {
            OnExecuted_ListView_Copy(sender, e);
            if (e.Handled)
            {
                RemoveItem(sender, e);
            }
        }

        private void OnExecuted_ListView_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            RemoveItem(sender, e);
        }

        private void OnExecuted_ListView_MoveUp(object sender, ExecutedRoutedEventArgs e)
        {
            if (Conditions is { } items)
            {
                BeforeEdit();
                ListView_Main.OnExecuted_MoveUp(items, e);
                this.OnEdit();
            }
        }

        private void OnExecuted_ListView_MoveDown(object sender, ExecutedRoutedEventArgs e)
        {
            if (Conditions is { } items)
            {
                BeforeEdit();
                ListView_Main.OnExecuted_MoveDown(items, e);
                this.OnEdit();
            }
        }

        private void OnExecuted_OpenSortDialog(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            BeforeEdit();
            var item = ListView_Main.SelectedItem;
            Conditions?.Sort();
            ListView_Main.SelectedItem = item;
            this.OnEdit();
        }

        private void Item_OnClick_ClearButton(object sender, RoutedEventArgs e)
        {
            EditingItem?.Items.Clear();
            e.Handled = true;
        }


        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.CardListView_PreviewMouseLeftButtonDown_Alt(sender, e);
            this.IDragDrop_PreviewMouseLeftButtonDown(sender, e);
        }

        bool IDragDrop.HandleMouseButtonEvent(object sender, MouseButtonEventArgs e) => IDragDropExtensions.HandleMouseButton_ListViewItem(sender, e);

        void IDragDrop.BuildDataObject(DataObject obj, object sender) => IDragDropExtensions.BuildDataObject_ListView(DataObjectTypes.HandInspectDragDrop, obj, sender);

        bool IDragDrop.HandleDrop(IDataObject obj, object sender)
        {
            if (obj.GetData(DataObjectTypes.HandInspectDragDrop) is IListView from)
            {
                var list = BuildSelection(from);
                if (sender is IListView to && from != to)
                {
                    if (to != ListView_Deck)
                    {
                        ProcessAdd(list, to);
                    }
                    if (from != ListView_Deck)
                    {
                        ProcessRemove(from);
                    }
                }
                else if (sender == ItemsControl_Hand)
                {
                    if (from != ListView_Deck)
                    {
                        ProcessRemove(from);
                    }
                    ProcessAdd(list, null);
                }
                return true;
            }
            return false;
        }

        private object? _rightClickObject;

        private void ListView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _rightClickObject = sender;
            if (sender is ListViewItem { DataContext: ICard card, IsSelected: false } f && f.TryGetAncestor<IListView>(out var lv))
            {
                lv.SelectedItems.Clear();
                lv.SelectedItems.Add(card);
            }
        }

        private void ListView_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_rightClickObject == sender && (sender as DependencyObject).TryGetAncestor<IListView>(out var lv))
            {
                if (lv == ListView_Deck)
                {
                    ProcessAdd(BuildSelection(lv), LastFocusedListView);
                }
                else
                {
                    ProcessRemove(lv);
                }
            }
            _rightClickObject = null;
        }


        private void ListView_OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is CtListView lv && lv != ListView_Deck)
            {
                LastFocusedListView = lv;
            }
        }

        private void Item_OnClick_Or(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ItemsControl_Hand.TryGetFirstDescendant<IListView>(out var lv);
            ProcessAdd(BuildSelection(ListView_Deck), LastFocusedListView ?? lv);
        }

        private void Item_OnClick_And(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ProcessAdd(BuildSelection(ListView_Deck), null);
        }

        private void Item_OnClick_Remove(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ProcessRemove(LastFocusedListView);
        }

        private void Hand_OnExecuted_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (sender == LastFocusedListView)
            {
                ProcessRemove(LastFocusedListView);
            }
        }

        private List<Card> BuildSelection(IListView source)
        {
            var list = _selection;
            list.Clear();
            var provider = CardProvider;
            foreach (var item in source.SelectedItems)
            {
                if (Card.TryGetCard(item, provider, out var card))
                {
                    list.Add(card);
                }
            }
            return list;
        }

        private void ProcessAdd(List<Card> source, IListView? to)
        {
            if (source.Count is 0)
            {
                return;
            }
            if (to?.DataContext is HandConditionItem item)
            {
                item.Cards.AddRange(source);
                to.Focus();
                to.SetSelectedItems(source);
                to.ScrollIntoView(source[0]);
            }
            else
            {
                var newItem = EditingItem!.AddNewItem(source);
                this.SetDispatcher(() =>
                {
                    if (ItemsControl_Hand.TryGetFirstDescendant<IListView>(lv => lv.DataContext == newItem, out var lv))
                    {
                        lv.Focus();
                        lv.SetSelectedItems(newItem.Cards);
                        lv.ScrollIntoView(newItem.Cards[0]);
                    }
                });
            }
        }

        private void ProcessRemove(IListView? from)
        {
            if (from?.DataContext is HandConditionItem item)
            {
                var list = BuildSelection(from);
                if (EditingItem!.RemoveCardsFrom(item, list) && from == LastFocusedListView)
                {
                    LastFocusedListView = null;
                }
            }
        }
    }
}
