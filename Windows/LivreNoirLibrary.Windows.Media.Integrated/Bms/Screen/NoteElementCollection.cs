using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Debug;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.ObjectModel;
using LivreNoirLibrary.Windows.Media.Bms.SkinInfo;
using LivreNoirLibrary.Numerics;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class NoteElementCollection : ObservableObjectBase
    {
        private readonly Dictionary<Channel, (int Lane, int Player)> _lanes = [];
        private readonly List<NoteInfo> _notes = [];
        private readonly List<BarLineInfo> _barLines = [];
        private readonly List<NoteInfo> _visible = [];
        private readonly TimerInfoList _timers = new();
        private int _barStart;
        private int _barLength;

        public int NoteCount { get; private set; }
        public ReadOnlySpan<NoteInfo> VisibleChildren => _visible.AsSpan();
        public ReadOnlySpan<BarLineInfo> BarLines => _barLines.AsSpan().Slice(_barStart, _barLength);

        public void DetermineExpressions(Skin skin, IVariableProvider provider)
        {
            var lanes = _lanes;
            lanes.Clear();
            foreach (var definition in skin.LaneDefinitions)
            {
                if (skin.TryResolveValue<int>(definition.Lane, provider, out var lane))
                {
                    var ch = BmsUtils.ToChannel(definition.Channel);
                    var player = skin.ResolveValue(definition.Player, provider, 1);
                    lanes[ch] = (lane, player);
                }
            }
        }

        public void Setup(TimingList timings)
        {
            var bars = _barLines;
            var notes = _notes;
            var timers = _timers;
            bars.Clear();
            notes.Clear();
            timers.Clear();
            var noteCount = 0;
            var lanes = _lanes;
            //StringBuilder sb = new();
            //sb.AppendLine("t\tpos\tch\ttype\tlength");
            foreach (var (channel, position, info) in timings.KeyInfos)
            {
                var time = info.Time;
                if (channel is Channel.Bar)
                {
                    bars.Add(new(time, position));
                }
                else
                {
                    if (lanes.TryGetValue(channel, out var tuple))
                    {
                        var (lane, player) = tuple;
                        var endTime = time + info.TimeLength;
                        var id = BmsTimer.Lane2TimerId(lane);
                        var type = info.Type;
                        var isVisible = type is NoteType.Normal;
                        if (isVisible)
                        {
                            timers.Add(id + TimerIdOffsets.Press, time);
                            timers.AddRelease(id + TimerIdOffsets.Bomb, id + TimerIdOffsets.LongBomb, endTime);
                            //sb.AppendLine($"{time}\t{startPos}\t{channel.ToBased()}\t{type}\t{info.Length}");
                        }
                        if (info.TimeLength is > 0)
                        {
                            notes.Add(new(lane, player, time, endTime, position, info.VisualLength, type));
                            timers.AddRelease(id + TimerIdOffsets.Release, id + TimerIdOffsets.Press, endTime);
                            timers.Add(id + TimerIdOffsets.LongBomb, time);
                            noteCount++;
                        }
                        else
                        {
                            notes.Add(new(lane, player, time, time, position, 0, type));
                            if (isVisible)
                            {
                                timers.AddRelease(id + TimerIdOffsets.Release, id + TimerIdOffsets.Press, time + 0.01);
                                noteCount++;
                            }
                        }
                    }
                }
            }
            NoteCount = noteCount;
            bars.Sort(BarLineInfoComparer.Instance);
            //sb.AppendLine(timings.GetTimingInfoText());
            //Clipboard.SetText(sb.ToString());
        }

        public void Update(in UpdateArgs args)
        {
            var timings = args.Timings;
            var visible = _visible;
            visible.Clear();
            var absTime = args.AbsoluteTime;
            var timer = args.Timer;
            var judge = args.ScoreManager;
            if (timer.TryGet(TimerId.Play_MusicStart, absTime, out var currentTime))
            {
                var timeOffset = absTime - currentTime;
                var start = timings.Time2Position(currentTime);
                var end = start + 1 / args.HighSpeed;
                foreach (var child in _notes.AsSpan())
                {
                    var visualStart = child.VisualStart;
                    if (visualStart > end)
                    {
                        break;
                    }
                    var endTime = child.EndTime;
                    if (endTime <= currentTime)
                    {
                        if (!child.IsProcessed)
                        {
                            if (child.IsVisible)
                            {
                                judge.UpdateJudge(timer, timeOffset + endTime, 0, new(JudgeType.Perfect, ComboChange.Increase, false, child.Player, 0, 2, 1));
                            }
                            child.IsProcessed = true;
                            child.IsActive = false;
                        }
                        continue;
                    }
                    if (visualStart + child.VisualLength >= start)
                    {
                        var offset = visualStart - start;
                        child.CurrentOffset = offset;
                        if (!child.IsActive && offset is <= 0)
                        {
                            var time = child.Time;
                            judge.UpdateJudge(timer, timeOffset + time, endTime - time, new(JudgeType.Perfect, ComboChange.Continue, false, child.Player, 0, 0, 1));
                            child.IsActive = true;
                        }
                        visible.Add(child);
                    }
                }
                _timers.Advance(timer, currentTime, timeOffset);
                (_barStart, _barLength) = _barLines.IndexRange<BarLineInfo, double, BarLineInfoComparer>(RangeUtils.Get(start, end));
                foreach (var bar in BarLines)
                {
                    bar.RelativePosition = bar.Position - start;
                }
            }
        }

        public record NoteInfo(int Lane, int Player, double Time, double EndTime, double VisualStart, double VisualLength, NoteType Type)
        {
            public double CurrentOffset { get; set; }
            public bool IsVisible => Type is NoteType.Normal;
            public bool IsActive { get; set; }
            public bool IsProcessed { get; set; }
        }

        public class TimerInfoList
        {
            private readonly Dictionary<TimerId, TimerInfo> _dic = [];

            public void Clear()
            {
                foreach (var (_, list) in _dic)
                {
                    list.Clear();
                }
            }

            public void Add(TimerId id, double value) => _dic.GetOrAdd(id).Add(value);

            public void AddRelease(TimerId releaseId, TimerId pressId, double value)
            {
                var list = _dic.GetOrAdd(releaseId);
                list.Add(value);
                list.ConflictId = pressId;
            }

            public void Advance(BmsTimer timer, double relativeTime, double offset)
            {
                foreach (var (id, list) in _dic)
                {
                    list.Advance(timer, id, relativeTime, offset);
                }
            }
        }

        public class TimerInfo
        {
            private readonly List<double> _list = [];

            public TimerId ConflictId { get; set; }
            private int _index = 0;

            public void Clear()
            {
                _list.Clear();
                _index = 0;
            }

            public void Add(double value) => _list.Add(value);

            public void Advance(BmsTimer timer, TimerId id, double relativeTime, double offset)
            {
                var index = _index;
                var list = _list;
                var count = list.Count;
                for (; index < count; index++)
                {
                    if (list[index] <= relativeTime)
                    {
                        timer.Set(id, relativeTime + offset);
                        if (ConflictId is not 0)
                        {
                            timer.Remove(ConflictId);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                _index = index;
            }
        }

        public record BarLineInfo(double Time, double Position)
        {
            public double RelativePosition { get; set; }
        }

        private class BarLineInfoComparer : IComparer<BarLineInfo>, IComparer<BarLineInfo, double>
        {
            public static BarLineInfoComparer Instance { get; } = new();

            private BarLineInfoComparer() { }

            public int Compare(BarLineInfo? x, BarLineInfo? y) => x!.Position.CompareTo(y!.Position);
            public static int Compare(BarLineInfo x, double y) => x.Position.CompareTo(y);
            public static bool IsXCloserThanY(BarLineInfo x, BarLineInfo y, double z) => x.Position + y.Position < z * 2;
        }
    }
}
