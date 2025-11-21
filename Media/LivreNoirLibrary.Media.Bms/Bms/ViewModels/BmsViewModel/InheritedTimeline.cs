using LivreNoirLibrary.ObjectModel;
using System;
using System.Collections.Generic;
using System.Threading;

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

        public IEnumerable<(BarPosition, List<Note>)> EnumerateList() => EnumerateCore(GetEnumerator());
        public IEnumerable<(BarPosition, List<Note>)> ReverseEnumerateList() => EnumerateCore(GetReverseEnumerator());

        public TwoMergedEnumerator<BarPosition, List<Note>> GetEnumerator() => new(ParentTimeline.EnumerateList(), CurrentTimeline.EnumerateList());
        public TwoMergedEnumerator<BarPosition, List<Note>> GetReverseEnumerator() => new(ParentTimeline.ReverseEnumerateList(), CurrentTimeline.ReverseEnumerateList());

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

        public void ApplyTimeline(IBarPositionProvider<double> provider, ITimelineViewModel vm, double initialTempo, bool abortIfInvalidTempo)
        {
            TimingInfoState state = new(initialTempo);
            vm.BeginTimelineRefresh(initialTempo);
            foreach (var (pos, list1, list2) in this)
            {
                if (abortIfInvalidTempo && state.IsInvalidTempo)
                {
                    break;
                }
                var beat = provider.GetAbsolutePosition(pos);
                state.Setup(beat);
                if (list1 is not null)
                {
                    vm.ApplyParentTimeline(pos, ref state, list1);
                }
                if (list2 is not null)
                {
                    vm.ApplyCurrentTimeline(pos, ref state, list2);
                }
                vm.ApplyTimeInfo(ref state);
            }
            vm.FinisTimelineRefresh();
        }
    }
}
