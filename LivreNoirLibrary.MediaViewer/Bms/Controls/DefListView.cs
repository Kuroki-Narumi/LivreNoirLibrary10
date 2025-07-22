using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{

}

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class DefListView : DefListViewBase
    {
        public const string PART_OpenButton = nameof(PART_OpenButton);
        public const string PART_StopButton = nameof(PART_StopButton);

        static DefListView()
        {
            PropertyUtils.OverrideDefaultStyleKey<DefListView>();
        }

        [DependencyProperty]
        private string? _clearText;
        [DependencyProperty]
        private string? _stopText;
        [DependencyProperty]
        private string? _openText;

        private int _last_selected = -1;
        private readonly SortedSet<int> _selected_index = [];

        public int LastSelectedIndex => _last_selected;
        public int MinSelectedIndex => _selected_index.Count is 0 ? -1 : _selected_index.Min;
        public int MaxSelectedIndex => _selected_index.Max;

        private Button? _stopButton;
        private Button? _openButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _stopButton = GetTemplateChild(PART_StopButton) as Button;
            _openButton = GetTemplateChild(PART_OpenButton) as Button;
            UpdateButton();
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            UpdateButton();
        }

        private void UpdateButton()
        {
            if (ItemsSource is DefListViewModel)
            {
                var type = GetDefType();
                if (_stopButton is not null)
                {
                    _stopButton.Visibility = type is DefType.Wav ? Visibility.Visible : Visibility.Collapsed;
                }
                if (_openButton is not null)
                {
                    _openButton.Visibility = type is DefType.Wav or DefType.Bmp ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        public DefType GetDefType() => (Items[0] as DefListItem)!.Type;

        public void Select(ListViewItem item)
        {
            if (item.DataContext is DefListItem d)
            {
                var index = d.Index;
                if (!item.IsSelected)
                {
                    SelectedIndex = index;
                }
                _last_selected = index;
            }
        }

        public void SelectNext()
        {
            if (CanMoveDown())
            {
                SelectedIndex = _last_selected + 1;
                ScrollIntoView(SelectedItems[0]);
            }
        }

        public void SelectPrevious()
        {
            if (CanMoveUp())
            {
                SelectedIndex = _last_selected - 1;
                ScrollIntoView(SelectedItems[0]);
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);
            foreach (var item in e.RemovedItems)
            {
                if (item is DefListItem d)
                {
                    _selected_index.Remove(d.Index);
                }
            }
            foreach (var item in e.AddedItems)
            {
                if (item is DefListItem d)
                {
                    var index = d.Index;
                    _selected_index.Add(index);
                    _last_selected = index;
                }
            }
            if (SelectedItems.Count is 0)
            {
                _last_selected = -1;
            }
        }

        public bool CanEdit() => (uint)_last_selected < (uint)Items.Count;
        public bool CanMoveUp() => MinSelectedIndex is > 1;
        public bool CanMoveDown()
        {
            var c = MaxSelectedIndex;
            return c is > 0 && c < Items.Count - 1;
        }

        public int[] GetSelectedIndex() => [.. _selected_index];

        private void MoveSelection(int offset)
        {
            List<object> selected = [];
            var items = Items;
            foreach(var index in _selected_index)
            {
                if (index is > 0)
                {
                    selected.Add(items[index + offset]);
                }
            }
            SetSelectedItems(selected);
        }

        public List<int> GetSelection() => [.. _selected_index.Where(i => i is > 0)];
        public void MoveUpSelection() => MoveSelection(-1);
        public void MoveDownSelection() => MoveSelection(1);
    }
}
