using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Media;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LivreNoirLibrary.SandBox
{
    public partial class MainWindow
    {
        public static readonly BlendMode[] BlendModes =
        [
            BlendMode.Alpha, BlendMode.Add, BlendMode.Subtract, BlendMode.Multiply, BlendMode.Screen, BlendMode.Overlay,
            BlendMode.Darken, BlendMode.Lighten, BlendMode.ColorDodge, BlendMode.ColorBurn, BlendMode.HardLight, BlendMode.SoftLight,
            BlendMode.Difference, BlendMode.Exclusion
        ];

        private readonly FloatBitmap _buffer1 = new(0, 0);
        private WriteableBitmap? _result;

        private void OnDragOver_Image(object sender, DragEventArgs e)
        {
            e.ApplyEffect(acceptExt: ExtRegs.Image);
        }

        private void OnDrop_Image(object sender, DragEventArgs e)
        {
            if (e.TryGetAvailable(ExtRegs.Image, out var path) && sender is FrameworkElement f)
            {
                WriteableBitmap bitmap = Bitmap.FromFile(path); 
                if (f.TryGetFirstDescendant<ImageRectSelectorView>(out var selector))
                {
                    selector.Source = bitmap;
                    selector.SetRect(bitmap.Rect);
                }
                else if (f.TryGetFirstDescendant<Image>(out var image))
                {
                    image.Source = bitmap;
                }
                _result = null;
            }
        }

        private void OnClick_Image_Blend(object sender, RoutedEventArgs e)
        {
            if (Image_Source.Source is WriteableBitmap source && Image_Target.Source is BitmapSource destination)
            {
                var destW = destination.PixelWidth;
                var destH = destination.PixelHeight;
                var result = _result ??= Bitmap.Create(destW, destH);
                using (var timer = ExStopwatch.ProcessTime("CopyTo"))
                using (var targetBitmap = result.BeginWrite())
                using (var sourceBitmap = source.BeginRead())
                {
                    destination.CopyPixels(targetBitmap);
                    sourceBitmap.CopyTo(Image_Source.GetRect().ToDoubleRect(),
                        targetBitmap, targetBitmap.DoubleRect, Image_Target.GetRect().ToDoubleRect(),
                        (BlendMode)ComboBox_BlendMode.SelectedItem, new FloatColor((float)Slider_Opacity.Value * 0.01f, 1, 1, 1), _buffer1, true);
                }
                Image_Result.Source = result;
            }
        }

        private void OnClick_Image_Save(object sender, RoutedEventArgs e)
        {
            if (Image_Result.Source is BitmapSource bitmap && this.SaveFileDialog(filters: Filters.Image_Save) is { } path)
            {
                bitmap.SaveImage(path, BitmapEncodeType.Auto);
            }
        }
    }
}
