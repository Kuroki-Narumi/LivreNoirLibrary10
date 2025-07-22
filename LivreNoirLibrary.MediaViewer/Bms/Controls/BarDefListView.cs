using System;
using System.Collections.Generic;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BarDefListView : DefListViewBase
    {
        static BarDefListView()
        {
            PropertyUtils.OverrideDefaultStyleKey<BarDefListView>();
        }

        [DependencyProperty]
        private string? _mergeText;
        [DependencyProperty]
        private string? _splitText;

        public bool CanEdit() => SelectedItems.Count is > 0;
        public bool CanMerge() => SelectedItems.Count is > 1;

        public List<int> GetSelection()
        {
            List<int> result = [];
            foreach (var item in SelectedItems)
            {
                if (item is BarDefItem bar)
                {
                    result.Add(bar.Number);
                }
            }
            return result;
        }

        public (int Number, int Count) GetSelectionRange()
        {
            var start = int.MaxValue;
            var end = -1;
            foreach (var item in SelectedItems)
            {
                if (item is BarDefItem bar)
                {
                    var n = bar.Number;
                    if (n < start)
                    {
                        start = n;
                    }
                    if (n > end)
                    {
                        end = n;
                    }
                }
            }
            return (start, end - start + 1);
        }
    }
}
