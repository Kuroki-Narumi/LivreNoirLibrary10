using System;
using System.Windows;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Windows.Input
{
    public static class Commands
    {
        public static RoutedUICommand Create(Type? ownerType = null, [CallerMemberName] string name = "")
        {
            return new(name, name, ownerType ?? typeof(IInputElement));
        }

        public static RoutedUICommand Create(Key key, ModifierKeys mod = ModifierKeys.None, string? keyText = null, Type? ownerType = null, [CallerMemberName] string name = "")
        {
            var command = Create(ownerType, name);
            AddGesture(command, key, mod, keyText);
            return command;
        }

        public static void AddGesture(this RoutedCommand command, Key key, ModifierKeys mod = ModifierKeys.None, string? text = null)
        {
            text ??= new Input.KeyInput(key, mod).ToString();
            command.InputGestures.Add(new KeyGesture(key, mod, text));
        }

        public static string? GetKeyGestureText(this RoutedCommand command)
        {
            var list = command.InputGestures;
            var c = list.Count;
            for (var i = 0; i < c; i++)
            {
                var gesture = list[i];
                if (gesture is KeyGesture { DisplayString: string ds})
                {
                    return string.IsNullOrWhiteSpace(ds) ? null : ds;
                }
            }
            return null;
        }

        // application commands
        public static RoutedUICommand Console { get; } = Create(Key.F1, ModifierKeys.Alt);
        public static RoutedUICommand ConsoleSlipThrough { get; } = Create(Key.S, ModifierKeys.Alt | ModifierKeys.Shift);
        public static RoutedUICommand Help => ApplicationCommands.Help;
        public static RoutedCommand CheckUpdate { get; } = Create(Key.F12);
        public static RoutedUICommand Quit { get; } = Create(Key.Q, ModifierKeys.Control);

        // file
        public static RoutedUICommand New => ApplicationCommands.New;
        public static RoutedUICommand Open => ApplicationCommands.Open;
        public static RoutedUICommand OpenFolder { get; } = Create(Key.O, ModifierKeys.Control | ModifierKeys.Shift);
        public static RoutedCommand OpenRecently { get; } = Create();
        public static RoutedUICommand Save => ApplicationCommands.Save;
        public static RoutedUICommand SaveAs => ApplicationCommands.SaveAs;
        public static RoutedUICommand Close => ApplicationCommands.Close;
        public static RoutedUICommand Properties => ApplicationCommands.Properties;
        public static RoutedUICommand Json { get; } = Create(Key.J, ModifierKeys.Control);

        // edit
        public static RoutedUICommand Undo => ApplicationCommands.Undo;
        public static RoutedUICommand Redo => ApplicationCommands.Redo;
        public static RoutedUICommand Cut => ApplicationCommands.Cut;
        public static RoutedUICommand Copy => ApplicationCommands.Copy;
        public static RoutedUICommand Paste => ApplicationCommands.Paste;
        public static RoutedUICommand Duplicate { get; } = Create(Key.D, ModifierKeys.Control);

        // list
        public static RoutedUICommand Edit { get; } = Create(Key.Enter);
        public static RoutedUICommand Insert { get; } = Create(Key.Insert);
        public static RoutedUICommand Clear { get; } = Create(Key.Back);
        public static RoutedUICommand Delete => ApplicationCommands.Delete;
        public static RoutedUICommand MoveUp { get; } = Create(Key.Up, ModifierKeys.Control);
        public static RoutedUICommand MoveDown { get; } = Create(Key.Down, ModifierKeys.Control);
        public static RoutedUICommand Next { get; } = Create(Key.Right, ModifierKeys.Alt);
        public static RoutedUICommand Prev { get; } = Create(Key.Left, ModifierKeys.Alt);
        public static RoutedUICommand Merge { get; } = Create(Key.M, ModifierKeys.Control);
        public static RoutedUICommand Split { get; } = Create(Key.L, ModifierKeys.Control);

        // dialog
        public static RoutedUICommand Decide { get; } = Create(Key.Enter);
        public static RoutedUICommand AltDecide { get; } = Create(Key.Enter, ModifierKeys.Alt);
        public static RoutedUICommand Cancel { get; } = Create(Key.Escape);

        // function
        public static RoutedCommand F1 { get; } = Create(Key.F1);
        public static RoutedCommand F2 { get; } = Create(Key.F2);
        public static RoutedCommand F3 { get; } = Create(Key.F3);
        public static RoutedCommand F4 { get; } = Create(Key.F4);
        public static RoutedCommand F5 { get; } = Create(Key.F5);
        public static RoutedCommand F6 { get; } = Create(Key.F6);
        public static RoutedCommand F7 { get; } = Create(Key.F7);
        public static RoutedCommand F8 { get; } = Create(Key.F8);
        public static RoutedCommand F9 { get; } = Create(Key.F9);
        public static RoutedCommand F10 { get; } = Create(Key.F10);
        public static RoutedCommand F11 { get; } = Create(Key.F11);
        public static RoutedCommand F12 { get; } = Create(Key.F12);

        public static RoutedCommand Alt1 { get; } = Create(Key.D1, ModifierKeys.Alt);
        public static RoutedCommand Alt2 { get; } = Create(Key.D2, ModifierKeys.Alt);
        public static RoutedCommand Alt3 { get; } = Create(Key.D3, ModifierKeys.Alt);
        public static RoutedCommand Alt4 { get; } = Create(Key.D4, ModifierKeys.Alt);
        public static RoutedCommand Alt5 { get; } = Create(Key.D5, ModifierKeys.Alt);
        public static RoutedCommand Alt6 { get; } = Create(Key.D6, ModifierKeys.Alt);
        public static RoutedCommand Alt7 { get; } = Create(Key.D7, ModifierKeys.Alt);
        public static RoutedCommand Alt8 { get; } = Create(Key.D8, ModifierKeys.Alt);
        public static RoutedCommand Alt9 { get; } = Create(Key.D9, ModifierKeys.Alt);
        public static RoutedCommand Alt0 { get; } = Create(Key.D0, ModifierKeys.Alt);

        public static RoutedCommand Ctrl1 { get; } = Create(Key.D1, ModifierKeys.Control);
        public static RoutedCommand Ctrl2 { get; } = Create(Key.D2, ModifierKeys.Control);
        public static RoutedCommand Ctrl3 { get; } = Create(Key.D3, ModifierKeys.Control);
        public static RoutedCommand Ctrl4 { get; } = Create(Key.D4, ModifierKeys.Control);
        public static RoutedCommand Ctrl5 { get; } = Create(Key.D5, ModifierKeys.Control);
        public static RoutedCommand Ctrl6 { get; } = Create(Key.D6, ModifierKeys.Control);
        public static RoutedCommand Ctrl7 { get; } = Create(Key.D7, ModifierKeys.Control);
        public static RoutedCommand Ctrl8 { get; } = Create(Key.D8, ModifierKeys.Control);
        public static RoutedCommand Ctrl9 { get; } = Create(Key.D9, ModifierKeys.Control);
        public static RoutedCommand Ctrl0 { get; } = Create(Key.D0, ModifierKeys.Control);

        static Commands()
        {
            AddGesture(ApplicationCommands.Close, Key.W, ModifierKeys.Control);
            AddGesture(ApplicationCommands.SaveAs, Key.S, ModifierKeys.Control | ModifierKeys.Shift);
            AddGesture(MediaCommands.TogglePlayPause, Key.Space);
        }
    }
}
