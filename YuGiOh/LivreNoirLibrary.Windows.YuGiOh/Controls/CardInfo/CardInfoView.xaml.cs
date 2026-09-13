using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Windows.YuGiOh.Converters;
using LivreNoirLibrary.YuGiOh.Data;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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

        private string? _currentImagePath;

        public CardInfoView()
        {
            InitializeComponent();
            MainGrid.DataContext = _viewModel;
            this.RegisterCommand(YgoCommands.CardImage_File, OnExecuted_CardImage_File, CanExecute_CardImage_File);
            this.RegisterCommand(YgoCommands.CardImage_Paste, OnExecuted_CardImage_Paste, CanExecute_CardImage_Paste);
            this.RegisterCommand(YgoCommands.CardImage_Copy, OnExecuted_CardImage_Copy, CanExecute_CardImage_Copy);
            this.RegisterCommand(YgoCommands.CardImage_Delete, OnExecuted_CardImage_Delete, CanExecute_CardImage_Delete);
        }

        private void OnSourceChanged(Card? value)
        {
            CardImage_Large.Close();
            _viewModel.Source = value;
            UpdateCardImage();
        }

        private void UpdateCardImage()
        {
            (CardImage.Source, _currentImagePath) = IdToCardImageConverter.GetImage(Source);
        }

        private void OnClick_DB1(object sender, RoutedEventArgs e)
        {
            if (Source is { } card)
            {
                e.Handled = true;
                this.RaiseRequestOpenCardUrl(card.Id, false);
            }
        }

        private void OnClick_DB2(object sender, RoutedEventArgs e)
        {
            if (Source is { } card)
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

        private void CanExecute_CardImage_File(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Source is not null;
        }

        private void OnExecuted_CardImage_File(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (Source is { } card && this.OpenFileDialog(null, Filters.Image) is { } path)
            {
                OverrideCardImage(card, path);
            }
        }

        private void CanExecute_CardImage_Paste(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Source is not null && Bitmap.CanCreateFromClipboard();
        }

        private void OnExecuted_CardImage_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (Source is { } card)
            {
                OverrideCardImageFromClipboard(card);
            }
        }

        private void CanExecute_CardImage_Copy(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Source is not null && File.Exists(_currentImagePath);
        }

        private void OnExecuted_CardImage_Copy(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (CardImage.Source is BitmapSource source && _currentImagePath is { } path)
            {
                try
                {
                    var obj = source.CreateDataObject();
                    obj.SetText(path);
                    obj.SetFileDropList([path]);
                    Clipboard.SetDataObject(obj);
                }
                catch
                {

                }
            }
        }

        private void CanExecute_CardImage_Delete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = File.Exists(_currentImagePath);
        }

        private void OnExecuted_CardImage_Delete(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (File.Exists(_currentImagePath))
            {
                File.Delete(_currentImagePath);
                UpdateCardImage();
            }
        }

        private void OnMouseDown_CardImage(object sender, MouseButtonEventArgs e)
        {
            if (_currentImagePath is not null)
            {
                CardImage_Large.Open();
            }
            else
            {
                OnExecuted_CardImage_File(sender, e);
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

        private void OverrideCardImage(Card card, string path)
        {
            var targetPath = IdToCardImageConverter.GetImagePath(card.Id);
            targetPath = Path.ChangeExtension(targetPath, Path.GetExtension(path));
            General.EnsureDirectory(targetPath);
            File.Copy(path, targetPath, true);
            UpdateCardImage();
        }

        private bool OverrideCardImageFromClipboard(Card card)
        {
            if (Bitmap.GetSourceFromClipboard() is { } source)
            {
                var targetPath = IdToCardImageConverter.GetImagePath(card.Id);
                Bitmap.SaveImage(source, targetPath, BitmapEncodeType.PNG);
                UpdateCardImage();
                return true;
            }
            return false;
        }
    }
}
