using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Controls;
using LivreNoirLibrary.Windows.Input;
using LivreNoirLibrary.YuGiOh.Data;
using LivreNoirLibrary.YuGiOh.Inspect;
using LivreNoirLibrary.YuGiOh.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
    /// HandTester.xaml の相互作用ロジック
    /// </summary>
    public partial class HandTestView : UserControl, IGridViewSort
    {
        public ObservableList<HandConditionResult> Results { get; } = [];

        [DependencyProperty]
        private IProgressReporter? _progressReporter;
        [DependencyProperty]
        private HandConditionsCollection? _conditions;
        [DependencyProperty]
        private IIdEnumerable? _cardSource;
        [DependencyProperty]
        private HandTestParams? _testParams;
        [DependencyProperty(SetterScope = Scope.Private)]
        private string? _resultText;
        [DependencyProperty(SetterScope = Scope.Private)]
        private string? _sumText;

        bool IGridViewSort.ClearSortIfEmptyTag => true;

        private readonly HandTestResult _result = new();
        private readonly Dictionary<int, double> _sumBuffer = [];
        private string? _currnetProp;
        private bool _currentAscending;

        public HandTestView()
        {
            InitializeComponent();
            MainGrid.DataContext = this;

            var lv = ListView_Priority;
            lv.RegisterCommand(Commands.MoveUp, lv.OnExecuted_MoveUp, lv.CanExecute_MoveUp);
            lv.RegisterCommand(Commands.MoveDown, lv.OnExecuted_MoveDown, lv.CanExecute_MoveDown);
        }

        private void OnClick_Start(object sender, RoutedEventArgs e)
        {
            if (CardSource is null || Conditions is null || TestParams is null)
            {
                return;
            }
            if (ProgressReporter is { } prog)
            {
                prog.StartTask(mainProcess: Test_MainProcess, finished: Test_Finished);
            }
            else
            {
                Test_MainProcess(null, default);
                Test_Finished(false);
            }
        }

        private void Test_MainProcess(ProgressReporter? p, CancellationToken c)
        {
            HandTest.Run(CardSource!, Conditions!, TestParams!, _result, p, c);
        }

        private void Test_Finished(bool aborted)
        {
            var result = _result;
            var r = Results;
            r.ClearWithoutNotify();
            r.AddRange(result.Conditions);

            using var o = ObjectPool.RentStringBuilder(out var sb);
            sb.AppendLine($"Count: {result.TotalCount}");
            sb.AppendLine();
            sb.AppendLine("Value1");
            result.Value1.AppendTo(sb);
            sb.AppendLine("Value2");
            result.Value2.AppendTo(sb);
            sb.AppendLine();
            foreach (var item in result.GroupResults.AsSpan())
            {
                sb.AppendLine($"Group {item.GroupId}: {item.Count}({item.ProbText})");
            }
            ResultText = sb.ToString();
        }

        private void OnClick_ColumnHeader(object sender, RoutedEventArgs e) => ControlExtensions.OnClick_ColumnHeader(this, sender, e);

        public void SortBy(ListBox control, string key)
        {
            var desc = control.Items.SortDescriptions;
            desc.Clear();
            var isDescending = key == _currnetProp && _currentAscending;
            var dir = isDescending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            _currnetProp = key;
            _currentAscending = !isDescending;
            desc.Add(new(key, dir));
            control.ScrollSelectedItemIntoView();
        }

        private void Result_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListView { SelectedItems: { } items })
            {
                return;
            }
            if (items.Count is 0)
            {
                SumText = null;
            }
            else
            {
                var sum = _sumBuffer;
                foreach (var item in items)
                {
                    if (item is HandConditionResult r)
                    {
                        var g = r.GroupId;
                        sum[g] = (sum.TryGetValue(g, out var current) ? current : 0) + r.Probability;
                    }
                }
                using var o = ObjectPool.RentStringBuilder(out var sb);
                var prob = 1.0;
                var second = false;
                foreach (var (g, p) in sum)
                {
                    if (second)
                    {
                        sb.Append(" * ");
                    }
                    prob *= p;
                    sb.Append($"G{g}:{p:P2}");
                    second = true;
                }
                if (sum.Count >= 2)
                {
                    sb.Append($" = {prob:P2}");
                }
                SumText = sb.ToString();
                sum.Clear();
            }
        }
    }
}
