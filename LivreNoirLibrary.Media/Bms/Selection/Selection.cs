using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class Selection() : SelectionBase<SelectionItem>
    {
        public const string Chid = "LNBSel";

        public void Add(BarPosition position, Rational actualPos, Note note) => Add(actualPos, new(position, actualPos, note));

        public bool TryFind(Note note, [MaybeNullWhen(false)]out SelectionItem item)
        {
            if (Find((p, n) => ReferenceEquals(n.Note, note), out var position, out item))
            {
                return true;
            }
            item = null;
            return false;
        }

        public void ChangeID(Dictionary<int, int> map)
        {
            ForEachItem(item =>
            {
                if (map.TryGetValue(item.Note.Id, out var value))
                {
                    item.Note.Id = value;
                }
            });
        }

        public void ChangeLane(Dictionary<int, int> map)
        {
            ForEachItem(item =>
            {
                if (map.TryGetValue(item.Note.Lane, out var value))
                {
                    item.Note.Lane = value;
                }
            });
        }

        public void CopyTo(Selection target) => ForEachItem(target.Add);

        public Rational GetFirstSound(bool includesLongEnd = true)
        {
            foreach (var items in CollectionsMarshal.AsSpan(_value_list))
            {
                foreach (var (_, p, note) in CollectionsMarshal.AsSpan(items))
                {
                    if (note.IsPlayableSound(includesLongEnd))
                    {
                        return p;
                    }
                }
            }
            return Rational.Zero;
        }

        public Rational GetFirstBarHead()
        {
            if (IsEmpty())
            {
                return Rational.Zero;
            }
            var item = _value_list[0][0];
            return item.ActualPosition - item.Position.Offset;
        }

        public HashSet<Note> GetNoteHash() => [.. EachItem().Select(item => item.Note)];

        public void Dump(BinaryWriter writer)
        {
            writer.WriteChid(Chid);
            writer.Write(Count);
            var offset = GetFirstBarHead();
            foreach (var (_, item) in this)
            {
                writer.Write(item.ActualPosition - offset);
                item.Note.Dump(writer);
            }
        }

        public static Selection Load(BinaryReader reader)
        {
            Selection result = [];
            reader.CheckChid(Chid);
            var count = reader.ReadInt32();
            for (var i = 0; i < count; i++)
            {
                var pos = reader.ReadRational();
                var note = Note.Load(reader);
                result.Add(new(new(0, pos), pos, note));
            }
            return result;
        }
    }
}
