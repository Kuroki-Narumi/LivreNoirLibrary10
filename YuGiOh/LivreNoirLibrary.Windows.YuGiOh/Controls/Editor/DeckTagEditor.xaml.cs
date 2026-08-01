using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.MasterDuel;
using System;
using System.Collections.Generic;
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
    /// DeckTagEditor.xaml の相互作用ロジック
    /// </summary>
    public partial class DeckTagEditor : DeckTagEditor_Base
    {
        protected override ListBox[] ListViews { get; }

        [RoutedEvent]
        public partial event TagNameChangedEventHandler? TagNameChanged;

        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _canAdd;
        [DependencyProperty(SetterScope = Scope.Private)]
        private bool _canSave;

        public DeckTagEditor()
        {
            ListViews = [ListView_Main];
            MainGrid.DataContext = this;

            var lv = ListView_Main;
            lv.RegisterCommand(Commands.Delete, ListView_Delete, lv.CanExecute_Item);
            lv.RegisterCommand(Commands.MoveUp, ListView_MoveUp, lv.CanExecute_MoveUp);
            lv.RegisterCommand(Commands.MoveDown, ListView_MoveDown, lv.CanExecute_MoveDown);

            this.RegisterCommand(Commands.OpenSortDialog, OnExecuted_Sort);
        }

        protected override void Initialize()
        {
            InitializeComponent();
        }

        protected override void ApplyHistory(DeckTagHistoryData historyData)
        {
            historyData.ConvertBack(ItemsSource);
            historyData.RestoreSelection(ListViews);
        }

        private void OnSearchExecuted(object sender, RoutedEventArgs<string> e)
        {
            var text = e.Value;
            if (!string.IsNullOrEmpty(text) && ItemsSource is { } items)
            {
                var lv = ListView_Main;
                var index = lv.SelectedIndex;
                var newIndex = items.FindIndex(index + 1, item => item.IsMatch(text));
                if (newIndex < 0 && index > 0)
                {
                    newIndex = items.FindIndex(0, index, item => item.IsMatch(text));
                }
                if (newIndex >= 0)
                {
                    lv.SelectedIndex = newIndex;
                    lv.ScrollSelectedItemIntoView();
                }
            }
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void OnClick_Add(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (ItemsSource is { } items)
            {
                DeckTag item = new() { Name = TextBox_Name.Text, SearchHint = TextBox_SearchHint.Text };
                BeforeEdit();
                items.Add(item);
                ListView_Main.SelectedItem = item;
                ListView_Main.ScrollSelectedItemIntoView();
                this.OnEdit();
            }
        }

        private void OnClick_Save(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (ItemsSource is { } items && ListView_Main.SelectedItem is DeckTag item)
            {
                var name = TextBox_Name.Text;
                var hint = TextBox_SearchHint.Text;
                var nameChanged = item.Name != name;
                var hintChanged = item.SearchHint != hint;
                if (!nameChanged && !hintChanged)
                {
                    return;
                }
                if (nameChanged)
                {
                    if (items.Contains(name))
                    {
                        var message = Vocab.Current.DLog.Confirm_TagReplace.Value ?? "";
                        if (message.Contains("{0}"))
                        {
                            message = string.Format(message, name);
                        }
                        if (this.ShowMessage_YesNo(message, MessageBoxImage.Warning) is not MessageBoxResult.Yes)
                        {
                            return;
                        }
                    }
                    var ret = this.ShowMessage_YesNoCancel(Vocab.Current.DLog.Confirm_TagUpdate.Value, MessageBoxImage.Question);
                    if (ret is MessageBoxResult.Cancel)
                    {
                        return;
                    }
                    var replace = ret is MessageBoxResult.Yes;
                    var oldName = item.Name;
                    BeforeEdit();
                    var newItem = items.Rename(item, name, hint);
                    ListView_Main.SelectedItem = newItem;
                    ListView_Main.ScrollSelectedItemIntoView();
                    if (replace)
                    {
                        RaiseEvent(new TagNameChangedEventArgs(oldName, name, TagNameChangedEvent, this));
                    }
                }
                else
                {
                    BeforeEdit();
                    item.SearchHint = hint;
                }
                this.OnEdit();
            }
        }

        private void OnClick_Load(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            LoadData();
        }

        private void LoadData()
        {
            if (ListView_Main.SelectedItem is DeckTag item)
            {
                TextBox_Name.Text = item.Name;
                TextBox_SearchHint.Text = item.SearchHint;
            }
        }

        private void OnTextChanged_Name(object sender, TextChangedEventArgs e)
        {
            var text = TextBox_Name.Text;
            CanAdd = !string.IsNullOrEmpty(text) && ItemsSource is { } items && !items.Contains(text);
            CanSave = !string.IsNullOrEmpty(text) && ListView_Main.SelectedItem is DeckTag;
        }

        private void ListView_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            if (ItemsSource is { } items)
            {
                BeforeEdit();
                ListView_Main.OnExecuted_Delete(items, e);
                this.OnEdit();
            }
        }

        private void ListView_MoveUp(object sender, ExecutedRoutedEventArgs e)
        {
            if (ItemsSource is { } items)
            {
                BeforeEdit();
                ListView_Main.OnExecuted_MoveUp(items, e);
                this.OnEdit();
            }
        }

        private void ListView_MoveDown(object sender, ExecutedRoutedEventArgs e)
        {
            if (ItemsSource is { } items)
            {
                BeforeEdit();
                ListView_Main.OnExecuted_MoveDown(items, e);
                this.OnEdit();
            }
        }

        private void OnExecuted_Sort(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (ItemsSource is { } items)
            {
                BeforeEdit();
                items.Sort();
                this.OnEdit();
                ListView_Main.ScrollSelectedItemIntoView();
            }
        }
    }
}
