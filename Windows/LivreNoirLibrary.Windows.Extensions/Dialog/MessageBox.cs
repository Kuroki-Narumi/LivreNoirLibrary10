using System;
using System.Windows;

namespace LivreNoirLibrary.Windows
{
    public static partial class WindowsExtensions
    {
        public static string? MessageBoxTitle { get; set; }

        private static string GetMessage(string? message)
        {
            if (message is null)
            {
                return "";
            }
            else
            {
                return message.Replace("\0", "");
            }
        }

        public static MessageBoxResult ShowMessage_OK(this Window window, string? message, MessageBoxImage image = MessageBoxImage.None)
        {
            return MessageBox.Show(window, GetMessage(message), MessageBoxTitle ?? window.Title, MessageBoxButton.OK, image);
        }

        public static MessageBoxResult ShowMessage_OK(this DependencyObject element, string? message, MessageBoxImage image = MessageBoxImage.None)
        {
            return ShowMessage_OK(Window.GetWindow(element), message, image);
        }

        public static MessageBoxResult ShowMessage_YesNo(this Window window, string? message, MessageBoxImage image = MessageBoxImage.None)
        {
            return MessageBox.Show(window, GetMessage(message), MessageBoxTitle ?? window.Title, MessageBoxButton.YesNo, image);
        }

        public static MessageBoxResult ShowMessage_YesNo(this DependencyObject element, string? message, MessageBoxImage image = MessageBoxImage.None)
        {
            return ShowMessage_YesNo(Window.GetWindow(element), message, image);
        }

        public static MessageBoxResult ShowMessage_YesNoCancel(this Window window, string? message, MessageBoxImage image = MessageBoxImage.None)
        {
            return MessageBox.Show(window, GetMessage(message), MessageBoxTitle ?? window.Title, MessageBoxButton.YesNoCancel, image);
        }

        public static MessageBoxResult ShowMessage_YesNoCancel(this DependencyObject element, string? message, MessageBoxImage image = MessageBoxImage.None)
        {
            return ShowMessage_YesNoCancel(Window.GetWindow(element), message, image);
        }

        public static MessageBoxResult ShowMessage_OKCancel(this Window window, string? message, MessageBoxImage image = MessageBoxImage.None)
        {
            return MessageBox.Show(window, GetMessage(message), MessageBoxTitle ?? window.Title, MessageBoxButton.OKCancel, image);
        }

        public static MessageBoxResult ShowMessage_OKCancel(this DependencyObject element, string? message, MessageBoxImage image = MessageBoxImage.None)
        {
            return ShowMessage_OKCancel(Window.GetWindow(element), message, image);
        }
    }
}
