using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.Collections.Generic;
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
    /// SmallWorldGraphControl.xaml の相互作用ロジック
    /// </summary>
    public partial class SmallWorldGraphControl : SaveImageBase
    {
        protected override Visual SavingVisual => GraphView;

        [DependencyProperty]
        private ICardEnumerable? _itemsSource;

        public SmallWorldGraphControl()
        {
            InitializeComponent();
        }

        private void OnItemsSourceChanged()
        {
            RefreshSource();
        }

        private void OnClick_Refresh(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            RefreshSource();
        }

        private void RefreshSource()
        {
            GraphView.LoadCards(ItemsSource);
        }
    }
}
