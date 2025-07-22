using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface IBmsData
    {
        public BarLengthCollection Bars { get; }
        public NoteTimeline Timeline { get; }

        public decimal Bpm { get; }
    }
}
