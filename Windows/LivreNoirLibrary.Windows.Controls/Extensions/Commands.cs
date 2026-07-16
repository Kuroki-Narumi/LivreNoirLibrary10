using System;
using System.Windows;
using System.Windows.Input;
using LivreNoirLibrary.ObjectModel;

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

        /// <summary>
        /// Marks the object as edited and records the current state in the undo history.
        /// </summary>
        /// <param name="obj">The object that owns the history to which the edit operation applies.</param>
        /// <param name="force">A value indicating whether to force the operation to record the current state in the undo history,
        /// even if no changes have been detected.</param>
        public static void OnEdit<T>(this T obj, bool force = false)
            where T : UIElement, IHistoryOwner
        {
            if (obj.History.PushUndo(force))
            {
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public static void ClearHistory<T>(this T obj)
            where T : UIElement, IHistoryOwner
        {
            obj.History.Clear();
            CommandManager.InvalidateRequerySuggested();
        }

        public static void RegisterHistoryCommands<T>(this T obj)
            where T : UIElement, IHistoryOwner
        {
            obj.RegisterCommand(ApplicationCommands.Undo, obj.History.OnExecuted_Undo, obj.History.CanExecute_Undo);
            obj.RegisterCommand(ApplicationCommands.Redo, obj.History.OnExecuted_Redo, obj.History.CanExecute_Redo);
        }

        public static void CanExecute_Undo(this IHistory history, object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = history.UndoCount is > 0;

        public static void CanExecute_Redo(this IHistory history, object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = history.RedoCount is > 0;

        public static void OnExecuted_Undo(this IHistory history, object sender, ExecutedRoutedEventArgs e)
        {
            if (history.Undo())
            {
                e.Handled = true;
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public static void OnExecuted_Redo(this IHistory history, object sender, ExecutedRoutedEventArgs e)
        {
            if (history.Redo())
            {
                e.Handled = true;
                CommandManager.InvalidateRequerySuggested();
            }
        }
    }
}
