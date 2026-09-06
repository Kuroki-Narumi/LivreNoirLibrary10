using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Capture;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.Windows.Media;
using InputManager = LivreNoirLibrary.Windows.Input.InputManager;
using System;
using System.ComponentModel;
using System.Diagnostics;
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
using LivreNoirLibrary.Windows;

namespace LivreNoir.WinCapture
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static AppSettings AppSettings => AppSettings.Instance;
        private int _hotkeyId;

        public MainWindow()
        {
            DataContext = AppSettings;
            AppSettings.WindowInfo.ApplyToWindow(this);
            InitializeComponent();
            this.RegisterCommand(ApplicationCommands.Paste, OnExecuted_Paste);
            AppSettings.InitializeMaskImage();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            RegisterHotKey();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            AppSettings.WindowInfo.SaveFromWindow(this);
            AppSettings.Save();
            base.OnClosing(e);
        }

        private void OnClick_Capture(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            OnExecuted_Capture();
        }

        private void RegisterHotKey()
        {
            var s = AppSettings.CaptureHotKey;
            if (s.Key is not 0)
            {
                _hotkeyId = this.RegisterHotKey(s.Key, s.Modifier, OnExecuted_Capture);
                if (_hotkeyId is < 0)
                {
                    this.ShowMessage_OK($$"""
                        指定されたホットキー({{s}})は既に使用されています。以下のいずれかの操作を行ってください。
                         - ホットキーの設定を変更する。
                         - このホットキーを使用しているアプリを終了し、WinCaptureを再起動する。
                        """, MessageBoxImage.Warning);
                }
            }
        }

        private void OnClick_HotKey(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var currentKey = AppSettings.CaptureHotKey;
            var (decided, key) = HotKeyWindow.StartDialog(this);
            if (decided)
            {
                InputManager.UnregisterHotKey(_hotkeyId);
                if (key == currentKey)
                {
                    AppSettings.CaptureHotKey = default;
                }
                else
                {
                    AppSettings.CaptureHotKey = key;
                    RegisterHotKey();
                }
            }
        }

        private void OnExecuted_Capture()
        {
            if (Capturer.Source is { } source)
            {
                var rect = Capturer.SourceRect;
                WriteableBitmap bitmap = new(rect.Width, rect.Height, 96, 96, PixelFormats.Pbgra32, null);
                bitmap.Lock();
                source.CopyPixels(rect, bitmap.BackBuffer, bitmap.PixelWidth * bitmap.BackBufferStride, bitmap.BackBufferStride);
                bitmap.AddDirtyRect(new(0, 0, rect.Width, rect.Height));
                bitmap.Unlock();
                bitmap.ToClipboard();
                AddItem(bitmap);
            }
        }

        private void OnExecuted_Paste(object sender, ExecutedRoutedEventArgs e)
        {
            if (Bitmap.GetSourceFromClipboard() is { } bitmap)
            {
                e.Handled = true;
                AddItem(new(bitmap));
            }
        }

        private static void AddItem(WriteableBitmap bitmap)
        {
            var a = AppSettings;
            a.CapturedItems.Add(bitmap, DateTime.Now.ToString("HH:mm:ss.ff"));
            a.SelectedItem = a.CapturedItems.LastAddedItem;
        }
    }
}