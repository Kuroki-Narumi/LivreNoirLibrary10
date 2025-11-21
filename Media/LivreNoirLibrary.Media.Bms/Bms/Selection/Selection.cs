using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using LivreNoirLibrary.IO;

namespace LivreNoirLibrary.Media.Bms
{
    public class Selection() : HashSet<SelectionItem>(new SelectionItem.Comparer()), ILoadable, IDumpable
    {
        public const string Chid = "LNBSel";

        public bool IsEmpty => Count is 0;

        public bool Add(BarPosition position, double head, double absolutePosition, double time, Note note) => Add(new(position, head, absolutePosition, time, note));
        public bool Add(BarPosition position, IBmsViewModel viewModel, Note note) => Add(new(position, viewModel, note));
        public bool Remove(Note note) => this.First(item => item.Note == note) is { } item && Remove(item);

        public void ReplaceToClone()
        {
            foreach (var item in this)
            {
                item.ReplaceToClone();
            }
        }

        public void EnsureTime(ITimeCounter timeCounter)
        {
            var head = timeCounter.Beat2Time(GetFirstBarHead());
            foreach (var item in this)
            {
                item.Time = timeCounter.Beat2Time(item.AbsolutePosition) - head;
            }
        }

        public void ChangeLane(Dictionary<Channel, Channel> map)
        {
            foreach (var item in this)
            {
                if (map.TryGetValue(item.Note.Channel, out var changed))
                {
                    item.Note.Channel = changed;
                }
            }
        }

        public double GetFirstBarHead() => this.MinBy(item => item.BarHead) is { } item ? item.BarHead : 0;

        public bool TryGetFirstSound([MaybeNullWhen(false)]out SelectionItem item, bool includeLongEnd)
        {
            if (this.Where(item => item.Note.IsNormal(includeLongEnd))
                    .MinBy(item => item.AbsolutePosition)
                    is { } actual)
            {
                item = actual;
                return true;
            }
            item = null;
            return false;
        }

        public IEnumerable<Note> EachNote()
        {
            foreach(var item in this)
            {
                yield return item.Note;
            }
        }

        public void GetNoteHash(HashSet<Note> set) => set.UnionWith(this.Select(item => item.Note));

        public void Dump(BinaryWriter writer)
        {
            writer.WriteChid(Chid);
            writer.Write(Count);
            var offset = GetFirstBarHead();
            foreach (var (_, p, t, n) in this)
            {
                writer.Write(p - offset);
                writer.Write(t);
                writer.Write(n);
            }
        }

        public void ProcessLoad(BinaryReader reader)
        {
            Clear();
            reader.CheckChid(Chid);
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var pos = reader.ReadDouble();
                var time = reader.ReadDouble();
                var note = reader.ReadNote();
                Add(new(default, 0, pos, time, note));
            }
        }
    }
}
