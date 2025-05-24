using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public class Timeline : RationalMultiTimeline<IObject>, IDumpable<Timeline>
    {
        public const string Chid = "LNMdTl";

        public void SetTempo(Rational position, int value)
        {
            if (TryGetIndex(position, out var index))
            {
                var list = _value_list[index];
                if (list.FindLast(obj => obj is TempoEvent) is TempoEvent tempo)
                {
                    tempo.Value = value;
                }
                else
                {
                    _value_list[index].Add(new TempoEvent(value));
                }
            }
            else
            {
                InsertItem(~index, position, [new TempoEvent(value)]);
            }
        }

        public int GetTempo(Rational position)
        {
            if (TryGetIndex(position, out var index))
            {
                for (; index >= 0; index--)
                {
                    var list = _value_list[index];
                    for (var i = list.Count - 1; i >= 0; i--)
                    {
                        if (list[i] is TempoEvent tempo)
                        {
                            return tempo.Value;
                        }
                    }
                }
            }
            return TempoTimeline.DefaultTempo;
        }

        public static Timeline Load(BinaryReader reader)
        {
            Timeline timeline = [];
            timeline.ProcessLoad(reader);
            return timeline;
        }

        public void Dump(BinaryWriter writer)
        {
            IObjectWriter w = new();
            ProcessDump(writer, w.Write, Chid);
        }

        public void ProcessLoad(BinaryReader reader)
        {
            IObjectReader r = new();
            ProcessLoad(reader, r.Read, Chid);
        }

        public void ExtendToEvent(RawData.RawTimeline timeline, int channel, long ticksPerWholeNote)
        {
            foreach (var (pos, item) in this)
            {
                var tick = IObject.GetTick(pos, ticksPerWholeNote);
                item.ExtendToEvent(timeline, channel, tick, pos, ticksPerWholeNote);
            }
        }

        public void RemoveDuplicated(Selection? selection = null)
        {
            Dictionary<int, IObject> dups = [];
            HashSet<IObject> remove = [];
            selection ??= [];

            void AddRemove(Rational pos, IObject obj)
            {
                selection.Remove(pos, obj);
                remove.Add(obj);
            }

            var poss = _pos_list;
            var lists = _value_list;
            var i = 0;
            while (i < poss.Count)
            {
                var pos = poss[i];
                var list = lists[i];
                dups.Clear();
                remove.Clear();
                foreach (var obj in CollectionsMarshal.AsSpan(list))
                {
                    if (obj is Note n)
                    {
                        if (!dups.TryAdd(n.Number, n))
                        {
                            AddRemove(pos, n);
                        }
                    }
                    else if (obj is NoteGroup ng)
                    {
                        var num = ng.FirstNote.Number;
                        if (dups.TryGetValue(num, out var current))
                        {
                            if (current is not NoteGroup)
                            {
                                dups[num] = ng;
                                AddRemove(pos, current);
                            }
                            else
                            {
                                AddRemove(pos, ng);
                            }
                        }
                        else
                        {
                            dups.Add(num, ng);
                        }
                    }
                }
                list.RemoveAll(remove.Contains);
                if (list.Count is 0)
                {
                    RemoveItem(i);
                }
                else
                {
                    i++;
                }
            }
        }
    }
}
