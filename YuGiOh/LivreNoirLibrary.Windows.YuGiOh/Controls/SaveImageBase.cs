using LivreNoirLibrary.IO;
using LivreNoirLibrary.Windows.Media;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public abstract partial class SaveImageBase : UserControl
    {
        protected abstract Visual SavingVisual { get; }

        private readonly FileDialogOptions _dialogOptions = new() { FileName = "image.png" };

        [DependencyProperty]
        private Color _saveBackgroundColor;
        [DependencyProperty]
        private double _saveScaleX;
        [DependencyProperty]
        private double _saveScaleY;

        public RenderVisualOptions GetRenderOptinos()
        {
            var color = SaveBackgroundColor;
            var brush = color.A > 0 ? MediaUtils.GetBrush(color) : null;
            return new RenderVisualOptions(background: brush, scale: new(SaveScaleX, SaveScaleY), sizeUnit: 2);
        }

        protected void OnClick_SaveAsImage(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            if (this.SaveFileDialog(_dialogOptions, Filters.Image_Save) is { } path)
            {
                _dialogOptions.FileName = System.IO.Path.GetFileNameWithoutExtension(path);
                Bitmap.SaveImage(SavingVisual, path, GetRenderOptinos(), BitmapEncodeType.Auto);
            }
        }

        protected void OnClick_CopyImage(object sender, RoutedEventArgs e)
        {
            try
            {
                var obj = Bitmap.CreateDataObject(SavingVisual, GetRenderOptinos());
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
