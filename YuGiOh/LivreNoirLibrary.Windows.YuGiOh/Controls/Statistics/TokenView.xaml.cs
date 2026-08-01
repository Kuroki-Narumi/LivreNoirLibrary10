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
    /// TokenView.xaml の相互作用ロジック
    /// </summary>
    public partial class TokenView : UserControl
    {
        private readonly CardSearchConditions _conds = new();

        [DependencyProperty]
        private TokenCollection? _itemsSource;

        public TokenView()
        {
            InitializeComponent();
            MainGrid.DataContext = ItemsSource;
        }

        private void OnItemsSourceChanged(TokenCollection? value)
        {
            MainGrid.DataContext = value;
        }

        private void OnClick_Unselect(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            ListView_Main.SelectedItem = null;
        }
    }
}
