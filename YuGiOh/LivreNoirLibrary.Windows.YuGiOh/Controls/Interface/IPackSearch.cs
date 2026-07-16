using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Windows.Controls;
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
        PackSearchConditions PackSearchConditions { get; }
        PackSearchConditions DefaultPackSearchConditions { get; }
        ListBox PackListBox { get; }
        void SetPackSearchText(string text);
    }

    public static class IPackSearchExtensions
    {
        public static void RegisterPackSearchCommands<T>(this UIElement element, T owner)
            where T : DependencyObject, IPackSearch
        {
            element.RegisterCommand(YgoCommands.OpenSearch, owner.PackList_RequestOpenSearch);
            element.RegisterCommand(YgoCommands.SearchClear, owner.PackList_RequestClear);
        }

        public static void RegisterPackSearchCommands<T>(this T obj) where T : UIElement, IPackSearch => RegisterPackSearchCommands(obj, obj);

        public static void UpdatePackFilter(this IPackSearch obj)
        {
            using var t = ExStopwatch.ProcessTime("Search");
            var conds = obj.PackSearchConditions;
            conds.Prepare();
            obj.SetPackSearchText(conds.SearchText);
            obj.PackListBox.Items.Filter = item => item is CardPack c && conds.IsMatch(c);
        }

        public static void PackList_RequestSearch(this IPackSearch obj, object sender, RoutedEventArgs<string> e)
        {
            e.Handled = true;
            obj.PackSearchConditions.SearchText = e.Value;
            UpdatePackFilter(obj);
        }

        public static void PackList_RequestClear(this IPackSearch obj, object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            var s = obj.PackSearchConditions;
            PackSearchConditions.Copy(obj.DefaultPackSearchConditions, s, false);
            s.SearchText = "";
            UpdatePackFilter(obj);
        }

        public static void PackList_RequestOpenSearch<T>(this T obj, object sender, ExecutedRoutedEventArgs e)
            where T : DependencyObject, IPackSearch
        {
            e.Handled = true;
            var owner = Window.GetWindow(obj);
            PackSearchWindow window = new() { Owner = owner };
            window.Setup(obj.PackSearchConditions, obj.DefaultPackSearchConditions);
            window.Search += obj.PackList_SearchExecuted;
            window.PlaceToCursor(-32, -16, owner);
            window.ShowDialog();
        }

        private static void PackList_SearchExecuted(this IPackSearch obj, object? sender, EventArgs e) => UpdatePackFilter(obj);
    }
}
