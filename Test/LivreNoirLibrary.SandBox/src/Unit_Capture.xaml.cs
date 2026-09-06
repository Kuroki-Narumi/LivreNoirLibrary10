using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media.Capture;
using LivreNoirLibrary.Win32Api;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Media;
using LivreNoirLibrary.Media;
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
using LivreNoirLibrary.IO;

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
            Console.WriteLine(General.GetAssemblyDir());
        }

        private void OnSelectionChanged_Monitor(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OnSelectionChanged_Window(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView { SelectedItem: WindowInfo info })
            {
                Capturer.SearchingTitle = info.Title;
                Capturer.SearchingFile = info.ExeFileName;
            }
        }

        private void OnClick_Capture(object sender, RoutedEventArgs e)
        {
            var handle = Capturer.CapturingWindowHandle;
            if (Capturer.CreateCroppedBitmap() is { } bitmap)
            {
                Console.WriteLine($"sourceHandle={handle:X16}, rect={Capturer.SourceRect} cropped=({bitmap.PixelWidth},{bitmap.PixelHeight})");
                Bitmap.ToClipboard(bitmap);
            }
        }
    }
}
