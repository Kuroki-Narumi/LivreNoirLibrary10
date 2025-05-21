using System;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public static partial class ControlExtensions
    {
        public static void RegisterCommand(this UIElement element, ICommand command, ExecutedRoutedEventHandler executed, CanExecuteRoutedEventHandler? canExecute = null)
        {
            element.CommandBindings.Add(new CommandBinding(command, executed, canExecute ?? element.CanExecute_Always));
        }

        public static void CanExecute_Always(this UIElement element, object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public static void OnExecuted_Minimize(this Window w, object sender, ExecutedRoutedEventArgs e)
        {
            w.WindowState = WindowState.Minimized;
            e.Handled = true;
        }

        public static void OnExecuted_Maximize(this Window w, object sender, ExecutedRoutedEventArgs e)
        {
            if (w.WindowState == WindowState.Maximized)
            {
                w.WindowState = WindowState.Normal;
            }
            else
            {
                w.WindowState = WindowState.Maximized;
            }
            e.Handled = true;
        }

        public static void OnExecuted_Close(this Window w, object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            w.Close();
        }

        public static void RegisterWindowCommands(this Window w, bool maximize = true)
        {
            RegisterCommand(w, SystemCommands.MinimizeWindowCommand, w.OnExecuted_Minimize);
            if (maximize)
            {
                RegisterCommand(w, SystemCommands.MaximizeWindowCommand, w.OnExecuted_Maximize);
            }
            RegisterCommand(w, SystemCommands.CloseWindowCommand, w.OnExecuted_Close);
        }

        public static void RegisterHistoryCommands<TSelf, TData>(this TSelf obj)
            where TSelf : UIElement, IHistoryOwner<TSelf, TData>
        {
            obj.RegisterCommand(ApplicationCommands.Undo, obj.OnExecuted_Undo<TSelf, TData>, obj.CanExecute_Undo<TSelf, TData>);
            obj.RegisterCommand(ApplicationCommands.Redo, obj.OnExecuted_Redo<TSelf, TData>, obj.CanExecute_Redo<TSelf, TData>);
        }

        public static void CanExecute_Undo<TSelf, TData>(this TSelf obj, object sender, CanExecuteRoutedEventArgs e)
            where TSelf : IHistoryOwner<TSelf, TData>
            => e.CanExecute = obj.History.UndoCount is > 0;

        public static void CanExecute_Redo<TSelf, TData>(this TSelf obj, object sender, CanExecuteRoutedEventArgs e)
            where TSelf : IHistoryOwner<TSelf, TData>
            => e.CanExecute = obj.History.RedoCount is > 0;

        public static void OnExecuted_Undo<TSelf, TData>(this TSelf obj, object sender, ExecutedRoutedEventArgs e)
            where TSelf : IHistoryOwner<TSelf, TData>
        {
            obj.History.Undo();
            e.Handled = true;
        }

        public static void OnExecuted_Redo<TSelf, TData>(this TSelf obj, object sender, ExecutedRoutedEventArgs e)
            where TSelf : IHistoryOwner<TSelf, TData>
        {
            obj.History.Redo();
            e.Handled = true;
        }
    }
}
