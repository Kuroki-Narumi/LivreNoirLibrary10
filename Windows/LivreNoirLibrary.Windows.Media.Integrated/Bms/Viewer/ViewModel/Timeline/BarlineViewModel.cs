using System;
using System.Collections.Generic;
using System.Windows;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public enum BarlineType : byte { Bar, Large, Small }

    public readonly record struct BarlineInfo(double Position, double Length);

    public partial class BarlineViewModel : DependencyObject
    {
        public event RequestRefreshEventHandler? RequestRefresh;

        public const int DefaultSmallGrid = 16;
        public const int DefaultLargeGrid = 4;
        public const int GridLimit = 1920;

        [DependencyProperty]
        private int _smallGrid = DefaultSmallGrid;
        [DependencyProperty]
        private int _largeGrid = DefaultLargeGrid;
        private double _maxBarLength;

        private bool _need_refresh_grid;
        private readonly BarlineInfo[] _bars = new BarlineInfo[BmsConstants.MaxBarNumber + 1];
        private readonly List<double> _gridPosList = [];
        private readonly List<BarlineType> _gridTypeList = [];

        public BarlineInfo this[int index] => _bars[index];

        public void ReserveRefreshBar()
        {
            _need_refresh_grid = true;
            _gridPosList.Clear();
            _gridTypeList.Clear();
            RequestRefresh?.Invoke(this, RequestRefreshEventArgs.RefreshAll);
        }

        private void OnSmallGridChanged() => ReserveRefreshBar();
        private void OnLargeGridChanged() => ReserveRefreshBar();

        public void LoadData(IBmsViewModel? source)
        {
            var bars = _bars;
            var max = 1d;
            if (source is not null)
            {
                foreach (var (num, pos, len) in source.EnumerateBars())
                {
                    bars[num] = new(pos, len);
                    if (len > max)
                    {
                        max = len;
                    }
                }
            }
            _maxBarLength = max;
            ReserveRefreshBar();
        }

        private void EnsureGridCore(double grid, BarlineType type, double length)
        {
            if (grid is > 0)
            {
                grid = Math.Min(grid, GridLimit);
                var posList = _gridPosList;
                var typeList = _gridTypeList;
                var index = typeList.LastIndexOf(type);
                var pos = index is >= 0 ? posList[index] : 0;
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

        public void RefreshLinePositions(List<double> headPos, List<double> linePos, List<BarlineType> lineType, double scaleY)
        {
            if (_need_refresh_grid)
            {
                _need_refresh_grid = false;
                EnsureGridCore(_smallGrid, BarlineType.Small, _maxBarLength);
                EnsureGridCore(_largeGrid, BarlineType.Large, _maxBarLength);
            }
            var gridPos = _gridPosList;
            var gridType = _gridTypeList;
            var c = gridPos.Count;
            headPos.Clear();
            linePos.Clear();
            lineType.Clear();
            foreach (var (pos, len) in _bars)
            {
                var y = pos * scaleY;
                headPos.Add(y);
                linePos.Add(y);
                lineType.Add(BarlineType.Bar);
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
    }
}
