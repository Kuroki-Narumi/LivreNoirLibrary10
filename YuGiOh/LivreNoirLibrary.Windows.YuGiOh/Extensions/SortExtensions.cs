using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;

namespace LivreNoirLibrary.Windows.YuGiOh
{
    public static class SortExtensions
    {
        public static readonly SortDescription DefaultSortDescription = new(nameof(Card.Id), ListSortDirection.Ascending);

        public static void UpdateSort(this ListBox control, string propName, ref string? currentSort, ref bool currentAscending, bool add_d = true)
        {
            var selectedItem = control.SelectedItem;
            var desc = control.Items.SortDescriptions;
            desc.Clear();
            var dir = propName == currentSort && currentAscending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            currentSort = propName;
            currentAscending = dir is ListSortDirection.Ascending;
            if (add_d && !currentAscending)
            {
                switch (propName)
                {
                    case nameof(Card.Attribute):
                    case nameof(Card.MonsterType):
                    case nameof(Card.Ability):
                    case nameof(Card.Level):
                    case nameof(Card.Atk):
                    case nameof(Card.Def):
                    case nameof(Card.PendulumScale):
                        propName += "D";
                        break;
                }
            }
            desc.Add(new(propName, dir));
            desc.Add(DefaultSortDescription);
            if (selectedItem is not null)
            {
                control.ScrollIntoView(selectedItem);
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
                    desc.Add(new(option.PropertyName, option.Direction is SortDirection.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending));
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
