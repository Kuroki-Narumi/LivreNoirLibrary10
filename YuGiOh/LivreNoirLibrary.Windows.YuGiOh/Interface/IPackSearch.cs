using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public interface IPackSearch
    {
        PackSearchConditions? PackSearchConditions { get; }
        PackSearchConditions? DefaultPackSearchConditions { get; }
        ListBox? PackListBox => null;
        void SetPackSearchText(string? text);
    }

    public static class IPackSearchExtensions
    {
        public static void RegisterPackSearchCommands<T>(this UIElement element, T owner)
            where T : DependencyObject, IPackSearch
        {
            element.RegisterCommand(Commands.OpenSearchDialog, owner.OnExecuted_OpenSearch, owner.CanExecute_Search);
            element.RegisterCommand(Commands.TextSearch, owner.OnExecuted_Search, owner.CanExecute_Search);
            element.RegisterCommand(Commands.TextSearchClear, owner.OnExecuted_SearchClear, owner.CanExecute_Search);
        }

        public static void RegisterPackSearchCommands<T>(this T obj) where T : UIElement, IPackSearch => RegisterPackSearchCommands(obj, obj);

        public static void UpdatePackFilter(this IPackSearch obj)
        {
            if (obj.PackSearchConditions is { } conds)
            {
                conds.Prepare();
                obj.SetPackSearchText(conds.SearchText);
                if (obj.PackListBox is { } lb)
                {
                    lb.Items.Filter = item => item is CardPack c && conds.IsMatch(c);
                    lb.ScrollSelectedItemIntoView();
                }
            }
        }

        public static void ClearPackFilter(this IPackSearch obj)
        {
            if (obj.PackSearchConditions is { } s)
            {
                PackSearchConditions.Copy(obj.DefaultPackSearchConditions ?? PackSearchConditions.Default, s, false);
                s.SearchText = "";
                UpdatePackFilter(obj);
            }
        }

        private static void CanExecute_Search(this IPackSearch obj, object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = obj.PackSearchConditions is not null;
        }

        private static void OnExecuted_Search(this IPackSearch obj, object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            obj.PackSearchConditions?.SearchText = e.Parameter as string;
            UpdatePackFilter(obj);
        }

        private static void OnExecuted_SearchClear(this IPackSearch obj, object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            ClearPackFilter(obj);
        }

        private static void OnExecuted_OpenSearch<T>(this T obj, object sender, ExecutedRoutedEventArgs e)
            where T : DependencyObject, IPackSearch
        {
            e.Handled = true;
            if (obj.PackSearchConditions is { } conds)
            {
                var owner = Window.GetWindow(obj);
                PackSearchWindow window = new() { Owner = owner };
                window.Setup(conds, obj.DefaultPackSearchConditions);
                window.Search += obj.OnSearchExecuted;
                window.PlaceToCursor(-32, -16, owner);
                window.ShowDialog();
            }
        }

        private static void OnSearchExecuted(this IPackSearch obj, object? sender, EventArgs e) => UpdatePackFilter(obj);
    }
}
