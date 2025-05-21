using System;
using System.Windows.Input;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public static class EditableTextCommands
    {
        public static RoutedUICommand Decide { get; } = Commands.Create(Key.Enter);
        public static RoutedUICommand CtrlDecide { get; } = Commands.Create(Key.Enter, ModifierKeys.Control);
        public static RoutedUICommand Cancel { get; } = Commands.Create(Key.Escape);
        public static RoutedUICommand Clear { get; } = Commands.Create(Key.Delete);
    }

    public partial class EditableTextBlock
    {
        private void CanExecute_Decide(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = IsTextValid;
        private void CanExecute_Clear(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = IsClearButtonVisible;

        private void OnExecuted_Decide(object sender, ExecutedRoutedEventArgs e)
        {
            if (_isEditing)
            {
                Decide();
            }
            else
            {
                OpenPopup();
            }
            e.Handled = true;
        }

        private void OnExecuted_Cancel(object sender, ExecutedRoutedEventArgs e)
        {
            if (_isEditing)
            {
                Cancel();
            }
            e.Handled = true;
        }

        private void OnExecuted_Clear(object sender, ExecutedRoutedEventArgs e)
        {
            Clear();
            e.Handled = true;
        }
    }
}
