using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.YuGiOh;
using LivreNoirLibrary.Windows.YuGiOh.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// Unit_Database.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_Database : UserControl
    {
        private static MainViewModel ViewModel => MainViewModel.Instance;

        public Unit_Database()
        {
            InitializeComponent();
            CardClipboard.RegisterCopy(ListView_CardList);

            var cond = CardSearchConditions.Default;
            foreach (var preset in ViewModel.CardSearchPresets)
            {
                if (preset.IsDefault)
                {
                    CardSearchConditions.Copy(preset.Conditions, cond, true);
                    break;
                }
            }
            UpdateDefaultSearchConditions(cond);
            CardSearchWindow.DefaultPresetChanged += CardSearchWindow_DefaultPresetChanged;

            CardSortOptionCollection op = [];
            foreach (var preset in ViewModel.CardSortPresets)
            {
                if (preset.IsDefault)
                {
                    op.AddRange(preset.Conditions);
                    break;
                }
            }
            UpdateDefaultSortOptions(op);
            CardSortWindow.DefaultPresetChanged += CardSortWindow_DefaultPresetChanged;
        }

        private void CardSearchWindow_DefaultPresetChanged(object? sender, CardSearchConditionsPreset? e) => UpdateDefaultSearchConditions(e?.Conditions);
        private static void UpdateDefaultSearchConditions(CardSearchConditions? source)
        {
            CardSearchConditions.Copy(source ?? CardSearchConditions.Default, ViewModel.Database.DefaultCardSearchConditions, true);
        }

        private void CardSortWindow_DefaultPresetChanged(object? sender, CardSortOptionsPreset? e) => UpdateDefaultSortOptions(e?.Conditions);

        private static void UpdateDefaultSortOptions(IEnumerable<CardSortOption>? source)
        {
            var o = ViewModel.Database.DefaultCardSortOptions;
            o.Clear();
            if (source is not null)
            {
                o.AddRange(source);
            }
        }

        private void OnSelectionChanged_Tab(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Database.IsUpdateVisible = sender is TabControl { SelectedItem: TabItem item } && (item == Tab_CardList || item == Tab_PackList);
        }

        public void SelectCard(int id)
        {
            if (ViewModel.CardPool.Cards.TryGet(id, out var card))
            {
                ViewModel.Database.SelectedCard = card;
                ListView_CardList.ScrollIntoView(card);
                Tab_CardList.IsSelected = true;
            }
        }

        public void SelectPack(string pid)
        {
            if (ViewModel.CardPool.Packs.TryGet(pid, out var pack))
            {
                ViewModel.Database.SelectedPack = pack;
                ListView_PackList.ScrollIntoView(pack);
                Tab_PackList.IsSelected = true;
            }
        }

        public void SearchCard(string text)
        {
            Tab_CardList.IsSelected = true;
            SearchBar.CardTextSearch(text);
        }

        public void OnRegulationLoad() => RegulationEditor.OnEdit();
    }
}
