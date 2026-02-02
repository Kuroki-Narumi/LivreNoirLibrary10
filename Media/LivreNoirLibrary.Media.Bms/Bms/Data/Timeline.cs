using System;
using System.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class Timeline : XMultiTimelineBase<BarPosition, Note, Operator_BarPosition>, ITimeline
    {
        public void Dump(BinaryWriter writer) => ProcessDump(writer, BmsExtensions.Write);

        public void ProcessLoad(BinaryReader reader)
        {
            Clear();
            ProcessLoad(reader, BmsExtensions.ReadNote);
        }

        public void InsertBar(int number, int count)
        {
            var poss = _pos_list;
            var (s, l) = GetPositionIndex(RangeUtils.StartAt(new BarPosition(number)));
            var e = s + l;
            for (; s < e; s++)
            {
                var (bar, offset) = poss[s];
                poss[s] = new(bar + count, offset);
            }
        }

        public void DeleteBar(int number, int count)
        {
            var first = new BarPosition(number);
            var last = new BarPosition(number + count);
            RemoveRange(RangeUtils.Get(first, last));
            var poss = _pos_list;
            var (s, l) = GetPositionIndex(RangeUtils.StartAt(last));
            var e = s + l;
            for (; s < e; s++)
            {
                var (bar, offset) = poss[s];
                poss[s] = new(bar - count, offset);
            }
        }
    }
}
