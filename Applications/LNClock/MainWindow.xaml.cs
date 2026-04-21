using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace LNClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const double DefaultSize = 28;
        public const double DefaultWidth = DefaultSize * 8;
        public const double DefaultHeight = DefaultSize;

        public static MainViewModel ViewModel => MainViewModel.Instance;

        private readonly Timer _timer;

        public MainWindow()
        {
            DataContext = ViewModel;
            ViewModel.UpdateIntervalChanged += OnUpdateIntervalChanged;
            InitializeComponent();
            ViewModel.WindowInfo.ApplyToWindow(this);
            _timer = new(OnUpdate, null, 1000 - DateTime.Now.Millisecond, ViewModel.UpdateInterval);
            OnUpdate();
        }

        private void OnUpdate(object? state = null)
        {
            ViewModel.CurrentText = DateTime.Now.ToString(ViewModel.StringFormat);
        }

        private void OnUpdateIntervalChanged(object? sender, int value)
        {
            _timer.Change(0, value);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _timer.Dispose();
            ViewModel.WindowInfo.SaveFromWindow(this);
            MainViewModel.Save();
            base.OnClosing(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            DragMove();
        }

        private void OnClick_Reset(object sender, RoutedEventArgs e)
        {
            Width = DefaultWidth;
            Height = DefaultHeight;
        }

        private void OnClick_Close(object sender, RoutedEventArgs e) => Close();
    }
}