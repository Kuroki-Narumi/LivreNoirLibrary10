using System;
using System.Collections.Generic;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class SelectionItem(Rational head, Rational absolutePosition, INote note)
    {
        public Rational BarHead { get; } = head;
        public Rational AbsolutePosition { get; } = absolutePosition;
        public INote Note { get; private set; } = note;

        public void ReplaceToClone()
        {
            Note = Note.Clone();
        }

        public void Deconstruct(out Rational head, out Rational absolutePosition, out INote value)
        {
            head = BarHead;
            absolutePosition = AbsolutePosition;
            value = Note;
        }

        public readonly struct Comparer : IEqualityComparer<SelectionItem>
        {
            bool IEqualityComparer<SelectionItem>.Equals(SelectionItem? x, SelectionItem? y) => ReferenceEquals(x!.Note, y!.Note);
            int IEqualityComparer<SelectionItem>.GetHashCode(SelectionItem obj) => obj.Note.GetHashCode();
        }
    }
}
