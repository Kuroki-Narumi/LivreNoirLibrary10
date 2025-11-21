using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public interface ITimelineViewModel
    {
        void BeginTimelineRefresh(double initialTempo);
        void ApplyTimeInfo(ref TimingInfoState state);
        void ApplyParentTimeline(in BarPosition position, ref TimingInfoState state, List<Note> list);
        void ApplyCurrentTimeline(in BarPosition position, ref TimingInfoState state, List<Note> list);
        void FinisTimelineRefresh() { }
    }
}
