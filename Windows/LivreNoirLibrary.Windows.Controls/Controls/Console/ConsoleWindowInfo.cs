using System;
using System.Windows;
using System.Windows.Data;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls
{
    public partial class ConsoleWindowInfo : WindowInfo
    {
        public bool Visible { get; set => SetValue(ref field, value); }
        public bool SlipThrough { get; set => SetValue(ref field, value); }
        public bool Topmost { get; set => SetValue(ref field, value); }
        public bool ShowInTaskbar { get; set => SetValue(ref field, value); }
        public double Opacity { get; set => SetValue(ref field, value); } = ConsoleViewer.DefaultBackgroundOpacity;

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
            window.SlipThrough = SlipThrough;
            window.Topmost = Topmost;
            window.ShowInTaskbar = ShowInTaskbar;
            window.BackgroundOpacity = Opacity;
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
            SlipThrough = source.SlipThrough;
            Topmost = source.Topmost;
            ShowInTaskbar = source.ShowInTaskbar;
            Opacity = source.Opacity;
            Visible = source.Visible;
        }

        public void SwitchVisible() => Visible = !Visible;
    }
}
