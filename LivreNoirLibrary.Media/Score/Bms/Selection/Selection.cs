using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LivreNoirLibrary.IO;
using LivreNoirLibrary.Numerics;

namespace LivreNoirLibrary.Media.Bms
{
    public sealed class Selection() : SelectionBase<SelectionItem>, IDumpable<Selection>
    {
        public const string Chid = "LNBSel";

        public void Add(BarPosition position, Rational actualPos, Note note) => Add(actualPos, new(position, actualPos, note));

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

        public Rational GetFirstBarHead()
        {
            if (IsEmpty())
            {
                return Rational.Zero;
            }
            var item = _value_list[0][0];
            return item.ActualPosition - item.Position.Beat;
        }

        public HashSet<Note> GetNoteHash() => [.. EachItem().Select(item => item.Note)];

        public void Dump(BinaryWriter writer)
        {
            ProcessDump(writer, GetFirstBarHead(), (writer, item) => item.Note.Dump(writer), Chid);
        }

        public static Selection Load(BinaryReader reader)
        {
            Selection selection = [];
            selection.ProcessLoad(reader, (reader, pos) => new(new(0, pos), pos, Note.Load(reader)), Chid);
            return selection;
        }
    }
}
