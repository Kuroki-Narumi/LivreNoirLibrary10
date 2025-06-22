using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public partial class BmsViewModel
    {
        public static readonly Rational DefaultSmallGrid = new(1, 16);
        public static readonly Rational DefaultLargeGrid = new(1, 4);
        public static readonly Rational MinimumGrid = new(1, 1920);
        public const double MinumumGridDouble = 1d / 1920d;

        public event EventHandler? RequestRefreshBar;

        [ObservableProperty]
        private Rational _smallGrid = DefaultSmallGrid;
        [ObservableProperty]
        private Rational _largeGrid = DefaultLargeGrid;

        private readonly BarDefItem[] _bars = CreateBarDefItems();
        private static BarDefItem[] CreateBarDefItems()
        {
            var bars = new BarDefItem[Constants.MaxBarNumber + 1];
            for (var i = 0; i <= Constants.MaxBarNumber; i++)
            {
                bars[i] = new(i);
            }
            return bars;
        }

        private readonly BarLineInfo[] _barInfos = new BarLineInfo[Constants.MaxBarNumber + 1];
        private Rational _max_bar_length;
        private bool _need_refresh_grid;
        private readonly List<Rational> _grid_pos_list = [];
        private readonly List<BarLineType> _grid_type_list = [];

        public BarDefItem[] Bars => _bars;

        public BarLineInfo GetBarLineInfo(int number) => _barInfos[number];

        private void ReserveRefreshBar() => RequestRefreshBar?.Invoke(this, EventArgs.Empty);

        internal void RefreshBars(BaseData source)
        {
            var bars = source.Bars;
            foreach (var item in _bars)
            {
                var number = item.Number;
                item.Value = bars.TryGetValue(number, out var value) ? value : 0;
                item.DefaultValue = bars.GetDefault(number);
            }
            var infos = _barInfos;
            var max = Rational.One;
            foreach (var (num, _, pos, len) in source.EachBar(0, Constants.MaxBarNumber))
            {
                infos[num] = new(pos, len);
                if (len > max)
                {
                    max = len;
                }
            }
            _max_bar_length = max;
            ReserveRefreshBar();
        }

        private void OnSmallGridChanged() => ReserveRefreshGrid();
        private void OnLargeGridChanged() => ReserveRefreshGrid();

        private void ReserveRefreshGrid()
        {
            _need_refresh_grid = true;
            _grid_pos_list.Clear();
            _grid_type_list.Clear();
            ReserveRefreshBar();
        }

        private void EnsureGridCore(Rational grid, BarLineType type, Rational length)
        {
            if (grid.IsPositiveThanZero())
            {
                if (grid <= MinumumGridDouble)
                {
                    grid = MinimumGrid;
                }
                var posList = _grid_pos_list;
                var typeList = _grid_type_list;
                var index = typeList.LastIndexOf(type);
                var pos = index is >= 0 ? posList[index] : Rational.Zero;
                while (pos < length)
                {
                    var nextPos = pos + grid;
                    index = posList.BinarySearch(nextPos);
                    if (index is >= 0)
                    {
                        typeList[index] = type;
                    }
                    else
                    {
                        index = ~index;
                        posList.Insert(index, nextPos);
                        typeList.Insert(index, type);
                    }
                    pos = nextPos;
                }
            }
        }

        public void RefreshLinePositions(List<double> headPos, List<double> linePos, List<BarLineType> lineType, double scaleY)
        {
            if (_need_refresh_grid)
            {
                _need_refresh_grid = false;
                EnsureGridCore(_smallGrid, BarLineType.Small, _max_bar_length);
                EnsureGridCore(_largeGrid, BarLineType.Large, _max_bar_length);
            }
            var gridPos = _grid_pos_list;
            var gridType = _grid_type_list;
            var c = gridPos.Count;
            foreach (var (pos, len) in _barInfos)
            {
                var y = pos * scaleY;
                headPos.Add(y);
                linePos.Add(y);
                lineType.Add(BarLineType.Bar);
                for (var i = 0; i < c; i++)
                {
                    var p = gridPos[i];
                    if (p >= len)
                    {
                        break;
                    }
                    linePos.Add(y + gridPos[i] * scaleY);
                    lineType.Add(gridType[i]);
                }
            }
        }

        private bool ProcessBarEdit(Func<BarEditResult> action)
        {
            var result = action();
            if (result is not BarEditResult.NoEffect)
            {
                var cur = _currentData;
                this.OnEdit(true);
                RefreshBars(cur);
                if (result is BarEditResult.NeedRefresh)
                {
                    RefreshNotes(cur);
                }
                else
                {
                    RefreshBarPosition(cur);
                }
                return true;
            }
            return false;
        }

        public bool InsertBar(int number, Rational length) => ProcessBarEdit(() => _currentData.InsertBar(number, length));
        public bool DeleteBar(int number) => ProcessBarEdit(() => _currentData.DeleteBar(number));
        public bool AddBarLineAt(BarPosition pos) => ProcessBarEdit(() => _currentData.AddBarLineAt(pos));
        public bool MergeBars(int number, int count) => ProcessBarEdit(() => _currentData.MergeBar(number, count));
        public bool SplitBar(BarSplitOptions options) => ProcessBarEdit(() => _currentData.SplitBar(options));
        public bool ResizeBar(BarResizeOptions options) => ProcessBarEdit(() => _currentData.ResizeBar(options));
    }
}
