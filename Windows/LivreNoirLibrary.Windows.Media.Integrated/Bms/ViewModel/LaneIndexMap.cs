using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class LaneIndexMap : IClear
    {
        private readonly List<double> _positions = [];
        private readonly List<(double Width, LaneInfo Info)> _lanes = [];
        private readonly Dictionary<Channel, int> _channel2index = [];
        private readonly Dictionary<Key, int> _key2index = [];
        private int _totalWidth;

        public int Padding { get; set; }
        public int TotalWidth => _totalWidth + Padding + 1;
        public int MetaLaneStartIndex { get; private set; }
        public int MetaLaneEndIndex { get; private set; }
        public int SoundLaneStartIndex { get; private set; }
        public int SoundLaneEndIndex { get; private set; }

        public void Clear()
        {
            _positions.Clear();
            _lanes.Clear();
            _channel2index.Clear();
            _key2index.Clear();
            _totalWidth = 0;
        }

        public void Add(ref int x, LaneInfo info, int scale)
        {
            if (info.IsSeparator)
            {
                x += info.Width;
            }
            else
            {
                var c = _lanes.Count;
                _channel2index[info.Channel] = c;
                if (info.Key is not 0)
                {
                    _key2index[info.Key] = c;
                }
                var width = info.Width * scale;
                _lanes.Add((width, info));
                _positions.Add(x);
                x += width;
            }
        }

        public void AddBgm(ref int x, LaneInfo info, int i, int scale)
        {
            BmsUtils.TryGetChannel(-i, out var channel);
            var clone = info.Clone();
            clone.Name = $"{info.Name}{i:D2}";
            clone.Channel = channel;
            var c = _lanes.Count;
            _channel2index[info.Channel] = c;
            if (_bgmKeyMap.TryGetValue(channel, out var key))
            {
                _key2index[key] = c;
            }
            var width = info.Width * scale;
            _lanes.Add((width, info));
            _positions.Add(x);
            x += width;
        }

        public double ApplyTheme(Theme theme, int conductorIndex, int metaIndex, int keyIndex, ScratchPosition scrPos, int bgmCount, int scale)
        {
            Clear();
            var sepWidth = theme.SeparatorWidth;
            // 左端パディング
            var x = sepWidth;

            // 指揮者レーン
            foreach (var info in theme.ConductorLanes[conductorIndex].Lanes.AsSpan())
            {
                Add(ref x, info, scale);
            }
            x += sepWidth;

            MetaLaneStartIndex = _lanes.Count;
            // メタレーン
            foreach (var info in theme.MetaLanes[metaIndex].Lanes.AsSpan())
            {
                Add(ref x, info, scale);
            }
            x += sepWidth;
            MetaLaneEndIndex = _lanes.Count - 1;

            // キーレーン
            SoundLaneStartIndex = _lanes.Count;
            var keys = theme.KeyLanes[keyIndex];
            var scr = keys.ScratchLane;
            if (scr is not null && scrPos is ScratchPosition.Left)
            {
                Add(ref x, scr, scale);
            }
            foreach (var info in keys.Lanes.AsSpan())
            {
                Add(ref x, info, scale);
            }
            if (scr is not null && scrPos is ScratchPosition.Right)
            {
                Add(ref x, scr, scale);
            }
            x += sepWidth;

            // BGMレーン
            var bgm = theme.BgmLane;
            for (var i = 0; i < bgmCount; i++)
            {
                AddBgm(ref x, bgm, i, scale);
            }
            x += sepWidth;
            SoundLaneEndIndex = _lanes.Count;

            _totalWidth = x;
            return TotalWidth;
        }

        public double Index2Pos(int index) => (uint)index < (uint)_positions.Count ? _positions[index] : double.NaN;
        public int Channel2Index(Channel channel) => _channel2index.TryGetValue(channel, out var index) ? index : -1;

        public bool TryGetLaneInfo(int index, out double x, out double width, [MaybeNullWhen(false)] out LaneInfo info)
        {
            if ((uint)index < (uint)_lanes.Count)
            {
                x = _positions[index];
                (width, info) = _lanes[index];
                return true;
            }
            x = double.NaN;
            width = 0;
            info = null;
            return false;
        }

        public bool TryGetLaneInfo(Note? note, out int index, out double x, out double width, [MaybeNullWhen(false)] out LaneInfo info)
        {
            index = note is not null ? Channel2Index(note.Channel) : -1;
            return TryGetLaneInfo(index, out x, out width, out info);
        }

        public Channel Index2Channel(int index) => TryGetLaneInfo(index, out _, out _, out var info) ? info.Channel : 0;

        public int Pos2Index(double position)
        {
            var index = _positions.BinarySearch(position);
            if (index is >= 0)
            {
                return index;
            }
            else
            {
                return (~index) - 1;
            }
        }

        public bool TrySearchPos(double position, SearchMode mode, out double actual) => _positions.TrySearch(position, mode, out _, out actual);

        public double Channel2Pos(Channel channel) => Index2Pos(Channel2Index(channel));

        public bool TryGetPos2Info(double x, out double actualX, [MaybeNullWhen(false)] out LaneInfo info) => TryGetLaneInfo(Pos2Index(x), out actualX, out _, out info);

        public bool TryGetKey2Info(Key key, [MaybeNullWhen(false)] out LaneInfo info)
        {
            info = _key2index.TryGetValue(key, out var index) ? _lanes[index].Info : null;
            return info is not null;
        }

        public IEnumerator<(double X, double Width, LaneInfo Info)> GetEnumerator()
        {
            var poss = _positions;
            var lanes = _lanes;
            var c = lanes.Count;
            for (var i = 0; i < c; i++)
            {
                var (w, info) = lanes[i];
                yield return (poss[i], w, info);
            }
        }

        private static readonly Dictionary<Channel, Key> _bgmKeyMap = new()
        {
            [Channel.Bgm_Start] = Key.D1,
            [Channel.Bgm_Start + 1] = Key.D2,
            [Channel.Bgm_Start + 2] = Key.D3,
            [Channel.Bgm_Start + 3] = Key.D4,
            [Channel.Bgm_Start + 4] = Key.D5,
            [Channel.Bgm_Start + 5] = Key.D6,
            [Channel.Bgm_Start + 6] = Key.D7,
            [Channel.Bgm_Start + 7] = Key.D8,
            [Channel.Bgm_Start + 8] = Key.D9,
            [Channel.Bgm_Start + 9] = Key.D0,
        };
    }
}
