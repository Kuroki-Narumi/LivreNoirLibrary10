using System;
using LivreNoirLibrary.Media.Bms;

namespace LivreNoirLibrary.Media.Controls.Bms
{
    public interface ICursorInfo
    {
        public BarPosition Position { get; }
        public NoteType NoteType { get; }
        public DefType DefType { get; }
        public int Lane { get; }
        public int Id { get; }
    }
}
