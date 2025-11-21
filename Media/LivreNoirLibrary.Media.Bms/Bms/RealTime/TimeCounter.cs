using System;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimeCounter : TimeCounterBase
    {
        public void Load(IBmsViewModel source) => Load(source.Bpm, source, source.CurrentTimeline);

        public void Load(double initialTempo, IBarPositionProvider<double> provider, IListEnumerable<BarPosition, Note> timeline)
        {
            Clear();
            InitializeTimeInfo(initialTempo);
            TimingInfoState state = new(initialTempo);
            foreach (var (pos, list) in timeline.EnumerateList())
            {
                // ÉeÉìÉ|Ç™ê≥Ç≈Ç»Ç¢èÍçáÇÕèIóπ
                if (state.IsInvalidTempo)
                {
                    break;
                }
                state.Setup(provider.GetAbsolutePosition(pos));
                foreach (var note in list.AsSpan())
                {
                    state.Update(note);
                }
                ApplyTimeInfo(ref state);
            }
        }
    }
}
