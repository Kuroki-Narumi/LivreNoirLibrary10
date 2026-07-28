using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public interface ICardSearch
    {
        bool CanOpenSearchDialog => true;
        ICardProvider? CardProvider { get; }
        CardSearchConditions? CardSearchConditions { get; }
        CardSearchConditions? DefaultCardSearchConditions { get; }
        void SetCardSearchText(string? text);
        void OnCardSearchExecuted() { }
    }

    public static class ICardSearchExtensions
    {
        public static void RegisterCardSearchCommands<T>(this UIElement element, T owner)
            where T : DependencyObject, ICardSearch
        {
            element.RegisterCommand(Commands.OpenSearchDialog, owner.OnExecuted_OpenSearch, owner.CanExecute_OpenSearch);
            element.RegisterCommand(Commands.TextSearch, owner.OnExecuted_Search, owner.CanExecute_Search);
            element.RegisterCommand(Commands.TextSearchClear, owner.OnExecuted_SearchClear, owner.CanExecute_Search);
        }

        public static void RegisterCardSearchCommands<T>(this T obj) where T : UIElement, ICardSearch => RegisterCardSearchCommands(obj, obj);

        public static void UpdateCardFilter(this ICardSearch obj)
        {
            if (obj.CardSearchConditions is { } conds)
            {
                conds.Prepare();
                obj.SetCardSearchText(conds.SearchText);
                if (obj is ISearchListBox { SearchListBox: { } lv })
                {
                    lv.Items.Filter = item => conds.IsMatch(item, obj.CardProvider);
                    lv.ScrollSelectedItemIntoView();
                }
                obj.OnCardSearchExecuted();
            }
        }

        public static void CardTextSearch(this ICardSearch obj, string? text)
        {
            obj.CardSearchConditions?.SearchText = text;
            UpdateCardFilter(obj);
        }

        public static void ClearCardFilter(this ICardSearch obj)
        {
            if (obj.CardSearchConditions is { } s)
            {
                CardSearchConditions.Copy(obj.DefaultCardSearchConditions ?? CardSearchConditions.Default, s, true);
                UpdateCardFilter(obj);
            }
        }

        public static void OpenCardSearchWindow<T>(this T obj)
            where T : DependencyObject, ICardSearch
        {
            var owner = Window.GetWindow(obj);
            CardSearchWindow window = new(obj) { Owner = owner };
            window.Search += obj.CardSearchWindow_OnSearch;
            window.PlaceToCursor(-32, -16, owner);
            window.ShowDialog();
        }

        private static void CanExecute_Search(this ICardSearch obj, object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = obj.CardSearchConditions is not null;
        }

        private static void CanExecute_OpenSearch(this ICardSearch obj, object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = obj.CanOpenSearchDialog && obj.CardSearchConditions is not null;
        }

        private static void OnExecuted_Search(this ICardSearch obj, object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            CardTextSearch(obj, e.Parameter as string);
        }

        private static void OnExecuted_SearchClear(this ICardSearch obj, object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            ClearCardFilter(obj);
        }

        private static void OnExecuted_OpenSearch<T>(this T obj, object sender, ExecutedRoutedEventArgs e)
            where T : DependencyObject, ICardSearch
        {
            e.Handled = true;
            OpenCardSearchWindow(obj);
        }

        private static void CardSearchWindow_OnSearch(this ICardSearch obj, object? sender, EventArgs e) => UpdateCardFilter(obj);
    }
}
