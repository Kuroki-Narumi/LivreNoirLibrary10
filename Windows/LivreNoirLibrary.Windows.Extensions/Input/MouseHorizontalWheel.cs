using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Input
{
    public static partial class InputManager
    {
        [RoutedEvent(typeof(IInputElement), typeof(MouseWheelEventHandler))]
        public static readonly RoutedEvent MouseHorizontalWheelEvent = RegisterEvent();

        [RoutedEvent(typeof(IInputElement), typeof(MouseWheelEventHandler))]
        public static readonly RoutedEvent PreviewMouseHorizontalWheelEvent = RegisterEvent();

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
