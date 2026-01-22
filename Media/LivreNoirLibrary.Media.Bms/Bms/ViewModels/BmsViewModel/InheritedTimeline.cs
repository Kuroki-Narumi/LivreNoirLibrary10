using LivreNoirLibrary.Collections;
using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public class InheritedTimeline : IListEnumerable<BarPosition, Note>
    {
        public Timeline ParentTimeline { get; } = [];
        public ITimeline CurrentTimeline { get; set; } = new Timeline();

        public BarPosition FirstPosition => CurrentTimeline.IsEmpty ? ParentTimeline.FirstPosition :
                                            ParentTimeline.IsEmpty ? CurrentTimeline.FirstPosition :
                                            BarPosition.Min(CurrentTimeline.FirstPosition, ParentTimeline.FirstPosition);

        public BarPosition LastPosition => CurrentTimeline.IsEmpty ? ParentTimeline.LastPosition :
                                           ParentTimeline.IsEmpty ? CurrentTimeline.LastPosition :
                                           BarPosition.Max(CurrentTimeline.LastPosition, ParentTimeline.LastPosition);

        public void ClearAndSetRoot(IBmsDataUnit root)
        {
            ParentTimeline.Clear();
            CurrentTimeline = root.Timeline;
        }

        public void LoadParent(IEnumerable<IBmsDataUnit> parents, IBmsDataUnit current)
        {
            var timeline = ParentTimeline;
            timeline.Clear();
            foreach (var data in parents)
            {
                data.Timeline.CopyTo(timeline);
            }
            CurrentTimeline = current.Timeline;
        }

        public TwoMergedEnumerator<BarPosition, List<Note>> GetEnumerator() 
            => new(ParentTimeline.EnumerateList(), CurrentTimeline.EnumerateList());
        public TwoMergedEnumerator<BarPosition, List<Note>> GetEnumerator(Range<BarPosition> range) 
            => new(ParentTimeline.EnumerateList(range), CurrentTimeline.EnumerateList(range));
        public TwoMergedEnumerator<BarPosition, List<Note>> GetReverseEnumerator() 
            => new(ParentTimeline.ReverseEnumerateList(), CurrentTimeline.ReverseEnumerateList());
        public TwoMergedEnumerator<BarPosition, List<Note>> GetReverseEnumerator(Range<BarPosition> range) 
            => new(ParentTimeline.ReverseEnumerateList(range), CurrentTimeline.ReverseEnumerateList(range));

        IEnumerable<(BarPosition, List<Note>)> IListEnumerable<BarPosition, Note>.EnumerateList() => EnumerateCore(GetEnumerator());
        IEnumerable<(BarPosition, List<Note>)> IListEnumerable<BarPosition, Note>.EnumerateList(Range<BarPosition> range) => EnumerateCore(GetEnumerator(range));
        IEnumerable<(BarPosition, List<Note>)> IListEnumerable<BarPosition, Note>.ReverseEnumerateList() => EnumerateCore(GetReverseEnumerator());
        IEnumerable<(BarPosition, List<Note>)> IListEnumerable<BarPosition, Note>.ReverseEnumerateList(Range<BarPosition> range) => EnumerateCore(GetReverseEnumerator(range));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<(BarPosition, List<Note>)> EnumerateCore(TwoMergedEnumerator<BarPosition, List<Note>> enumer)
        {
            var buffer = ObjectPool.Rent<List<Note>>();
            try
            {
                foreach (var (pos, list1, list2) in enumer)
                {
                    if (list1 is not null && list2 is not null)
                    {
                        buffer.Clear();
                        buffer.AddRange(list1);
                        buffer.AddRange(list2);
                        yield return (pos, buffer);
                    }
                    else
                    {
                        yield return (pos, (list1 ?? list2)!);
                    }
                }
            }
            finally
            {
                ObjectPool.Return(buffer);
            }
        }

        public void RefreshTimeline(IBarPositionProvider<double> provider, ITimelineViewModel target, double initialTempo, bool abortIfInvalidTempo)
        {
            TimingInfoState state = new(initialTempo);
            target.BeginTimelineRefresh(initialTempo);
            foreach (var (pos, list1, list2) in this)
            {
                var beat = provider.GetAbsolutePosition(pos);
                state.Setup(beat);
                if (list1 is not null)
                {
                    target.ApplyParentTimeline(pos, ref state, list1);
                }
                if (list2 is not null)
                {
                    target.ApplyCurrentTimeline(pos, ref state, list2);
                }
                target.ApplyTimeInfo(ref state);
            }
            target.FinisTimelineRefresh();
        }
    }
}
