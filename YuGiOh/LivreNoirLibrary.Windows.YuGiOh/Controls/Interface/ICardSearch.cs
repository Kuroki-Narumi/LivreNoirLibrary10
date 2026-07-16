using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public interface ICardSearch : ICardListView
    {
        ICardProvider? CardProvider { get; }
        CardSearchConditions CardSearchConditions { get; }
        CardSearchConditions DefaultCardSearchConditions { get; }
        void SetCardSearchText(string text);
        void OnCardSearchExecuted() { }
    }

    public static class ICardSearchExtensions
    {
        public static void RegisterCardSearchCommands<T>(this UIElement element, T owner)
            where T : DependencyObject, ICardSearch
        {
            element.RegisterCommand(YgoCommands.OpenSearch, owner.CardList_RequestOpenSearch);
            element.RegisterCommand(YgoCommands.SearchClear, owner.CardList_RequestClear);
            try
            {
                ClearCardFilter(owner);
            }
            catch { }
        }

        public static void RegisterCardSearchCommands<T>(this T obj) where T : UIElement, ICardSearch => RegisterCardSearchCommands(obj, obj);

        public static void UpdateCardFilter(this ICardSearch obj)
        {
            using var t = ExStopwatch.ProcessTime("Search");
            var conds = obj.CardSearchConditions;
            conds.Prepare();
            obj.SetCardSearchText(conds.SearchText);
            obj.CardListBox.Items.Filter = item => conds.IsMatch(item, obj.CardProvider);
            obj.OnCardSearchExecuted();
        }

        public static void ClearCardFilter(this ICardSearch obj)
        {
            var s = obj.CardSearchConditions;
            CardSearchConditions.Copy(obj.DefaultCardSearchConditions, s, false);
            s.SearchText = "";
            UpdateCardFilter(obj);
        }

        public static void OpenCardSearchWindow<T>(this T obj)
            where T : DependencyObject, ICardSearch
        {
            var owner = Window.GetWindow(obj);
            CardSearchWindow window = new() { Owner = owner };
            window.Setup(obj.CardSearchConditions, obj.DefaultCardSearchConditions);
            window.Search += obj.CardList_SearchExecuted;
            window.PlaceToCursor(-32, -16, owner);
            window.ShowDialog();
        }

        public static void CardList_RequestSearch(this ICardSearch obj, object sender, RoutedEventArgs<string> e)
        {
            e.Handled = true;
            obj.CardSearchConditions.SearchText = e.Value;
            UpdateCardFilter(obj);
        }

        public static void CardList_RequestClear(this ICardSearch obj, object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            ClearCardFilter(obj);
        }

        public static void CardList_RequestOpenSearch<T>(this T obj, object sender, ExecutedRoutedEventArgs e)
            where T : DependencyObject, ICardSearch
        {
            e.Handled = true;
            OpenCardSearchWindow(obj);
        }

        private static void CardList_SearchExecuted(this ICardSearch obj, object? sender, EventArgs e) => UpdateCardFilter(obj);
    }
}
