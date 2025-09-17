using System;

namespace LivreNoirLibrary.Media.Bms
{
    public interface ISoundNote : IIntValueNote
    {
        public NoteType Type { get; set; }
        public int Lane { get; set; }
    }
}
