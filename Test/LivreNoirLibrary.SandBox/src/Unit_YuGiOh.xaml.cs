using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.YuGiOh;
using LivreNoirLibrary.Windows.YuGiOh.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System.Windows;
using System.Windows.Controls;

namespace LivreNoirLibrary.SandBox
{
    /// <summary>
    /// Unit_YuGiOh.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_YuGiOh : UserControl, IProgressReporter
    {
        public static CardPool CardPool => CardPool.Instance;
        public static Regulation Regulation => Regulation.Instance;
        public static string CardPoolFilePath { get; } = YuGiOh.Utils.GetFullPath(CardPool.DefaultResourceName);
        public static string RegulationFilePath { get; } = YuGiOh.Utils.GetFullPath("Resources/Regulation.json");
        public CardSearchConditions CardSearchConditions { get; } = new();
        public CardSortOptionCollection CardSortOptions { get; } = [];

        UIElement IProgressReporter.MainElement => MainUI;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get; set; }

        public Unit_YuGiOh()
        {
            InitializeComponent();
            Dispatcher.Invoke(InitializeCardPool);
        }

        private void InitializeCardPool()
        {
            CardPool.LoadFile(CardPoolFilePath);
            Regulation.LoadFile(RegulationFilePath);
            CardPool.Cards.NotifyLimitChanged();
        }

        private void OnRequestSearch(object sender, RoutedEventArgs<string> e)
        {
            CardSearchConditions.SearchText = e.Value;
            UpdateFilter();
        }

        private void OnRequestOpenSort(object sender, RoutedEventArgs e)
        {
            var owner = Window.GetWindow(this);
            CardSortWindow window = new() { Owner = owner };
            window.Setup(CardSortOptions);
            window.Sort += Window_SortExecuted;
            window.PlaceToCursor(-32, -16, owner);
            window.ShowDialog();
        }

        private void OnRequestOpenSearch(object sender, RoutedEventArgs e)
        {
            var owner = Window.GetWindow(this);
            CardSearchWindow window = new() { Owner = owner };
            window.Setup(CardSearchConditions);
            window.Search += Window_SearchExecuted;
            window.PlaceToCursor(-32, -16, owner);
            window.ShowDialog();
        }

        private void OnRequestClear(object sender, RoutedEventArgs e)
        {
            var s = CardSearchConditions;
            CardSearchConditions.CopyTo(CardSearchConditions.Default, s, false);
            s.SearchText = "";
            UpdateFilter();
        }

        private void Window_SortExecuted(object? sender, EventArgs e) => UpdateSort();

        private void UpdateSort()
        {
            ListView_Cards.ApplySortDescriptions(CardSortOptions);
        }

        private void Window_SearchExecuted(object? sender, EventArgs e) => UpdateFilter();

        private void UpdateFilter()
        {
            using var t = ExStopwatch.ProcessTime("Search");
            var conds = CardSearchConditions;
            conds.Prepare();
            SearchBar.SearchText = conds.SearchText;
            ListView_Cards.Items.Filter = item => item is ICard c && conds.IsMatch(c);
        }

        private void OnClick_Test(object sender, RoutedEventArgs e)
        {
            this.StartTask(asyncProcess: UpdateDatabase, isAbortable: false);
        }

        private async Task UpdateDatabase(ProgressReporter p, CancellationToken c)
        {
            var database = CardPool;
            var ids = await YuGiOh.Scraping.CardPack.GetCardList(database.Packs, p, c);
            await YuGiOh.Scraping.Card.UpdateAllCards(ids, database.Cards, database.Packs, p, c);
            database.SaveJson(CardPoolFilePath);

            await Dispatcher.BeginInvoke(() =>
            {
                if (ids.Count is 0)
                {
                    MessageBox.Show("更新はありません。");
                }
                else
                {
                    MessageBox.Show($"{ids.Count}件のカード情報を更新しました。");
                }
            });
        }

        private void OnClick_Regulation(object sender, RoutedEventArgs e)
        {
            this.StartTask(asyncProcess: UpdateRegulation, isAbortable: false);
        }

        private async Task UpdateRegulation(ProgressReporter p, CancellationToken c)
        {
            await YuGiOh.Scraping.Regulation.Update(Regulation, false, p, c);
            Regulation.SaveJson(RegulationFilePath);
            CardPool.Cards.NotifyLimitChanged();
        }

        private void CardInfoView_CardLinkClicked(object sender, CardLinkClickedEventArgs e)
        {
            this.OpenUrl_Card(e.Id, e.IsTcg);
        }

        private void CardInfoView_PackLinkClicked(object sender, RoutedEventArgs<string> e)
        {
            this.OpenUrl_Pack(e.Value);
        }

        private void CardInfoView_Detach(object sender, RoutedEventArgs<Card> e)
        {
            Window_CardInfo window = new(e.Value, Window.GetWindow(this), new()
            {
                CardLink = CardInfoView_CardLinkClicked,
                PackLink = CardInfoView_PackLinkClicked,
                RelatedText = OnRequestSearch,
            });
            window.Show();
        }
    }
}
