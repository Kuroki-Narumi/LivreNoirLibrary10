using System;
using System.Windows;

namespace LivreNoirLibrary.Windows.Controls
{
    public delegate void EditableTextChangedEventHandler(object sender, EditableTextChangedEventArgs e);

    public class EditableTextChangedEventArgs : RoutedEventArgs
    {
        public string? OldText { get; }
        public string? NewText { get; }

        public EditableTextChangedEventArgs() : base() {}
        public EditableTextChangedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) {}
        public EditableTextChangedEventArgs(RoutedEvent routedEvent, object source, string? oldText, string? newText) : base(routedEvent, source)
        {
            OldText = oldText;
            NewText = newText;
        }
    }

    public partial class EditableTextBlock
    {
        public static readonly RoutedEvent TextChangedEvent = EventRegister.Register<EditableTextBlock, EditableTextChangedEventHandler>();

        public event EditableTextChangedEventHandler? TextChanged
        {
            add => AddHandler(TextChangedEvent, value);
            remove => RemoveHandler(TextChangedEvent, value);
        }

        private void RaiseTextChanged(string? oldText, string? newText)
        {
            RaiseEvent(new EditableTextChangedEventArgs(TextChangedEvent, this, oldText, newText));
        }
    }
}
