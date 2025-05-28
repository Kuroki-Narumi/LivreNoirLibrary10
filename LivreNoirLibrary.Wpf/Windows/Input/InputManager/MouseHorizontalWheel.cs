using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LivreNoirLibrary.Windows.Controls;

namespace LivreNoirLibrary.Windows.Input
{
    public static partial class InputManager
    {
        public static readonly RoutedEvent MouseHorizontalWheelEvent = EventRegister.Register<UIElement, MouseWheelEventHandler>();
        public static readonly RoutedEvent PreviewMouseHorizontalWheelEvent = EventRegister.Register<UIElement, MouseWheelEventHandler>(RoutingStrategy.Tunnel);

        public static void AddMouseHorizontalWheelHandler(DependencyObject d, MouseWheelEventHandler handler)
            => (d as UIElement)?.AddHandler(MouseHorizontalWheelEvent, handler);
        public static void RemoveMouseHorizontalWheelHandler(DependencyObject d, MouseWheelEventHandler handler)
            => (d as UIElement)?.RemoveHandler(MouseHorizontalWheelEvent, handler);
        public static void AddPreviewMouseHorizontalWheelHandler(DependencyObject d, MouseWheelEventHandler handler)
            => (d as UIElement)?.AddHandler(PreviewMouseHorizontalWheelEvent, handler);
        public static void RemovePreviewMouseHorizontalWheelHandler(DependencyObject d, MouseWheelEventHandler handler)
            => (d as UIElement)?.RemoveHandler(PreviewMouseHorizontalWheelEvent, handler);

        private static void InitializeMouseHorizontalWheel()
        {
            EventManager.RegisterClassHandler(typeof(ScrollViewer), MouseHorizontalWheelEvent, new MouseWheelEventHandler(OnMouseHorizontalWheel));
            //EventManager.RegisterClassHandler(typeof(ScrollViewer), PreviewMouseHorizontalWheelEvent, new MouseWheelEventHandler(OnPreviewMouseHorizontalWheel));
        }

        private static void OnMouseHorizontalWheel(object sender, MouseWheelEventArgs e)
        {
            (sender as ScrollViewer)?.HandleHorizontalWheel(e);
        }

        private static void HandleMouseHorizontalWheel(nint wParam)
        {
            // 水平ホイールのデルタ値を取得
            var delta = unchecked((short)(wParam >> 16));
            if (Mouse.DirectlyOver is IInputElement element)
            {
                MouseWheelEventArgs args = new(Mouse.PrimaryDevice, Environment.TickCount, delta)
                {
                    RoutedEvent = PreviewMouseHorizontalWheelEvent,
                    Source = element,
                };
                element.RaiseEvent(args);
                if (!args.Handled)
                {
                    args.RoutedEvent = MouseHorizontalWheelEvent;
                    element.RaiseEvent(args);
                }
            }
        }
    }
}
