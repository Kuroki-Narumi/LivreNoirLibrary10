using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public interface ICardSort : ICardListView
    {
        CardSortOptionCollection CardSortOptions { get; }
        void OnCardSortExecuted(SortDescriptionCollection options) { }
    }

    public static class ICardSortExtensions
    {        
        public static void RegisterCardSortCommands<T>(this UIElement element, T owner)
            where T : UIElement, ICardSort
        {
            element.RegisterCommand(YgoCommands.OpenSort, owner.CardList_RequestOpenSort);
        }

        public static void RegisterCardSortCommands<T>(this T obj) where T : UIElement, ICardSort => RegisterCardSortCommands(obj, obj);

        public static void UpdateCardSort(this ICardSort obj)
        {
            obj.CardListBox.ApplySortDescriptions(obj.CardSortOptions);
            obj.OnCardSortExecuted(obj.CardListBox.Items.SortDescriptions);
        }

        public static void CardList_RequestOpenSort<T>(this T obj, object sender, ExecutedRoutedEventArgs e)
            where T : DependencyObject, ICardSort
        {
            e.Handled = true;
            var owner = Window.GetWindow(obj);
            CardSortWindow window = new() { Owner = owner };
            window.Setup(obj.CardSortOptions);
            window.Sort += obj.CardList_SortExecuted;
            window.PlaceToCursor(-32, -16, owner);
            window.ShowDialog();
        }

        private static void CardList_SortExecuted(this ICardSort obj, object? sender, EventArgs e) => UpdateCardSort(obj);

        public static readonly SortDescription DefaultSortDescription = new(nameof(Card.Id), ListSortDirection.Ascending);

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
            desc.Add(DefaultSortDescription);

            object? item = null;
            if (control.SelectionMode is SelectionMode.Single)
            {
                item = control.SelectedItem;
            }
            else if (control.SelectedItems.Count is > 0)
            {
                item = control.SelectedItems[0];
            }
            if (item is not null)
            {
                control.ScrollIntoView(item);
            }
        }

        public static void ApplySortDescriptions(this ListBox control, CardSortOptionCollection options)
        {
            var selectedItem = control.SelectedItem;
            var desc = control.Items.SortDescriptions;
            desc.Clear();
            foreach (var option in options)
            {
                if (option.Key is not 0)
                {
                    desc.Add(new(option.PropertyName, option.Direction));
                }
            }
            desc.Add(DefaultSortDescription);
            if (selectedItem is not null)
            {
                control.ScrollIntoView(selectedItem);
            }
        }
    }
}
