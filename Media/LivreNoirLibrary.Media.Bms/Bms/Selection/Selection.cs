using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public class Selection() : HashSet<SelectionItem>(new SelectionItem.Comparer()), IDumpable, ILoadable<Selection>
    {
        public const string Chid = "LNBSel";

        public bool IsEmpty() => Count is 0;

        public bool Add(Rational head, Rational absolutePosition, INote note) => Add(new(head, absolutePosition, note));

        public void ReplaceToClone()
        {
            foreach (var item in this)
            {
                item.ReplaceToClone();
            }
        }

        public void ChangeLane(Dictionary<Channel, Channel> channelMap, Dictionary<int, int> laneMap)
        {
            foreach (var item in this)
            {
                if (item is ISoundNote s)
                {
                    if (laneMap.TryGetValue(s.Lane, out var lane))
                    {
                        s.Lane = lane;
                    }
                }
                else if (item is IChannelNote c && channelMap.TryGetValue(c.Channel, out var channel))
                {
                    c.Channel = channel;
                }
            }
        }

        public Rational GetFirstBarHead() => this.MinBy(item => item.BarHead) is { } item ? item.BarHead : Rational.Zero;

        public bool TryGetFirstSound([MaybeNullWhen(false)]out SelectionItem item, bool includeLongEnd)
        {
            if (this.Where(item => item.Note.IsNormal(includeLongEnd, out var actual))
                    .MinBy(item => item.AbsolutePosition)
                    is { } actual)
            {
                item = actual;
                return true;
            }
            item = null;
            return false;
        }

        public IEnumerable<INote> EachNote()
        {
            foreach(var item in this)
            {
                yield return item.Note;
            }
        }

        public HashSet<INote> GetNoteHash() => [.. this.Select(item => item.Note)];

        public void Dump(BinaryWriter writer)
        {
            writer.WriteChid(Chid);
            writer.Write(Count);
            var offset = GetFirstBarHead();
            foreach (var (_, p, n) in this)
            {
                writer.Write(p - offset);
                INoteExtensions.Write(writer, n);
            }
        }

        public static Selection Load(BinaryReader reader)
        {
            reader.CheckChid(Chid);
            var count = reader.ReadInt32();
            Selection selection = [];
            for (var i = 0; i < count; i++)
            {
                var pos = reader.ReadRational();
                var note = INoteExtensions.ReadINote(reader);
                selection.Add(Rational.Zero, pos, note);
            }
            return selection;
        }
    }
}
