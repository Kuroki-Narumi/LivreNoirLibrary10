using System;

namespace LivreNoirLibrary.Media.Bms
{
    public static partial class BmsExtensions
    {
        extension(IBmsDataUnit data)
        {
            public void InsertBar(int number, int count)
            {
                data.BarDefs.Insert(number, count);
                data.Timeline.InsertBar(number, count);
            }

            public void DeleteBar(int number, int count)
            {
                data.BarDefs.Delete(number, count);
                data.Timeline.DeleteBar(number, count);
            }
        }

        extension(IBmsData root)
        {
            public void InsertBar(int number) => InsertBar(root, root.Root, number, 1);
            public void InsertBar(int number, int count) => InsertBar(root, root.Root, number, count);
            public void InsertBar(IBmsDataUnit start, int number) => InsertBar(root, start, number, 1);
            public void InsertBar(IBmsDataUnit start, int number, int count)
            {
                foreach (var (_, data) in root.EnumerateChildren(start, true))
                {
                    data.InsertBar(number, count);
                }
            }

            public void DeleteBar(int number) => DeleteBar(root, root.Root, number, 1);
            public void DeleteBar(int number, int count) => DeleteBar(root, root.Root, number, count);
            public void DeleteBar(IBmsDataUnit start, int number) => DeleteBar(root, start, number, 1);
            public void DeleteBar(IBmsDataUnit start, int number, int count)
            {
                foreach (var (_, data) in root.EnumerateChildren(start, true))
                {
                    data.DeleteBar(number, count);
                }
            }

            public (BarPosition First, BarPosition Last) GetRange()
            {
                var first = BarPosition.MaxValue;
                var last = BarPosition.Zero;
                foreach (var (_, data) in root.EnumerateAllData())
                {
                    var poss = data.Timeline.GetPositions();
                    if (poss.Length is > 0)
                    {
                        first = BarPosition.Min(first, poss[0]);
                        last = BarPosition.Max(last, poss[^1]);
                    }
                }
                if (first > last)
                {
                    first = last = default;
                }
                return (first, last);
            }

            public (BarPosition First, BarPosition Last) GetRange(Predicate<Note> predicate)
            {
                var first = BarPosition.MaxValue;
                var last = BarPosition.Zero;
                foreach (var (_, data) in root.EnumerateAllData())
                {
                    var timeline = data.Timeline;
                    if (timeline.Find((_, note) => predicate(note), out var pos, out _))
                    {
                        first = BarPosition.Min(first, pos);
                    }
                    if (timeline.FindLast((_, note) => predicate(note), out pos, out _))
                    {
                        last = BarPosition.Max(last, pos);
                    }
                }
                if (first > last)
                {
                    first = last = default;
                }
                return (first, last);
            }
        }

        extension (IListEnumerable<BarPosition, Note> timeline)
        {
            public (BarPosition First, BarPosition Last) GetRange() => (timeline.FirstPosition, timeline.LastPosition);
            public (BarPosition First, BarPosition Last) GetRange(Predicate<Note> predicate)
            {
                timeline.Find((_, note) => predicate(note), out var first, out _);
                timeline.FindLast((_, note) => predicate(note), out var last, out _);
                if (first > last)
                {
                    first = last = default;
                }
                return (first, last);
            }
        }
    }
}
