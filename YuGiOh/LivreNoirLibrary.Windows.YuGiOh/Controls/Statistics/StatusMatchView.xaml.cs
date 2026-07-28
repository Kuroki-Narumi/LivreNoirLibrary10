using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
    /// StatusMatchView.xaml の相互作用ロジック
    /// </summary>
    public partial class StatusMatchView : UserControl, IProgressReporter, IToggleButtonContainer
    {
        public MatchConditions MatchConditions { get; } = new(MatchConditions.Sculptor);

        UIElement IProgressReporter.MainElement => MainGrid;
        TaskProgressBar IProgressReporter.ProgressBar => TaskProgressBar;
        Task? IProgressReporter.WorkingTask { get; set; }
        bool IToggleButtonContainer.MousePressed { get; set; }
        bool IToggleButtonContainer.MouseToggleState { get; set; }

        [DependencyProperty]
        private ICardEnumerable? _itemsSource;
        [DependencyProperty(BindsTwoWayByDefault = true)]
        private MatchCard? _selectedItem;

        private readonly List<Card> _source = [];
        private readonly List<MatchCard> _candidates = [];
        private readonly ObjectCache<MatchCard> _cache = new(() => new());

        public StatusMatchView()
        {
            InitializeComponent();
            MainGrid.DataContext = this;

            this.RegisterCommand(YgoCommands.RefreshItems, OnExeucted_Refresh);
            this.InitializeIToggleButtonContainer();
        }

        private void OnExeucted_Refresh(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
            if (ItemsSource is { } source)
            {
                _cache.Clear();
                ListView_Candidates.ItemsSource = null;
                this.StartTask(mainProcess: Process_Refresh, finished: Refresh_Finished);
            }
        }

        private void Process_Refresh(ProgressReporter p, CancellationToken c)
        {
            using var sw = ExStopwatch.ProcessTime(nameof(Process_Refresh));
            p.ReportInitial("preparing...");
            var list = _source;
            list.Clear();
            list.AddRange(ItemsSource!.CardEnumerable.Where(Extensions.IsMonster));
            MatchConditions.BuildMatchList(list, _candidates, _cache.GetNext, p, c);
        }

        private void Refresh_Finished(bool aborted)
        {
            ListView_Candidates.ItemsSource = _candidates;
        }

        private void OnSelectedItemChanged(MatchCard? value)
        {
            if (value is not null)
            {
                ListView_Targets.ItemsSource = MatchConditions.EnumerateMatches(value.ThisCard, _source);
            }
            else
            {
                ListView_Targets.ItemsSource = null;
            }
        }

        private void OnMouseWheel_RadioContainer(object sender, MouseWheelEventArgs e)
        {
            (sender as Panel)?.ChangeRadioButtonByWheel(e);
        }

        private void OnClick_Preset_Sculptor(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MatchConditions.CopyFrom(MatchConditions.Sculptor);
        }

        private void OnClick_Preset_SmallWorld(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MatchConditions.CopyFrom(MatchConditions.SmallWorld);
        }

        private void OnClick_Preset_Nightmell(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MatchConditions.CopyFrom(MatchConditions.Nightmell);
        }

        private void OnClick_Preset_Hedgehog(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MatchConditions.CopyFrom(MatchConditions.Hedgehog);
        }
    }
}
