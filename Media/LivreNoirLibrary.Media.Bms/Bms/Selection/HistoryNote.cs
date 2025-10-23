using System;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly record struct HistoryNote(int Lane, decimal Value, NoteType Type)
    {
        public const int LanePadding = 10000;

        public HistoryNote(ISoundNote note) : this(note.Lane, note.Value, note.Type) { }
        public HistoryNote(IMetaNote note) : this((int)note.Channel + LanePadding, note.Value, NoteType.Invalid) { }
        public HistoryNote(IConductorNote note) : this((int)note.Channel + LanePadding, note.Value, NoteType.Invalid) { }

        public static HistoryNote Create(INote note) => note switch
        {
            ISoundNote n => new(n),
            IMetaNote n => new(n),
            IConductorNote n => new(n),
            _ => default,
        };

        public bool Equals(INote note)
        {
            return
                (note is ISoundNote s && s.Lane == Lane && s.Value == Value && s.Type == Type) ||
                (note is IMetaNote m && (int)m.Channel == Lane - LanePadding && m.Value == Value) ||
                (note is IConductorNote c && (int)c.Channel == Lane - LanePadding && c.Value == Value);
        }
    }
}
