using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.Collections;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly record struct NoteInfo(BarPosition Position, Rational ActualPosition, Note Note) : INoteWrapper;

    public partial class BaseData
    {
        public static BarPosition DefaultLength { get; } = new(4);

        public BarPosition GetFirstPosition() => Timeline.FirstPosition;
        public int GetFirstBar() => GetFirstPosition().Bar;
        public Rational GetFirstBeat() => GetHead(GetFirstPosition());

        public BarPosition GetLastPosition() => BarPosition.Max(Timeline.LastPosition, DefaultLength);
        public int GetLastBar() => GetLastPosition().Bar;
        public Rational GetLastBeat() => GetTail(GetLastPosition());

        public IEnumerable<NoteInfo> EachNote(bool inherit = false)
        {
            if (inherit)
            {
                if (InheritedTimeline is not null)
                {
                    foreach (var (pos, item) in InheritedTimeline)
                    {
                        yield return new(pos, GetBeat(pos), item);
                    }
                }
            }
            else
            {
                foreach (var (pos, item) in Timeline)
                {
                    yield return new(pos, GetBeat(pos), item);
                }
            }
        }
    }
}
