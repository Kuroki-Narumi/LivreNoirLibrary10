using System;

namespace LivreNoirLibrary.Media.Midi
{
    public sealed partial class MidiData : MidiData<Track>
    {
        public void SetTrack(int index, Track track)
        {
            InitializeTracks(index + 1);
            _tracks[index] = track;
        }
    }
}
