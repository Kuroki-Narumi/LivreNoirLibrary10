using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.YuGiOh;
using LivreNoirLibrary.Windows.YuGiOh.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoir.YuGiOhDatabase
{
    /// <summary>
    /// Unit_Database.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_Database : UserControl, ICardSearch, ICardSort, IPackSearch
    {
        private static MainViewModel ViewModel => MainViewModel.Instance;

        ListBox ICardListView.CardListBox => ListView_CardList;
        public ICardProvider? CardProvider => ViewModel.CardPool.Cards;
        CardSearchConditions ICardSearch.CardSearchConditions => ViewModel.Database.CardSearchConditions;
        CardSearchConditions ICardSearch.DefaultCardSearchConditions => CardSearchConditions.Default;
        CardSortOptionCollection ICardSort.CardSortOptions => ViewModel.Database.CardSortOptions;
        PackSearchConditions IPackSearch.PackSearchConditions => ViewModel.Database.PackSearchConditions;
        PackSearchConditions IPackSearch.DefaultPackSearchConditions => PackSearchConditions.Default;
        ListBox IPackSearch.PackListBox => ListView_PackList;

        public Unit_Database()
        {
            InitializeComponent();
            CardClipboard.RegisterCopy(ListView_CardList);
            Tab_CardList.RegisterCardSearchCommands(this);
            Tab_CardList.RegisterCardSortCommands(this);
            Tab_PackList.RegisterPackSearchCommands(this);
        }

        private void OnSelectionChanged_Tab(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.Database.IsUpdateVisible = sender is TabControl { SelectedItem: TabItem item } && (item == Tab_CardList || item == Tab_PackList);
        }

        private void CardList_OnClick_Detach(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            CardListWindow window = new(Window.GetWindow(this), ViewModel.CardPool.Cards);
            HyperLinks.AddPackLinkClickedHandler(window, CardListWindow_PackLinkClicked);
            window.Show();
        }

        private void CardListWindow_PackLinkClicked(object sender, RoutedEventArgs<string> e)
        {
            Window.GetWindow(this).Activate();
            CardInfoView_PackLinkClicked(sender, e);
        }

        private void CardInfoView_CardLinkClicked(object sender, CardLinkClickedEventArgs e)
        {
            e.Handled = true;
            this.OpenUrl_Card(e.Id, e.IsTcg);
        }

        private void CardInfoView_PackLinkClicked(object sender, RoutedEventArgs<string> e)
        {
            e.Handled = true;
            SelectPack(e.Value);
        }

        private void CardInfoView_Detach(object sender, RoutedEventArgs<Card> e)
        {
            e.Handled = true;
            CardInfoWindow window = new(e.Value, Window.GetWindow(this), new()
            {
                CardLink = CardInfoView_CardLinkClicked,
                PackLink = CardInfoView_PackLinkClicked,
                RelatedText = this.CardList_RequestSearch,
            });
            window.Show();
        }

        void ICardSearch.SetCardSearchText(string text) => ViewModel.Database.CardSearchText = text;

        private void PackInfoView_CardLinkClicked(object sender, CardLinkClickedEventArgs e)
        {
            e.Handled = true;
            SelectCard(e.Id);
        }

        private void PackInfoView_PackLinkClicked(object sender, RoutedEventArgs<string> e)
        {
            e.Handled = true;
            this.OpenUrl_Pack(e.Value);
        }

        void IPackSearch.SetPackSearchText(string text) => ViewModel.Database.PackSearchText = text;

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

        void ICardSort.OnCardSortExecuted(SortDescriptionCollection descriptions) => RegulationEditor.OnCardSortExecuted(descriptions);
        public void OnRegulationLoad() => RegulationEditor.OnEdit();
    }
}
