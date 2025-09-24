using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;
using LivreNoirLibrary.ObjectModel;

namespace LivreNoirLibrary.Media.Bms
{
    public class Timeline : XMultiTimelineBase<BarPosition, INote, Operator_BarPosition>, ITimeline, ICloneable<Timeline>, IDumpable, ILoadable<Timeline>
    {
        public Timeline Clone()
        {
            Timeline result = [];
            CopyTo(result);
            return result;
        }

        public void Dump(BinaryWriter writer) => ProcessDump(writer, INoteExtensions.Write);
        public void ProcessLoad(BinaryReader reader) => ProcessLoad(reader, INoteExtensions.ReadINote);

        public static Timeline Load(BinaryReader reader)
        {
            Timeline result = [];
            result.ProcessLoad(reader);
            return result;
        }

        public void ReplaceBy(BinaryReader reader)
        {
            Clear();
            ProcessLoad(reader);
        }

        public void InsertBar(int number)
        {
            var poss = _pos_list;
            var (s, l) = GetPositionIndex(RangeUtils.StartAt(new BarPosition(number)));
            var e = s + l;
            for (var i = s; i < e; i++)
            {
                poss[s] = new(poss[s].Bar + 1, poss[s].Offset);
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
            for (var i = s; i < e; i++)
            {
                poss[s] = new(poss[s].Bar - count, poss[s].Offset);
            }
        }
    }
}
