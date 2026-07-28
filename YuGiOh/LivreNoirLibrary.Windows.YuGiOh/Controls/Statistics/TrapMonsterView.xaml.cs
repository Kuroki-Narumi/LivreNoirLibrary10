using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections;
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
    /// TrapMonsterView.xaml の相互作用ロジック
    /// </summary>
    public partial class TrapMonsterView : UserControl
    {
        [DependencyProperty]
        private IEnumerable? _itemsSource;

        public TrapMonsterView()
        {
            InitializeComponent();
        }

        private void OnItemsSourceChanged(IEnumerable? value)
        {
            ListView_Main.ItemsSource = value;
        }

        private void OnClick_Copy(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            CardInfoView.SetTextBoxToClipboard(sender);
        }

        private void OnClick_DB1(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkContentElement { DataContext: Card card })
            {
                e.Handled = true;
                this.RaiseRequestOpenCardUrl(card.Id, false);
            }
        }

        private void OnClick_DB2(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkContentElement { DataContext: Card card })
            {
                e.Handled = true;
                this.RaiseRequestOpenCardUrl(card.Id, true);
            }
        }
    }
}
