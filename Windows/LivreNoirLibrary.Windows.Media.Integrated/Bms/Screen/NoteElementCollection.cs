using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.Media.Bms.Play;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class NoteElementCollection : ObservableObjectBase
    {
        private readonly List<NoteInfo> _notes = [];
        private readonly List<BarLineInfo> _barLines = [];
        private readonly List<NoteInfo> _visible = [];
        private readonly TimerInfoList _timers = new();
        private int _barStart;
        private int _barLength;

        public int NoteCount { get; private set; }
        public ReadOnlySpan<NoteInfo> VisibleChildren => _visible.AsSpan();
        public ReadOnlySpan<BarLineInfo> BarLines => _barLines.AsSpan().Slice(_barStart, _barLength);

        public void Setup(TimingList timings)
        {
            var bars = _barLines;
            var notes = _notes;
            var timers = _timers;
            bars.Clear();
            notes.Clear();
            timers.Clear();
            var noteCount = 0;
            foreach (var (channel, time, info) in timings.KeyInfos)
            {
                var startPos = (double)timings.Time2Position(time);
                if (channel is Channel.Bar)
                {
                    bars.Add(new(time, startPos));
                }
                else
                {
                    var end = time + info.Length;
                    var lane = channel - Channel.Visible_Start;
                    var id = BmsTimer.Lane2TimerId(lane);
                    var isVisible = !info.IsMine;
                    if (isVisible)
                    {
                        timers.Add(id + TimerIdOffsets.Press, time);
                        timers.AddRelease(id + TimerIdOffsets.Bomb, id + TimerIdOffsets.LongBomb, end);
                    }
                    if (info.Length is > 0)
                    {
                        var endPos = (double)timings.Time2Position(end);
                        notes.Add(new(lane, time, end, startPos, endPos - startPos, info.IsMine));
                        timers.AddRelease(id + TimerIdOffsets.Release, id + TimerIdOffsets.Press, end);
                        timers.Add(id + TimerIdOffsets.LongBomb, time);
                        noteCount++;
                    }
                    else
                    {
                        notes.Add(new(lane, time, time, startPos, 0, info.IsMine));
                        if (isVisible)
                        {
                            timers.AddRelease(id + TimerIdOffsets.Release, id + TimerIdOffsets.Press, time + 0.01);
                            noteCount++;
                        }
                    }
                }
            }
            NoteCount = noteCount;
            bars.Sort(BarLineInfoComparer.Instance);
        }

        // kari
        public static JudgeInfo Judge_Perfect { get; } = new(JudgeType.Perfect, ComboChange.Increase, false, 0, 2, 1);
        public static JudgeInfo Judge_LongEnd { get; } = new(JudgeType.Perfect, ComboChange.Continue, false, 0, 0, 0);

        public void Update(in UpdateArgs args)
        {
            var timings = args.Timings;
            var visible = _visible;
            visible.Clear();
            var absTime = args.AbsoluteTime;
            var timer = args.Timer;
            var judge = args.ScoreManager;
            judge.IsJudgeActive = false;
            if (timer.TryGet(TimerId.Play_MusicStart, absTime, out var currentTime))
            {
                var start = timings.Time2Position(currentTime);
                var end = start + 1 / args.HighSpeed;
                foreach (var child in _notes.AsSpan())
                {
                    if (child.EndTime < currentTime)
                    {
                        child.IsActive = false;
                        if (!child.IsProcessed)
                        {
                            judge.UpdateJudge(timer, absTime + child.EndTime - currentTime, Judge_Perfect);
                            child.IsProcessed = true;
                        }
                        continue;
                    }
                    if (child.VisualStart <= end && (child.VisualStart + child.VisualLength) >= start)
                    {
                        var offset = child.VisualStart - start;
                        child.CurrentOffset = offset;
                        if (offset is <= 0)
                        {
                            child.IsActive = true;
                            judge.UpdateJudge(timer, absTime + child.Time - currentTime, Judge_LongEnd);
                            judge.IsJudgeActive = true;
                        }
                        visible.Add(child);
                    }
                }
                _timers.Advance(timer, currentTime, absTime - currentTime);
                (_barStart, _barLength) = _barLines.IndexRange<BarLineInfo, double, BarLineInfoComparer>(RangeUtils.Get(start, end));
                foreach (var bar in BarLines)
                {
                    bar.RelativePosition = bar.Position - start;
                }
            }
        }

        public record NoteInfo(int Lane, double Time, double EndTime, double VisualStart, double VisualLength, bool IsMine)
        {
            public double CurrentOffset { get; set; }
            public bool IsVisible { get; set; }
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
