using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
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
    /// HedgehogView.xaml の相互作用ロジック
    /// </summary>
    public partial class HedgehogView : UserControl, IGridViewSort
    {
        private readonly HedgehogItemCollection _dictionary = new();

        [DependencyProperty]
        private ICardEnumerable? _itemsSource;
        [DependencyProperty]
        private int _levelLimit = 3;
        [DependencyProperty]
        private HedgehogItem? _selectedItem;

        bool IGridViewSort.ClearSortIfEmptyTag => true;

        public HedgehogView()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
            this.RegisterCommand(YgoCommands.RefreshItems, OnExeucted_Refresh);

            var desc = ListView_Main.Items.SortDescriptions;
            desc.Add(new(nameof(HedgehogItem.Level), ListSortDirection.Ascending));
            desc.Add(new(nameof(HedgehogItem.Attribute), ListSortDirection.Ascending));
            desc.Add(new(nameof(HedgehogItem.MonsterType), ListSortDirection.Ascending));
            desc.Add(new(nameof(HedgehogItem.NormalCount), ListSortDirection.Ascending));
            desc.Add(new(nameof(HedgehogItem.EffectCount), ListSortDirection.Ascending));
        }

        private void OnExeucted_Refresh(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            ListView_Main.ItemsSource = null;
            if (ItemsSource is { } source)
            {
                var dic = _dictionary;
                dic.LevelLimit = LevelLimit;
                dic.Refresh(source);
                ListView_Main.ItemsSource = dic.Items;
            }
        }

        private void OnClick_GridViewColumnHeader(object sender, RoutedEventArgs e) => this.OnClick_ColumnHeader(sender, e);

        private string? _currentSort_normal;
        private bool _currentAscending_normal;

        private string? _currentSort_effect;
        private bool _currentAscending_effect;

        void IGridViewSort.SortBy(ListBox control, string key)
        {
            if (control == ListView_Normal)
            {
                control.UpdateSort(key, ref _currentSort_normal, ref _currentAscending_normal);
            }
            else if (control == ListView_Effect)
            {
                control.UpdateSort(key, ref _currentSort_effect, ref _currentAscending_effect);
            }
            else
            {
                var desc = control.Items.SortDescriptions;
                var direction = ListSortDirection.Ascending;
                if (desc[0].PropertyName == key)
                {
                    direction = desc[0].Direction is ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
                    desc.RemoveAt(0);
                }
                else
                {
                    for (var i = 0; i < desc.Count; i++)
                    {
                        if (desc[i].PropertyName == key)
                        {
                            desc.RemoveAt(i);
                            break;
                        }
                    }
                }
                desc.Insert(0, new(key, direction));
                control.ScrollSelectedItemIntoView();
            }
        }
    }
}
