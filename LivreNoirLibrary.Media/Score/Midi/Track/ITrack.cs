using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public interface ITrack
    {
        public int Port { get; set; }
        public int Channel { get; set; }
        public string? Title { get; set; }
        public Timeline Timeline { get; }
        public Span<KeySwitchOption> KeySwitchOptions { get; }
        public void Clear();
        public bool ContainsNote();
        public bool IsNormalNote(IObject obj);
        public Rational GetFirstNotePosition();
        public Rational GetFirstMetaPosition(MetaType type);
        public Rational GetLastPosition();
        public bool BulkEdit(BulkEditOptions options, Selection? selection, out Selection newSelection);
    }
}
