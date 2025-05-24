using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls
{
    public static partial class ControlExtensions
    {
        public static void RegisterListCommands(this Selector selector, bool move = true, bool delete = true, Func<object?>? addFunc = null, Func<object?, object?>? duplicateFunc = null)
        {
            if (move)
            {
                selector.RegisterCommand(Commands.MoveUp, selector.OnExecuted_MoveUp, selector.CanExecute_MoveUp);
                selector.RegisterCommand(Commands.MoveDown, selector.OnExecuted_MoveDown, selector.CanExecute_MoveDown);
            }
            if (delete)
            {
                selector.RegisterCommand(Commands.Delete, selector.OnExecuted_Delete, selector.CanExecute_Item);
            }
            if (addFunc is not null)
            {
                selector.RegisterCommand(Commands.Insert, (s, e) => selector.OnExecuted_Insert(addFunc, e), selector.CanExecute_Always);
            }
            if (duplicateFunc is not null)
            {
                selector.RegisterCommand(Commands.Duplicate, (s, e) => selector.OnExecuted_Duplicate(duplicateFunc, e), selector.CanExecute_Item);
            }
        }

        public static void RegisterListCommands<T>(this Selector selector, IList<T> itemsSource, bool move = true, bool delete = true, Func<T>? addFunc = null, Func<T, T>? duplicateFunc = null)
        {
            if (move)
            {
                selector.RegisterCommand(Commands.MoveUp, (s, e) => selector.OnExecuted_MoveUp(itemsSource, e), selector.CanExecute_MoveUp);
                selector.RegisterCommand(Commands.MoveDown, (s, e) => selector.OnExecuted_MoveDown(itemsSource, e), selector.CanExecute_MoveDown);
            }
            if (delete)
            {
                selector.RegisterCommand(Commands.Delete, (s, e) => selector.OnExecuted_Delete(itemsSource, e), selector.CanExecute_Item);
            }
            if (addFunc is not null)
            {
                selector.RegisterCommand(Commands.Insert, (s, e) => selector.OnExecuted_Insert(itemsSource, addFunc, e));
            }
            if (duplicateFunc is not null)
            {
                selector.RegisterCommand(Commands.Duplicate, (s, e) => selector.OnExecuted_Duplicate(itemsSource, duplicateFunc, e), selector.CanExecute_Item);
            }
        }

        public static void CanExecute_Item(this Selector selector, object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = selector.SelectedItem is not null;
        }

        public static void CanExecute_Item<T>(this Selector selector, object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = selector.SelectedItem is T;
        }

        public static void CanExecute_MoveUp(this Selector selector, object sender, CanExecuteRoutedEventArgs e)
        {
            var index = selector.SelectedIndex;
            e.CanExecute = Collections.CollectionExtensions.CanMoveUp(selector.Items, index);
        }

        public static void CanExecute_MoveDown(this Selector selector, object sender, CanExecuteRoutedEventArgs e)
        {
            var index = selector.SelectedIndex;
            e.CanExecute = Collections.CollectionExtensions.CanMoveDown(selector.Items, index);
        }

        public static void ProcessSelect(this Selector selector, int index)
        {
            selector.SelectedIndex = index;
            if (index >= 0 && selector is ListBox lb)
            {
                lb.ScrollIntoView(selector.SelectedItem);
            }
        }

        public static void OnExecuted_Insert(this Selector selector, Func<object?> addFunc, ExecutedRoutedEventArgs e)
        {
            var obj = addFunc();
            if (obj is not null)
            {
                var index = selector.SelectedIndex + 1;
                var list = (selector.ItemsSource as IList) ?? selector.Items;
                list.Insert(index, obj);
                ProcessSelect(selector, index);
            }
            e.Handled = true;
        }

        public static void OnExecuted_Duplicate(this Selector selector, Func<object?, object?> duplicateFunc, ExecutedRoutedEventArgs e)
        {
            var index = selector.SelectedIndex;
            var list = (selector.ItemsSource as IList) ?? selector.Items;
            var obj = duplicateFunc(list[index]);
            if (obj is not null)
            {
                list.Insert(index + 1, obj);
                ProcessSelect(selector, index + 1);
            }
            e.Handled = true;
        }

        public static void OnExecuted_Delete(this Selector selector, object sender, ExecutedRoutedEventArgs e)
        {
            var index = selector.SelectedIndex;
            var list = (selector.ItemsSource as IList) ?? selector.Items;
            list.RemoveAt(index);
            ProcessSelect(selector, index >= list.Count ? index - 1 : index);
            e.Handled = true;
        }

        public static void OnExecuted_MoveUp(this Selector selector, object sender, ExecutedRoutedEventArgs e)
        {
            var index = selector.SelectedIndex;
            var list = (selector.ItemsSource as IList) ?? selector.Items;
            Collections.CollectionExtensions.MoveUp(list, index);
            ProcessSelect(selector, index - 1);
            e.Handled = true;
        }

        public static void OnExecuted_MoveDown(this Selector selector, object sender, ExecutedRoutedEventArgs e)
        {
            var index = selector.SelectedIndex;
            var list = (selector.ItemsSource as IList) ?? selector.Items;
            Collections.CollectionExtensions.MoveDown(list, index);
            ProcessSelect(selector, index + 1);
            e.Handled = true;
        }

        public static void OnExecuted_Insert<T>(this Selector selector, IList<T> list, Func<T> addFunc, ExecutedRoutedEventArgs e)
        {
            var index = selector.SelectedIndex + 1;
            list.Insert(index, addFunc());
            ProcessSelect(selector, index);
            e.Handled = true;
        }

        public static void OnExecuted_Duplicate<T>(this Selector selector, IList<T> list, Func<T, T> duplicateFunc, ExecutedRoutedEventArgs e)
        {
            var index = selector.SelectedIndex;
            list.Insert(index + 1, duplicateFunc(list[index]));
            ProcessSelect(selector, index + 1);
            e.Handled = true;
        }

        public static void OnExecuted_Delete<T>(this Selector selector, IList<T> list, ExecutedRoutedEventArgs e)
        {
            var index = selector.SelectedIndex;
            list.RemoveAt(index);
            ProcessSelect(selector, index >= list.Count ? index - 1 : index);
            e.Handled = true;
        }

        public static void OnExecuted_MoveUp<T>(this Selector selector, IList<T> list, ExecutedRoutedEventArgs e)
        {
            var index = selector.SelectedIndex;
            list.MoveUp(index);
            ProcessSelect(selector, index - 1);
            e.Handled = true;
        }

        public static void OnExecuted_MoveDown<T>(this Selector selector, IList<T> list, ExecutedRoutedEventArgs e)
        {
            var index = selector.SelectedIndex;
            list.MoveDown(index);
            ProcessSelect(selector, index + 1);
            e.Handled = true;
        }
    }
}
