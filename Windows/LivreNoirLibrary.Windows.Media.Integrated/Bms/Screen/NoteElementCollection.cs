using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Media;
using LivreNoirLibrary.Media.Bms;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Windows.Media.Bms
{
    public class NoteElementCollection : ObservableObjectBase
    {
        private readonly List<NoteInfo> _children = [];
        internal readonly List<NoteInfo> _visible = [];

        public void Setup(TimingList timings)
        {
            var children = _children;
            children.Clear();
            foreach (var (lane, tick, info) in timings.KeyInfos)
            {
                var startPos = (double)timings.Time2Position(info.Time);
                if (info.Length is > 0)
                {
                    var end = info.Time + info.Length;
                    var endTick = TimeUtils.Seconds2Ticks(end);
                    var endPos = (double)timings.Time2Position(end);
                    children.Add(new(lane, tick, endTick, startPos, endPos - startPos, info.IsMine));
                }
                else
                {
                    children.Add(new(lane, tick, tick, startPos, 0, info.IsMine));
                }
            }
        }

        public void Update(TimingList timings, BmsTimer timer, long absoluteTick, double highSpeed)
        {
            var visible = _visible;
            visible.Clear();
            if (timer.TryGet(TimerId.Play_MusicStart, absoluteTick, out var currentTick))
            {
                var start = (double)timings.Time2Position(TimeUtils.Ticks2SecondsM(currentTick));
                var end = start + 1 / highSpeed;
                foreach (var child in CollectionsMarshal.AsSpan(_children))
                {
                    if (child.EndTick >= currentTick && child.VisualStart <= end && (child.VisualStart + child.VisualLength) >= start)
                    {
                        var offset = child.VisualStart - start;
                        child.CurrentOffset = offset;
                        child.IsVisible = true;
                        child.IsActive = offset is <= 0;
                        visible.Add(child);
                    }
                }
            }
        }

        public record NoteInfo(int Lane, long Tick, long EndTick, double VisualStart, double VisualLength, bool IsMine)
        {
            public double CurrentOffset { get; set; }
            public bool IsVisible { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
