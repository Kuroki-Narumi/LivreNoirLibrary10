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
        private int _barStart;
        private int _barLength;

        public ReadOnlySpan<NoteInfo> VisibleChildren => _visible.AsSpan();
        public ReadOnlySpan<BarLineInfo> BarLines => _barLines.AsSpan().Slice(_barStart, _barLength);

        public void Setup(TimingList timings)
        {
            var bars = _barLines;
            var children = _children;
            bars.Clear();
            children.Clear();
            foreach (var (channel, time, info) in timings.KeyInfos)
            {
                var startPos = (double)timings.Time2Position(info.Time);
                if (channel is Channel.Bar)
                {
                    bars.Add(new(time, startPos));
                }
                else
                {
                    var lane = channel - Channel.Visible_Start;
                    if (info.Length is > 0)
                    {
                        var end = info.Time + info.Length;
                        var endPos = (double)timings.Time2Position(end);
                        children.Add(new(lane, time, end, startPos, endPos - startPos, info.IsMine));
                    }
                    else
                    {
                        children.Add(new(lane, time, time, startPos, 0, info.IsMine));
                    }
                }
            }
            bars.Sort(new BarLineInfoComparer());
        }

        public void Update(in UpdateArgs args)
        {
            var timings = args.Timings;
            var visible = _visible;
            visible.Clear();
            if (args.Timer.TryGet(TimerId.Play_MusicStart, args.AbsoluteTime, out var currentTime))
            {
                var start = timings.Time2Position(currentTime);
                var end = start + 1 / args.HighSpeed;
                foreach (var child in _children.AsSpan())
                {
                    if (child.EndTime >= currentTime && child.VisualStart <= end && (child.VisualStart + child.VisualLength) >= start)
                    {
                        var offset = child.VisualStart - start;
                        child.CurrentOffset = offset;
                        child.IsVisible = true;
                        child.IsActive = offset is <= 0;
                        visible.Add(child);
                    }
                }
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
        }

        public record BarLineInfo(double Time, double Position)
        {
            public double RelativePosition { get; set; }
        }

        private readonly struct BarLineInfoComparer : IComparer<BarLineInfo>, IComparer<BarLineInfo, double>
        {
            public int Compare(BarLineInfo? x, BarLineInfo? y) => x!.Time.CompareTo(y!.Time);
            public static int Compare(BarLineInfo x, double y) => x.Position.CompareTo(y);
            public static bool IsXCloserThanY(BarLineInfo x, BarLineInfo y, double z) => x.Position + y.Position < z * 2;
        }
    }
}
