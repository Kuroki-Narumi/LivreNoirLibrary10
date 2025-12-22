using System;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class TimeCounter : TimeCounterBase
    {
        public void Load(IBmsViewModel source) => Load(source.Bpm, source, source.CurrentTimeline);

        public void Load(double initialTempo, IBarPositionProvider<double> provider, IListEnumerable<BarPosition, Note> timeline)
        {
            BeginInit(initialTempo);
            TimingInfoState state = new(initialTempo);
            var firstSound = double.NaN;
            var lastSound = 0d;
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
                    var time = state.CurrentTime;
                    if (double.IsNaN(firstSound))
                    {
                        firstSound = time;
                    }
                    lastSound = time;
                }
                ApplyTimeInfo(ref state);
            }
            FirstSoundTime = firstSound.Validate(0);
            LastSoundTime = lastSound;
            EndInit(ref state);
        }
    }
}
