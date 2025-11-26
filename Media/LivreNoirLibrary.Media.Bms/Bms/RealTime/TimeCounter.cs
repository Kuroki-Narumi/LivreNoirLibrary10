using System;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimeCounter : TimeCounterBase
    {
        public void Load(IBmsViewModel source) => Load(source.Bpm, source, source.CurrentTimeline);

        public void Load(double initialTempo, IBarPositionProvider<double> provider, IListEnumerable<BarPosition, Note> timeline)
        {
            BeginInit(initialTempo);
            TimingInfoState state = new(initialTempo);
            foreach (var (pos, list) in timeline.EnumerateList())
            {
                // ƒeƒ“ƒ|‚ª³‚Å‚È‚¢ê‡‚ÍI—¹
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
            EndInit(ref state);
        }
    }
}
