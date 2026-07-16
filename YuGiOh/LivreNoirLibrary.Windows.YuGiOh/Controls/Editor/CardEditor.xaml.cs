using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// CardEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class CardEditor : CardEditor_Base
    {
        protected override IListView[] ListViews { get; }

        public CardEditor()
        {
            ListViews = [ListView_CardList];

            var lv = ListView_CardList;
            this.RegisterCommand(Commands.Insert, ListView_Insert);
            lv.RegisterCommand(Commands.Duplicate, ListView_Duplicate, lv.CanExecute_Item);
            lv.RegisterCommand(Commands.Delete, ListView_Delete, lv.CanExecute_Item);
            lv.RegisterCommand(Commands.MoveUp, ListView_MoveUp, lv.CanExecute_MoveUp);
            lv.RegisterCommand(Commands.MoveDown, ListView_MoveDown, lv.CanExecute_MoveDown);
            lv.RegisterCommand(Commands.Cut, ListView_Cut, lv.CanExecute_Item);
            CardClipboard.RegisterCopy(lv);
            this.RegisterCommand(Commands.Paste, ListView_Paste, CardClipboard.CanExecute_Paste);
        }

        protected override void Initialize()
        {
            InitializeComponent();
        }

        protected override void OnItemsSourceChanged(ObservableList<Card>? value)
        {
            base.OnItemsSourceChanged(value);
            ListView_CardList.ItemsSource = value;
        }

        protected override void ApplyHistory(CardEditorHistoryData historyData)
        {
            historyData.ConvertBack(ItemsSource);
            if (ListView_CardList.SelectedIndex == historyData.SelectedIndex)
            {
                ListView_CardList.SelectedIndex = -1;
            }
            ListView_CardList.SelectedIndex = historyData.SelectedIndex;
        }

        private void OnClick_Merge(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (ItemsSource is { } items && this.OpenFileDialog(null, OpenFilter) is { } path && Json.TryOpen<Card[]>(path, out var cards))
            {
                BeforeEdit();
                items.AddRange(cards);
                this.OnEdit();
            }
        }

        private void OnClick_Load(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var selected = CardInfoEditor.Source;
            ListView_CardList.SelectedItem = null;
            ListView_CardList.SelectedItem = selected;
        }

        private void OnClick_Apply(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            BeforeEdit();
            CardInfoEditor.Save();
            this.OnEdit();
        }

        private void InsertItem(Card card, int index = -1)
        {
            if (ItemsSource is not { } items)
            {
                return;
            }
            if (index < 0)
            {
                index = ListView_CardList.SelectedIndex;
            }
            if (index < 0)
            {
                index = items.Count;
            }
            BeforeEdit();
            items.Insert(index, card);
            ListView_CardList.ProcessSelect(index);
            this.OnEdit();
        }

        private void RemoveItem(object sender, ExecutedRoutedEventArgs e)
        {
            if (ItemsSource is { } items)
            {
                BeforeEdit();
                ListView_CardList.OnExecuted_Delete(items, e);
                this.OnEdit();
            }
        }

        private void ListView_Insert(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            Card card = new()
            {
                Name = $"Card#{Random.Shared.Next():X8}",
                CardType = LivreNoirLibrary.YuGiOh.CardType.Main_Monster,
            };
            InsertItem(card);
        }

        private void ListView_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            if (CardClipboard.TryGet(out var card))
            {
                e.Handled = true;
                InsertItem(card);
            }
        }

        private void ListView_Duplicate(object sender, ExecutedRoutedEventArgs e)
        {
            if (ListView_CardList.SelectedItem is Card card)
            {
                e.Handled = true;
                card = card.Clone();
                InsertItem(card, ListView_CardList.SelectedIndex + 1);
            }
        }

        private void ListView_Cut(object sender, ExecutedRoutedEventArgs e)
        {
            ListView_CardList.OnExecuted_Copy(sender, e);
            if (e.Handled)
            {
                RemoveItem(sender, e);
            }
        }

        private void ListView_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            RemoveItem(sender, e);
        }

        private void ListView_MoveUp(object sender, ExecutedRoutedEventArgs e)
        {
            if (ItemsSource is { } items)
            {
                BeforeEdit();
                ListView_CardList.OnExecuted_MoveUp(items, e);
                this.OnEdit();
            }
        }

        private void ListView_MoveDown(object sender, ExecutedRoutedEventArgs e)
        {
            if (ItemsSource is { } items)
            {
                BeforeEdit();
                ListView_CardList.OnExecuted_MoveDown(items, e);
                this.OnEdit();
            }
        }
    }
}
