using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.ComponentModel;
using System.DirectoryServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public interface ICardSort : ISearchListBox
    {
        bool CanOpenSortDialog => true;
        CardSortOptionCollection? CardSortOptions { get; }
        CardSortOptionCollection? DefaultCardSortOptions => null;
        void OnCardSortExecuted(SortDescriptionCollection descriptions) { }
    }

    public static class ICardSortExtensions
    {        
        public static void RegisterCardSortCommands<T>(this UIElement element, T owner)
            where T : UIElement, ICardSort
        {
            element.RegisterCommand(Commands.OpenSortDialog, owner.OnExecuted_OpenSort, owner.CanExecute_OpenSort);
        }

        public static void RegisterCardSortCommands<T>(this T obj) where T : UIElement, ICardSort => RegisterCardSortCommands(obj, obj);

        public static void UpdateCardSort(this ICardSort obj)
        {
            if (obj.SearchListBox is { } lb)
            {
                lb.ApplySortDescriptions(obj.CardSortOptions);
                obj.OnCardSortExecuted(lb.Items.SortDescriptions);
            }
        }

        public static void ClearCardSort(this ICardSort obj)
        {
            if (obj.CardSortOptions is { } ops)
            {
                ops.Clear();
                if (obj.DefaultCardSortOptions is { } list)
                {
                    ops.AddRange(list);
                }
            }
            UpdateCardSort(obj);
        }

        public static void OpenCardSortWindow<T>(this T obj)
            where T : DependencyObject, ICardSort
        {
            var owner = Window.GetWindow(obj);
            CardSortWindow window = new(obj) { Owner = owner };
            window.Sort += obj.SortWindow_OnSort;
            window.PlaceToCursor(-32, -16, owner);
            window.ShowDialog();
        }

        private static void CanExecute_OpenSort(this ICardSort obj, object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = obj.CanOpenSortDialog && obj.CardSortOptions is not null && obj.SearchListBox is { } lb && !(lb is ListView { View: GridView });
        }

        private static void OnExecuted_OpenSort<T>(this T obj, object sender, ExecutedRoutedEventArgs e)
            where T : DependencyObject, ICardSort
        {
            e.Handled = true;
            OpenCardSortWindow(obj);
        }

        private static void SortWindow_OnSort(this ICardSort obj, object? sender, EventArgs e) => UpdateCardSort(obj);

        public static readonly SortDescription DefaultSortDescription = new("ThisCard.Id", ListSortDirection.Ascending);

        public static void UpdateSort(this ListBox control, string propName, ref string? currentSort, ref bool currentAscending)
        {
            var desc = control.Items.SortDescriptions;
            desc.Clear();
            var isDescending = propName == currentSort && currentAscending;
            var dir = isDescending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            currentSort = propName;
            currentAscending = !isDescending;
            propName = CardSortOption.GetActualProperyName(propName, isDescending);
            desc.Add(new(propName, dir));
            if (propName != "ThisCard.Id")
            {
                desc.Add(DefaultSortDescription);
            }
            control.ScrollSelectedItemIntoView();
        }

        public static void ApplySortDescriptions(this ListBox control, CardSortOptionCollection? options)
        {
            var desc = control.Items.SortDescriptions;
            desc.Clear();
            if (options is not null)
            {
                var containsId = false;
                foreach (var option in options)
                {
                    if (option.Key is not 0)
                    {
                        desc.Add(new(option.PropertyName, option.Direction));
                        if (option.PropertyName is "ThisCard.Id")
                        {
                            containsId = true;
                        }
                    }
                }
                if (!containsId)
                {
                    desc.Add(DefaultSortDescription);
                }
            }
            control.ScrollSelectedItemIntoView();
        }
    }
}
