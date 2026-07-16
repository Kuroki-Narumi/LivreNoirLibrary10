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
        public const double ExpandedPackListHeight = 224;

        private readonly CardInfoViewModel _viewModel = new(true);

        [DependencyProperty]
        private Card? _source;

        public CardInfoView()
        {
            InitializeComponent();
            MainGrid.DataContext = _viewModel;
        }

        private void OnSourceChanged(Card? value)
        {
            if (value is not null)
            {
                _viewModel.CopyFrom(value);
            }
        }

        private void OnClick_DB1(object sender, RoutedEventArgs e)
        {
            if (_source is { } card)
            {
                e.Handled = true;
                this.RaiseCardLinkClicked(card.Id, false);
            }
        }

        private void OnClick_DB2(object sender, RoutedEventArgs e)
        {
            if (_source is { } card)
            {
                e.Handled = true;
                this.RaiseCardLinkClicked(card.Id, true);
            }
        }

        private void OnClick_Pack(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkContentElement { DataContext: PackInfo info })
            {
                e.Handled = true;
                this.RaisePackLinkClicked(info.ProductId);
            }
        }

        private void OnClick_RelatedText(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkContentElement { DataContext: string text })
            {
                this.RaiseRelatedTextClicked(text);
            }
        }

        private void OnClick_Copy(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            SetTextBoxToClipboard(sender);
        }

        public static void SetTextBoxToClipboard(object sender)
        {
            if (sender is FrameworkElement { Tag: TextBox { Text: string text } })
            {
                if (!string.IsNullOrEmpty(text))
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

        private int _detachListenerCount;

        public static readonly RoutedEvent DetachEvent = Events.Register<CardInfoView, RoutedEventHandler<Card>>();

        public event RoutedEventHandler<Card>? Detach
        {
            add
            {
                AddHandler(DetachEvent, value);
                if (++_detachListenerCount > 0)
                {
                    _viewModel.CanDetach = true;
                }
            }
            remove
            {
                RemoveHandler(DetachEvent, value);
                if (--_detachListenerCount <= 0)
                {
                    _viewModel.CanDetach = false;
                }
            }
        }

        private void OnClick_Detach(object sender, RoutedEventArgs e)
        {
            if (Source is { } card)
            {
                RaiseEvent(new RoutedEventArgs<Card>(card, DetachEvent, this));
            }
        }
    }
}
