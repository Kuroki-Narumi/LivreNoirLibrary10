using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Text;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LivreNoirLibrary.Windows.Media;
using System.Diagnostics.CodeAnalysis;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.SandBox
{
    /// <summary>
    /// Unit_YuGiOh.xaml の相互作用ロジック
    /// </summary>
    public partial class Unit_YuGiOh : UserControl, IProgressReporter
    {
        public static CardPool CardPool => CardPool.Instance;
        public static string CardPoolFilePath { get; } = YuGiOh.Utils.GetFullPath(CardPool.DefaultResourceName);
        public TextSearchConditions TextSearchConditions { get; } = new();

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
        }

        private void OnClick_Search(object sender, RoutedEventArgs e)
        {
            using var t = ExStopwatch.ProcessTime("Search");
            var text = SearchInput.Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                ListView_Cards.Items.Filter = null;
                return;
            }
            var conds = TextSearchConditions;
            conds.Text = text;
            conds.Prepare();
            ListView_Cards.Items.Filter = item => item is ICard c && conds.IsMatch(c);
        }

        private void OnClick_Test(object sender, RoutedEventArgs e)
        {
            this.StartTask(asyncProcess: MainProcess, isAbortable: false);
        }

        private async Task MainProcess(ProgressReporter p, CancellationToken c)
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
    }
}
