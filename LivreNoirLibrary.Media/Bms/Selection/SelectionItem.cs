using System;
using System.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class SelectionItem(BarPosition position, Rational actualPos, Note note) : INoteWrapper, ISelectionItem
    {
        public BarPosition Position { get; } = position;
        public Rational ActualPosition { get; } = actualPos;
        public Note Note { get; private set; } = note;

        public void Deconstruct(out BarPosition position, out Rational actualPos, out Note value)
        {
            position = Position;
            actualPos = ActualPosition;
            value = Note;
        }

        public void ReplaceToClone()
        {
            Note = Note.Clone();
        }
    }
}
