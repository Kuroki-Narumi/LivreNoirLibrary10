using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.Search;
using System.Windows;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public interface ITextSearch
    {
        TextSearchConditions? TextSearchConditions { get; }
        string? DefaultSearchText => "";
        TextSearchFlags DefaultTextFlags => TextSearchFlags.Default;
        void SetSearchText(string? text);
        void OnTextSearchExecuted() { }
    }

    public static class ITextSearchExtensions
    {
        public static void RegisterTextSearchCommands<T>(this UIElement element, T owner)
            where T : DependencyObject, ITextSearch
        {
            element.RegisterCommand(Commands.TextSearch, owner.OnExecuted_Search, owner.CanExecute_Search);
            element.RegisterCommand(Commands.TextSearchClear, owner.OnExecuted_Clear, owner.CanExecute_Search);
        }

        public static void RegisterTextSearchCommands<T>(this T obj) where T : UIElement, ITextSearch => RegisterTextSearchCommands(obj, obj);

        public static void UpdateTextFilter(this ITextSearch obj)
        {
            if (obj.TextSearchConditions is { } conds)
            {
                conds.PrepareText();
                obj.SetSearchText(conds.SearchText);
                if (obj is ISearchListBox { SearchListBox: { } lv })
                {
                    lv.Items.Filter = item => item is INamedObject o && conds.IsTextMatch(o);
                    lv.ScrollSelectedItemIntoView();
                }
                obj.OnTextSearchExecuted();
            }
        }

        public static void ClearTextFilter(this ITextSearch obj)
        {
            if (obj.TextSearchConditions is { } conds)
            {
                conds.SearchText = obj.DefaultSearchText;
                conds.TextFlags = obj.DefaultTextFlags;
                UpdateTextFilter(obj);
            }
        }

        private static void CanExecute_Search(this ITextSearch obj, object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = obj.TextSearchConditions is not null;
        }

        private static void OnExecuted_Search(this ITextSearch obj, object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            obj.TextSearchConditions?.SearchText = e.Parameter as string;
            UpdateTextFilter(obj);
        }

        private static void OnExecuted_Clear(this ITextSearch obj, object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            ClearTextFilter(obj);
        }
    }
}
