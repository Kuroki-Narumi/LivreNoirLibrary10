using System;
using System.Windows.Markup;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Windows.Media.Bms.SkinInfo
{
    [ContentProperty(nameof(Items))]
    public sealed class ListOption : OptionBase
    {
        public int SelectedIndex
        {
            get;
            set
            {
                if (!_selectionChanging)
                {
                    _selectionChanging = true;
                    if ((uint)value >= (uint)Items.Count)
                    {
                        value = -1;
                    }
                    SelectedItem = value is -1 ? null : Items[value];
                    _selectionChanging = false;
                }
                SetValue(ref field, value);
            }
        } = -1;

        public Option? SelectedItem
        {
            get;
            set
            {
                if (!_selectionChanging)
                {
                    _selectionChanging = true;
                    var index = value is not null ? Items.IndexOf(value) : -1;
                    SelectedIndex = index;
                    _selectionChanging = false;
                }
                SetValue(ref field, value, [nameof(SelectedValue)]);
            }
        }

        public override string? SelectedValue => SelectedItem?.Value;

        private bool _selectionChanging;

        public ObservableList<Option> Items { get; } = [];

        public ListOption()
        {
            Items.CollectionChanged += Items_CollectionChanged;
        }

        private void Items_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (SelectedIndex is < 0 && e.NewItems is not null && e.NewItems[0] is Option { IsDefault: true })
            {
                SelectedIndex = e.NewStartingIndex;
            }
        }
    }

    public class Option : SkinNode
    {
        public string? Value { get; set => SetValue(ref field, value); }
        public bool IsDefault { get; set => SetValue(ref field, value); }
    }
}
