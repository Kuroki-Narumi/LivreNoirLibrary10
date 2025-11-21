using System;

namespace LivreNoirLibrary.Media.Bms.ViewModels
{
    public readonly record struct SelectionHistoryItem(double BarHead, double AbsolutePosition, double Time, Channel Channel, NoteType Type, double Value)
    {
        public static SelectionHistoryItem Create(SelectionItem source)
        {
            var note = source.Note;
            return new(source.BarHead, source.AbsolutePosition, source.Time, note.Channel, note.Type, note.Value);
        }

        public SelectionItem Restore(BarPosition position) => new(position, BarHead, AbsolutePosition, Time, new(Channel, Type, Value));

        public bool Equals(Note note) => note.Channel == Channel && note.Type == Type && note.Value == Value;
    }
}
