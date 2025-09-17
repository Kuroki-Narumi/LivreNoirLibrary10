using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Input
{
    public static partial class InputManager
    {
        public static readonly RoutedEvent MouseHorizontalWheelEvent = Events.Register<IInputElement, MouseWheelEventHandler>();
        public static readonly RoutedEvent PreviewMouseHorizontalWheelEvent = Events.Register<IInputElement, MouseWheelEventHandler>(RoutingStrategy.Tunnel);

        public static void AddMouseHorizontalWheelHandler(DependencyObject d, MouseWheelEventHandler handler)
            => (d as IInputElement)?.AddHandler(MouseHorizontalWheelEvent, handler);
        public static void RemoveMouseHorizontalWheelHandler(DependencyObject d, MouseWheelEventHandler handler)
            => (d as IInputElement)?.RemoveHandler(MouseHorizontalWheelEvent, handler);
        public static void AddPreviewMouseHorizontalWheelHandler(DependencyObject d, MouseWheelEventHandler handler)
            => (d as IInputElement)?.AddHandler(PreviewMouseHorizontalWheelEvent, handler);
        public static void RemovePreviewMouseHorizontalWheelHandler(DependencyObject d, MouseWheelEventHandler handler)
            => (d as IInputElement)?.RemoveHandler(PreviewMouseHorizontalWheelEvent, handler);

        public static void RegisterHorizontalWheelClassHandler<T>(MouseWheelEventHandler? handler = null, MouseWheelEventHandler? previewHandler = null)
            where T : IInputElement
        {
            if (handler is not null)
            {
                EventManager.RegisterClassHandler(typeof(T), MouseHorizontalWheelEvent, handler);
            }
            if (previewHandler is not null)
            {
                EventManager.RegisterClassHandler(typeof(T), PreviewMouseHorizontalWheelEvent, previewHandler);
            }
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
