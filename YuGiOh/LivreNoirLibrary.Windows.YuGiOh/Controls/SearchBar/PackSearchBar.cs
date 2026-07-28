using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    public partial class PackSearchBar : SearchBarBase, IPackSearch
    {
        [DependencyProperty]
        private PackSearchConditions? _defaultSearchConditions;

        public PackSearchConditions PackSearchConditions { get; } = new();
        PackSearchConditions? IPackSearch.DefaultPackSearchConditions => DefaultSearchConditions;

        ListBox? IPackSearch.PackListBox => ListBox;

        public PackSearchBar()
        {
            this.RegisterPackSearchCommands();
        }

        private void OnDefaultSearchConditionsChanged()
        {
            this.ClearPackFilter();
        }

        void IPackSearch.SetPackSearchText(string? text) => SearchText = text;
    }
}
