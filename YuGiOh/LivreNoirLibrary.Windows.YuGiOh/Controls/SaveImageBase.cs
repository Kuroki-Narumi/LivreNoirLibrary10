using LivreNoirLibrary.IO;
using LivreNoirLibrary.Windows.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract class SaveImageBase : UserControl
    {
        protected abstract Visual SavingVisual { get; }

        private readonly FileDialogOptions _dialogOptions = new() { FileName = "image.png" };

        protected void OnClick_SaveAsImage(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (this.SaveFileDialog(_dialogOptions, Filters.Image_Save) is { } path)
            {
                _dialogOptions.FileName = System.IO.Path.GetFileNameWithoutExtension(path);
                Bitmap.SaveImage(SavingVisual, path, new(sizeUnit: 2), BitmapEncodeType.Auto);
            }
        }

        protected void OnClick_CopyImage(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = Bitmap.CreateDataObject(SavingVisual, new(sizeUnit: 2));
                SetExtraData(obj);
                Clipboard.SetDataObject(obj);
                e.Handled = true;
            }
            catch (COMException)
            {
                Thread.Sleep(10);
                OnClick_CopyImage(sender, e);
            }
        }

        protected virtual void SetExtraData(DataObject obj)
        {

        }
    }
}
