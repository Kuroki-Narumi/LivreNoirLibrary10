using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public abstract partial class BaseData : IBmsData
    {
        public List<string> Comments { get; protected set; } = [];
        public HeaderCollection Headers { get; protected set; } = [];
        public DefListCollection DefLists { get; protected set; } = new() { { DefType.Wav, new() }, { DefType.Bmp, new() } };
        public BarLengthCollection Bars { get; protected set; } = [];
        public NoteTimeline Timeline { get; protected set; } = [];

        public abstract BmsData Root { get; }
        public BaseData? Parent { get; protected set; }
        protected NoteTimeline? InheritedTimeline { get; set; }
        public RawData.Sequence RawData { get; } = [];

        public virtual void Clear()
        {
            Comments.Clear();
            Headers.Clear();
            DefLists.Clear();
            Bars.Clear();
            Timeline.Clear();
            Insulate();
            RawData.Clear();
        }

        public int GetNotesCount(bool countEnd = false)
        {
            int result = 0;
            foreach (var (_, note) in Timeline)
            {
                if (note.IsVisibleKey(countEnd))
                {
                    result++;
                }
            }
            return result;
        }

        public int GetNotesCount(Predicate<Note> selector)
        {
            int result = 0;
            foreach (var (_, note) in Timeline)
            {
                if (selector(note))
                {
                    result++;
                }
            }
            return result;
        }

        public double CalcTotal(double defaultValue = 0)
        {
            var t = Total;
            if (t is <= 0)
            {
                t = defaultValue;
            }
            if (t is <= 0)
            {
                t = BmsUtils.CalcTotal(GetNotesCount());
            }
            return t;
        }

        public long CalcResolution()
        {
            var result = 1L;
            foreach (var (pos, _) in Timeline.EachList())
            {
                result = result.LCM(GetBeat(pos).Denominator);
            }
            return result;
        }

        public int GetMaxBgmLane()
        {
            int max = 0;
            foreach (var (_, note) in Timeline)
            {
                if (note.IsBgm() && note.Lane < max)
                {
                    max = note.Lane;
                }
            }
            return 1 - max;
        }

        public void Merge(BaseData data)
        {
            Comments.AddRange(data.Comments);
            Headers.Merge(data.Headers);
            DefLists.Merge(data.DefLists);
            Bars.Merge(data.Bars);
            Timeline.Merge(data.Timeline);
        }

        public void Inherit(BaseData parent)
        {
            Root.ClearBarCache();
            Parent = parent;
            InheritedTimeline = parent.CreateInheritedTimeline();
            Headers.Parent = parent.Headers;
            DefLists.Parent = parent.DefLists.GetForChild();
            Bars.Parent = parent.Bars;
        }

        protected NoteTimeline CreateInheritedTimeline()
        {
            NoteTimeline timeline;
            if (InheritedTimeline is not null)
            {
                timeline = InheritedTimeline.Clone();
            }
            else if (Parent is not null)
            {
                timeline = Parent.CreateInheritedTimeline();
            }
            else
            {
                timeline = [];
            }
            timeline.Merge(Timeline);
            return timeline;
        }

        public void Insulate()
        {
            Root.ClearBarCache();
            Parent = null;
            InheritedTimeline = null;
            Headers.Parent = null;
            DefLists.Parent = null;
            Bars.Parent = null;
        }

        protected void ProcessInherit(BaseData? parent, Action action)
        {
            var currentParent = Parent;
            if (parent is not null)
            {
                Inherit(parent);
            }
            action();
            if (currentParent is not null)
            {
                if (currentParent != parent)
                {
                    Inherit(currentParent);
                }
            }
            else
            {
                Insulate();
            }
        }
    }
}
