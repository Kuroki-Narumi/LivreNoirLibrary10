using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class NoteTimeline : XMultiTimelineBase<BarPosition, Note, Operator_BarPosition>, IDumpable<NoteTimeline>
    {
        public void Merge(NoteTimeline timeline)
        {
            timeline.CopyTo(this);
        }

        public NoteTimeline Clone()
        {
            NoteTimeline timeline = [];
            CopyTo(timeline);
            return timeline;
        }

        protected void ProcessSlice(NoteTimeline destination, BarPosition from, BarPosition to)
        {
            var (s, l) = GetPositionIndex(RangeUtils.Get(from, to));
            var e = s + l;
            for (var i = s; i < e; i++)
            {
                destination.Add(_pos_list[i] - from, _value_list[i].Select(n => n.Clone()));
            }
        }

        public void Dump(BinaryWriter writer) => ProcessDump(writer, Dumpable.Dump);
        public void ProcessLoad(BinaryReader reader) => ProcessLoad(reader, Note.Load);

        public NoteTimeline Slice(BarPosition from, BarPosition to)
        {
            NoteTimeline timeline = [];
            ProcessSlice(timeline, from, to);
            return timeline;
        }

        public static NoteTimeline Load(BinaryReader reader)
        {
            NoteTimeline result = [];
            result.ProcessLoad(reader);
            return result;
        }

        public void ReplaceBy(BinaryReader reader)
        {
            Clear();
            ProcessLoad(reader);
        }

        internal bool StretchWithTempo(IBmsData data, SortedSet<int> nums, Rational value, bool ratioMode)
        {
            var changed = false;
            var bars = data.Bars;
            var poss = _pos_list;
            var vals = _value_list;

            var index = 0;
            bool IsMatch(Predicate<BarPosition> condition) => index < poss.Count && condition(poss[index]);

            var actualBpm = (Rational)data.Bpm;
            var currentBpm = actualBpm;

            void AppendHeadBpm(int number)
            {
                BarPosition head = new(number);
                // 現在のノート位置が小節頭の場合
                if (IsMatch(p => p == head))
                {
                    var list = vals[index];
                    // 小節頭にテンポノートが無い場合は、本来のテンポに戻すためのノートを追加する
                    if (list.FindLast(n => n.IsTempo()) is null)
                    {
                        list.Insert(0, Note.Tempo(actualBpm));
                    }
                }
                else
                {
                    // 小節頭に正しいテンポのノートを配置
                    InsertItem(index, head, [Note.Tempo(actualBpm)]);
                }
            }

            foreach (var n in nums)
            {
                // 変更されない小節はテンポ変化のチェックだけ行う
                for (; IsMatch(p => p.Bar < n); index++)
                {
                    var list = vals[index];
                    if (list.FindLast(n => n.IsTempo()) is Note note)
                    {
                        currentBpm = actualBpm = note.Value;
                    }
                }

                var current = bars.Get(n);
                Rational newLength, ratio;
                if (ratioMode)
                {
                    newLength = current * value;
                    ratio = value;
                }
                else
                {
                    newLength = value;
                    ratio = value / current;
                    if (current == newLength)
                    {
                        continue;
                    }
                }
                changed = true;
                bars.Set(n, newLength);
                // 小節頭に現在のテンポをノートとして設置
                AppendHeadBpm(n);
                // 小節内位置の変更
                while (IsMatch(p => p.Bar == n))
                {
                    // 位置の更新
                    poss[index] = new(n, poss[index].Beat * ratio);
                    var list = vals[index];
                    // テンポ変化の反映
                    for (var i = 0; i < list.Count;)
                    {
                        var note = list[i];
                        if (note.IsTempo())
                        {
                            actualBpm = note.Value;
                            var newBpm = actualBpm * ratio;
                            // テンポ変化がなくなる場合は、そのテンポノートを削除する
                            if (newBpm == currentBpm)
                            {
                                list.RemoveAt(i);
                                continue;
                            }
                            note.Value = currentBpm = newBpm;
                        }
                        else if (note.IsStop())
                        {
                            note.Value *= ratio;
                        }
                        i++;
                    }
                    if (list.Count is 0)
                    {
                        RemoveItem(index);
                    }
                    else
                    {
                        index++;
                    }
                }
                // 次の小節頭にBPMを元に戻すためのノートを設置
                AppendHeadBpm(n + 1);
            }
            return changed;
        }
    }
}
