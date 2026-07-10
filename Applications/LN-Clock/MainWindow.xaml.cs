using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.Windows.Media;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace LivreNoir.Clock
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

        private Timer _timer;
        private Window_Config? _configWindow;

        public MainWindow()
        {
            DataContext = ViewModel;
            ViewModel.UpdateIntervalChanged += OnUpdateIntervalChanged;
            InitializeComponent();
            BindLocation();
            _timer = ResetTimer();
            OnUpdate();
        }

        private void BindLocation()
        {
            var vm = ViewModel;
            if (!double.IsFinite(vm.Left))
            {
                vm.Left = Left;
            }
            if (!double.IsFinite(vm.Top))
            {
                vm.Top = Top;
            }
            if (!double.IsFinite(vm.Width))
            {
                vm.Width = Width;
            }
            if (!double.IsFinite(vm.Height))
            {
                vm.Height = Height;
            }
            SetBinding(LeftProperty, new Binding(nameof(MainViewModel.Left)) { Mode = BindingMode.TwoWay });
            SetBinding(TopProperty, new Binding(nameof(MainViewModel.Top)) { Mode = BindingMode.TwoWay });
            SetBinding(WidthProperty, new Binding(nameof(MainViewModel.Width)) { Mode = BindingMode.TwoWay });
            SetBinding(HeightProperty, new Binding(nameof(MainViewModel.Height)) { Mode = BindingMode.TwoWay });
        }

        public Timer ResetTimer()
        {
            _timer?.Dispose();
            var timer = new Timer(OnUpdate, null, 1000 - DateTime.Now.Millisecond, ViewModel.UpdateInterval);
            _timer = timer;
            return timer;
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
            MainViewModel.Save();
            base.OnClosing(e);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.DragMoveWithSnap();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var horz = 0;
            var vert = 0;
            switch (e.Key)
            {
                case Key.Left:
                    horz = -1;
                    break;
                case Key.Right:
                    horz = 1;
                    break;
                case Key.Up:
                    vert = -1;
                    break;
                case Key.Down:
                    vert = 1;
                    break;
                default:
                    base.OnKeyDown(e);
                    return;
            }
            if (KeyInput.IsShiftDown())
            {
                horz *= 10;
                vert *= 10;
            }
            var bounds = this.GetScreenBounds();
            double newLeft, newTop;
            if (KeyInput.IsCtrlDown())
            {
                newLeft = Math.Clamp(Left + horz, bounds.Left, bounds.Right - ActualWidth);
                newTop = Math.Clamp(Top + vert, bounds.Top, bounds.Bottom - ActualHeight);
                Left = newLeft;
                Top = newTop;
            }
            else
            {
                Left += horz;
                Top += vert;
            }
        }

        private void OnClick_Config(object sender, RoutedEventArgs e)
        {
            if (_configWindow is null)
            {
                var window = new Window_Config()
                {
                    Owner = this,
                };
                window.Closed += OnConfigWindowClosed;
                CorrectPosition(window);
                window.Show();
                _configWindow = window;
            }
            else
            {
                CorrectPosition(_configWindow);
                _configWindow.Focus();
            }
        }

        private void CorrectPosition(Window window)
        {
            window.Left = Left + ActualWidth;
            window.Top = Top;
            window.SetDispatcher(() => window.CorrectPosition(this), System.Windows.Threading.DispatcherPriority.DataBind);
        }

        private void OnConfigWindowClosed(object? sender, EventArgs e)
        {
            _configWindow?.Closed -= OnConfigWindowClosed;
            _configWindow = null;
        }

        private void OnClick_Close(object sender, RoutedEventArgs e) => Close();
    }
}