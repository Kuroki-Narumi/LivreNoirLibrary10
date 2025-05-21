using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    /// <summary>
    /// ConsoleWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ConsoleWindow : FollowOwnerWindow
    {
        public const string DefaultShowInTaskbarText = "Show in taskbar";
        public const string DefaultTopmostText = "Topmost";

        public static readonly DependencyProperty BackgroundOpacityProperty = ConsoleViewer.BackgroundOpacityProperty.AddOwnerTwoWay(typeof(ConsoleWindow), ConsoleViewer.DefaultBackgroundOpacity);
        public static readonly DependencyProperty CopyTextProperty = ConsoleViewer.CopyTextProperty.AddOwner(typeof(ConsoleWindow), ConsoleViewer.DefaultCopyText);
        public static readonly DependencyProperty FlushTextProperty = ConsoleViewer.FlushTextProperty.AddOwner(typeof(ConsoleWindow), ConsoleViewer.DefaultFlushText);

        [DependencyProperty]
        private bool _slipThrough;
        [DependencyProperty]
        private string? _showInTaskbarText = DefaultShowInTaskbarText;
        [DependencyProperty]
        private string? _topmostText = DefaultTopmostText;

        public double BackgroundOpacity { get => (double)GetValue(BackgroundOpacityProperty); set => SetValue(BackgroundOpacityProperty, value); }
        public string? CopyText { get => GetValue(CopyTextProperty) as string; set => SetValue(CopyTextProperty, value); }
        public string? FlushText { get => GetValue(FlushTextProperty) as string; set => SetValue(FlushTextProperty, value); }

        public ConsoleWindow()
        {
            DataContext = this;
            InitializeComponent();
            InitializeCommands();
            Viewer.Focus();
        }

        private void InitializeCommands()
        {
            this.RegisterCommand(Commands.Console, this.OnExecuted_Close);
            this.RegisterCommand(Commands.ConsoleSlipThrough, OnExecuted_SlipThrough);
        }

        private void OnSlipThroughChanged(bool value)
        {
            this.SetSlipThrough(value);
        }

        private void OnLeftButtonDown_Inner(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(Viewer);
            if (pos.X is >= 0 && pos.X < Viewer.ViewportWidth)
            {
                DragMove();
                e.Handled = true;
            }
        }

        private void OnExecuted_SlipThrough(object sender, ExecutedRoutedEventArgs e)
        {
            SlipThrough = !SlipThrough;
        }

        private void OnMouseWheel_Slider(object sender, MouseWheelEventArgs e) => (sender as Slider)!.ChangeByWheel(e);
    }
}
