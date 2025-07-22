using System;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public partial class BaseData
    {
        public Note AddNote(BarPosition position, Note note)
        {
            Timeline.Add(position, note);
            return note;
        }

        public Note AddOrReplaceNote(BarPosition position, Note note)
        {
            if (Timeline.TryGet(position, out var list))
            {
                var lane = note.Lane;
                if (list.Find(n => n.Lane == lane) is Note current)
                {
                    current.Replace(note);
                    return current;
                }
                else
                {
                    list.Add(note);
                }
            }
            else
            {
                Timeline.Add(position, note);
            }
            return note;
        }

        public Note AddNote(BarPosition position, NoteType type, int lane, int id) => AddNote(position, new(type, lane, id));
        public Note AddTempo(BarPosition position, Rational value) => AddNote(position, Note.Tempo(value));
        public Note AddStop(BarPosition position, Rational value) => AddNote(position, Note.Stop(value));
        public Note AddScroll(BarPosition position, Rational value) => AddNote(position, Note.Scroll(value));
        public Note AddSpeed(BarPosition position, Rational value) => AddNote(position, Note.Speed(value));

        public bool RemoveAt(BarPosition position, int lane)
        {
            var count = Timeline.RemoveIf(position, n => n.Lane == lane);
            return count is > 0;
        }

        public bool RemoveNote(BarPosition position, Note note) => Timeline.Remove(position, note);
    }
}
