using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.MasterDuel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LivreNoirLibrary.Windows.YuGiOh.Controls
{
    /// <summary>
    /// DuelLogStatisticsView.xaml の相互作用ロジック
    /// </summary>
    public partial class DuelLogStatisticsView : UserControl, IProgressReporter, IToggleButtonContainer, IGridViewSort
    {
        UIElement IProgressReporter.MainElement => MainGrid;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get; set; }
        bool IToggleButtonContainer.MousePressed { get; set; }
        bool IToggleButtonContainer.MouseToggleState { get; set; }

        public DuelLogSearchFlags Flags { get; } = new();
        public LivreNoirLibrary.YuGiOh.MasterDuel.DuelLogStatistics Statistics { get; } = new();

        [DependencyProperty]
        private ICardProvider? _cardProvider;
        [DependencyProperty]
        private ICollection<DuelLog>? _itemsSource;
        [DependencyProperty]
        private DuelLogSearchConditions? _searchConditions;
        [DependencyProperty]
        private DeckTagCollection? _deckTagSource;

        private readonly Dictionary<ListBox, (string?, bool)> _listSort;

        public DuelLogStatisticsView()
        {
            InitializeComponent();
            SearchConditions ??= new();
            MainGrid.DataContext = this;
            this.InitializeIToggleButtonContainer();
            _listSort = new()
            {
                [ListView_Logs] = (nameof(DuelLog.DateTime), true),
            };
            (this as IGridViewSort).SortBy(ListView_Logs, nameof(DuelLog.DateTime));
            this.RegisterCommand(ApplicationCommands.New, OnExecuted_AllClear);
        }

        private void OnSearchConditionsChanged(DuelLogSearchConditions? value)
        {
            if (value is not null)
            {
                UserSelector.SetFlags(value.UserTags);
                OpponentSelector.SetFlags(value.OpponentTags);
                Flags.Load(value);
            }
        }

        private void OnClick_ColumnHeader(object sender, RoutedEventArgs e) => ControlExtensions.OnClick_ColumnHeader(this, sender, e);

        void IGridViewSort.SortBy(ListBox control, string key)
        {
            var (prop, ascending) = _listSort.GetValueOrDefault(control);
            DuelLogEditor.SortDuelLogGridView(control, key, ref prop, ref ascending);
            _listSort[control] = (prop, ascending);
        }

        private void OnExecuted_AllClear(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (SearchConditions is { } cond)
            {
                cond.Clear();
                OnSearchConditionsChanged(cond);
            }
        }

        private void User_TagChanged(object sender, RoutedEventArgs<IEnumerable<string>> e)
        {
            SearchConditions?.SetUserTags(e.Value);
        }

        private void Opponent_TagChanged(object sender, RoutedEventArgs<IEnumerable<string>> e)
        {
            SearchConditions?.SetOpponentTags(e.Value);
        }

        private void DDMB_Rank_Closed(object sender, EventArgs e)
        {
            if (SearchConditions is { } cond)
            {
                Flags.SaveRanks(cond);
            }
        }

        private void DDMB_Order_Closed(object sender, EventArgs e)
        {
            if (SearchConditions is { } cond)
            {
                Flags.SaveOrders(cond);
            }
        }

        private void DDMB_Result_Closed(object sender, EventArgs e)
        {
            if (SearchConditions is { } cond)
            {
                Flags.SaveResults(cond);
            }
        }

        private void OnClick_Refresh(object sender, RoutedEventArgs e)
        {
            this.StartTask(ProcessRefresh, finished: OnFinished_Refresh, initialReport: ProgressReport.Initial("refreshing statistics..."));
        }

        private void ProcessRefresh(ProgressReporter p, CancellationToken c)
        {
            if (SearchConditions is { } cond && ItemsSource is { } logSource)
            {
                var stats = Statistics;
                stats.BeginInit();
                stats.Update(CardProvider, logSource, cond);
            }
        }

        private void OnFinished_Refresh(bool aborted)
        {
            var stats = Statistics;
            stats.EndInit();
            AdjustGridViewColumn(ListView_DeckTagSet, stats.DeckTagSet);
            AdjustGridViewColumn(ListView_DeckTagSingle, stats.DeckTagSingle);
            AdjustGridViewColumn(ListView_InitialHand, stats.InitialHand);
            AdjustGridViewColumn(ListView_TotalHand, stats.TotalHand);
        }

        private static void AdjustGridViewColumn(ListView lv, StatisticsCollectionBase source)
        {
            if (lv.View is GridView gv)
            {
                var columns = gv.Columns;
                for (var i = 1; i < columns.Count; i++)
                {
                    var column = columns[i];
                    if (source.IsEmptyRow(i))
                    {
                        column.Width = 0;
                    }
                    else
                    {
                        if (double.IsNaN(column.Width))
                        {
                            column.Width = column.ActualWidth;
                        }
                        column.Width = double.NaN;
                    }
                }
            }
        }

        private void OnClick_CopyImage(object sender, RoutedEventArgs e)
        {
            using var o = ObjectPool.RentStringBuilder(out var sb);
            StatisticsCollectionBase stats;
            VocabData firstRowHeader;
            switch (StatisticsView.SelectedIndex)
            {
                case 0:
                    stats = Statistics.DeckTagSet;
                    firstRowHeader = Vocab.Current.DLog.Header_Tag;
                    break;
                case 1:
                    stats = Statistics.DeckTagSingle;
                    firstRowHeader = Vocab.Current.DLog.Header_Tag;
                    break;
                case 2:
                    stats = Statistics.InitialHand;
                    firstRowHeader = Vocab.Current.DLog.Header_Card;
                    break;
                case 3:
                    stats = Statistics.TotalHand;
                    firstRowHeader = Vocab.Current.DLog.Header_Card;
                    break;
                default:
                    return;
            }
            WriteHeader(sb, firstRowHeader);
            stats.AppendItemLines(sb);
            sb.AppendLine();

            var text = sb.ToString();
            try
            {
                Clipboard.SetText(text);
            }
            catch { }
        }

        private static void WriteHeader(StringBuilder sb, VocabData firstRowHeader)
        {
            var vocab = Vocab.Current.DLog;
            sb.Append(firstRowHeader.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Total.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Win.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Lose.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Draw.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_DiscWin.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_DiscLose.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_WinLike.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_First.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Second.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_CFirst.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_CSecond.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_FirstWin.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_SecondWin.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_CFirstWin.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_CSecondWin.Value);
            sb.Append('\t');
            sb.Append(vocab.Header_Percent.Value);
            sb.AppendLine();
        }
    }
}
