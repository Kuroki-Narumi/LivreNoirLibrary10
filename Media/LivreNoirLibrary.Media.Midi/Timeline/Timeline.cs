using System;
using System.Collections.Generic;
using System.IO;
using LivreNoirLibrary.Collections;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Midi
{
    public class Timeline : RationalMultiTimeline<IObject>, ITimeline, IDumpable, ILoadable<Timeline>
    {
        public const string Chid = "LNMdTl";

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

        public void RemoveDuplicated(ISelection? selection = null)
        {
            Dictionary<int, IObject> dups = [];
            HashSet<IObject> remove = [];
            selection ??= new Selection();

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
                foreach (var obj in list.AsSpan())
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
