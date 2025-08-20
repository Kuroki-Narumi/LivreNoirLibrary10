using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public abstract partial class BaseData : ObjectBase, IBmsData
    {
        public List<string> Comments { get; } = [];
        public HeaderCollection Headers { get; } = new();
        public DefListCollection DefLists { get; } = new() { { DefType.Wav, new() }, { DefType.Bmp, new() } };
        public BarLengthCollection Bars { get; } = [];
        public NoteTimeline Timeline { get;} = [];

        public abstract BmsData Root { get; }
        IRootData IBmsData.Root => Root;
        public BaseData? Parent { get; protected set; }
        internal NoteTimeline? InheritedTimeline { get; set; }

        public virtual void Clear()
        {
            Comments.Clear();
            Headers.Clear();
            DefLists.Clear();
            Bars.Clear();
            Timeline.Clear();
            Insulate();
        }

        public Rational GetBarLength(int number) => Bars.Get(number);
        public Rational GetAbsolutePosition(BarPosition position) => Root.BarLengthCache.GetAbsolutePosition(position, Bars);
        public BarPosition GetBarPosition(Rational absolutePosition) => Root.BarLengthCache.GetBarPosition(absolutePosition, Bars);

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
            Root.BarLengthCache.Clear();
            Parent = parent;
            Headers.Parent = parent.Headers;
            DefLists.Parent = parent.DefLists;
            Bars.Parent = parent.Bars;
        }

        public NoteTimeline GetInheritTimeline()
        {
            InheritedTimeline = CreateInheritedTimeline();
            return InheritedTimeline;
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
            Root.BarLengthCache.Clear();
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

        public IEnumerable<NoteInfo> EachNote<T>(bool inherit = false)
        {
            if (inherit)
            {
                if (InheritedTimeline is not null)
                {
                    foreach (var (pos, item) in InheritedTimeline)
                    {
                        yield return new(pos, GetAbsolutePosition(pos), item);
                    }
                }
            }
            else
            {
                foreach (var (pos, item) in Timeline)
                {
                    yield return new(pos, GetAbsolutePosition(pos), item);
                }
            }
        }
    }

    public readonly record struct NoteInfo(BarPosition Position, Rational AbsolutePosition, Note Note) : INoteWrapper;
}
