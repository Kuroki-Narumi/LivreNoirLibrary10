using LivreNoirLibrary.Windows.YuGiOh.Converters;
using LivreNoirLibrary.YuGiOh.Data;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// PackInfoView.xaml の相互作用ロジック
    /// </summary>
    public partial class PackInfoView : UserControl
    {
        [DependencyProperty]
        private CardPack? _source;
        [DependencyProperty]
        private ICardProvider? _cardProvider;

        public PackInfoView()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
        }

        private void OnCardProviderChanged(ICardProvider? value)
        {

        }

        private void OnClick_Copy(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            CardInfoView.SetTextBoxToClipboard(sender);
        }

        private void OnClick_Database(object sender, RoutedEventArgs e)
        {
            if (_source is { } pack)
            {
                e.Handled = true;
                this.RaisePackLinkClicked(pack.ProductId);
            }
        }

        private void OnClick_CardName(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkContentElement { DataContext: CardPackItem item })
            {
                e.Handled = true;
                this.RaiseCardLinkClicked(item.Card.Id, false);
            }
        }
    }
}
