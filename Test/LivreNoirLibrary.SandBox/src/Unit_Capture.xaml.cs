using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Capture;
using LivreNoirLibrary.Win32Api;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Media;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace LivreNoirLibrary.SandBox
{
    /// <summary>
    /// Unit_Capture.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_Capture : UserControl
    {
        public IEnumerable<WindowInfo> WindowInfos => CaptureService.WindowInfos;
        public IEnumerable<MonitorInfo> MonitorInfos => CaptureService.MonitorInfos;

        public Unit_Capture()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OnClick_UpdateList(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            CaptureService.RefreshInfo();
        }

        private void OnSelectionChanged_Window(object sender, SelectionChangedEventArgs e)
        {
            Capturer.CaptureTarget = (sender as ListView)?.SelectedItem;
        }

        private void OnClick_Capture(object sender, RoutedEventArgs e)
        {
            var handle = Capturer.CapturingHandle;
            if (Capturer.CreateCroppedBitmap() is { } bitmap)
            {
                Console.WriteLine($"rect={Capturer.SourceRect} cropped=({bitmap.PixelWidth},{bitmap.PixelHeight})");
                Bitmap.ToClipboard(bitmap);
            }
        }
    }
}
