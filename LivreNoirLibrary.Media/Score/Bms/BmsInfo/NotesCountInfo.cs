using System;
using System.Collections.Generic;
using System.Text;

namespace LivreNoirLibrary.Media.Bms
{
    public readonly struct NotesCountInfo(int n, int i, int l, int m)
    {
        public readonly int Normal = n;
        public readonly int Invisible = i;
        public readonly int LongEnd = l;
        public readonly int Mine = m;

        public NotesCountInfo() : this(0, 0, 0, 0) { }

        public static NotesCountInfo Create(NoteType type)
        {
            return type switch
            {
                NoteType.Invisible => new(0, 1, 0, 0),
                NoteType.LongEnd => new(0, 0, 1, 0),
                NoteType.Mine => new(0, 0, 0, 1),
                _ => new(1, 0, 0, 0),
            };
        }

        public NotesCountInfo Add(NoteType type)
        {
            return type switch
            {
                NoteType.Invisible => new(Normal, Invisible + 1, LongEnd, Mine),
                NoteType.LongEnd => new(Normal, Invisible, LongEnd + 1, Mine),
                NoteType.Mine => new(Normal, Invisible, LongEnd, Mine + 1),
                _ => new(Normal + 1, Invisible, LongEnd, Mine),
            };
        }

        public static NotesCountInfo operator +(NotesCountInfo left, NotesCountInfo right) => new(
            left.Normal + right.Normal, 
            left.Invisible + right.Invisible, 
            left.LongEnd + right.LongEnd,
            left.Mine + right.Mine);

        public bool Exists => Normal is > 0 || Invisible is > 0 || LongEnd is > 0 || Mine is > 0;

        public override string ToString()
        {
            List<string> ops = [];
            if (Invisible is > 0)
            {
                ops.Add($"IV{Invisible}");
            }
            if (LongEnd is > 0)
            {
                ops.Add($"LE{LongEnd}");
            }
            if (Mine is > 0)
            {
                ops.Add($"M{LongEnd}");
            }
            return ops.Count is > 0 ? $"{Normal}({string.Join(',', ops)})" : Normal.ToString();
        }
    }
}
