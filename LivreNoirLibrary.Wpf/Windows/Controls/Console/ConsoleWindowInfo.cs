using System;
using System.Windows;
using System.Windows.Data;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ConsoleWindowInfo : WindowInfo
    {
        [ObservableProperty]
        private bool _visible = false;
        [ObservableProperty]
        private bool _slipThrough = false;
        [ObservableProperty]
        private bool _topmost = false;
        [ObservableProperty]
        private bool _showInTaskbar = false;
        [ObservableProperty]
        private double _opacity = ConsoleViewer.DefaultBackgroundOpacity;

        public void SaveFromWindow(ConsoleWindow window)
        {
            base.SaveFromWindow(window);
            SlipThrough = window.SlipThrough;
            Topmost = window.Topmost;
            ShowInTaskbar = window.ShowInTaskbar;
            Opacity = window.BackgroundOpacity;
        }

        public void ApplyToWindow(ConsoleWindow window)
        {
            base.ApplyToWindow(window);
            window.SlipThrough = _slipThrough;
            window.Topmost = _topmost;
            window.ShowInTaskbar = _showInTaskbar;
            window.BackgroundOpacity = _opacity;
        }

        public void Bind(ConsoleWindow window)
        {
            window.SetBinding(ConsoleWindow.SlipThroughProperty, new Binding(nameof(SlipThrough)) { Source = this, Mode = BindingMode.TwoWay });
            window.SetBinding(Window.TopmostProperty, new Binding(nameof(Topmost)) { Source = this, Mode = BindingMode.TwoWay });
            window.SetBinding(Window.ShowInTaskbarProperty, new Binding(nameof(ShowInTaskbar)) { Source = this, Mode = BindingMode.TwoWay });
            window.SetBinding(ConsoleWindow.BackgroundOpacityProperty, new Binding(nameof(Opacity)) { Source = this, Mode = BindingMode.TwoWay });
        }

        public void Load(ConsoleWindowInfo source)
        {
            base.Load(source);
            SlipThrough = source._slipThrough;
            Topmost = source._topmost;
            ShowInTaskbar = source._showInTaskbar;
            Opacity = source._opacity;
            Visible = source._visible;
        }

        public void SwitchVisible() => Visible = !_visible;
    }
}
