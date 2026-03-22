using System;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimeCounter : TimeCounterBase
    {
        public override void Load(IBmsViewModel source) => Load(source.Bpm, source, source.CurrentTimeline);

        public void Load(double initialTempo, IBarPositionProvider provider, IListEnumerable<BarPosition, Note> timeline)
        {
            BeginInit(initialTempo);
            TimingInfoState state = new(initialTempo);
            foreach (var (pos, list) in timeline.EnumerateList())
            {
                state.Setup(provider.GetAbsolutePosition(pos));
                var containsNote = false;
                foreach (var note in list.AsSpan())
                {
                    if (!state.Update(note) && note.IsMainSound(false))
                    {
                        containsNote = true;
                    }
                }
                if (containsNote)
                {
                    state.UpdateFirstTime();
                    state.UpdateLastTime();
                }
                ApplyTimeInfo(ref state);
            }
            EndInit(ref state);
        }
    }
}
