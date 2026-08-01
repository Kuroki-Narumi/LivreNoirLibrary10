using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.YuGiOh.Converters;
using LivreNoirLibrary.YuGiOh.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// CardInfoView.xaml の相互作用ロジック
    /// </summary>
    public partial class CardInfoView : UserControl
    {
        public const double DefaultPackListHeight = 104;
        public const double ExpandedPackListHeight = 304;

        private readonly CardInfoViewModel _viewModel = new(true);

        [DependencyProperty]
        private Card? _source;

        public CardInfoView()
        {
            InitializeComponent();
            MainGrid.DataContext = _viewModel;
        }

        private void OnSourceChanged(Card? value) => _viewModel.Source = value;

        private void OnClick_DB1(object sender, RoutedEventArgs e)
        {
            if (_source is { } card)
            {
                e.Handled = true;
                this.RaiseRequestOpenCardUrl(card.Id, false);
            }
        }

        private void OnClick_DB2(object sender, RoutedEventArgs e)
        {
            if (_source is { } card)
            {
                e.Handled = true;
                this.RaiseRequestOpenCardUrl(card.Id, true);
            }
        }

        private void OnClick_Copy(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            SetTextBoxToClipboard(sender);
        }

        public static void SetTextBoxToClipboard(object sender)
        {
            if (sender is FrameworkElement { Tag: TextBox { Text: string text } } && !string.IsNullOrEmpty(text))
            {
                try
                {
                    Clipboard.SetText(text);
                }
                catch
                {

                }
            }
        }
    }
}
