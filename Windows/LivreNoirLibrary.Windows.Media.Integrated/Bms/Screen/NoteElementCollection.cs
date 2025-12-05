using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Windows.Controls.Bms
{
    public class NoteElementCollection : ObservableObjectBase
    {
        private readonly List<NoteInfo> _children = [];
        private readonly List<BarLineInfo> _barLines = [];
        private readonly List<NoteInfo> _visible = [];
        private readonly TimerInfoList _timers = [];
        private int _barStart;
        private int _barLength;

        public ReadOnlySpan<NoteInfo> VisibleChildren => _visible.AsSpan();
        public ReadOnlySpan<BarLineInfo> BarLines => _barLines.AsSpan().Slice(_barStart, _barLength);

        public void Setup(TimingList timings)
        {
            var bars = _barLines;
            var children = _children;
            var timers = _timers;
            bars.Clear();
            children.Clear();
            timers.Clear();
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
                        children.Add(new(lane, time, end, startPos, endPos - startPos, info.IsMine));
                        timers.AddRelease(id + TimerIdOffsets.Release, id + TimerIdOffsets.Press, end);
                        timers.Add(id + TimerIdOffsets.LongBomb, time);
                    }
                    else
                    {
                        children.Add(new(lane, time, time, startPos, 0, info.IsMine));
                        if (isVisible)
                        {
                            timers.AddRelease(id + TimerIdOffsets.Release, id + TimerIdOffsets.Press, time + 0.01);
                        }
                    }
                }
            }
            bars.Sort(new BarLineInfoComparer());
        }

        // kari
        public static JudgeInfo Judge_Perfect { get; } = new(JudgeType.Perfect, ComboChange.Increase, 2, 1);
        public static JudgeInfo Judge_LongEnd { get; } = new(JudgeType.Perfect, ComboChange.Continue, 0, 0);

        public void Update(in UpdateArgs args)
        {
            var timings = args.Timings;
            var visible = _visible;
            visible.Clear();
            var absTime = args.AbsoluteTime;
            var timer = args.Timer;
            var judge = args.Score;
            judge.IsActive = false;
            if (timer.TryGet(TimerId.Play_MusicStart, absTime, out var currentTime))
            {
                var start = timings.Time2Position(currentTime);
                var end = start + 1 / args.HighSpeed;
                foreach (var child in _children.AsSpan())
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
                            judge.IsActive = true;
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

        public class TimerInfoList : Dictionary<TimerId, TimerInfo>
        {
            public void Add(TimerId id, double value) => this.GetOrAdd(id).Add(value);

            public void AddRelease(TimerId releaseId, TimerId pressId, double value)
            {
                var list = this.GetOrAdd(releaseId);
                list.Add(value);
                list.ConflictId = pressId;
            }

            public void Advance(BmsTimer timer, double relativeTime, double offset)
            {
                foreach (var (id, list) in this)
                {
                    list.Advance(timer, id, relativeTime, offset);
                }
            }
        }

        public class TimerInfo : List<double>
        {
            public TimerId ConflictId { get; set; }
            private int _index = 0;

            public void Advance(BmsTimer timer, TimerId id, double relativeTime, double offset)
            {
                var index = _index;
                for (; index < Count; index++)
                {
                    if (this[index] <= relativeTime)
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

        private readonly struct BarLineInfoComparer : IComparer<BarLineInfo>, IComparer<BarLineInfo, double>
        {
            public int Compare(BarLineInfo? x, BarLineInfo? y) => x!.Position.CompareTo(y!.Position);
            public static int Compare(BarLineInfo x, double y) => x.Position.CompareTo(y);
            public static bool IsXCloserThanY(BarLineInfo x, BarLineInfo y, double z) => x.Position + y.Position < z * 2;
        }
    }
}
