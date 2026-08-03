using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.YuGiOh.Converters;
using LivreNoirLibrary.YuGiOh.Data;
using System.IO;
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

        private void OnSourceChanged(Card? value)
        {
            CardImage_Large.Close();
            _viewModel.Source = value;
        }

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

        private void OnMouseDown_CardImage(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Source is { } card)
            {
                if (IdToCardImageConverter.ImageExists(card.Id))
                {
                    CardImage_Large.Open();
                }
                else
                {
                    OnClick_CardImage(sender, e);
                }
            }
        }

        private void CardImage_DragOver(object sender, DragEventArgs e)
        {
            if (Source is not null)
            {
                e.ApplyEffect(acceptExts: ExtRegs.Image);
            }
        }

        private void CardImage_Drop(object sender, DragEventArgs e)
        {
            if (Source is { } card && e.TryGetAvailable(out var path, ExtRegs.Image))
            {
                e.Handled = true;
                OverrideCardImage(card, path);
            }
        }

        private void OnClick_CardImage(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (Source is { } card && this.OpenFileDialog(null, Filters.Image) is { } path)
            {
                OverrideCardImage(card, path);
            }
        }

        private void OverrideCardImage(Card card, string path)
        {
            var targetPath = IdToCardImageConverter.GetImagePath(card.Id);
            targetPath = Path.ChangeExtension(targetPath, Path.GetExtension(path));
            General.EnsureDirectory(targetPath);
            File.Copy(path, targetPath, true);
            Source?.NotifyPropertyChanged(nameof(Card.Id));
        }
    }
}
