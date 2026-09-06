using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.MasterDuel;
using LivreNoirLibrary.YuGiOh.MasterDuel.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class DeckTagSelector : Control, IToggleButtonContainer
    {
        const string PART_DropDown = nameof(PART_DropDown);
        const string PART_Search_TextBox = nameof(PART_Search_TextBox);
        const string PART_Search_Container = nameof(PART_Search_Container);

        static DeckTagSelector()
        {
            PropertyUtils.OverrideDefaultStyleKey<DeckTagSelector>();
        }

        [RoutedEvent]
        public partial event RoutedEventHandler<IEnumerable<string>>? SelectedItemChanged;

        bool IToggleButtonContainer.MousePressed { get; set; }
        bool IToggleButtonContainer.MouseToggleState { get; set; }

        [DependencyProperty]
        private DeckTagCollection? _itemsSource;
        [DependencyProperty(SetterScope = Scope.Private)]
        private string? _valueText = "(none)";
        [DependencyProperty]
        private Style? _toggleButtonStyle;

        private readonly CheckableDeckTagCollection _items = new();

        private DropDownMenuButton? _dropDown;
        private TextBox? _searchTextBox;
        private Panel? _searchContainer;

        private readonly ObjectCache<ToggleButtonCacheItem> _buttonCache;
        private readonly List<ToggleButtonCacheItem> _children = [];

        public DeckTagSelector()
        {
            this.InitializeIToggleButtonContainer();
            this.RegisterCommand(Commands.Clear, OnExecuted_Clear);
            this.RegisterCommand(Commands.ItemsFilter, OnExecuted_ItemsFilter);
            this.RegisterCommand(Commands.ItemsFilterClear, OnExecuted_ItemsFilterClear);
            _items.CollectionChanged += Items_CollectionChanged;
            _items.CheckedItemChanged += Items_CheckedItemChanged;
            _buttonCache = new(() => new(this));
        }

        private void Items_CheckedItemChanged(object? sender, EventArgs e)
        {
            UpdateValueText(true);
        }

        public void SetFlags(IEnumerable<string> source)
        {
            _items.LoadFlags(source);
            UpdateValueText(false);
        }

        public void ClearFlags()
        {
            _items.ClearFlags();
        }

        private void OnItemsSourceChanged(DeckTagCollection? oldValue, DeckTagCollection? newValue)
        {
            if (oldValue is not null)
            {
                _items.DetachSource(oldValue);
            }
            if (newValue is not null)
            {
                _items.AttachSource(newValue);
            }
            _searchContainer?.Children.Clear();
            _children.Clear();
            _buttonCache.Clear();
            ClearFilter();
        }

        public override void OnApplyTemplate()
        {
            var dd = _dropDown;
            dd?.DropDownOpened -= DropDown_DropDownOpened;
            dd?.DropDownClosed -= DropDown_DropDownClosed;

            _searchTextBox?.TextChanged -= TextBox_TextChanged;

            base.OnApplyTemplate();

            dd = _dropDown = GetTemplateChild(PART_DropDown) as DropDownMenuButton;
            dd?.DropDownOpened += DropDown_DropDownOpened;
            dd?.DropDownClosed += DropDown_DropDownClosed;

            _searchTextBox = GetTemplateChild(PART_Search_TextBox) as TextBox;
            _searchTextBox?.TextChanged += TextBox_TextChanged;

            _searchContainer = GetTemplateChild(PART_Search_Container) as Panel;
            UpdateFilter(false);
        }

        private void OnExecuted_Clear(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            _items.ClearFlags();
        }

        private void UpdateValueText(bool notify)
        {
            var enumer = _items.EnumerateCheckedKeys();
            ValueText = DuelLog.GetTagText(enumer, "(none)").Text;
            if (notify)
            {
                RaiseEvent(new RoutedEventArgs<IEnumerable<string>>(enumer, SelectedItemChangedEvent, this));
            }
        }

        private bool _filterChanging;

        private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ClearFilter();
        }

        private void DropDown_DropDownOpened(object? sender, EventArgs e)
        {
            ClearFilter();
            _searchTextBox?.Focus();
        }

        private void DropDown_DropDownClosed(object? sender, EventArgs e)
        {
            UpdateValueText(true);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_filterChanging)
            {
                _filterChanging = true;
                UpdateFilter(false);
            }
        }

        private void OnExecuted_ItemsFilter(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            _filterChanging = true;
            _searchTextBox?.Text = "";
            UpdateFilter(true);
        }

        private void OnExecuted_ItemsFilterClear(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            ClearFilter();
        }

        private void ClearFilter()
        {
            _filterChanging = true;
            _searchTextBox?.Text = "";
            UpdateFilter(false);
        }

        private void UpdateFilter(bool isChecked)
        {
            _filterChanging = false;
            if (_searchContainer is not { Children: { } children })
            {
                return;
            }
            var cache = _buttonCache;
            var holders = _children;

            Predicate<CheckableDeckTag> predicate = 
                isChecked ? (static item => item.IsChecked)
                : _searchTextBox?.Text is { } t && !string.IsNullOrEmpty(t) ? (item => item.IsMatch(t)) 
                : (static item => true);

            var span = _items.AsSpan();
            var itemsCount = span.Length;
            var holderCount = holders.Count;
            var elementCount = children.Count;
            for(var i = 0; i < itemsCount; i++)
            {
                var item = span[i];
                ToggleButtonCacheItem holder;
                if (i < holderCount)
                {
                    holder = holders[i];
                }
                else
                {
                    holder = cache.GetNext();
                    holders.Add(holder);
                }
                holder.Bind(item);
                holder.Button.Visibility = predicate(item) ? Visibility.Visible : Visibility.Collapsed;
                if (i >= elementCount)
                {
                    children.Add(holder.Button);
                }
            }
            if (holderCount > itemsCount)
            {
                var holSpan = holders.AsSpan()[itemsCount..holderCount];
                foreach (var holder in holSpan)
                {
                    holder.Clear();
                }
                holders.RemoveRange(itemsCount, holderCount - itemsCount);
            }
            if (elementCount > itemsCount)
            {
                children.RemoveRange(itemsCount, elementCount - itemsCount);
            }
        }

        private class ToggleButtonCacheItem : IClear
        {
            public Windows.Controls.ToggleButton Button { get; }
            public CheckableDeckTag? Source { get; set; }

            public ToggleButtonCacheItem(DeckTagSelector owner)
            {
                Windows.Controls.ToggleButton button = new();
                button.SetBinding(StyleProperty, new Binding(nameof(ToggleButtonStyle)) { Source = owner });
                button.PreviewMouseLeftButtonDown += owner.OnMouseLeftButtonDown_ToggleButton;
                button.MouseEnter += owner.OnMouseEnter_ToggleButton;
                Button = button;
            }

            public void Clear()
            {
                BindingOperations.ClearBinding(Button, ContentControl.ContentProperty);
                BindingOperations.ClearBinding(Button, System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty);
                Source = null;
            }

            public void Bind(CheckableDeckTag item)
            {
                if (Source != item)
                {
                    Button.SetBinding(ContentControl.ContentProperty, new Binding(nameof(CheckableDeckTag.Name)) { Source = item });
                    Button.SetBinding(System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty, new Binding(nameof(CheckableDeckTag.IsChecked)) { Source = item });
                    Source = item;
                }
            }
        }
    }
}
