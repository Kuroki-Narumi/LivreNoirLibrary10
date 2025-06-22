using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class LaneIndexConverter
    {
        private uint _count;
        private readonly List<int> _lane_list = [];
        private readonly List<double> _pos_list = [];
        private readonly List<LaneInfo> _info_list = [];
        private readonly Dictionary<Key, int> _key_list = [];
        private int _total_width;

        public int Count => (int)_count;
        public int ConductorLaneCount { get; private set; }
        public int TotalWidth => _total_width;

        public void Clear()
        {
            _count = 0;
            _lane_list.Clear();
            _pos_list.Clear();
            _info_list.Clear();
            _key_list.Clear();
            _total_width = 0;
        }

        public double Add(double offset, LaneInfo info, int scale)
        {
            var x = (int)offset;
            if (_count is > 0)
            {
                x += (int)_pos_list[^1];
                x += _info_list[^1].Width * scale;
            }
            Add(ref x, info, scale);

            _total_width = x + 1;
            return _total_width;
        }

        public double ApplyTheme(Theme theme, int conductorIndex, int metaIndex, int keyIndex, ScratchPosition scrPos, int bgmCount, int scale)
        {
            Clear();
            var sepWidth = theme.SeparatorWidth;
            var x = sepWidth;

            var conductor = theme.ConductorLanes[conductorIndex];
            ConductorLaneCount = conductor.Lanes.Count;
            Add(ref x, conductor, scale);
            x += sepWidth;

            foreach (var type in theme.LaneOrder)
            {
                switch (type)
                {
                    case LaneOrderType.Meta:
                        Add(ref x, theme.MetaLanes[metaIndex], scale);
                        break;
                    case LaneOrderType.Key:
                        Add(ref x, theme.KeyLanes[keyIndex], scrPos, scale);
                        break;
                    case LaneOrderType.Bgm:
                        var bgm = theme.BgmLane;
                        for (var i = 0; i < bgmCount; i++)
                        {
                            var lane = bgm.Clone();
                            lane.Name = $"{bgm.Name}{i + 1:D2}";
                            lane.Lane = -i;
                            Add(ref x, lane, scale);
                        }
                        break;
                }
                x += sepWidth;
            }

            _total_width = x + 1;
            return _total_width;
        }

        private void Add(ref int x, LaneInfo info, int scale)
        {
            if (info.IsSeparator)
            {
                x += info.Width;
            }
            else
            {
                _count++;
                _pos_list.Add(x);
                _lane_list.Add(info.Lane);
                _info_list.Add(info);
                _key_list[info.Key] = _info_list.Count - 1;
                x += info.Width * scale;
            }
        }

        private void Add(ref int x, KeyLaneInfoBundle list, ScratchPosition scrPos, int scale)
        {
            var lanes = list.Lanes;
            var scr = list.ScratchLane;
            var c = lanes.Count;
            if (scr is not null && scrPos is ScratchPosition.Left)
            {
                Add(ref x, scr, scale);
            }
            for (int i = 0; i < c; i++)
            {
                Add(ref x, lanes[i], scale);
            }
            if (scr is not null && scrPos is ScratchPosition.Right)
            {
                Add(ref x, scr, scale);
            }
        }

        private void Add(ref int x, LaneInfoBundle list, int scale)
        {
            var lanes = list.Lanes;
            var c = lanes.Count;
            for (int i = 0; i < c; i++)
            {
                Add(ref x, lanes[i], scale);
            }
        }

        public int Index2Lane(int index) => (uint)index < _count ? _lane_list[index] : int.MaxValue;
        public double Index2Pos(int index) => (uint)index < _count ? _pos_list[index] : double.NaN;
        public bool TryGetIndex2Info(int index, out double x, [MaybeNullWhen(false)] out LaneInfo info)
        {
            if ((uint)index < _count)
            {
                x = _pos_list[index];
                info = _info_list[index];
                return true;
            }
            else
            {
                x = 0;
                info = null;
                return false;
            }
        }

        public int Lane2Index(int lane) => _lane_list.IndexOf(lane);
        public int Pos2Index(double position)
        {
            var index = _pos_list.BinarySearch(position);
            if (index is >= 0)
            {
                return index;
            }
            else
            {
                return (~index) - 1;
            }
        }

        public double Lane2Pos(int lane) => Index2Pos(Lane2Index(lane));
        public bool TryGetLane2Info(int lane, out double x, [MaybeNullWhen(false)] out LaneInfo info) => TryGetIndex2Info(Lane2Index(lane), out x, out info);

        public int Pos2Lane(double position) => Index2Lane(Pos2Index(position));
        public bool TryGetPos2Info(double position, out double x, [MaybeNullWhen(false)] out LaneInfo info) => TryGetIndex2Info(Pos2Index(position), out x, out info);

        public int Key2Index(Key key) => _key_list.TryGetValue(key, out var index) ? index : -1;
        public bool TryGetKey2Info(Key key, [MaybeNullWhen(false)] out LaneInfo info) => TryGetIndex2Info(Key2Index(key), out _, out info);

        public IEnumerator<(double X, LaneInfo Info)> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return (_pos_list[i], _info_list[i]);
            }
        }
    }
}
