using System;
using System.Collections.Generic;

namespace LivreNoirLibrary.Media.Bms
{
    public class SelectionItem
    {
        public BarPosition BarPosition { get; }
        public double BarHead { get; }
        public double AbsolutePosition { get; }
        public double Time { get; internal set; }
        public Note Note { get; private set; }

        public SelectionItem(BarPosition pos, double barHead, double absolutePosition, double time, Note note)
        {
            BarPosition = pos;
            BarHead = barHead;
            AbsolutePosition = absolutePosition;
            Time = time;
            Note = note;
        }

        public SelectionItem(BarPosition position, IBmsViewModel viewModel, Note note)
        {
            BarPosition = position;
            BarHead = viewModel.GetHead(position);
            var absPos = viewModel.GetAbsolutePosition(position);
            AbsolutePosition = absPos;
            Time = viewModel.TimeCounter.Beat2Time(absPos);
            Note = note;
        }

        public void ReplaceToClone()
        {
            Note = Note.Clone();
        }

        public void Deconstruct(out double absolutePosition, out Note value)
        {
            absolutePosition = AbsolutePosition;
            value = Note;
        }

        public void Deconstruct(out double head, out double absolutePosition, out Note value)
        {
            head = BarHead;
            absolutePosition = AbsolutePosition;
            value = Note;
        }

        public void Deconstruct(out double head, out double absolutePosition, out double time, out Note value)
        {
            head = BarHead;
            absolutePosition = AbsolutePosition;
            time = Time;
            value = Note;
        }

        public readonly struct Comparer : IEqualityComparer<SelectionItem>
        {
            bool IEqualityComparer<SelectionItem>.Equals(SelectionItem? x, SelectionItem? y) => ReferenceEquals(x!.Note, y!.Note);
            int IEqualityComparer<SelectionItem>.GetHashCode(SelectionItem obj) => obj.Note.GetHashCode();
        }
    }
}
